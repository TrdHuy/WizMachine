#pragma once
#ifndef _PAK_UTILS
#define _PAK_UTILS

#include <iostream>
#include <fstream>

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

inline char* GetFileStreamBuffer(std::ifstream& fileStream) {
	if (!fileStream.is_open()) {
		return nullptr;
	}
	int fileSize = GetFileStreamSize(fileStream);

	std::streampos currentPos = fileStream.tellg();
	fileStream.seekg(0, std::ios::beg);
	char* buffer = new char[fileSize];
	if (fileStream.read(buffer, fileSize)) {
		fileStream.seekg(currentPos, std::ios::beg);
	}
	else {
		delete[]buffer;
		return nullptr;
	}
	return buffer;
}
unsigned long long GetCurrentLocalTimeMillisecond();
unsigned int GetCurrentLocalTimeSecond();
std::time_t GetTimeFromSecond(unsigned int seconds);
std::time_t GetTimeFromMillisecond(unsigned long long millisec);
void GetLTimeFromSecond(tm* pFormatTime, unsigned int seconds);
void GetLTimeFromMillisecond(tm* pFormatTime, unsigned long long millisec);

inline char* Wchar_t2CharPtr(wchar_t* str) {
	const size_t length = wcslen(str);
	char* charString = new char[length + 1]; // +1 for null terminator
	size_t numConverted = 0;
	wcstombs_s(&numConverted, charString, length + 1, str, length + 1);
	return charString;
}
#endif