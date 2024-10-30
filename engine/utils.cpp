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


std::string GetTempFilePath(const std::string& fileName, bool useAppData) {
	char tempPath[MAX_PATH];

	if (useAppData) {
		// Lấy đường dẫn tới thư mục AppData\Local\Temp
		PWSTR appDataPath = NULL;
		if (SUCCEEDED(SHGetKnownFolderPath(FOLDERID_LocalAppData, 0, NULL, &appDataPath))) {
			std::string appDataTempPath = std::filesystem::path(appDataPath).string() + "\\Temp\\";
			CoTaskMemFree(appDataPath); // Giải phóng bộ nhớ được cấp phát bởi SHGetKnownFolderPath

			// Tạo thư mục nếu chưa tồn tại
			std::filesystem::create_directories(appDataTempPath);
			return appDataTempPath + fileName;
		}
	}
	else {
		// Lấy đường dẫn tới thư mục tạm của thư viện DLL hiện tại
		HMODULE hModule = GetModuleHandle(NULL);
		if (hModule != NULL) {
			GetModuleFileNameA(hModule, tempPath, MAX_PATH);
			std::string dllDirectory = std::filesystem::path(tempPath).parent_path().string();
			std::string dllTempPath = dllDirectory + "\\Temp\\";

			// Tạo thư mục nếu chưa tồn tại
			std::filesystem::create_directories(dllTempPath);
			return dllTempPath + fileName;
		}
	}

	// Nếu không thể lấy đường dẫn, trả về chuỗi rỗng
	return "";
}

std::string intToHexString(int number) {
	std::stringstream ss;
	ss << std::hex << std::uppercase << number; // Định dạng thành hexa với chữ in hoa
	return ss.str(); // Trả về chuỗi hexa
}

std::string formatTimeToString(int time) {
	std::stringstream ss;
	tm* pFormatTime = MemoryManager::getInstance()->allocate<tm>();
	GetLTimeFromSecond(pFormatTime, time);
	ss << std::setw(2) << std::setfill('0') << (pFormatTime->tm_mday) << '-' // Ngày
		<< std::setw(2) << std::setfill('0') << (pFormatTime->tm_mon + 1) << '-' // Tháng (cộng thêm 1)
		<< (pFormatTime->tm_year + 1900) << ' ' // Năm (cộng thêm 1900)
		<< std::setw(2) << std::setfill('0') << (pFormatTime->tm_hour) << ':' // Giờ
		<< std::setw(2) << std::setfill('0') << (pFormatTime->tm_min) << ':' // Phút
		<< std::setw(2) << std::setfill('0') << (pFormatTime->tm_sec); // Giây
	MemoryManager::getInstance()->deallocate(pFormatTime);
	return ss.str(); // Trả về chuỗi định dạng
}