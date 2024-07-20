#pragma once

#include <comdef.h>
#include <iostream>
#include <fstream>
#include <vector>
#include "spr.h"

#ifdef ENGINE_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

extern "C" {
	DLL_API void LoadSPRFile(const char* filePath,
		SPRFileHead* fileHead,
		Color** palette,
		int* paletteLength,
		int* frameDataBeginPos,
		FrameData** frame,
		int* frameCount);

	DLL_API void ExportToSPRFile(const char* filePath,
		SPRFileHead fileHead,
		Color palette[],
		int paletteSize,
		FrameData frame[]);

	DLL_API void ExtractPakFile(const char* pakFilePath,
		const char* pakInfoPath,
		const char* outputRootPath);

	DLL_API void CompressFolderToPakFile(const char* folderPath,
		const char* outputPath,
		bool bExcludeOfCheckId);

	DLL_API void FreeArrData(unsigned char* data);
}