#ifndef _ENGINE_SPR_H_
#define _ENGINE_SPR_H_


#define	SPR_COMMENT_FLAG				0x525053

#include "MemoryManager.h"

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
	unsigned int EncryptedFrameDataOffset;
	unsigned int EncryptedLength;
	unsigned int DecodedLength;
	unsigned char* DecodedFrameData;
	int* ColorMap;

	void initMemory(unsigned int decodedByteLength) {
		DecodedLength = decodedByteLength;
		DecodedFrameData = MemoryManager::getInstance()->allocateArray<unsigned char>(decodedByteLength * 4);
		ColorMap = MemoryManager::getInstance()->allocateArray<int>(decodedByteLength);

	}
	void free() {
		MemoryManager::getInstance()->deallocate(DecodedFrameData);
		MemoryManager::getInstance()->deallocate(ColorMap);
		DecodedFrameData = nullptr;
		ColorMap = nullptr;
	}
};

void LoadSPRFileInternal(const char* filePath,
	SPRFileHead* fileHead,
	Color** palette,
	int* paletteLength,
	int* frameDataBeginPos,
	FrameData** frame,
	int* frameCount);

void ExportToSPRFileInternal(const char* filePath,
	SPRFileHead fileHead,
	Color palette[],
	int paletteSize,
	FrameData frame[]);

#endif