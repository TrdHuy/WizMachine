#ifndef PAK_WORK_MANAGER_H
#define PAK_WORK_MANAGER_H

#include <string>
#include <unordered_map>
#include <vector>
#include <fstream>
#include <random>
#include <iostream>
#include "pak.h"
#include "LogUtil.h"
#include "BigAloneFile.h"

class PakWorkManager {
public:
    static PakWorkManager* GetInstance();

    // API: Load file .pak và tạo file tạm, trả về token phiên và thông tin file Pak nội bộ
    std::string LoadPakFile(const std::string& filePath, PakInfoInternal& pakInfoInternal);

    // API: Đọc một đoạn byte từ file con
    bool ReadSubFileData(const std::string& sessionToken, int subFileIndex, unsigned char*& buffer, size_t* subFileSize);

    // API: Trích xuất file con ra file output
    bool ExtractSubFile(const std::string& sessionToken, int subFileIndex, const std::string& outputPath);

    // API: Hủy phiên và xóa file tạm
    void CloseSession(const std::string& sessionToken);

private:
    static const std::string TAG;

    // Instance duy nhất của PakWorkManager
    static PakWorkManager* instance;

    // Constructor và Destructor để ngăn chặn việc tạo instance bên ngoài
    PakWorkManager() = default;
    ~PakWorkManager() = default;

    // Không cho phép copy hoặc gán đối tượng
    PakWorkManager(const PakWorkManager&) = delete;
    PakWorkManager& operator=(const PakWorkManager&) = delete;


    // Lưu trữ thông tin phiên làm việc với key là token
    struct SessionInfo {
        BigAloneFile* tempFile;
        std::vector<std::streampos> blockOffsets; // Offset của từng block trong file tạm
    };

    // Bản đồ để quản lý các phiên đang hoạt động
    std::unordered_map<std::string, SessionInfo> sessionFiles;

    // Tạo một token phiên ngẫu nhiên
    std::string GenerateSessionToken();

    // Xác thực token phiên từ file tạm
    bool ValidateSessionToken(const std::string& sessionToken, const SessionInfo& sessionInfo);

    // Đọc vị trí của file con từ file tạm
    std::pair<std::streampos, std::streampos> GetSubFilePosition(const SessionInfo& sessionInfo, int subFileIndex);
};

#endif 