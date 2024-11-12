#include "pch.h"
#include "LogUtil.h"
#include <fstream>
#include <iostream>
#include <filesystem>
#include <sstream>
#include <chrono>
#include <iomanip>

namespace fs = std::filesystem;

std::atomic<bool> Log::isRunning = false;
std::thread Log::logThread;
std::queue<std::string> Log::logQueue;
std::mutex Log::queueMutex;
std::condition_variable Log::logCondition;
std::ofstream Log::logFile;

void Log::Init() {
    if (isRunning) return;

    // Mở file log
    fs::path logFilePath = fs::current_path() / "log.txt";
    logFile.open(logFilePath, std::ios::app);
    if (!logFile.is_open()) {
        std::cerr << "Failed to open log file!" << std::endl;
        return;
    }

    // Khởi chạy thread ghi log
    isRunning = true;
    logThread = std::thread(LogThreadFunction);
}

void Log::Close() {
    if (!isRunning) return;

    // Dừng thread
    isRunning = false;
    logCondition.notify_all();
    if (logThread.joinable()) {
        logThread.join();
    }

    // Đóng file log
    if (logFile.is_open()) {
        logFile.close();
    }
}

void Log::LogThreadFunction() {
    while (isRunning || !logQueue.empty()) {
        std::unique_lock<std::mutex> lock(queueMutex);
        logCondition.wait(lock, [] { return !logQueue.empty() || !isRunning; });

        while (!logQueue.empty()) {
            auto logMessage = logQueue.front();
            logQueue.pop();
            lock.unlock();

            if (logFile.is_open()) {
                logFile << logMessage << std::endl;
            }

            lock.lock();
        }
    }
}

void Log::EnqueueLog(const std::string& level, const std::string& tag, const std::string& message) {
    std::string timestamp = GetCurrentTimestamp();
    std::ostringstream oss;
    oss << "[" << timestamp << "] [" << level << "] [" << tag << "] " << message;

    {
        std::lock_guard<std::mutex> lock(queueMutex);
        logQueue.push(oss.str());
    }
    logCondition.notify_one();
}

std::string Log::GetCurrentTimestamp() {
    auto now = std::chrono::system_clock::now();
    auto time = std::chrono::system_clock::to_time_t(now);
    auto milliseconds = std::chrono::duration_cast<std::chrono::milliseconds>(now.time_since_epoch()) % 1000;

    std::tm localTime;
    localtime_s(&localTime, &time);

    std::ostringstream oss;
    oss << std::put_time(&localTime, "%Y-%m-%d %H:%M:%S") << "." << std::setw(3) << std::setfill('0') << milliseconds.count();
    return oss.str();
}
