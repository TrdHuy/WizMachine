#ifndef _ENGINE_BIG_ALONE_FILE_H_
#define _ENGINE_BIG_ALONE_FILE_H_

#include <iostream>
#include <fstream>
#include <string>
#include <filesystem>
#include "file.h"
#include "MemoryManager.h"

class BigAloneFile {
private:
    std::string fullFolderFilePath;
    std::fstream fileStream;
    void* contentBuffer = nullptr;
    unsigned long long contentBufferSize = 0; // Sử dụng unsigned long long cho kích thước

public:
    // Constructor
    BigAloneFile() {
        contentBuffer = nullptr;
        contentBufferSize = 0;
    }

    void close() {
        if (fileStream.is_open()) {
            fileStream.close();
        }
    }

    bool isOpen() {
        return fileStream.is_open() && fileStream.good();
    }

    // Đọc từ file
    unsigned long long read(void* buffer, unsigned long long readBytes) {
        if (fileStream.is_open() && fileStream.good()) {
            fileStream.read(static_cast<char*>(buffer), readBytes);
            return fileStream.gcount(); // Trả về số byte đã đọc
        }
        return 0;
    }

    // Đọc từ file
    unsigned long long readFrom(long long fromPos, void* buffer, unsigned long long readBytes) {
        if (fileStream.is_open() && fileStream.good()) {
            std::streampos currentPos = fileStream.tellg(); // Lưu vị trí hiện tại
            fileStream.seekg(fromPos); // đưa về vị trí cần đọc
            fileStream.read(static_cast<char*>(buffer), readBytes);
            fileStream.seekg(currentPos); // Khôi phục vị trí

            return fileStream.gcount(); // Trả về số byte đã đọc
        }
        return 0;
    }

    // Ghi vào file
    unsigned long long write(const void* buffer, unsigned long long writeBytes) {
        if (fileStream.is_open() && fileStream.good()) {
            fileStream.write(static_cast<const char*>(buffer), writeBytes);
            return fileStream.good() ? writeBytes : 0;
        }
        return 0;
    }

    unsigned long long tell() {
        if (fileStream.is_open()) {
            return fileStream.tellg(); // Trả về vị trí hiện tại
        }
        return 0;
    }

    unsigned long long seek(unsigned long long offset, int origin) {
        if (fileStream.is_open()) {
            std::ios_base::seekdir dir;
            switch (origin) {
            case std::ios::beg: dir = std::ios_base::beg; break;
            case std::ios::cur: dir = std::ios_base::cur; break;
            case std::ios::end: dir = std::ios_base::end; break;
            default: return -1; // Invalid origin
            }

            fileStream.seekg(offset, dir);
            fileStream.seekp(offset, dir);

            if (fileStream.fail()) {
                fileStream.clear(); // Clear fail state
                return -1;
            }

            return fileStream.tellg(); // Trả về vị trí mới
        }
        return -1;
    }

    unsigned long long size() {
        if (fileStream.is_open()) {
            std::streampos currentPos = fileStream.tellg(); // Lưu vị trí hiện tại
            fileStream.seekg(0, std::ios::end);
            unsigned long long fileSize = fileStream.tellg(); // Lấy kích thước file
            fileStream.seekg(currentPos); // Khôi phục vị trí
            return fileSize; // Trả về kích thước file
        }
        return 0;
    }

    void* getBuffer() {
        if (contentBuffer == nullptr && fileStream.is_open()) {
            contentBufferSize = size();
            if (contentBufferSize > 0) {
                contentBuffer = malloc(contentBufferSize);
                if (contentBuffer) {
                    seek(0, std::ios::beg);
                    if (read(contentBuffer, contentBufferSize) != contentBufferSize) {
                        free(contentBuffer);
                        contentBuffer = nullptr;
                    }
                }
            }
        }
        return contentBuffer;
    }

    void release() {
        close(); // Đảm bảo file được đóng
        MemoryManager::getInstance()->deallocate(this);
    }

    char* getFullPath() {
        if (fileStream.is_open()) {
            return fullFolderFilePath.data();
        }
        return nullptr;
    }

    // Destructor
    ~BigAloneFile() {
        close();
    }

public:
    // Mở file với việc tự động tạo thư mục và file
    bool open(const char* fileName, bool writeSupport = false, bool forceCreate = false) {
        close(); // Đảm bảo file trước đó được đóng

        // Tạo đường dẫn đầy đủ cho file
        std::filesystem::path fullPath = std::filesystem::absolute(fileName);
        fullFolderFilePath = fullPath.string();

        std::string mode = "rb"; // Chế độ đọc mặc định

        if (writeSupport) {
            if (!std::filesystem::exists(fullPath) || forceCreate) {
                mode = "w+b"; // Chế độ đọc-ghi (file sẽ được tạo)
            }
            else {
                mode = "r+b"; // Chế độ đọc-ghi (file phải tồn tại)
            }
        }

        // Kiểm tra và tạo thư mục nếu nó không tồn tại
        std::filesystem::path dirPath = std::filesystem::path(fullPath).parent_path();
        if (!std::filesystem::exists(dirPath)) {
            std::filesystem::create_directories(dirPath);
        }

        // Chuyển đổi chế độ thành std::ios_base::openmode
        std::ios_base::openmode fmode = std::ios::in;
        if (mode.find('w') != std::string::npos) {
            fmode = std::ios::out | std::ios::trunc; // Tạo mới hoặc cắt ngắn
        }
        if (mode.find('r') != std::string::npos) {
            fmode |= std::ios::in;
        }
        if (mode.find('+') != std::string::npos) {
            fmode |= std::ios::in | std::ios::out; // Đọc và ghi
        }
        if (mode.find('b') != std::string::npos) {
            fmode |= std::ios::binary; // Chế độ nhị phân
        }

        fileStream.open(fullPath, fmode);
        return fileStream.is_open(); // Trả về trạng thái mở file
    }
};

#endif //#ifndef _ENGINE_BIG_ALONE_FILE_H_
