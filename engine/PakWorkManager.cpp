#include "pch.h"

#include "PakWorkManager.h"

const std::string PakWorkManager::TAG = "PakWorkManager";

// Khởi tạo instance thành nullptr ban đầu
PakWorkManager* PakWorkManager::instance = nullptr;

// Hàm trả về instance duy nhất của PakWorkManager
PakWorkManager* PakWorkManager::GetInstance() {
	if (!instance) {
		instance = new PakWorkManager();
	}
	return instance;
}

std::string PakWorkManager::GenerateSessionToken() {
	static const char chars[] =
		"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	std::random_device rd;
	std::mt19937 generator(rd());
	std::uniform_int_distribution<> distribution(0, sizeof(chars) - 2);

	std::string token;
	for (int i = 0; i < 16; ++i) {
		token += chars[distribution(generator)];
	}
	return token;
}

bool PakWorkManager::ValidateSessionToken(const std::string& sessionToken, const SessionInfo& sessionInfo) {
	auto tempFile = sessionInfo.tempFile;
	if (!tempFile->isOpen()) {
		Log::E(TAG, "Failed to open temporary file for validation: ", std::string(tempFile->getFullPath()));
		return false;
	}

	char tokenBuffer[16];
	tempFile->readFrom(0, tokenBuffer, sessionToken.size());

	return sessionToken == std::string(tokenBuffer, sessionToken.size());
}

void PakWorkManager::CloseSession(const std::string& sessionToken) {
	auto sessionIt = sessionFiles.find(sessionToken);
	if (sessionIt == sessionFiles.end()) {
		Log::E(TAG, "Invalid session token: ", sessionToken);
		throw std::runtime_error("Invalid session token!");
	}
	auto tempFile = sessionIt->second.tempFile;
	const std::string& tempFilePath = tempFile->getFullPath();
	tempFile->release();
	if (std::remove(tempFilePath.c_str()) != 0) {
		Log::E(TAG, "Failed to delete temporary file:  ", tempFilePath);
	}
	else {
		Log::I(TAG, "Temporary file deleted:  ", tempFilePath);
	}

	sessionFiles.erase(sessionIt);
}

// Cập nhật hàm LoadPakFile để tạo 1 file tạm duy nhất
std::string PakWorkManager::LoadPakFile(const std::string& filePath,
	PakInfoInternal& pakInfoInternal, ProgressCallback progressCallback) {
	double progress = 0;

	if(progressCallback != nullptr)
		progressCallback(progress, "Init session");

	// Khởi tạo AloneFile để mở file .pak
	BigAloneFile pakFile;
	if (!pakFile.open(filePath.c_str(), false, false)) {
		Log::E(TAG, "LoadPakFile: Failed to open .pak file: ", filePath);
		return "";
	}

	// Tạo token phiên
	std::string sessionToken = GenerateSessionToken();

	// Tạo header
	std::unique_ptr<PakHeader> header;
	ReadPakHeader(header, pakFile); // Giữ nguyên hàm ReadPakHeader

	if (!header) {
		Log::E(TAG, "LoadPakFile: Failed to read pak header.");
		return "";
	}

	// Kiểm tra số lượng block trong header
	int blockCount = header->Count;
	if (blockCount <= 0) {
		Log::E(TAG, "LoadPakFile: Invalid block count in pak file. ");
		return "";
	}

	// Tạo đường dẫn file tạm cho phiên làm việc
	std::string tempFilePath = GetTempFilePath("temp_" + sessionToken + ".tmp", true);

	BigAloneFile* tempFile = MemoryManager::getInstance()->allocate<BigAloneFile>();
	if (!tempFile->open(tempFilePath.c_str(), true, false)) {
		Log::E(TAG, "LoadPakFile: Failed to create temporary file: ", tempFilePath);
		return "";
	}

	// Ghi token phiên vào đầu file tạm
	tempFile->write(sessionToken.c_str(), sessionToken.size());

	// Lưu thông tin phiên làm việc
	SessionInfo sessionInfo = { tempFile, {} };
	std::string pakTime = formatTimeToString(header->PakTime);
	pakInfoInternal.totalFiles = blockCount;
	pakInfoInternal.pakTimeSave = intToHexString(header->PakTime);
	pakInfoInternal.pakTime = formatTimeToString(header->PakTime);
	pakInfoInternal.crc = intToHexString(header->CRC32);

	std::this_thread::sleep_for(std::chrono::seconds(2));
	progress = 2;
	if (progressCallback != nullptr)
		progressCallback(progress, "Loading block...");

	double progressLeft = 100 - progress;
	double progressEachBlockCount = progressLeft / blockCount;
	// Vòng lặp đọc từng block và ghi vào file tạm
	for (int block = 0; block < blockCount; ++block) {
		int decompressLength;
		PakBlockHeader blockHeader;
		XPackIndexInfo packHeader;

		// Lưu vị trí offset của block trong file tạm
		std::streampos blockOffset = tempFile->tell(); // Sử dụng AloneFile
		sessionInfo.blockOffsets.push_back(blockOffset);

		// Đọc block từ file .pak
		auto extractedBuffer = ReadBlock(block, blockCount, header->Index, pakFile, &decompressLength,
			[&blockHeader, &packHeader](PakBlockHeader header, XPackIndexInfo xPackHeader) {
				blockHeader = header;
				packHeader = xPackHeader;
				return true;
			});

		// Nếu đọc thành công block
		if (extractedBuffer) {
			// Ghi dữ liệu block vào file tạm
			tempFile->write(reinterpret_cast<const char*>(extractedBuffer.get()), decompressLength);
			CompressedFileInfoInternal compressFileInfo;

			compressFileInfo.index = block;
			compressFileInfo.id = intToHexString(blockHeader.ID);
			compressFileInfo.convertIdToValue();
			compressFileInfo.comprFlag = packHeader.getPackMethod();
			compressFileInfo.inPakSize = packHeader.getStoredSize();
			compressFileInfo.size = decompressLength;
			compressFileInfo.time = pakTime;
			compressFileInfo.fileName = "block_" + std::to_string(block);

			pakInfoInternal.addFile(compressFileInfo);
			Log::D(TAG, "LoadPakFile: Block ", block, " saved to temporary file at offset: ", blockOffset);
			progress += progressEachBlockCount;
			if (progressCallback != nullptr)
				progressCallback(progress, "Loaded block");
		}
	}

	// Đóng file tạm và file .pak
	pakFile.close(); // Đảm bảo đóng file

	// Lưu thông tin phiên vào sessionFiles
	sessionFiles[sessionToken] = sessionInfo;

	Log::I(TAG, "LoadPakFile: Session created with token: ", sessionToken);
	progress = 100;

	if (progressCallback != nullptr)
		progressCallback(progress, "Done");
	return sessionToken;
}

std::pair<std::streampos, std::streampos> PakWorkManager::GetSubFilePosition(
	const SessionInfo& sessionInfo, int subFileIndex) {

	// Kiểm tra xem subFileIndex có hợp lệ không
	if (subFileIndex < 0 || subFileIndex >= sessionInfo.blockOffsets.size()) {
		throw std::out_of_range("subFileIndex is out of range");
	}

	// Vị trí bắt đầu của sub file
	std::streampos start = sessionInfo.blockOffsets[subFileIndex];

	// Tính toán vị trí kết thúc dựa trên kích thước của tempFile
	std::streampos end;
	if (subFileIndex + 1 < sessionInfo.blockOffsets.size()) {
		end = sessionInfo.blockOffsets[subFileIndex + 1]; // Vị trí bắt đầu của block tiếp theo
	}
	else {
		// Nếu subFileIndex là phần tử cuối cùng, lấy kích thước của tempFile
		end = sessionInfo.tempFile->size(); // Lấy kích thước của tempFile để làm vị trí kết thúc
	}

	return { start, end }; // Trả về cặp vị trí bắt đầu và kết thúc
}


bool PakWorkManager::ReadSubFileData(const std::string& sessionToken, int subFileIndex, unsigned char*& buffer, size_t* subFileSize) {
	auto sessionIt = sessionFiles.find(sessionToken);
	if (sessionIt == sessionFiles.end()) {
		Log::E(TAG, "Invalid session token: ", sessionToken);
		throw std::runtime_error("Invalid session token!");
	}

	auto tempFile = sessionIt->second.tempFile;
	auto tempFilePath = std::string(tempFile->getFullPath());

	if (!tempFile->isOpen()) {
		Log::E(TAG, "Failed to open temporary file: ", tempFilePath);
		throw std::runtime_error("Failed to open temporary file!");
	}

	auto subFilePos = GetSubFilePosition(sessionIt->second, subFileIndex);
	if (subFilePos.first == -1) {
		Log::E(TAG, "Invalid sub-file index: ", subFileIndex);
		throw std::runtime_error("Invalid sub-file index!");
	}

	// Tính toán kích thước của sub-file
	*subFileSize = subFilePos.second - subFilePos.first;
	buffer = MemoryManager::getInstance()->allocateArray<unsigned char>(*subFileSize);
	// Đảm bảo buffer đủ lớn để chứa dữ liệu
	if (buffer == nullptr) {
		Log::E(TAG, "Buffer is null");
		throw std::runtime_error("Buffer is null!");
	}

	// Đọc dữ liệu từ file tạm
	tempFile->readFrom(subFilePos.first, buffer, *subFileSize);

	return true; // Trả về true nếu đọc thành công
}


//Triển khai hàm ExtractSubFile: Trích xuất file con ra file output
bool PakWorkManager::ExtractSubFile(const std::string& sessionToken, int subFileIndex, const std::string& outputPath) {
	size_t subFileSize;
	unsigned char* buffer = nullptr; // Khai báo con trỏ buffer
	ReadSubFileData(sessionToken, subFileIndex, buffer, &subFileSize);

	MakeDirFromFilePathIfNotExisted(outputPath);

	std::ofstream outputFile(outputPath, std::ios::binary);
	if (!outputFile.is_open()) {
		Log::E(TAG, "Failed to create output file: ", outputPath);
		throw std::runtime_error("Failed to create output file!");
	}

	if (buffer == nullptr) {
		Log::E(TAG, "Buffer is null");
		throw std::runtime_error("Buffer is null!");
	}

	outputFile.write(reinterpret_cast<const char*>(buffer), subFileSize);
	outputFile.close();
	MemoryManager::getInstance()->deallocate(buffer);
	return true;
}

