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

namespace fs = std::filesystem;

struct CompressedFileInfo {
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

struct PakInfo {
	int totalFiles;
	std::string pakTime;
	std::string pakTimeSave;
	std::string crc;
	std::unordered_map<unsigned long, CompressedFileInfo> fileMap;

	void addFile(const CompressedFileInfo& file) {
		fileMap[file.idValue] = file;
	}

	std::unique_ptr<CompressedFileInfo> findFileByIdValue(unsigned long searchIdValue) {
		auto it = fileMap.find(searchIdValue);
		if (it != fileMap.end()) {
			return std::make_unique<CompressedFileInfo>(it->second);
		}
		return nullptr;
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
	XPACK_FLAG_FRAGMENT = 0x10000000,
	XPACK_COMPRESS_SIZE_FILTER = 0x07ffffff,
	XPACK_COMPRESS_SIZE_BIT = 27
};

struct XPackIndexInfo {
	unsigned int uId;
	unsigned int uOffset;
	unsigned int uSize;
	unsigned int uCompressSizeFlag;
};

struct XPackFileFragmentElemHeader {
	int nNumFragment;
	int nFragmentInfoOffset;
};

struct XPackFileFragmentInfo {
	unsigned int uOffset;
	unsigned int uSize;
	unsigned int uCompressSizeFlag;
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

int ParsePakInfoFile(const char* filename, PakInfo& pakInfo);

int LoadPakInternal(const char* pakfilePath,
	const char* outputRootPath,
	PakInfo pakInfo,
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