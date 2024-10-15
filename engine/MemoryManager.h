#ifndef MEMORYMANAGER_H
#define MEMORYMANAGER_H

#include <unordered_map>
#include <mutex>
#include <typeindex>
#include <typeinfo>

struct PointerInfo {
    std::type_index type;  // Kiểu dữ liệu của đối tượng
    size_t size;           // Kích thước (nếu là mảng), 0 nếu không phải mảng
    bool isArray;          // true nếu là mảng, false nếu là biến đơn

    PointerInfo() : type(typeid(void)), size(0), isArray(false) {}

    // Hàm khởi tạo để khởi tạo 'type' và 'size'
    PointerInfo(std::type_index t, size_t s, bool array) : type(t), size(s), isArray(array) {}
};

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
        allocatedPointers[ptr] = PointerInfo(std::type_index(typeid(T)), 0, false);  // Đối với biến đơn
        std::cout << "Allocated memory at: " << ptr << " (single object of type " << typeid(T).name() << ")" << std::endl;
        return ptr;
    }

    // Cấp phát cho mảng
    template<typename T>
    T* allocateArray(size_t size) {
        std::lock_guard<std::mutex> lock(mtx);
        T* ptr = new T[size];
        allocatedPointers[ptr] = PointerInfo(std::type_index(typeid(T)), size, true);  // Đối với mảng
        std::cout << "Allocated memory at: " << ptr << " (array of " << size << " elements of type " << typeid(T).name() << ")" << std::endl;
        return ptr;
    }

    template<typename T>
    void deallocate(T* ptr) {
        std::lock_guard<std::mutex> lock(mtx);
        auto it = allocatedPointers.find(static_cast<void*>(ptr));
        if (it != allocatedPointers.end() && it->second.type == std::type_index(typeid(T))) {
            if (it->second.isArray) {
                // Giải phóng mảng
                std::cout << "Deallocated memory at: " << ptr << " (array of type " << typeid(T).name() << ", size " << it->second.size << ")" << std::endl;
                delete[] ptr;
            }
            else {
                // Giải phóng biến đơn
                std::cout << "Deallocated memory at: " << ptr << " (single object of type " << typeid(T).name() << ")" << std::endl;
                delete ptr;
            }
            allocatedPointers.erase(it);  // Xóa con trỏ khỏi cache
        }
        else {
            std::cerr << "Pointer not found in cache or type mismatch. Cannot deallocate." << std::endl;
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
