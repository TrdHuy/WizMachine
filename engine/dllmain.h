#pragma once

#include <comdef.h>
#include <iostream>
#include <fstream>
#include <vector>
#include "base.h"
#include "cert.h"
#include "PakWorkManager.h"

#ifdef ENGINE_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif


extern "C" {
	/*DLL_API void LoadSPRFile(const char* filePath,
		SPRFileHead* fileHead,
		Color** palette,
		int* paletteLength,
		int* frameDataBeginPos,
		FrameData** frame,
		int* frameCount);*/

	DLL_API APIResult LoadSPRFile(const char* filePath,
		SPRFileHead* fileHead,
		Color** palette,
		int* paletteLength,
		int* frameDataBeginPos,
		FrameData** frameData,
		int* frameCount);
	DLL_API APIResult LoadSPRMemory(
		const uint8_t* data,           // Mảng byte chứa dữ liệu SPR
		size_t dataLength,             // Độ dài của mảng byte
		SPRFileHead* fileHead,         // Con trỏ đến cấu trúc SPRFileHead để lưu thông tin
		Color** palette,               // Con trỏ đến bảng màu sẽ được khởi tạo và lưu trữ
		int* paletteLength,            // Con trỏ đến độ dài bảng màu
		int* frameDataBeginPos,        // Con trỏ đến vị trí bắt đầu dữ liệu frame
		FrameData** frame,             // Con trỏ đến mảng chứa dữ liệu frame sẽ được khởi tạo
		int* frameCount                // Con trỏ đến số lượng khung hình
	);

	DLL_API APIResult FreeSPRMemory(
		Color* palette,
		FrameData* frame, int frameCount);
	DLL_API APIResult ExportToSPRFile(const char* filePath,
		SPRFileHead fileHead,
		Color palette[],
		int paletteSize,
		FrameData frame[]);

	DLL_API APIResult	ForceCheckCertPermission(CertInfo certinfo);
	DLL_API APIResult	GetCertificateInfo(const char* filePath, CertInfo* certInfo);
	DLL_API APIResult	FreeCertInfo(CertInfo* certInfo);

	DLL_API APIResult	CompressFolderToPakFile(const char* folderPath,
		const char* outputPath,
		bool bExcludeOfCheckId, 
		ProgressCallback progressCallback);
	DLL_API const char* LoadPakFileToWorkManager(const char* filePath, PakInfo* pakInfo, ProgressCallback progressCallback);
	DLL_API APIResult FreePakInfo(PakInfo* pakInfo);
	DLL_API APIResult ParsePakInfoFile(const char* pakInfoPath, PakInfo* pakInfo);
	DLL_API APIResult ExtractPakFile(const char* pakFilePath, const char* pakInfoPath, const char* outputRootPath, ProgressCallback progressCallback);
	DLL_API void ClosePakFileSession(const char* sessionString);
	DLL_API bool ExtractBlockFromPakFile(const char* sessionString, int subFileIndex, const char* outputPath);
	DLL_API bool FreeBuffer(void* buffer);
	DLL_API unsigned char* ReadBlockFromPakFile(const char* sessionToken, int subFileIndex, size_t* subFileSize);
	DLL_API unsigned int GetBlockIdFromPath(const char* blockPath);

}