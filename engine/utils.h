#pragma once
#ifndef _PAK_UTILS
#define _PAK_UTILS

#include <iostream>
#include <fstream>
#include "MemoryManager.h"

#define SAFE_FREE(a)	if (a) {free(a); (a)=NULL;}

inline std::streamsize GetFileStreamSize(std::ifstream& fileStream) {
	if (!fileStream.is_open()) {
		return -1;
	}

	std::streampos currentPos = fileStream.tellg();

	fileStream.seekg(0, std::ios::end);

	std::streamsize fileSize = fileStream.tellg();

	fileStream.seekg(currentPos, std::ios::beg);

	return fileSize;
}

char* GetFileStreamBuffer(std::ifstream& fileStream);
unsigned long long GetCurrentLocalTimeMillisecond();
unsigned int GetCurrentLocalTimeSecond();
std::time_t GetTimeFromSecond(unsigned int seconds);
std::time_t GetTimeFromMillisecond(unsigned long long millisec);
void GetLTimeFromSecond(tm* pFormatTime, unsigned int seconds);
void GetLTimeFromMillisecond(tm* pFormatTime, unsigned long long millisec);

char* Wchar_t2CharPtr(wchar_t* str);
#endif