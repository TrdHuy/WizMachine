#pragma once


#include <comdef.h>

extern "C" __declspec(dllexport) int Multiply(int a, int b);
extern "C" __declspec(dllexport) BSTR GetCalcOptions();

#include <iostream>
#include <fstream>
#include <vector>

#ifdef EXPORT_FUNCTIONS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

struct Color {
	unsigned char R;
	unsigned char G;
	unsigned char B;
};

struct SPRFileHead {
	char VersionInfo[4];
	unsigned short GlobalWidth;
	unsigned short GlobalHeight;
	short OffX;
	short OffY;
	unsigned short FrameCounts;
	unsigned short ColorCounts;
	unsigned short DirectionCount;
	unsigned short Interval;
	unsigned char Reserved[12];
};

struct FrameOffsetInfo
{
	unsigned int FrameOffset;
	unsigned int DataLength;
};

struct FrameInfo
{
	unsigned short Width;
	unsigned short Height;
	unsigned short OffX;
	unsigned short OffY;
};

struct FrameData {
	FrameInfo FrameInfo;
	unsigned int EncyptedFrameDataOffset;
	unsigned int EncryptedLength;
	unsigned int DecodedLength;
	unsigned char* DecodedFrameData;
	int* ColorMap;
};

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

	DLL_API void ExportToSPRFile2(FrameData frame[]);


	DLL_API int* LoadSPRFile2(const char* filePath);
	DLL_API void FreeArrData(unsigned char* data);
	DLL_API void MyCppFunc(const char* filePath, Color** ros, int* length);
}


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

int* LoadSPRFile2(const char* filePath) {
	std::ifstream file(filePath, std::ios::binary);
	int len = 5;
	int* arr = new int[len + 1];
	arr[0] = len;
	arr[1] = 1;
	arr[2] = 2;
	arr[3] = 3;
	arr[4] = 4;
	arr[5] = 5;
	return arr;
}

void FreeArrData(unsigned char* data) {
	delete[] data;
}

void MyCppFunc(const char* filePath, Color** ros, int* length)
{
	std::ifstream file(filePath, std::ios::binary);
	if (!file.is_open()) {
		std::cerr << "Failed to open file." << std::endl;
		ros = nullptr;
		return;
	}
	SPRFileHead* fileHead = new SPRFileHead();
	// Đọc 32 byte từ đầu file vào struct SPRFileHead
	file.read(reinterpret_cast<char*>(fileHead), sizeof(SPRFileHead));

	*ros = (Color*)::CoTaskMemAlloc(sizeof(Color) * fileHead->ColorCounts);
	::memset(*ros, 0, sizeof(Color) * fileHead->ColorCounts);
	for (int i = 0; i < fileHead->ColorCounts; i++) {
		(*ros)[i].R = file.get();
		(*ros)[i].G = file.get();
		(*ros)[i].B = file.get();
	}
	*length = fileHead->ColorCounts;
	delete fileHead;
	file.close();
}

void ExportToSPRFile2(FrameData frame[]) {
	SPRFileHead* fileHead = new SPRFileHead();
	delete fileHead;

}
