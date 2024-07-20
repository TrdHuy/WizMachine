// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
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

void ExtractPakFile(const char* pakFilePath, const char* pakInfoFilePath, const char* outputRootPath) {
	PakInfo pakInfo;
	int p = ParsePakInfoFile(pakInfoFilePath, pakInfo);
	std::unique_ptr<PakHeader> header;
	int res = LoadPakInternal(pakFilePath, outputRootPath, pakInfo, header);
}

void CompressFolderToPakFile(const char* inputFolderPath, const char* outputFolderPath, bool bExcludeOfCheckId) {
	CompressFolderToPakFileInternal(inputFolderPath, outputFolderPath, bExcludeOfCheckId);
}

