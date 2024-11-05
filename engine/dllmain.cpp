// dllmain.cpp : Defines the entry point for the DLL application.

#include "pch.h"

#pragma comment (lib, "crypt32.lib")
#pragma comment (lib, "wintrust.lib")

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	{
		Log::Init();


		MemoryManager::getInstance();
		wchar_t path[MAX_PATH];
		if (GetModuleFileName(NULL, path, MAX_PATH) != 0)
		{
			CertInfo* certInfo = MemoryManager::getInstance()->allocate<CertInfo>();
			char* pathChar = Wchar_t2CharPtr(path);
			Log::I("MAIN", "Start verify cert for path: " + std::string(pathChar));
			if (GetCertificateInfo(pathChar, certInfo) == 0) {
				ForceCheckCertPermissionInternal(*certInfo);
				MemoryManager::getInstance()->deallocate(static_cast<char*>(pathChar));
				MemoryManager::getInstance()->deallocate(certInfo);
			}
			else {
				MemoryManager::getInstance()->deallocate(static_cast<char*>(pathChar));
				MemoryManager::getInstance()->deallocate(certInfo);


				Log::E("MAIN", "Security exception: Path" + std::string(pathChar) + " cert is invalid! Package: ");
				throw std::exception("Security exception:Calling package's cert is invalid!");
			}
		}
		else
		{
			Log::E("MAIN", "Failed to get module file!");
			throw std::exception("Security exception:Calling package's cert is invalid!");
		}
		break;
	}
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH: {
		delete MemoryManager::getInstance();
		Log::Close();
		break;
	}
	}
	return TRUE;
}

void LoadSPRFile(const char* filePath,
	SPRFileHead* fileHead,
	Color** palette,
	int* paletteLength,
	int* frameDataBeginPos,
	FrameData** frame,
	int* frameCount) {
	LoadSPRFileInternal(filePath,
		fileHead,
		palette,
		paletteLength,
		frameDataBeginPos,
		frame,
		frameCount);
}

void LoadSPRMemory(
	const uint8_t* data,           // Mảng byte chứa dữ liệu SPR
	size_t dataLength,             // Độ dài của mảng byte
	SPRFileHead* fileHead,         // Con trỏ đến cấu trúc SPRFileHead để lưu thông tin
	Color** palette,               // Con trỏ đến bảng màu sẽ được khởi tạo và lưu trữ
	int* paletteLength,            // Con trỏ đến độ dài bảng màu
	int* frameDataBeginPos,        // Con trỏ đến vị trí bắt đầu dữ liệu frame
	FrameData** frame,             // Con trỏ đến mảng chứa dữ liệu frame sẽ được khởi tạo
	int* frameCount                // Con trỏ đến số lượng khung hình
) {
	LoadSPRMemoryInternal(data,
		dataLength,
		fileHead,
		palette,
		paletteLength,
		frameDataBeginPos,
		frame,
		frameCount);
}

void FreeSPRMemory(
	Color* palette,
	FrameData* frameData, int frameCount) {
	MemoryManager::getInstance()->deallocate(palette);
	palette = nullptr;
	for (int i = 0; i < frameCount; i++) {
		frameData[i].free();
	}
	MemoryManager::getInstance()->deallocate(frameData);
	frameData = nullptr;
}

void ExportToSPRFile(const char* filePath,
	SPRFileHead fileHead,
	Color palette[],
	int paletteSize,
	FrameData frame[]) {
	ExportToSPRFileInternal(filePath,
		fileHead,
		palette,
		paletteSize,
		frame);
}

void ParsePakInfoFile(const char* pakInfoPath,
	PakInfo* pakInfo) {
	PakInfoInternal pakInfoInternal;
	int result = ParsePakInfoFileInternal(pakInfoPath, pakInfoInternal);

	if (result != 0) {
		throw std::exception("Failed to parse pak info file!");
	}
	pakInfoInternal.ConvertToPakInfo(*pakInfo);
}

void FreePakInfo(PakInfo* pakInfo) {
	if (pakInfo) {
		pakInfo->freeMem();
	}
}

bool ExtractPakFile(const char* pakFilePath,
	const char* pakInfoFilePath,
	const char* outputRootPath,
	ProgressCallback progressCallback) {
	std::unique_ptr<PakHeader> header;
	if (pakInfoFilePath != nullptr) {
		PakInfoInternal pakInfo;
		if (progressCallback != nullptr)
			progressCallback(0, "Parsing pak info...");

		if (ParsePakInfoFileInternal(pakInfoFilePath, pakInfo) == -1) {
			return false;
		}
		double progress = 10;


		return ExtractPakInternal(pakFilePath,
			outputRootPath,
			pakInfo,
			header,
			[progressCallback, &progress](int p, const char* m) {
				progress += p * 0.9;
				progressCallback(progress, m);
			}) > 0;
	}
	else {
		return ExtractPakInternal(pakFilePath,
			outputRootPath,
			header,
			[progressCallback](int p, const char* m) {
				progressCallback(p, m);
			}) > 0;
	}
}

void CompressFolderToPakFile(const char* inputFolderPath, const char* outputFolderPath, bool bExcludeOfCheckId, ProgressCallback progressCallback) {
	CompressFolderToPakFileInternal(inputFolderPath, outputFolderPath, bExcludeOfCheckId, [progressCallback](int p, const char* m) {
		if(progressCallback != nullptr)
			progressCallback(p, m);
		});
}

void ForceCheckCertPermission(CertInfo certinfo) {
	ForceCheckCertPermissionInternal(certinfo);
}

int GetCertificateInfo(const char* filePath, CertInfo* certInfo) {
	return GetCertificateInfoInternal(filePath, certInfo);
}

void FreeCertInfo(CertInfo* certInfo) {
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
}

const char* LoadPakFileToWorkManager(const char* filePath, PakInfo* pakInfo, ProgressCallback progressCallback) {
	// Lấy instance duy nhất của PakWorkManager
	PakWorkManager* manager = PakWorkManager::GetInstance();
	PakInfoInternal pakInfoInternal;

	// Gọi hàm LoadPakFile nội bộ và trả về token phiên
	std::string sessionToken = manager->LoadPakFile(filePath, pakInfoInternal, progressCallback);

	// Nếu không tạo được token, trả về null
	if (sessionToken.empty()) {
		return nullptr;
	}

	// Lấy instance của MemoryManager để cấp phát bộ nhớ cho chuỗi kết quả
	MemoryManager* memManager = MemoryManager::getInstance();
	char* result = memManager->allocateArray<char>(sessionToken.size() + 1);
	strcpy_s(result, sessionToken.size() + 1, sessionToken.c_str());

	pakInfoInternal.ConvertToPakInfo(*pakInfo);
	return result;
}

void ClosePakFileSession(const char* sessionString) {
	PakWorkManager* manager = PakWorkManager::GetInstance();
	manager->CloseSession(sessionString);
}

bool ExtractBlockFromPakFile(const char* sessionString, int subFileIndex, const char* outputPath) {
	PakWorkManager* manager = PakWorkManager::GetInstance();
	return manager->ExtractSubFile(sessionString, subFileIndex, outputPath);
}

bool FreeBuffer(void* buffer) {
	MemoryManager* memManager = MemoryManager::getInstance();
	return memManager->deallocate(buffer);
}

unsigned char* ReadBlockFromPakFile(const char* sessionToken, int subFileIndex, size_t* subFileSize) {
	unsigned char* buffer = nullptr;
	PakWorkManager* manager = PakWorkManager::GetInstance();
	manager->ReadSubFileData(sessionToken, subFileIndex, buffer, subFileSize);
	return buffer;
}

unsigned int GetBlockIdFromPath(const char* blockPath) {
	return g_FileNameHash(blockPath);
}