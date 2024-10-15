#include "pch.h"

#include "MemoryManager.h"
#include <iostream>

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
    for (auto& entry : allocatedPointers) {
        if (entry.second.size > 0) {
            delete[] static_cast<char*>(entry.first);  // Giải phóng mảng
        }
        else {
            delete static_cast<char*>(entry.first);  // Giải phóng biến đơn
        }
        std::cout << "Deallocated memory at: " << entry.first << " (type " << entry.second.type.name() << ", size " << entry.second.size << ")" << std::endl;
    }
    allocatedPointers.clear();
}

// Destructor
MemoryManager::~MemoryManager() {
	deallocateAll();
}
