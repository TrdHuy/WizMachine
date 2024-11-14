#ifndef LOGGER_H
#define LOGGER_H

#include <string>
#include <queue>
#include <mutex>
#include <condition_variable>
#include <thread>
#include <atomic>

class Log {
public:
    static void Init();
    static void Close();

    template<typename... Args>
    static void I(const std::string& tag, Args&&... args) {
        EnqueueLog("INFO", tag, BuildLogMessage(std::forward<Args>(args)...));
    }

    template<typename... Args>
    static void E(const std::string& tag, Args&&... args) {
        EnqueueLog("ERROR", tag, BuildLogMessage(std::forward<Args>(args)...));
    }

    template<typename... Args>
    static void D(const std::string& tag, Args&&... args) {
#ifdef _DEBUG
        EnqueueLog("DEBUG", tag, BuildLogMessage(std::forward<Args>(args)...));
#endif
    }

private:
    static void LogThreadFunction();

    static void EnqueueLog(const std::string& level, const std::string& tag, const std::string& message);

    static std::string GetCurrentTimestamp();

    static std::atomic<bool> isRunning;
    static std::thread logThread;
    static std::queue<std::string> logQueue;
    static std::mutex queueMutex;
    static std::condition_variable logCondition;

    static std::ofstream logFile;

    template<typename... Args>
    static std::string BuildLogMessage(Args&&... args) {
        std::ostringstream oss;
        (oss << ... << std::forward<Args>(args));
        return oss.str();
    }
};

#endif // LOGGER_H
