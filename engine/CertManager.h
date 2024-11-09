#ifndef CERT_MANAGER_H
#define CERT_MANAGER_H

#include <string>
#include "LogUtil.h" // Sử dụng để log thông tin
#include "MemoryManager.h" // Quản lý bộ nhớ cho CertInfo
#include "utils.h" // Quản lý bộ nhớ cho CertInfo
#include "cert.h"

enum class CertCheckResult {
	Success = 0,               // Chứng chỉ hợp lệ
	ModuleFilePathNotSet = -1, // Đường dẫn tệp module chưa được khởi tạo
	PathConversionFailed = -2, // Không thể chuyển đổi đường dẫn
	CertInfoAllocationFailed = -3, // Không thể cấp phát CertInfo
	InvalidCertPermission = -4,    // Quyền chứng chỉ không hợp lệ
	GetCertInfoFailed = -5         // Lỗi khi lấy thông tin chứng chỉ
};


class CertManager {
public:
	// Lấy instance Singleton
	static CertManager& getInstance() {
		static CertManager instance;
		return instance;
	}

	// Thiết lập đường dẫn tệp module
	void initialize(const std::wstring& moduleFilePath) {
		this->moduleFilePath = moduleFilePath;
		this->isCertChecked = false;
	}

	CertCheckResult checkCertificate() {
		if (isCertChecked) {
			// Chứng chỉ đã được kiểm tra và hợp lệ
			return CertCheckResult::Success;
		}

		if (moduleFilePath.empty()) {
			Log::E("CertManager", "Module file path not initialized");
			return CertCheckResult::ModuleFilePathNotSet;
		}

		// Chuyển đổi đường dẫn sang định dạng char*
		char* pathChar = Wchar_t2CharPtr(moduleFilePath.c_str());
		if (!pathChar) {
			Log::E("CertManager", "Failed to convert module file path to char*");
			return CertCheckResult::PathConversionFailed;
		}

		// Tạo CertInfo
		CertInfo* certInfo = MemoryManager::getInstance()->allocate<CertInfo>();
		if (!certInfo) {
			MemoryManager::getInstance()->deallocate(static_cast<char*>(pathChar));
			Log::E("CertManager", "Failed to allocate memory for CertInfo");
			return CertCheckResult::CertInfoAllocationFailed;
		}

		// Ghi log và bắt đầu kiểm tra
		Log::I("CertManager", "Start verifying cert for path: " + std::string(pathChar));
		int certResult = GetCertificateInfoInternal(pathChar, certInfo);

		if (certResult == 0) { // Thành công khi gọi GetCertificateInfo
			if (ForceCheckCertPermissionInternal(*certInfo)) {
				// Chứng chỉ hợp lệ
				isCertChecked = true;
				MemoryManager::getInstance()->deallocate(static_cast<char*>(pathChar));
				MemoryManager::getInstance()->deallocate(certInfo);
				Log::I("CertManager", "Verified certificate!");
				return CertCheckResult::Success;
			}
			else {
				MemoryManager::getInstance()->deallocate(static_cast<char*>(pathChar));
				MemoryManager::getInstance()->deallocate(certInfo);
				Log::E("CertManager", "Invalid cert permission");
				return CertCheckResult::InvalidCertPermission;
			}
		}
		else {
			MemoryManager::getInstance()->deallocate(static_cast<char*>(pathChar));
			MemoryManager::getInstance()->deallocate(certInfo);
			Log::E("CertManager", "Failed to get certificate info");
			return CertCheckResult::GetCertInfoFailed;
		}
	}

private:
	// Đường dẫn tệp module
	std::wstring moduleFilePath;

	// Trạng thái kiểm tra chứng chỉ
	bool isCertChecked;

	// Constructor và Destructor
	CertManager() : isCertChecked(false) {}
	~CertManager() {}

	// Xóa copy constructor và operator=
	CertManager(const CertManager&) = delete;
	CertManager& operator=(const CertManager&) = delete;
};

#endif // CERT_MANAGER_H
