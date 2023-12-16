#include "pch.h"
#include "calc.h"

std::vector<UCHAR> EncryptFrameData(
	Color palette[],
	int paletteSize,
	FrameData frame)
{
	std::vector<UCHAR> encryptedFrameData;
	encryptedFrameData.resize(sizeof(FrameInfo));
	std::memcpy(encryptedFrameData.data(), &(frame.FrameInfo), sizeof(FrameInfo));

	for (int i = 0; i < frame.DecodedLength;)
	{
		UCHAR size = 0;
		UCHAR alpha = frame.DecodedFrameData[i + 3];
		if (alpha == 0) {
			while (i < frame.DecodedLength && frame.DecodedFrameData[i + 3] == 0 && size < 255) {
				i += 4;
				size++;
				if ((i / 4) % frame.FrameInfo.Width == 0) {
					break;
				}
			}
			encryptedFrameData.push_back(size);
			encryptedFrameData.push_back(alpha);
		}
		else {
			std::vector<UCHAR> temp;
			while (i < frame.DecodedLength && frame.DecodedFrameData[i + 3] == alpha && size < 255) {
				UCHAR index = FindPaletteIndex(
					frame.DecodedFrameData[i],
					frame.DecodedFrameData[i + 1],
					frame.DecodedFrameData[i + 2],
					palette,
					paletteSize);
				temp.push_back(index);
				i += 4;
				size++;
				if ((i / 4) % frame.FrameInfo.Width == 0) {
					break;
				}

			}
			encryptedFrameData.push_back(size);
			encryptedFrameData.push_back(alpha);
			encryptedFrameData.insert(encryptedFrameData.end(), temp.begin(), temp.end());
		}
	}
	return encryptedFrameData;
}

void ExportToSPRFile(const char* filePath,
	SPRFileHead fileHead,
	Color palette[],
	int paletteSize,
	FrameData frame[])
{
	std::ofstream file(filePath, std::ios::binary);
	if (file.is_open()) {
		std::vector<unsigned char> exportedFileData;

		// Export file head
		exportedFileData.resize(sizeof(SPRFileHead));
		std::memcpy(exportedFileData.data(), &fileHead, sizeof(SPRFileHead));

		// Export palette
		exportedFileData.resize(exportedFileData.size() + paletteSize * sizeof(Color));
		for (int i = 0; i < paletteSize; i++) {
			std::memcpy(exportedFileData.data() + sizeof(SPRFileHead) + i * sizeof(Color),
				&(palette[i]),
				sizeof(Color));
		}

		// Encrypt frames
		std::vector<std::vector<UCHAR>> encryptedFramesData;
		for (int i = 0; i < fileHead.FrameCounts; i++) {
			encryptedFramesData.push_back(EncryptFrameData(
				palette,
				paletteSize,
				frame[i]
			));
		}

		// Export frame offset info
		int oldSize = exportedFileData.size();
		int totalEncryptedDataLength = 0;
		exportedFileData.resize(exportedFileData.size() + fileHead.FrameCounts * sizeof(FrameOffsetInfo));
		for (int frameIndex = 0; frameIndex < fileHead.FrameCounts; frameIndex++) {
			FrameOffsetInfo fOI;
			fOI.FrameOffset = 0;
			for (int j = 0; j < frameIndex; j++) {
				fOI.FrameOffset += encryptedFramesData[j].size();
			}
			fOI.DataLength = encryptedFramesData[frameIndex].size();
			totalEncryptedDataLength += fOI.DataLength;
			std::memcpy(exportedFileData.data() + oldSize + frameIndex * sizeof(FrameOffsetInfo),
				&(fOI),
				sizeof(FrameOffsetInfo));
		}

		// Export frame data
		oldSize = exportedFileData.size();
		exportedFileData.resize(oldSize + totalEncryptedDataLength);
		for (int frameIndex = 0; frameIndex < fileHead.FrameCounts; frameIndex++) {
			int dataLenght = encryptedFramesData[frameIndex].size();
			std::memcpy(exportedFileData.data() + oldSize,
				encryptedFramesData[frameIndex].data(),
				dataLenght);
			oldSize += dataLenght;
		}

		file.write(reinterpret_cast<char*>(exportedFileData.data()), exportedFileData.size());
		file.close();
	}
}

void LoadSPRFile(const char* filePath,
	SPRFileHead* fileHead,
	Color** palette,
	int* paletteLength,
	int* frameDataBeginPos,
	FrameData** frame,
	int* frameCount)
{
	std::ifstream file(filePath, std::ios::binary);
	if (!file.is_open()) {
		std::cerr << "Failed to open file." << std::endl;
		palette = nullptr;
		return;
	}

	file.read(reinterpret_cast<char*>(fileHead), sizeof(SPRFileHead));

	*palette = (Color*)::CoTaskMemAlloc(sizeof(Color) * fileHead->ColorCounts);
	::memset(*palette, 0, sizeof(Color) * fileHead->ColorCounts);
	for (int i = 0; i < fileHead->ColorCounts; i++) {
		(*palette)[i].R = file.get();
		(*palette)[i].G = file.get();
		(*palette)[i].B = file.get();
	}
	*paletteLength = fileHead->ColorCounts;
	*frameDataBeginPos = sizeof(SPRFileHead) + fileHead->ColorCounts * sizeof(Color);

	*frame = (FrameData*)::CoTaskMemAlloc(sizeof(FrameData) * fileHead->FrameCounts);
	*frameCount = fileHead->FrameCounts;
	for (int frameIndex = 0; frameIndex < fileHead->FrameCounts; frameIndex++) {
		// Frame offset
		file.seekg(*frameDataBeginPos + sizeof(FrameOffsetInfo) * frameIndex, std::ios::beg);
		FrameOffsetInfo* offsetInfo = new FrameOffsetInfo();
		file.read(reinterpret_cast<char*>(offsetInfo), sizeof(FrameOffsetInfo));

		unsigned int frameBeginPos = *frameDataBeginPos +
			sizeof(FrameOffsetInfo) * fileHead->FrameCounts +
			offsetInfo->FrameOffset;
		unsigned int dataLength = offsetInfo->DataLength;
		(*frame)[frameIndex].EncyptedFrameDataOffset = offsetInfo->FrameOffset;
		(*frame)[frameIndex].EncryptedLength = offsetInfo->DataLength;
		if (dataLength == 0)
		{
			continue;
		}

		file.seekg(frameBeginPos, std::ios::beg);
		file.read(reinterpret_cast<char*>(&(*frame)[frameIndex].FrameInfo),
			sizeof(FrameInfo));
		long decDataLength = (*frame)[frameIndex].FrameInfo.Height * (*frame)[frameIndex].FrameInfo.Width;
		if (decDataLength == 0)
		{
			continue;
		}
		(*frame)[frameIndex].DecodedFrameData = new unsigned char[decDataLength * 4];
		(*frame)[frameIndex].ColorMap = new int[decDataLength];
		(*frame)[frameIndex].DecodedLength = decDataLength * 4;

		long frameDataPos = frameBeginPos + sizeof(FrameInfo);
		file.seekg(frameDataPos, std::ios::beg);
		long curDecPos = 0;
		for (int i = 0; i < dataLength - 8;) {
			if (curDecPos > decDataLength) {
				throw std::exception("Failed to decrypt, somethings wrong!");
			}
			int size = file.get();
			int alpha = file.get();

			if (size == -1 || alpha == -1) {
				throw std::exception("Failed to decrypt, somethings wrong!");
			}

			if (alpha == 0x00) {
				for (int j = 0; j < size; j++)
				{
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4] = 0;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 1] = 0;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 2] = 0;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 3] = 0;
					(*frame)[frameIndex].ColorMap[curDecPos] = -1;
					curDecPos++;
				}
			}
			else {
				for (int j = 0; j < size; j++)
				{
					int colorIndex = (int)file.get();
					if (colorIndex == -1)
					{
						throw std::exception("Failed to decrypt, colorIndex must greater than -1!");
					}
					i++;

					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4] = (*palette)[colorIndex].B;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 1] = (*palette)[colorIndex].G;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 2] = (*palette)[colorIndex].R;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 3] = alpha;
					(*frame)[frameIndex].ColorMap[curDecPos] = colorIndex;
					curDecPos++;
				}
			}
			i += 2;
		}
		delete offsetInfo;
	}

	file.close();
}
