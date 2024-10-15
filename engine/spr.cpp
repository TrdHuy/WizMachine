#include "pch.h"
#include "base.h"
#include "ucl/ucl.h"

UCHAR FindPaletteIndex(UCHAR B, UCHAR G, UCHAR R, Color palette[], int paletteSize) {
	for (int i = 0; i < paletteSize; i++)
	{
		if (R == palette[i].R &&
			G == palette[i].G &&
			B == palette[i].B)
		{
			return i;
		}
	}
	throw std::exception("Color not found in palette!");
}


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

void ExportToSPRFileInternal(const char* filePath,
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

void LoadSPRFileInternal(const char* filePath,
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

	// CoTaskMemAlloc Là hàm của COM API trong Windows, 
	// chủ yếu được sử dụng trong các ngữ cảnh COM hoặc khi bạn 
	// cần tương tác với các hàm cấp phát bộ nhớ tương thích với hệ thống COM.
	//*palette = (Color*)::CoTaskMemAlloc(sizeof(Color) * fileHead->ColorCounts);
	//*palette = new Color[fileHead->ColorCounts];
	*palette = MemoryManager::getInstance()->allocateArray<Color>(fileHead->ColorCounts);
	::memset(*palette, 0, sizeof(Color) * fileHead->ColorCounts);
	for (int i = 0; i < fileHead->ColorCounts; i++) {
		(*palette)[i].R = file.get();
		(*palette)[i].G = file.get();
		(*palette)[i].B = file.get();
	}
	*paletteLength = fileHead->ColorCounts;
	*frameDataBeginPos = sizeof(SPRFileHead) + fileHead->ColorCounts * sizeof(Color);

	//*frame = (FrameData*)::CoTaskMemAlloc(sizeof(FrameData) * fileHead->FrameCounts);
	*frame = MemoryManager::getInstance()->allocateArray<FrameData>(fileHead->FrameCounts);
	*frameCount = fileHead->FrameCounts;
	for (int frameIndex = 0; frameIndex < fileHead->FrameCounts; frameIndex++) {
		// Frame offset
		file.seekg(*frameDataBeginPos + sizeof(FrameOffsetInfo) * frameIndex, std::ios::beg);
		FrameOffsetInfo* offsetInfo = MemoryManager::getInstance()->allocate<FrameOffsetInfo>();
		file.read(reinterpret_cast<char*>(offsetInfo), sizeof(FrameOffsetInfo));

		unsigned int frameBeginPos = *frameDataBeginPos +
			sizeof(FrameOffsetInfo) * fileHead->FrameCounts +
			offsetInfo->FrameOffset;
		unsigned int dataLength = offsetInfo->DataLength;
		(*frame)[frameIndex].EncryptedFrameDataOffset = offsetInfo->FrameOffset;
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
		(*frame)[frameIndex].initMemory(decDataLength * 4);

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
		MemoryManager::getInstance()->deallocate(offsetInfo);
	}

	file.close();
}


static void intToHexString(int value, char* output)
{
	const char* hexDigits = "0123456789abcdef";
	for (int i = 7; i >= 0; --i)
	{
		output[i] = hexDigits[value & 0xF];
		value >>= 4;
	}
	output[8] = '\0'; // Null-terminate the string
}

void TestAloneFile() {

	// Mở file mới để viết, nếu file không tồn tại, nó sẽ được tạo
	const char* fileName = "t\\example.txt";
	IFile* file = g_OpenFile(fileName, true, true, true);

	assert(file->isOpen());
	if (file->isOpen()) {  // true cho writeSupport để có thể viết vào file
		std::cout << "File opened successfully." << std::endl;

		// Viết dữ liệu vào file
		const char* data = "Hello, World!";
		unsigned long bytesWritten = file->write(data, strlen(data));
		if (bytesWritten > 0) {
			std::cout << "Successfully wrote " << bytesWritten << " bytes to the file." << std::endl;
		}
		else {
			std::cout << "Failed to write data to the file." << std::endl;
		}

		// Đóng file để lưu các thay đổi
		file->close();
	}

	file = g_OpenFile(fileName, true, false, false);
	assert(file->isOpen());
	if (file->isOpen()) {  // false cho writeSupport vì chỉ đọc
		// Đọc dữ liệu từ file
		unsigned long fileSize = file->size();
		char* buffer = MemoryManager::getInstance()->allocateArray<char>(fileSize + 1);
		unsigned long bytesRead = file->read(buffer, fileSize);
		if (bytesRead > 0) {
			buffer[bytesRead] = '\0';  // Đảm bảo chuỗi kết thúc bằng null
			std::cout << "Read from file: " << buffer << std::endl;
		}
		else {
			std::cout << "Failed to read data from the file." << std::endl;
		}

		// Dọn dẹp
		MemoryManager::getInstance()->deallocate(buffer);
		file->close();
	}
	else {
		std::cout << "Failed to open file for reading." << std::endl;
	}

}



void LoadPakFile(const char* pakFilePath, const char* pakInfoFilePath, const char* outputRootPath) {
	/*std::ifstream file(filePath, std::ios::binary);
	if (!file.is_open()) {
		std::cerr << "Failed to open file." << std::endl;
		return;
	}
	int hash = g_FileNameHash(filePath);
	char hashHexString[9];
	intToHexString(hash, hashHexString);
	file.close();*/

	/*PakInfo pakInfo;
	int p = ParsePakInfoFile(pakInfoFilePath, pakInfo);
	std::unique_ptr<PakHeader> header;
	int res = LoadPakInternal(pakFilePath, outputRootPath, pakInfo, header);*/

	//TestAloneFile();


	//char* message = get_message();  // Gọi hàm assembly để lấy thông điệp
	//std::cout << message << std::endl;  // In thông điệp nhận được

	const char* fileName = "t\\example.txt";
	IFile* file = g_OpenFile(fileName, true, false, false);

	if (file->isOpen()) {
		void* pSrcBuffer = file->getBuffer();
		unsigned long nSrcSize = file->size();

#ifdef x32
		int uCRC = Misc_CRC32(0, pSrcBuffer, nSrcSize);
		int size = 0;
#endif

#ifdef x64
		int uCRC = compute_crc32(0, pSrcBuffer, nSrcSize);
		int c = 100;
#endif


	}
}

