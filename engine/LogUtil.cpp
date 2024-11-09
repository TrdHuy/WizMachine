#include "pch.h"
#include "LogUtil.h"
#include <windows.h>
#include <filesystem>
#include <mutex>
#include <chrono>
#include <iomanip>
#include <sstream>
#include <iostream>

namespace fs = std::filesystem;

std::mutex logMutex;
std::ofstream Log::logFile;

// Hàm lấy đường dẫn của DLL hiện tại
std::string GetDllPath() {
    char path[MAX_PATH];
    HMODULE hModule = NULL;

    if (GetModuleHandleExA(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
        (LPCSTR)&GetDllPath, &hModule)) {
        GetModuleFileNameA(hModule, path, MAX_PATH);
        fs::path dllPath(path);
        return dllPath.parent_path().string();
    }
    return "";
}

// Hàm lấy thời gian hiện tại dưới dạng chuỗi theo định dạng yêu cầu
std::string Log::GetCurrentTimestamp() {
    auto now = std::chrono::system_clock::now();
    auto time = std::chrono::system_clock::to_time_t(now);
    auto milliseconds = std::chrono::duration_cast<std::chrono::milliseconds>(now.time_since_epoch()) % 1000;

    std::tm localTime;
    localtime_s(&localTime, &time);

    std::ostringstream oss;
    oss << std::put_time(&localTime, "%d-%m-%Y %H-%M-%S") << "-" << std::setw(3) << std::setfill('0') << milliseconds.count();
    return oss.str();
}

// Khởi tạo file log (mở file một lần khi khởi động chương trình)
void Log::Init() {
    std::lock_guard<std::mutex> guard(logMutex);

    std::string dllPath = GetDllPath();
    if (dllPath.empty()) {
        std::cerr << "Failed to get DLL path." << std::endl;
        return;
    }

    fs::path logDirectory = dllPath + "\\log";
    fs::create_directories(logDirectory);

    fs::path logFilePath = logDirectory / "native_log.txt";

    logFile.open(logFilePath, std::ios::app);  // Mở file log ở chế độ append
    if (!logFile.is_open()) {
        std::cerr << "Failed to open log file: " << logFilePath << std::endl;
    }
}

// Đóng file log khi chương trình kết thúc
void Log::Close() {
    std::lock_guard<std::mutex> guard(logMutex);
    if (logFile.is_open()) {
        logFile.close();
    }
}

// Hàm log chính để ghi log vào file với TAG
void Log::LogToFile(const std::string& logLevel, const std::string& tag, const std::string& message) {
    std::lock_guard<std::mutex> guard(logMutex);

    if (!logFile.is_open()) {
        std::cerr << "Log file is not open!" << std::endl;
        return;
    }

    std::string timestamp = GetCurrentTimestamp();
    logFile << timestamp << "\t" << logLevel << "\t" << tag << "\t" << message << std::endl;

    // Gửi log tới Output Window của Visual Studio
    std::string debugMessage = "[" + GetCurrentTimestamp() + "] [" + logLevel + "] [" + tag + "] " + message + "\n";
    OutputDebugStringA(debugMessage.c_str());
}

//// Ghi log thông tin (INFO) với TAG
//void Log::I(const std::string& tag, const std::string& message) {
//    LogToFile("INFO", tag, message);
//}
//
//// Ghi log lỗi (ERROR) với TAG
//void Log::E(const std::string& tag, const std::string& message) {
//    LogToFile("ERROR", tag, message);
//}
//
//// Ghi log debug (DEBUG) với TAG
//void Log::D(const std::string& tag, const std::string& message) {
//#ifdef _DEBUG
//    LogToFile("DEBUG", tag, message);
//#endif
//}
