#ifndef _ENGINE_PAK_H_
#define _ENGINE_PAK_H_

#include <comdef.h>
#include <memory>
#include <vector>
#include <iostream>
#include <fstream>
#include <sstream>
#include <cassert>
#include <string>
#include "ucl/ucl.h"
#include <functional>
#include <unordered_map>
#include <filesystem>
#include <regex>
#include "utils.h"
#include "file.h"
#include "pakpart.h"
#include "MemoryManager.h"

namespace fs = std::filesystem;

/// <summary>
/// This struct for C# comunicating
/// </summary>
struct CompressedFileInfo {
	int index;
	char* id;
	unsigned long idValue;
	char* time;
	char* fileName;
	int size;
	int inPakSize;
	int comprFlag;
	char* crc;

	void freeMem() {
		MemoryManager* memoryManager = MemoryManager::getInstance();

		if (id) {
			memoryManager->deallocate(id);  // Giải phóng id
		}
		if (time) {
			memoryManager->deallocate(time);  // Giải phóng time
		}
		if (fileName) {
			memoryManager->deallocate(fileName);  // Giải phóng fileName
		}
		if (crc) {
			memoryManager->deallocate(crc);  // Giải phóng crc
		}
	}
};

/// <summary>
/// This struct for C# comunicating
/// </summary>
struct PakInfo {
	int totalFiles;
	char* pakTime;
	char* pakTimeSave;
	char* crc;

	CompressedFileInfo* files; // Mảng các CompressedFileInfo
	int fileCount;             // Số lượng file trong mảng

	void freeMem() {
		MemoryManager* memoryManager = MemoryManager::getInstance();

		if (pakTime) {
			memoryManager->deallocate(pakTime);  // Giải phóng pakTime
			pakTime = nullptr;
		}
		if (pakTimeSave) {
			memoryManager->deallocate(pakTimeSave);  // Giải phóng pakTimeSave
			pakTimeSave = nullptr;
		}
		if (crc) {
			memoryManager->deallocate(crc);  // Giải phóng crc
			crc = nullptr;
		}

		if (files) {
			// Giải phóng từng phần tử của mảng files
			for (int i = 0; i < fileCount; i++) {
				files[i].freeMem();  // Gọi freeMem của từng CompressedFileInfo
			}

			memoryManager->deallocate(files);  // Giải phóng toàn bộ mảng files
			files = nullptr;
		}
	}
};


struct CompressedFileInfoInternal {
	int index;
	std::string id;
	unsigned long idValue;
	std::string time;
	std::string fileName;
	int size;
	int inPakSize;
	int comprFlag;
	std::string crc;
	void convertIdToValue() {
		idValue = std::stoul(id, nullptr, 16);
	}
};

struct PakInfoInternal {
	int totalFiles;
	std::string pakTime;
	std::string pakTimeSave;
	std::string crc;
	std::unordered_map<unsigned long, CompressedFileInfoInternal> fileMap;

	void addFile(const CompressedFileInfoInternal& file) {
		fileMap[file.idValue] = file;
	}

	std::unique_ptr<CompressedFileInfoInternal> findFileByIdValue(unsigned long searchIdValue) {
		auto it = fileMap.find(searchIdValue);
		if (it != fileMap.end()) {
			return std::make_unique<CompressedFileInfoInternal>(it->second);
		}
		return nullptr;
	}

	// Hàm chuyển đổi từ PakInfoInternal thành PakInfo
	void ConvertToPakInfo(PakInfo& external) {
		// Sao chép các thông tin cơ bản
		external.totalFiles = totalFiles;

		size_t pakTimeLen = pakTime.length() + 1;
		external.pakTime = MemoryManager::getInstance()->allocateArray<char>(pakTimeLen);
		strcpy_s(external.pakTime, pakTimeLen, pakTime.c_str());
		size_t pakTimeSaveLen = pakTimeSave.length() + 1;
		external.pakTimeSave = MemoryManager::getInstance()->allocateArray<char>(pakTimeSaveLen);
		strcpy_s(external.pakTimeSave, pakTimeSaveLen, pakTimeSave.c_str());
		size_t crcLen = crc.length() + 1;
		external.crc = MemoryManager::getInstance()->allocateArray<char>(crcLen);
		strcpy_s(external.crc, crcLen, crc.c_str());

		// Sao chép các file từ fileMap sang mảng files
		external.fileCount = fileMap.size();
		external.files = MemoryManager::getInstance()->allocateArray<CompressedFileInfo>(external.fileCount); // Cấp phát bộ nhớ cho mảng files

		int i = 0;
		for (const auto& pair : fileMap) {
			const CompressedFileInfoInternal& internalFile = pair.second;

			// Sao chép các thuộc tính từ internalFile sang externalFile
			external.files[i].index = internalFile.index;
			// Thay thế _strdup bằng MemoryManager
			size_t idLen = internalFile.id.length() + 1;
			external.files[i].id = MemoryManager::getInstance()->allocateArray<char>(idLen);
			strcpy_s(external.files[i].id, idLen, internalFile.id.c_str());
			external.files[i].idValue = internalFile.idValue;
			size_t timeLen = internalFile.time.length() + 1;
			external.files[i].time = MemoryManager::getInstance()->allocateArray<char>(timeLen);
			strcpy_s(external.files[i].time, timeLen, internalFile.time.c_str());
			size_t fileNameLen = internalFile.fileName.length() + 1;
			external.files[i].fileName = MemoryManager::getInstance()->allocateArray<char>(fileNameLen);
			strcpy_s(external.files[i].fileName, fileNameLen, internalFile.fileName.c_str());
			external.files[i].size = internalFile.size;
			external.files[i].inPakSize = internalFile.inPakSize;
			external.files[i].comprFlag = internalFile.comprFlag;
			size_t crcLen = internalFile.crc.length() + 1;
			external.files[i].crc = MemoryManager::getInstance()->allocateArray<char>(crcLen);
			strcpy_s(external.files[i].crc, crcLen, internalFile.crc.c_str());
			i++;
		}
	}

};

struct PakHeader {
	unsigned char Signature[4];
	unsigned long Count;
	unsigned long Index;
	unsigned long Data;
	unsigned long CRC32;
	unsigned int  PakTime;
	unsigned char Reserved[8];
};

struct PakBlockHeader {
	unsigned long ID;
	unsigned long Offset;
	unsigned long RealLength;
	BYTE Length[3];
	unsigned char Method;

	PakBlockHeader& operator=(const PakBlockHeader& other) {
		if (this != &other) {
			ID = other.ID;
			Offset = other.Offset;
			RealLength = other.RealLength;
			std::copy(std::begin(other.Length), std::end(other.Length), std::begin(Length));
			Method = other.Method;
		}
		return *this;
	}
};

enum IPACK_FILE_SHELL_PARAM
{
	PACK_FILE_SHELL_MAX_SUPPORT_PAK_NUM = 24, //The maximum number of package files supp,orted at the same time
	PACK_FILE_SHELL_MAX_SUPPORT_ELEM_FILE_NUM = 200000, //The maximum number of sub-files that can be included in a package file
	IPACK_FILE_SHELL_OPTION_SPR_SPLIT_FRAME_BALANCE_MIN = 100,
	IPACK_FILE_SHELL_OPTION_SPR_SPLIT_FRAME_BALANCE_MAX = 1000,
	IPACK_FILE_SIGNATURE_FLAG = 0x4b434150, //'PACK', package file character.
	IPACK_FILE_SHELL_OPTION_SPR_SPLIT_FRAME_BALANCE_DEF = 131072, //128K, the default is that if the spr file is larger than 128K, it will be compressed by frames.
	//Adjust this setting through SetOption(IPACK_FILE_SHELL_OPTION_SPR_SPLIT_FRAME_BALANCE..)

};

enum XPACK_METHOD_AND_FLAG {
	XPACK_METHOD_NONE = 0x00000000,
	XPACK_METHOD_UCL = 0x20000000,
	XPACK_METHOD_FILTER = 0xf0000000,
	XPACK_FLAG_FRAGMENT = 0x10000000, // pack bằng cách chia nhỏ file theo từng fragment
	XPACK_COMPRESS_SIZE_FILTER = 0x07ffffff,
	XPACK_COMPRESS_SIZE_BIT = 27
};

struct XPackIndexInfo {
	unsigned int uId;
	unsigned int uOffset;
	unsigned int uSize;
	unsigned int uCompressSizeFlag;

	unsigned int getPackMethod() {
		unsigned int pakMethod = uCompressSizeFlag & XPACK_METHOD_FILTER;
		return pakMethod;
	}

	unsigned int getStoredSize() {
		unsigned int storedSize = uCompressSizeFlag & XPACK_COMPRESS_SIZE_FILTER;
		return storedSize;
	}

	// Kiểm tra block có nén theo dạng phân mảnh 
	bool isBlockFragment() {
		return (uCompressSizeFlag & XPACK_FLAG_FRAGMENT) != 0;
	}
};

struct XPackFileFragmentElemHeader {
	int nNumFragment;
	int nFragmentInfoOffset;
};

struct XPackFileFragmentInfo {
	unsigned int uOffset;
	unsigned int uSize;
	unsigned int uCompressSizeFlag;

	unsigned int getStoredSize() {
		unsigned int storedSize = uCompressSizeFlag & XPACK_COMPRESS_SIZE_FILTER;
		return storedSize;
	}

	unsigned int getPackMethod() {
		unsigned int pakMethod = uCompressSizeFlag & XPACK_METHOD_FILTER;
		return pakMethod;
	}
};


struct PACK_ITEM
{
	IFile* pIOFile;
	PakHeader		Header;
	XPackIndexInfo* pIndexList;			//Sub file index list buffer
	int				nDataEndOffset;	    //The offset position of the end position of the current packed file data (relative to the file header)
	bool			bModified;			//has been modified
	bool			bExcludeOfCheckId;	//Whether it is excluded from checking the same id
	char			PackFileName[MAX_PATH];	//Package file name
};

#define COMMON_FILE_SPLIT_SIZE		2097152		//2M
#define COMPRESS_BUFFER_SIZE		4194304		//4M
#define MAX_SUPPORTABLE_STORE_SIZE	XPACK_COMPRESS_SIZE_FILTER
#define PAK_SIGNATURE "PACK"

int ParsePakInfoFileInternal(const char* filename, PakInfoInternal& pakInfo);

int ExtractPakInternal(const char* pakfilePath,
	const char* outputRootPath,
	PakInfoInternal pakInfo,
	std::unique_ptr<PakHeader>& header);

int ExtractPakInternal(const char* pakfilePath,
	const char* outputRootPath,
	std::unique_ptr<PakHeader>& header);

bool AddFileToPak(PACK_ITEM& currentPackItem,
	KPackFilePartner& partnerInfo,
	const char* fullFolderFilePath,
	int elementFileRootPathNotEnderLen,
	int packFileShellOptionSprSplitFrameBalance);
void CompressFolderToPakFileInternal(const char* inputFolderPath,
	const char* outputFolderPath,
	bool bExcludeOfCheckId);

//Lowercase a string and then convert it into a hash value
inline unsigned int g_FileNameHash(const char* pString)
{
	unsigned int Id = 0;
	char c = 0;
	for (int i = 0; pString[i]; i++)
	{
		c = pString[i];
		if (c >= 'A' && c <= 'Z')
			c += 0x20;	//The last bytes of Chinese characters may also be converted, artificially increasing the probability of repeated codes■
		else if (c == '/')
			c = '\\';
		Id = (Id + (i + 1) * c) % 0x8000000b * 0xffffffef;
	}
	return (Id ^ 0x12345678);
}
inline unsigned int g_FileNameHash2(const char* a1)
{
	const char* v1; // ecx@1
	char v2; // al@1
	int i; // esi@1
	char v4; // dl@2
	unsigned int v5; // edx@6

	v1 = a1;
	v2 = *a1;
	for (i = 0; *v1; v2 = *v1)
	{
		v4 = v2;
		if ((unsigned __int8)(v2 - 65) > 0x19u)
		{
			if (v2 == 47)
				v4 = 92;
		}
		else
		{
			v4 = v2 + 32;
		}
		v5 = (i + v4 * (signed int)&(v1++)[1 - (DWORD)a1]) % 0x8000000B;
		i = -17 * v5;
	}
	return i ^ 0x12345678;
}

#endif