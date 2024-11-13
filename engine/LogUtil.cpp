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

bool Log::directMode = false;

void Log::EnableDirectMode(bool enable) {
	directMode = enable;
}

void Log::Init() {
	if (isRunning) return;
	fs::path logDirectory = fs::current_path() / "temp/logs";
	fs::create_directories(logDirectory);
	std::string logFileName = "nte_" + GetCurrentTimestampFileName() + ".log";
	fs::path logFilePath = logDirectory / logFileName;

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

void Log::DirectLogMessage(const std::string& level, const std::string& tag, const std::string& message) {
	if (!logFile.is_open()) {
		std::cerr << "Log file is not open!" << std::endl;
		return;
	}
	if (directMode) {
		std::string timestamp = GetCurrentTimestamp();
		logFile << "[" << timestamp << "] [" << level << "] [" << tag << "] " << message << std::endl;
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

std::string Log::GetCurrentTimestampFileName() {
	auto now = std::chrono::system_clock::now();
	auto time = std::chrono::system_clock::to_time_t(now);
	auto milliseconds = std::chrono::duration_cast<std::chrono::milliseconds>(now.time_since_epoch()) % 1000;

	std::tm localTime;
	localtime_s(&localTime, &time);

	std::ostringstream oss;
	oss << std::put_time(&localTime, "%Y%m%d_%H%M%S") << "." << std::setw(3) << std::setfill('0') << milliseconds.count();
	return oss.str();
}
