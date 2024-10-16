#ifndef LOGGER_H
#define LOGGER_H

#include <string>
#include <fstream>

class Log {
public:
    //// Hàm log cho INFO với TAG
    //static void I(const std::string& tag, const std::string& message);

    //// Hàm log cho ERROR với TAG
    //static void E(const std::string& tag, const std::string& message);

    //// Hàm log cho DEBUG với TAG
    //static void D(const std::string& tag, const std::string& message);

    // Hàm khởi tạo và đóng file log
    static void Init();
    static void Close();

    template<typename... Args>
    static void E(const std::string& tag, Args&&... args) {
        LogToFile("E", tag, BuildLogMessage(std::forward<Args>(args)...));
    }

    template<typename... Args>
    static void I(const std::string& tag, Args&&... args) {
        LogToFile("I", tag, BuildLogMessage(std::forward<Args>(args)...));
    }

    template<typename... Args>
    static void D(const std::string& tag, Args&&... args) {
#if _DEBUG
        LogToFile("D", tag, BuildLogMessage(std::forward<Args>(args)...));
#endif
    }
private:
    // Hàm dùng để thực hiện ghi log chung với TAG
    static void LogToFile(const std::string& logLevel, const std::string& tag, const std::string& message);

    // Hàm lấy thời gian hiện tại
    static std::string GetCurrentTimestamp();

    // Luồng file log (mở một lần và sử dụng xuyên suốt)
    static std::ofstream logFile;

    template<typename... Args>
    static std::string BuildLogMessage(Args&&... args) {
        std::ostringstream oss;
        (oss << ... << std::forward<Args>(args));  // Fold expression để nối tất cả các tham số
        return oss.str();
    }
};

#endif // LOGGER_H
