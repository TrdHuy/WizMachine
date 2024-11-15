// dllmain.cpp : Defines the entry point for the DLL application.

#include "pch.h"
#include <atomic>
#pragma comment (lib, "crypt32.lib")
#pragma comment (lib, "wintrust.lib")

static std::atomic<bool> isInitialized = false;

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	{

		break;
	}
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH: {
		Log::EnableDirectMode(true);
		MemoryManager::getInstance()->deallocateAll();
		delete MemoryManager::getInstance();

		Log::EnableDirectMode(false);
		Log::Close();
		break;
	}
	}
	return TRUE;
}

APIResult Initialize() {
	Log::Init();

	MemoryManager::getInstance();
	wchar_t path[MAX_PATH];

	if (GetModuleFileName(NULL, path, MAX_PATH) != 0)
	{
		char* pathChar = Wchar_t2CharPtr(path);
		Log::I("MAIN", "Start verify cert for path: " + std::string(pathChar));
		MemoryManager::getInstance()->deallocate(pathChar);
	}
	CertManager::getInstance().initialize(path);

	isInitialized = true;
	return APIResult();
}

APIResult LoadSPRFile(const char* filePath,
	SPRFileHead* fileHead,
	Color** palette,
	int* paletteLength,
	int* frameDataBeginPos,
	FrameData** frame,
	int* frameCount) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() == CertCheckResult::Success) {
		return LoadSPRFileInternal(filePath,
			fileHead,
			palette,
			paletteLength,
			frameDataBeginPos,
			frame,
			frameCount);
	}
	else {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
}

APIResult LoadSPRFileForTestOnly(const char* filePath,
	SPRFileHead* fileHead,
	Color** palette,
	int* paletteLength,
	int* frameDataBeginPos,
	FrameData** frame,
	int* frameCount) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() == CertCheckResult::Success) {
		return LoadSPRFileForTestOnlyInternal(filePath,
			fileHead,
			palette,
			paletteLength,
			frameDataBeginPos,
			frame,
			frameCount);
	}
	else {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
}

APIResult LoadSPRMemory(
	const uint8_t* data,           // Mảng byte chứa dữ liệu SPR
	size_t dataLength,             // Độ dài của mảng byte
	SPRFileHead* fileHead,         // Con trỏ đến cấu trúc SPRFileHead để lưu thông tin
	Color** palette,               // Con trỏ đến bảng màu sẽ được khởi tạo và lưu trữ
	int* paletteLength,            // Con trỏ đến độ dài bảng màu
	int* frameDataBeginPos,        // Con trỏ đến vị trí bắt đầu dữ liệu frame
	FrameData** frame,             // Con trỏ đến mảng chứa dữ liệu frame sẽ được khởi tạo
	int* frameCount                // Con trỏ đến số lượng khung hình
) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() == CertCheckResult::Success) {
		return LoadSPRMemoryInternal(data,
			dataLength,
			fileHead,
			palette,
			paletteLength,
			frameDataBeginPos,
			frame,
			frameCount);
	}
	else {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
}

APIResult FreeSPRMemory(
	Color* palette,
	FrameData* frameData, int frameCount) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	MemoryManager::getInstance()->deallocate(palette);
	palette = nullptr;
	for (int i = 0; i < frameCount; i++) {
		frameData[i].free();
	}
	MemoryManager::getInstance()->deallocate(frameData);
	frameData = nullptr;
	return APIResult();
}

APIResult ExportToSPRFile(const char* filePath,
	SPRFileHead fileHead,
	Color palette[],
	int paletteSize,
	FrameData frame[]) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	return ExportToSPRFileInternal(filePath,
		fileHead,
		palette,
		paletteSize,
		frame);
}

APIResult ParsePakInfoFile(const char* pakInfoPath,
	PakInfo* pakInfo) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	PakInfoInternal pakInfoInternal;
	int result = ParsePakInfoFileInternal(pakInfoPath, pakInfoInternal);

	if (result != 0) {
		return APIResult(ErrorCode::InternalError, "Failed to parse pak info file!");
	}
	pakInfoInternal.ConvertToPakInfo(*pakInfo);
	return APIResult();
}

APIResult FreePakInfo(PakInfo* pakInfo) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	if (pakInfo) {
		pakInfo->freeMem();
	}
	return APIResult();
}

APIResult ExtractPakFile(const char* pakFilePath,
	const char* pakInfoFilePath,
	const char* outputRootPath,
	ProgressCallback progressCallback) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	std::unique_ptr<PakHeader> header;
	if (pakInfoFilePath != nullptr) {
		PakInfoInternal pakInfo;
		if (progressCallback != nullptr)
			progressCallback(0, "Parsing pak info...");

		if (ParsePakInfoFileInternal(pakInfoFilePath, pakInfo) == -1) {
			return APIResult(ErrorCode::InternalError, "Failed to parse pak file!");
		}
		double progress = 10;


		return ExtractPakInternal(pakFilePath,
			outputRootPath,
			pakInfo,
			header,
			[progressCallback, &progress](int p, const char* m) {
				progress += p * 0.9;
				progressCallback(progress, m);
			});
	}
	else {
		return ExtractPakInternal(pakFilePath,
			outputRootPath,
			header,
			[progressCallback](int p, const char* m) {
				progressCallback(p, m);
			});
	}
}

APIResult CompressFolderToPakFile(const char* inputFolderPath, const char* outputFolderPath, bool bExcludeOfCheckId, ProgressCallback progressCallback) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	return CompressFolderToPakFileInternal(inputFolderPath, outputFolderPath, bExcludeOfCheckId, [progressCallback](int p, const char* m) {
		if (progressCallback != nullptr)
			progressCallback(p, m);
		});
}

APIResult ForceCheckCertPermission(CertInfo certinfo) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	if (ForceCheckCertPermissionInternal(certinfo)) {
		return APIResult();
	}
	return APIResult(ErrorCode::SecurityError, "Cert is invalid!");
}

int GetCertificateInfo(const char* filePath, CertInfo* certInfo) {

	return GetCertificateInfoInternal(filePath, certInfo);
}

// TODO: Gộp GetCertificateInfo -> GetCertificateInfo2
APIResult GetCertificateInfo2(const char* filePath, CertInfo* certInfo) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	return GetCertificateInfoInternal2(filePath, certInfo);
}

APIResult FreeCertInfo(CertInfo* certInfo) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	if (certInfo) {
		free((void*)certInfo->Subject);
		free((void*)certInfo->Issuer);
		free((void*)certInfo->Thumbprint);
		free((void*)certInfo->SerialNumber);
		certInfo->Subject = nullptr;
		certInfo->Issuer = nullptr;
		certInfo->Thumbprint = nullptr;
		certInfo->SerialNumber = nullptr;
	}
	return APIResult();
}

const APIResult LoadPakFileToWorkManager(const char* filePath,
	PakInfo* pakInfo,
	ProgressCallback progressCallback,
	char** sessionTokenOut) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	// Lấy instance duy nhất của PakWorkManager
	PakWorkManager* manager = PakWorkManager::GetInstance();
	PakInfoInternal pakInfoInternal;

	// Gọi hàm LoadPakFile nội bộ và trả về token phiên
	std::string sessionToken = manager->LoadPakFile(filePath, pakInfoInternal, progressCallback);

	// Nếu không tạo được token, trả về null
	if (sessionToken.empty()) {
		Log::E("MAIN", "LoadPakFileToWorkManager: Failed to create a session!");
		return APIResult(ErrorCode::InternalError, "LoadPakFileToWorkManager: Failed to create a session!");
	}

	// Lấy instance của MemoryManager để cấp phát bộ nhớ cho chuỗi kết quả
	MemoryManager* memManager = MemoryManager::getInstance();
	*sessionTokenOut = memManager->allocateArray<char>(sessionToken.size() + 1);
	strcpy_s(*sessionTokenOut, sessionToken.size() + 1, sessionToken.c_str());

	pakInfoInternal.ConvertToPakInfo(*pakInfo);
	return APIResult();
}

APIResult ClosePakFileSession(const char* sessionString) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	PakWorkManager* manager = PakWorkManager::GetInstance();
	return manager->CloseSession(sessionString);
}

APIResult ExtractBlockFromPakFile(const char* sessionString, int subFileIndex, const char* outputPath) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	PakWorkManager* manager = PakWorkManager::GetInstance();
	return manager->ExtractSubFile(sessionString, subFileIndex, outputPath);
}

APIResult FreeBuffer(void* buffer) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	MemoryManager* memManager = MemoryManager::getInstance();
	memManager->deallocate(buffer);
	return APIResult();
}

APIResult ReadBlockFromPakFile(const char* sessionToken,
	int subFileIndex,
	size_t* subFileSize,
	char** blockData) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	unsigned char* buffer = nullptr;
	PakWorkManager* manager = PakWorkManager::GetInstance();
	auto result = manager->ReadSubFileData(sessionToken, subFileIndex, buffer, subFileSize);
	*blockData = reinterpret_cast<char*>(buffer);
	if (result.errorCode != ErrorCode::Success && buffer != nullptr) {
		MemoryManager::getInstance()->deallocate(buffer);
	}
	return result;
}

APIResult GetBlockIdFromPath(const char* blockPath, unsigned int* blockId) {
	if (!isInitialized) {
		return APIResult(ErrorCode::SecurityError, "DLL has not been initialized.");
	}
	if (CertManager::getInstance().checkCertificate() != CertCheckResult::Success) {
		return APIResult(ErrorCode::SecurityError, "Caller is not allowed!");
	}
	*blockId = g_FileNameHash(blockPath);
	return APIResult();
}
