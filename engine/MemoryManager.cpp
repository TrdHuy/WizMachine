#include "pch.h"

#include "MemoryManager.h"
#include <iostream>
const std::string MemoryManager::TAG = "MemoryManager";
MemoryManager* MemoryManager::instance = nullptr;
std::mutex MemoryManager::mtx;

// Lấy instance của singleton
MemoryManager* MemoryManager::getInstance() {
	std::lock_guard<std::mutex> lock(mtx);
	if (instance == nullptr) {
		instance = new MemoryManager();
	}
	return instance;
}

// Giải phóng tất cả bộ nhớ quản lý
void MemoryManager::deallocateAll() {
    std::lock_guard<std::mutex> lock(mtx);
    Log::I(TAG, "Deallocated all of declared memory! cache size: ", allocatedPointers.size());

    for (auto& entry : allocatedPointers) {
        if (entry.second.size > 0) {
            delete[] static_cast<char*>(entry.first);  // Giải phóng mảng
        }
        else {
            delete static_cast<char*>(entry.first);  // Giải phóng biến đơn
        }
        Log::D(TAG, "Deallocated memory at: ", static_cast<void*>(entry.first));
    }
    allocatedPointers.clear();
}

// Destructor
MemoryManager::~MemoryManager() {
	deallocateAll();
}
