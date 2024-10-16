#ifndef MEMORYMANAGER_H
#define MEMORYMANAGER_H

#include <unordered_map>
#include <mutex>

#include <vector>
#include "LogUtil.h"

#ifdef _DEBUG
#pragma comment(lib, "DbgHelp.lib")
#include <typeindex>
#include <typeinfo>
#include <windows.h>  // Thư viện Windows để lấy call stack
#include <dbghelp.h>  // Thư viện để giải mã địa chỉ thành tên hàm (Symbol)
#include <sstream>    // Để tạo chuỗi định dạng
// Debug mode: thêm thông tin call stack vào PointerInfo
struct PointerInfo {
	std::type_index type;                // Kiểu dữ liệu của đối tượng
	size_t size;                         // Kích thước (nếu là mảng), 0 nếu không phải mảng
	bool isArray;                        // true nếu là mảng, false nếu là biến đơn
	std::vector<void*> callStack;        // Call stack
	std::string formattedCallStack;      // Chuỗi format của call stack

	PointerInfo() : type(typeid(void)), size(0), isArray(false) {}

	PointerInfo(std::type_index t, size_t s, bool array) : type(t), size(s), isArray(array) {
		// Lấy call stack khi tạo PointerInfo
		const int maxFrames = 10;
		void* stack[maxFrames];
		USHORT frames = CaptureStackBackTrace(0, maxFrames, stack, NULL);
		callStack.assign(stack, stack + frames);  // Lưu call stack vào vector

		// Chuyển call stack thành chuỗi định dạng
		formattedCallStack = formatCallStack();
	}

	// Hàm định dạng call stack thành chuỗi
	std::string formatCallStack() const {
		std::ostringstream oss;
		oss << "Call stack:\n";

		// Tạo SYMBOL_INFO để lưu thông tin của các hàm
		SYMBOL_INFO* symbolInfo = (SYMBOL_INFO*)malloc(sizeof(SYMBOL_INFO) + 256 * sizeof(char));
		symbolInfo->MaxNameLen = 255;
		symbolInfo->SizeOfStruct = sizeof(SYMBOL_INFO);

		// Lấy handle của tiến trình hiện tại để giải mã địa chỉ
		HANDLE process = GetCurrentProcess();
		SymInitialize(process, NULL, TRUE);

		for (const auto& frame : callStack) {
			// Giải mã địa chỉ thành tên hàm và thêm vào chuỗi định dạng
			SymFromAddr(process, (DWORD64)frame, 0, symbolInfo);
			oss << "0x" << frame << ": " << symbolInfo->Name << std::endl;
		}

		free(symbolInfo);  // Giải phóng bộ nhớ cho SYMBOL_INFO
		SymCleanup(process);  // Dọn dẹp sau khi dùng SymInitialize

		return oss.str();
	}

	// Hàm in call stack ra màn hình (debug)
	void printCallStack() const {
		std::cout << formattedCallStack;
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
	static const std::string TAG;
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
		Log::D(TAG, "Allocated memory at: ", static_cast<void*>(ptr), " (single object). Stack call: ", allocatedPointers[ptr].formattedCallStack);
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
		Log::D(TAG, "Allocated memory at: ", static_cast<void*>(ptr), " (array of ", size, " elements).  Stack call: ", allocatedPointers[ptr].formattedCallStack);

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
					Log::D(TAG, "Deallocated memory at: ", static_cast<void*>(ptr), " (array of ", it->second.size, ")");
					delete[] ptr;
				}
				else {
					Log::D(TAG, "Deallocated memory at: ", static_cast<void*>(ptr), " (single object)");
					delete ptr;
				}
				allocatedPointers.erase(it);  // Xóa con trỏ khỏi cache
#ifdef _DEBUG
			}
			else {
				Log::E(TAG, "Type mismatch. Cannot deallocate.");
			}
#endif
		}
		else {
			Log::E(TAG, "Pointer not found in cache. Cannot deallocate.");
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
