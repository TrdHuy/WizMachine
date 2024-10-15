#include "pch.h"
#include "base.h"
#include "MemoryManager.h"

unsigned long long GetCurrentLocalTimeMillisecond() {
	auto start = std::chrono::system_clock::now();
	auto now_ms = std::chrono::time_point_cast<std::chrono::milliseconds>(start);
	auto epoch = now_ms.time_since_epoch();
	auto value = std::chrono::duration_cast<std::chrono::milliseconds>(epoch);
	unsigned long long res = (unsigned long long)value.count();
	return res;
}

unsigned int GetCurrentLocalTimeSecond() {
	auto start = std::chrono::system_clock::now();
	auto now_ms = std::chrono::time_point_cast<std::chrono::seconds>(start);
	auto epoch = now_ms.time_since_epoch();
	auto value = std::chrono::duration_cast<std::chrono::seconds>(epoch);
	unsigned int res = (unsigned int)value.count();
	return res;
}

std::time_t GetTimeFromSecond(unsigned int seconds) {
	auto dur = std::chrono::seconds(seconds);
	std::chrono::time_point<std::chrono::system_clock> dt(dur);
	std::time_t time_t_format = std::chrono::system_clock::to_time_t(dt);
	return time_t_format;
}

std::time_t GetTimeFromMillisecond(unsigned long long millisec) {
	auto dur = std::chrono::milliseconds(millisec);
	std::chrono::time_point<std::chrono::system_clock> dt(dur);
	std::time_t time_t_format = std::chrono::system_clock::to_time_t(dt);
	return time_t_format;
}

void GetLTimeFromSecond(tm* pFormatTime, unsigned int seconds) {
	std::time_t time_t_format = GetTimeFromSecond(seconds);
	localtime_s(pFormatTime, &time_t_format);
}

void GetLTimeFromMillisecond(tm* pFormatTime, unsigned long long millisec) {
	std::time_t time_t_format = GetTimeFromMillisecond(millisec);
	localtime_s(pFormatTime, &time_t_format);
}

char* Wchar_t2CharPtr(wchar_t* str) {
	const size_t length = wcslen(str);
	//char* charString = new char[length + 1]; // +1 for null terminator
	char* charString = MemoryManager::getInstance()->allocateArray<char>(length + 1);
	size_t numConverted = 0;
	wcstombs_s(&numConverted, charString, length + 1, str, length + 1);
	return charString;
}

char* GetFileStreamBuffer(std::ifstream& fileStream) {
	if (!fileStream.is_open()) {
		return nullptr;
	}
	int fileSize = GetFileStreamSize(fileStream);

	std::streampos currentPos = fileStream.tellg();
	fileStream.seekg(0, std::ios::beg);
	char* buffer = MemoryManager::getInstance()->allocateArray<char>(fileSize);
	if (fileStream.read(buffer, fileSize)) {
		fileStream.seekg(currentPos, std::ios::beg);
	}
	else {
		MemoryManager::getInstance()->deallocate(buffer);
		return nullptr;
	}
	return buffer;
}