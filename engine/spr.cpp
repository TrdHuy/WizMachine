#include "pch.h"

UCHAR FindPaletteIndex(UCHAR B, UCHAR G, UCHAR R, Color palette[], int paletteSize, ErrorCode& error) {
	for (int i = 0; i < paletteSize; i++)
	{
		if (R == palette[i].R &&
			G == palette[i].G &&
			B == palette[i].B)
		{
			error = ErrorCode::Success;
			return i;
		}
	}
	error = ErrorCode::ShouldNeverHappen;
}


std::pair<APIResult, std::vector<UCHAR>> EncryptFrameData(
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
				ErrorCode code = ErrorCode::Success;
				UCHAR index = FindPaletteIndex(
					frame.DecodedFrameData[i],
					frame.DecodedFrameData[i + 1],
					frame.DecodedFrameData[i + 2],
					palette,
					paletteSize,
					code);

				if (code != ErrorCode::Success) {
					// Tạo thông báo lỗi
					std::string errorMessage = "Failed to find palette index for pixel at position: " + std::to_string(i);
					// Trả về sớm với mã lỗi và dữ liệu hiện tại
					return { APIResult(code, errorMessage.c_str()), encryptedFrameData };
				}

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
	// Trả về thành công
	return { APIResult(ErrorCode::Success, "Operation completed successfully."), encryptedFrameData };
}

APIResult ExportToSPRFileInternal(const char* filePath,
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

			auto encryptRes = EncryptFrameData(
				palette,
				paletteSize,
				frame[i]
			);
			if (encryptRes.first.errorCode != ErrorCode::Success) {
				return encryptRes.first;
			}
			encryptedFramesData.push_back(encryptRes.second);
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
		return APIResult();
	}
	else {
		std::string message = "Failed to open file: " + std::string(filePath);
		return APIResult(ErrorCode::InternalError, message.c_str());
	}
}

APIResult LoadSPRMemoryInternal(
	const uint8_t* data,           // Mảng byte chứa dữ liệu SPR
	size_t dataLength,             // Độ dài của mảng byte
	SPRFileHead* fileHead,         // Con trỏ đến cấu trúc SPRFileHead để lưu thông tin
	Color** palette,               // Con trỏ đến bảng màu sẽ được khởi tạo và lưu trữ
	int* paletteLength,            // Con trỏ đến độ dài bảng màu
	int* frameDataBeginPos,        // Con trỏ đến vị trí bắt đầu dữ liệu frame
	FrameData** frame,             // Con trỏ đến mảng chứa dữ liệu frame sẽ được khởi tạo
	int* frameCount				// Con trỏ đến số lượng khung hình
) {
	Log::I("SPR", "Start LoadSPRMemoryInternal");

	// Kiểm tra dữ liệu truyền vào để đảm bảo không có giá trị null
	if (!data || !palette || !paletteLength || !frameDataBeginPos || !frame || !frameCount) {
		Log::E("Invalid arguments passed to LoadSPRMemoryInternal.");
		return APIResult(ErrorCode::InvalidArgument, "Invalid arguments passed to LoadSPRMemoryInternal.");
	}

	// Đọc thông tin file header từ bộ nhớ
	std::memcpy(fileHead, data, sizeof(SPRFileHead));

	// Khởi tạo bảng màu
	*palette = MemoryManager::getInstance()->allocateArray<Color>(fileHead->ColorCounts);
	std::memset(*palette, 0, sizeof(Color) * fileHead->ColorCounts);
	size_t paletteOffset = sizeof(SPRFileHead);
	for (int i = 0; i < fileHead->ColorCounts; i++) {
		(*palette)[i].R = data[paletteOffset + i * 3];
		(*palette)[i].G = data[paletteOffset + i * 3 + 1];
		(*palette)[i].B = data[paletteOffset + i * 3 + 2];
	}
	*paletteLength = fileHead->ColorCounts;
	*frameDataBeginPos = paletteOffset + fileHead->ColorCounts * sizeof(Color);

	// Khởi tạo và đọc dữ liệu frame
	*frame = MemoryManager::getInstance()->allocateArray<FrameData>(fileHead->FrameCounts);
	*frameCount = fileHead->FrameCounts;
	for (int frameIndex = 0; frameIndex < fileHead->FrameCounts; frameIndex++) {
		// Đọc thông tin offset cho từng frame
		FrameOffsetInfo offsetInfo;
		size_t offsetPos = *frameDataBeginPos + sizeof(FrameOffsetInfo) * frameIndex;
		std::memcpy(&offsetInfo, data + offsetPos, sizeof(FrameOffsetInfo));

		unsigned int frameBeginPos = *frameDataBeginPos +
			sizeof(FrameOffsetInfo) * fileHead->FrameCounts +
			offsetInfo.FrameOffset;
		unsigned int dataLength = offsetInfo.DataLength;
		(*frame)[frameIndex].EncryptedFrameDataOffset = offsetInfo.FrameOffset;
		(*frame)[frameIndex].EncryptedLength = offsetInfo.DataLength;

		if (dataLength == 0) {
			continue;
		}

		// Đọc thông tin frame
		std::memcpy(&(*frame)[frameIndex].FrameInfo, data + frameBeginPos, sizeof(FrameInfo));
		long decDataLength = (*frame)[frameIndex].FrameInfo.Height * (*frame)[frameIndex].FrameInfo.Width;
		if (decDataLength == 0) {
			continue;
		}
		(*frame)[frameIndex].initMemory(decDataLength * 4);

		// Đọc dữ liệu giải mã cho frame
		long frameDataPos = frameBeginPos + sizeof(FrameInfo);
		long curDecPos = 0;
		for (int i = 0; i < dataLength - 8;) {
			if (curDecPos > decDataLength) {
				Log::E("Failed to decrypt, something's wrong!");
				return APIResult(ErrorCode::InternalError, "Failed to decrypt, something's wrong!");
			}
			int size = data[frameDataPos + i];
			int alpha = data[frameDataPos + i + 1];

			if (size == -1 || alpha == -1) {
				Log::E("Failed to decrypt, something's wrong!");
				return APIResult(ErrorCode::InternalError, "Failed to decrypt, something's wrong!");
			}

			if (alpha == 0x00) {
				for (int j = 0; j < size; j++) {
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4] = 0;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 1] = 0;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 2] = 0;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 3] = 0;
					(*frame)[frameIndex].ColorMap[curDecPos] = -1;
					curDecPos++;
				}
			}
			else {
				for (int j = 0; j < size; j++) {
					int colorIndex = data[frameDataPos + i + 2 + j];
					if (colorIndex == -1) {
						Log::E("Failed to decrypt, colorIndex must be greater than -1!");
						return APIResult(ErrorCode::InternalError, "Failed to decrypt, colorIndex must be greater than -1!");
					}

					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4] = (*palette)[colorIndex].B;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 1] = (*palette)[colorIndex].G;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 2] = (*palette)[colorIndex].R;
					(*frame)[frameIndex].DecodedFrameData[curDecPos * 4 + 3] = alpha;
					(*frame)[frameIndex].ColorMap[curDecPos] = colorIndex;
					curDecPos++;
				}
			}
			i += 2 + size;
		}
	}

	return APIResult();
}

APIResult LoadSPRFileInternal(const char* filePath,
	SPRFileHead* fileHead,
	Color** palette,
	int* paletteLength,
	int* frameDataBeginPos,
	FrameData** frame,
	int* frameCount)
{
	Log::I("SPR", "Start LoadSPRFileInternal");

	// Mở file và kiểm tra xem có mở thành công không
	std::ifstream file(filePath, std::ios::binary | std::ios::ate);
	if (!file.is_open()) {
		Log::E("LoadSPRFileInternal: Failed to open file");
		*palette = nullptr;
		return APIResult(ErrorCode::InternalError, "LoadSPRFileInternal: Failed to open file");
	}
	std::streamsize fileSize = GetFileStreamSize(file);
	char* fileData = GetFileStreamBuffer(file);
	file.close();

	// Gọi hàm LoadSPRMemoryInternal để xử lý dữ liệu từ mảng byte trong bộ nhớ
	APIResult result = LoadSPRMemoryInternal(
		reinterpret_cast<uint8_t*>(fileData),
		fileSize,
		fileHead,
		palette,
		paletteLength,
		frameDataBeginPos,
		frame,
		frameCount
	);

	MemoryManager::getInstance()->deallocate(fileData);
	return result;
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
