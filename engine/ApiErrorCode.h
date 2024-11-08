#pragma once

#ifndef API_ERROR_CODE_H
#define API_ERROR_CODE_H

#include <string>
#include <iostream>

// Định nghĩa mã lỗi sử dụng enum class
enum class ErrorCode {
    Success = 0,        // Không có lỗi
    InvalidArgument,    // Tham số không hợp lệ
    OutOfMemory,        // Không đủ bộ nhớ
    NotFound,           // Không tìm thấy
    InternalError,      // Lỗi nội bộ
    UnknownError,        // Lỗi không xác định
    SecurityError,        // Lỗi bảo mật
    ShouldNeverHappen,      // Lỗi không nên xảy ra 
};

// Cấu trúc trả về
struct APIResult {
    ErrorCode errorCode;
    char errorMessage[256];
    APIResult(ErrorCode code, std::string message) : errorCode(code) {
        strncpy_s(errorMessage, message.c_str(), _TRUNCATE);
    }
    APIResult(ErrorCode code, const char* message) : errorCode(code) {
        strncpy_s(errorMessage, message, _TRUNCATE);
    }
    APIResult() : errorCode(ErrorCode::Success) {
        strncpy_s(errorMessage, "No error.", _TRUNCATE);
    }
};

#endif