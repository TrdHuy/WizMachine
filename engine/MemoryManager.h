#ifndef MEMORYMANAGER_H
#define MEMORYMANAGER_H

#include <unordered_map>
#include <mutex>
#include <typeindex>
#include <typeinfo>
#include <iostream>
#include <vector>
#include <windows.h>  // Thư viện Windows để lấy call stack

#ifdef _DEBUG
// Debug mode: thêm thông tin call stack vào PointerInfo
struct PointerInfo {
    std::type_index type;       // Kiểu dữ liệu của đối tượng
    size_t size;                // Kích thước (nếu là mảng), 0 nếu không phải mảng
    bool isArray;               // true nếu là mảng, false nếu là biến đơn
    std::vector<void*> callStack;  // Call stack

    PointerInfo() : type(typeid(void)), size(0), isArray(false) {}

    PointerInfo(std::type_index t, size_t s, bool array) : type(t), size(s), isArray(array) {
        // Lấy call stack khi tạo PointerInfo
        const int maxFrames = 10;
        void* stack[maxFrames];
        USHORT frames = CaptureStackBackTrace(0, maxFrames, stack, NULL);
        callStack.assign(stack, stack + frames);  // Lưu call stack vào vector
    }

    // Hàm in call stack ra màn hình (debug)
    void printCallStack() const {
        std::cout << "Call stack: \n";
        for (const auto& frame : callStack) {
            std::cout << frame << std::endl;
        }
    }
};
#else
// Release mode: không cần lưu call stack
struct PointerInfo {
    size_t size;           // Kích thước (nếu là mảng), 0 nếu không phải mảng
    bool isArray;          // true nếu là mảng, false nếu là biến đơn

    PointerInfo() : size(0), isArray(false) {}

    PointerInfo(size_t s, bool array) : size(s), isArray(array) {}
};
#endif

class MemoryManager {
private:
    static MemoryManager* instance;
    static std::mutex mtx;
    std::unordered_map<void*, PointerInfo> allocatedPointers;

    MemoryManager() {}  // Constructor private để ngăn tạo đối tượng trực tiếp

public:
    // Cấp phát cho biến đơn
    template<typename T>
    T* allocate() {
        std::lock_guard<std::mutex> lock(mtx);
        T* ptr = new T;
#ifdef _DEBUG
        allocatedPointers[ptr] = PointerInfo(std::type_index(typeid(T)), 0, false);  // Debug: lưu type và call stack
#else
        allocatedPointers[ptr] = PointerInfo(0, false);  // Release: không lưu type
#endif
        std::cout << "Allocated memory at: " << ptr << " (single object)" << std::endl;
        return ptr;
    }

    // Cấp phát cho mảng
    template<typename T>
    T* allocateArray(size_t size) {
        std::lock_guard<std::mutex> lock(mtx);
        T* ptr = new T[size];
#ifdef _DEBUG
        allocatedPointers[ptr] = PointerInfo(std::type_index(typeid(T)), size, true);  // Debug: lưu type và call stack
#else
        allocatedPointers[ptr] = PointerInfo(size, true);  // Release: không lưu type
#endif
        std::cout << "Allocated memory at: " << ptr << " (array of " << size << " elements)" << std::endl;
        return ptr;
    }

    template<typename T>
    void deallocate(T* ptr) {
        std::lock_guard<std::mutex> lock(mtx);
        auto it = allocatedPointers.find(static_cast<void*>(ptr));
        if (it != allocatedPointers.end()) {
#ifdef _DEBUG
            // In call stack nếu là debug mode
            //it->second.printCallStack();

            if (it->second.type == std::type_index(typeid(T))) {  // Debug: kiểm tra type
#endif
                if (it->second.isArray) {
                    std::cout << "Deallocated memory at: " << ptr << " (array of size " << it->second.size << ")" << std::endl;
                    delete[] ptr;
                }
                else {
                    std::cout << "Deallocated memory at: " << ptr << " (single object)" << std::endl;
                    delete ptr;
                }
                allocatedPointers.erase(it);  // Xóa con trỏ khỏi cache
#ifdef _DEBUG
            }
            else {
                std::cerr << "Type mismatch. Cannot deallocate." << std::endl;
            }
#endif
        }
        else {
            std::cerr << "Pointer not found in cache. Cannot deallocate." << std::endl;
        }
    }

    // Lấy instance của singleton
    static MemoryManager* getInstance();

    // Giải phóng toàn bộ bộ nhớ quản lý
    void deallocateAll();

    // Destructor
    ~MemoryManager();
};

#endif // MEMORYMANAGER_H
