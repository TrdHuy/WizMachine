#pragma once

#include <comdef.h>
#include <iostream>
#include <fstream>
#include <vector>
#include "spr.h"
#include "cert.h"
#include "pak.h"

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
	
	DLL_API void LoadSPRFile(const char* filePath,
		SPRFileHead* fileHead,
		Color** palette,
		int* paletteLength,
		int* frameDataBeginPos,
		FrameData** frameData,
		int* frameCount);
	DLL_API void LoadSPRMemory(
		const uint8_t* data,           // Mảng byte chứa dữ liệu SPR
		size_t dataLength,             // Độ dài của mảng byte
		SPRFileHead* fileHead,         // Con trỏ đến cấu trúc SPRFileHead để lưu thông tin
		Color** palette,               // Con trỏ đến bảng màu sẽ được khởi tạo và lưu trữ
		int* paletteLength,            // Con trỏ đến độ dài bảng màu
		int* frameDataBeginPos,        // Con trỏ đến vị trí bắt đầu dữ liệu frame
		FrameData** frame,             // Con trỏ đến mảng chứa dữ liệu frame sẽ được khởi tạo
		int* frameCount                // Con trỏ đến số lượng khung hình
	);

	DLL_API void FreeSPRMemory(
		Color* palette,
		FrameData* frame, int frameCount);

	DLL_API void ExportToSPRFile(const char* filePath,
		SPRFileHead fileHead,
		Color palette[],
		int paletteSize,
		FrameData frame[]);



	DLL_API void CompressFolderToPakFile(const char* folderPath,
		const char* outputPath,
		bool bExcludeOfCheckId);

	DLL_API void	ForceCheckCertPermission(CertInfo certinfo);
	DLL_API int		GetCertificateInfo(const char* filePath, CertInfo* certInfo);
	DLL_API void	FreeCertInfo(CertInfo* certInfo);


	DLL_API const char* LoadPakFileToWorkManager(const char* filePath, PakInfo* pakInfo);
	DLL_API void FreePakInfo(PakInfo* pakInfo);
	DLL_API void ParsePakInfoFile(const char* pakInfoPath, PakInfo* pakInfo);
	DLL_API bool ExtractPakFile(const char* pakFilePath, const char* pakInfoPath, const char* outputRootPath);
	DLL_API void CloseSession(const char* sessionString);
	DLL_API bool ExtractBlockFromPakFile(const char* sessionString, int subFileIndex, const char* outputPath);
	DLL_API bool FreeBuffer(void* buffer);
	DLL_API unsigned char* ReadBlockFromPakFile(const char* sessionToken, int subFileIndex, size_t* subFileSize);
	DLL_API unsigned int GetBlockIdFromPath(const char* blockPath);
	
}