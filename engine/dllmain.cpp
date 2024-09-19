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
		wchar_t path[MAX_PATH];
		if (GetModuleFileName(NULL, path, MAX_PATH) != 0)
		{

			CertInfo* certInfo = new CertInfo;
			char* pathChar = Wchar_t2CharPtr(path);
			if (GetCertificateInfo(pathChar, certInfo) == 0) {
				ForceCheckCertPermissionInternal(*certInfo);
				delete certInfo;
				delete pathChar;
			}
			else {
				delete pathChar;
				throw std::exception("Security exception:Calling package's cert is invalid!");
			}
		}
		else
		{
			throw std::exception("Security exception:Calling package's cert is invalid!");
		}
		break;
	}
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

void FreeArrData(unsigned char* data) {
	delete[] data;
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


void ExtractPakFile(const char* pakFilePath, const char* pakInfoFilePath, const char* outputRootPath) {
	PakInfoInternal pakInfo;
	int p = ParsePakInfoFileInternal(pakInfoFilePath, pakInfo);
	std::unique_ptr<PakHeader> header;
	int res = LoadPakInternal(pakFilePath, outputRootPath, pakInfo, header);
}

void CompressFolderToPakFile(const char* inputFolderPath, const char* outputFolderPath, bool bExcludeOfCheckId) {
	CompressFolderToPakFileInternal(inputFolderPath, outputFolderPath, bExcludeOfCheckId);
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
	}
}
