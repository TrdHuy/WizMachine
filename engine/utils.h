#pragma once
#ifndef _PAK_UTILS
#define _PAK_UTILS

#include <iostream>
#include <fstream>
#include "MemoryManager.h"
#include <string>
#include <windows.h>
#include <shlobj.h> // Dành cho SHGetKnownFolderPath
#include <filesystem>
namespace fs = std::filesystem;

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

inline size_t countRegualarFilesInFolder(const fs::path& inputPath) {
	return std::count_if(
		fs::recursive_directory_iterator(inputPath),
		fs::recursive_directory_iterator{},
		[](const auto& entry) { return fs::is_regular_file(entry); }
	);
}

char* GetFileStreamBuffer(std::ifstream& fileStream);
unsigned long long GetCurrentLocalTimeMillisecond();
unsigned int GetCurrentLocalTimeSecond();
std::time_t GetTimeFromSecond(unsigned int seconds);
std::time_t GetTimeFromMillisecond(unsigned long long millisec);
void GetLTimeFromSecond(tm* pFormatTime, unsigned int seconds);
void GetLTimeFromMillisecond(tm* pFormatTime, unsigned long long millisec);

char* Wchar_t2CharPtr(wchar_t* str);
char* Wchar_t2CharPtr(const wchar_t* str);
std::string GetTempFilePath(const std::string& fileName, bool useAppData);
std::string intToHexString(int number);
std::string formatTimeToString(int time);
void MakeDirFromFilePathIfNotExisted(const std::string& filePath);

#endif