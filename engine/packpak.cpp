#include "pch.h"
#include "base.h"

bool CompressData(const unsigned char* pSrcBuffer, unsigned int nSrcLen,
	unsigned char* pDestBuffer, unsigned int* pDestLen, int nCompressLevel)
{
	return (!ucl_nrv2b_99_compress(pSrcBuffer, nSrcLen, pDestBuffer, pDestLen, NULL, nCompressLevel, NULL, NULL));
}

//Find the location of the sub-file in the package. If it is found, it returns true, and uIndex returns the found location; if it is not found, it returns false, and uIndex returns the location where it should be inserted.
/**
 * @brief Find the location of the sub-file in the package. If it is found, it returns true, and uIndex returns the found location; if it is not found, it returns false, and uIndex returns the location where it should be inserted.
 * @param uElemId The ID of the file, obtained after hashing
 * @param nPakIndex pak reference used
 * @param uIndex returns the found position, reference
 * @return If it is not found, it returns false, and uIndex returns the position where it should be inserted.
 */
bool FindElem(PACK_ITEM& currentPackItem,
	unsigned int uElemId,
	unsigned int& uIndex)
{

	int nBegin, nEnd, nMid;
	nBegin = 0;
	nEnd = currentPackItem.Header.Count - 1;
	while (nBegin <= nEnd)
	{
		nMid = (nBegin + nEnd) / 2;
		if (uElemId < currentPackItem.pIndexList[nMid].uId) // The prerequisite is: ID values ​​are stored in ascending order
		{
			nEnd = nMid - 1;
		}
		else if (uElemId > currentPackItem.pIndexList[nMid].uId)
		{
			nBegin = nMid + 1;
		}
		else
		{
			uIndex = nMid;
			return true;
		}
	}

	if (nBegin == nEnd)
		uIndex = (uElemId < currentPackItem.pIndexList[nMid].uId) ? nMid : (nMid + 1);
	else
		uIndex = nBegin;
	return false;
}

bool GenerateElemIndexAndHashId(
	PACK_ITEM& currentPackItem,
	const char* fullFolderFilePath,
	unsigned int& uElemIndex,
	unsigned int& uHashId)
{
	uHashId = g_FileNameHash(fullFolderFilePath);

	unsigned int uIndex;
	if (FindElem(currentPackItem, uHashId, uIndex))
	{
		printf("Error: %s has the same id %X!\n", fullFolderFilePath, uHashId);
		return false;
	}
	uElemIndex = uIndex;

	return true;
}

static bool AddBufferToFile(PACK_ITEM& currentPackItem,
	unsigned char* pSrcBuffer,
	int nSrcSize,
	unsigned int uCompressType,
	unsigned int& uDestSize,
	unsigned int& uDestCompressType)
{
	void* pWriteBuffer = pSrcBuffer;
	unsigned char* compressBuffer = MemoryManager::getInstance()->allocateArray<unsigned char>(COMPRESS_BUFFER_SIZE);
	uDestSize = nSrcSize;
	uDestCompressType = XPACK_METHOD_NONE;
	if (uCompressType == XPACK_METHOD_UCL)
	{
		if (CompressData(pSrcBuffer, nSrcSize, compressBuffer, &uDestSize, 5))
		{
			if (nSrcSize > (int)uDestSize)
			{	// Compress
				if (uDestSize <= COMPRESS_BUFFER_SIZE)
				{
					pWriteBuffer = compressBuffer;
					uDestCompressType = XPACK_METHOD_UCL;
				}
				else
				{
					printf("Warning : compressbuffer overflow!");
				}
			}
		}
		if (pWriteBuffer == pSrcBuffer)
			uDestSize = nSrcSize;
	}

	if (currentPackItem.pIOFile->write(pWriteBuffer, uDestSize) == uDestSize)
	{
		MemoryManager::getInstance()->deallocate(compressBuffer);
		return true;
	}

	MemoryManager::getInstance()->deallocate(compressBuffer);
	printf("Error: Cannot write XPackFileFragment\n");
	return false;
}

bool AddElemToPakFragment(PACK_ITEM& currentPackItem,
	unsigned char* pSrcBuffer,
	int nNumFragment,
	int* pFragmentSizeList,
	unsigned int& uCompressSize)
{
	uCompressSize = 0;

	unsigned int nBufferCompressType;
	unsigned int uBufferCompressSize = 0;
	int			 nFragmentInfoListSize = sizeof(XPackFileFragmentInfo) * nNumFragment;
	int nSrcOffset = 0;

	XPackFileFragmentInfo	FragmentInfoList[100];
	XPackFileFragmentInfo* pFragmentInfoList = NULL;
	if (nNumFragment <= 100)
		pFragmentInfoList = FragmentInfoList;
	else
		pFragmentInfoList = (XPackFileFragmentInfo*)malloc(nFragmentInfoListSize);
	if (!pFragmentInfoList)
		return false;

	long lItemFileBegin = currentPackItem.pIOFile->tell();

	XPackFileFragmentElemHeader fragmentHeader = { 0, 0 };

	//Step 1: [XPackFileFragmentElem Header] Reserve position
	{
		if (currentPackItem.pIOFile->write(&fragmentHeader, sizeof(XPackFileFragmentElemHeader))
			!= sizeof(XPackFileFragmentElemHeader))
		{
			printf("Error: Cannot write XPackFileFragmentElemHeader [%s]\n", currentPackItem.pIOFile->getFullPath());
			goto ERROR_EXIT;
		}
		fragmentHeader.nFragmentInfoOffset += sizeof(XPackFileFragmentElemHeader);
	}

	// Step 2: Store each block of data
	for (int nFragment = 0; nFragment < nNumFragment; nFragment++)
	{
		if (!AddBufferToFile(currentPackItem,
			pSrcBuffer + nSrcOffset,
			pFragmentSizeList[nFragment],
			XPACK_METHOD_UCL,
			uBufferCompressSize,
			nBufferCompressType))
		{
			printf("Error: Cannot add fragment of [%s]", currentPackItem.pIOFile->getFullPath());
			goto ERROR_EXIT;
		}
		nSrcOffset += pFragmentSizeList[nFragment];
		pFragmentInfoList[nFragment].uSize = pFragmentSizeList[nFragment];
		pFragmentInfoList[nFragment].uOffset = fragmentHeader.nFragmentInfoOffset;
		pFragmentInfoList[nFragment].uCompressSizeFlag = uBufferCompressSize | nBufferCompressType;
		fragmentHeader.nFragmentInfoOffset += uBufferCompressSize;
		if (fragmentHeader.nFragmentInfoOffset + nFragmentInfoListSize > MAX_SUPPORTABLE_STORE_SIZE)
		{
			printf("Warning: file store size exceed limit [%s].", currentPackItem.pIOFile->getFullPath());
			goto ERROR_EXIT;
		}
	}

	//Step 3: [nFragment¸öXPackFileFragmentInfoµÄÊý×é]
	{
		if (currentPackItem.pIOFile->write(pFragmentInfoList, nFragmentInfoListSize) != nFragmentInfoListSize)
		{
			printf("Error: Cannot write XPackFileFragmentInfo [%s]\n", currentPackItem.pIOFile->getFullPath());
			goto ERROR_EXIT;
		}
	}

	//Step 4: [XPackFileFragmentElemHeader] ÖØÐÂÐ´Èë
	currentPackItem.pIOFile->seek(lItemFileBegin, SEEK_SET);
	fragmentHeader.nNumFragment = nNumFragment;
	if (currentPackItem.pIOFile->write(&fragmentHeader, sizeof(XPackFileFragmentElemHeader))
		!= sizeof(XPackFileFragmentElemHeader))
	{
		printf("Error: Cannot write XPackFileFragmentElemHeader [%s]\n", currentPackItem.pIOFile->getFullPath());
		goto ERROR_EXIT;
	}
	uCompressSize = fragmentHeader.nFragmentInfoOffset + nFragmentInfoListSize;
	currentPackItem.pIOFile->seek(lItemFileBegin + uCompressSize, SEEK_SET);

	if (nNumFragment > 100)
		SAFE_FREE(pFragmentInfoList);
	return true;

ERROR_EXIT:
	if (nNumFragment > 100)
		SAFE_FREE(pFragmentInfoList);
	return false;
}


bool AddElemToPakFragmentSPR(PACK_ITEM& currentPackItem,
	unsigned char* pSrcBuffer,
	int nSrcSize,
	unsigned int& uCompressSize)
{
	uCompressSize = 0;
	SPRFileHead* head = (SPRFileHead*)pSrcBuffer;
	unsigned int const uSprHeadSize = sizeof(SPRFileHead) + head->ColorCounts * 3;
	unsigned int const uOffsetTableSize = sizeof(FrameOffsetInfo) * head->FrameCounts;
	FrameOffsetInfo* const pSprOffsTable = (FrameOffsetInfo*)(pSrcBuffer + uSprHeadSize);

	int nNumFragment = head->FrameCounts + 2;
	int	nFragmentSizeList[100];
	int* pFragmentSizeList = nullptr;
	if (nNumFragment <= 100)
		pFragmentSizeList = nFragmentSizeList;
	else
		pFragmentSizeList = (int*)malloc(sizeof(int) * nNumFragment);
	if (!pFragmentSizeList)
		return false;

	pFragmentSizeList[0] = uSprHeadSize;
	pFragmentSizeList[1] = uOffsetTableSize;
	for (int i = 0; i < head->FrameCounts; i++)
	{
		FrameOffsetInfo* pSprOffs = pSprOffsTable + i;
		pFragmentSizeList[i + 2] = pSprOffs->DataLength;
	}

	bool bResult = AddElemToPakFragment(currentPackItem, pSrcBuffer, nNumFragment, pFragmentSizeList, uCompressSize);
	if (nNumFragment > 100) {
		SAFE_FREE(pFragmentSizeList);
	}
	return bResult;
}

bool AddElemToPakCommon(PACK_ITEM& currentPackItem,
	unsigned char* pSrcBuffer,
	int nSrcSize,
	unsigned int& uCompressType,
	unsigned int& uCompressSize)
{
	if (nSrcSize <= COMMON_FILE_SPLIT_SIZE)
		return AddBufferToFile(currentPackItem, pSrcBuffer, nSrcSize, uCompressType, uCompressSize, uCompressType);
	int nNumFragment = (nSrcSize + COMMON_FILE_SPLIT_SIZE - 1) / COMMON_FILE_SPLIT_SIZE;
	int	nFragmentSizeList[20];
	int* pFragmentSizeList = NULL;
	if (nNumFragment <= 20)
		pFragmentSizeList = nFragmentSizeList;
	else
		pFragmentSizeList = (int*)malloc(sizeof(int) * nNumFragment);
	if (!pFragmentSizeList)
		return false;
	for (int i = 0; i < nNumFragment; i++)
		pFragmentSizeList[i] = COMMON_FILE_SPLIT_SIZE;
	if (nSrcSize % COMMON_FILE_SPLIT_SIZE)
		pFragmentSizeList[nNumFragment - 1] = nSrcSize % COMMON_FILE_SPLIT_SIZE;
	uCompressType = XPACK_FLAG_FRAGMENT;
	bool bResult = AddElemToPakFragment(currentPackItem, pSrcBuffer, nNumFragment, pFragmentSizeList, uCompressSize);
	if (nNumFragment > 20)
		SAFE_FREE(pFragmentSizeList);
	return bResult;
}



/**
 * Add 1 file vào tệp pak.
 *
 * @param currentPackItem
 * @param partnerInfo
 * @param fullFolderFilePath
 * @param elementFileRootPathNotEnderLen: độ dài chuỗi từ thư mục gốc đến relative path, giả
 * sử chuỗi là C:/Data/file.txt thì elementFileRootPathNotEnderLen = độ dài của C:/Data
 * relative path = /file.txt
 * @param packFileShellOptionSprSplitFrameBalance: Nếu kích thước file SPR > packFileShellOptionSprSplitFrameBalance,
 * thì sẽ nén theo kiểu chia nhỏ (fragment)
 *
 * @return bool: Add file thành công hay không
 */
bool AddFileToPak(PACK_ITEM& currentPackItem,
	KPackFilePartner& partnerInfo,
	const char* fullFolderFilePath,
	int elementFileRootPathNotEnderLen,
	int packFileShellOptionSprSplitFrameBalance)
{
	unsigned int uElemIndex = 0;
	unsigned int uHashId = 0;
	unsigned int uCRC = 0;
	if (!GenerateElemIndexAndHashId(currentPackItem,
		fullFolderFilePath + elementFileRootPathNotEnderLen,
		uElemIndex,
		uHashId))
		return false;


	std::ifstream srcFile(fullFolderFilePath, std::ios::binary | std::ios::ate);
	if (!srcFile.is_open()) {
		return false;
	}

	int	nSrcSize = GetFileStreamSize(srcFile);
	if (nSrcSize == -1)
		return false;

	if (nSrcSize == 0)
		return true;

	unsigned int uCompressType = XPACK_METHOD_UCL;
	unsigned int uCompressSize;

	unsigned char* pSrcBuffer = (unsigned char*)GetFileStreamBuffer(srcFile);
	if (pSrcBuffer == nullptr)
		return false;

	const char* fileExtension = strrchr(fullFolderFilePath, '.');

	//Kiểm tra có phải file SPR hay không
	if (fileExtension && !_stricmp(fileExtension + 1, "spr"))
	{
		if ((unsigned int)nSrcSize >= packFileShellOptionSprSplitFrameBalance)
		{
			SPRFileHead* pSpr = (SPRFileHead*)pSrcBuffer;
			if (*(int*)(&(pSpr->VersionInfo)) == SPR_COMMENT_FLAG && pSpr->FrameCounts > 1)
				uCompressType = XPACK_FLAG_FRAGMENT;
		}
	}
	currentPackItem.pIOFile->seek(currentPackItem.nDataEndOffset, std::ios::beg);

	bool bOk = false;
	if (uCompressType == XPACK_FLAG_FRAGMENT)
		bOk = AddElemToPakFragmentSPR(currentPackItem, pSrcBuffer, nSrcSize, uCompressSize);
	else
		bOk = AddElemToPakCommon(currentPackItem, pSrcBuffer, nSrcSize, uCompressType, uCompressSize);

	uCRC = Misc_CRC32(0, pSrcBuffer, nSrcSize);
	srcFile.close();
	if (bOk)
	{
		for (unsigned int i = currentPackItem.Header.Count; i > uElemIndex; i--)
			currentPackItem.pIndexList[i] = currentPackItem.pIndexList[i - 1];
		currentPackItem.Header.Count++;
		currentPackItem.pIndexList[uElemIndex].uCompressSizeFlag = uCompressSize | uCompressType;
		currentPackItem.pIndexList[uElemIndex].uSize = nSrcSize;
		currentPackItem.pIndexList[uElemIndex].uId = uHashId;
		currentPackItem.pIndexList[uElemIndex].uOffset = currentPackItem.nDataEndOffset;
		currentPackItem.nDataEndOffset += uCompressSize;
		currentPackItem.bModified = true;

		KPackFilePartner::PACKPARTNER_ELEM_INFO	info;
		info.nElemIndex = uElemIndex;
		strcpy_s(info.szFileName, fullFolderFilePath + elementFileRootPathNotEnderLen);
		info.uCRC = uCRC;
		info.uId = uHashId;
		info.uSize = nSrcSize;
		info.uStoreSizeAndCompressFlag = uCompressSize | uCompressType;
		info.uTime = GetCurrentLocalTimeSecond();
		partnerInfo.AddElem(info);
		assert(bOk);
	}
	MemoryManager::getInstance()->deallocate((char*)(pSrcBuffer));
	return bOk;
}


void CompressFolderToPakFileInternal(const char* inputFolderPath,
	const char* outputFolderPath, 
	bool bExcludeOfCheckId, 
	ProgressCallbackInternal progressCallback) {
	double progress = 0;

	fs::path inputPath = fs::absolute(inputFolderPath);
	fs::path outputPath = fs::absolute(outputFolderPath);

	if (!fs::exists(inputPath) && !fs::is_directory(inputPath)) {
		throw std::runtime_error("Not found folder: " + inputPath.string());
	}
	std::string folderName = inputPath.filename().string();

	fs::create_directories(outputPath);
	std::string outputFileName = outputPath.string() + "/" + folderName + ".pak";
	std::string outputPartnerFileName = outputPath.string() + "/" + folderName + ".pak.txt";

	// Create new pak item
	progressCallback(progress, "Start creating new pak item!");
	PACK_ITEM currentPakItem = PACK_ITEM();
	*(int*)(&(currentPakItem.Header.Signature)) = IPACK_FILE_SIGNATURE_FLAG;
	currentPakItem.pIOFile = g_OpenFile(outputFileName.c_str(), false, true, true);
	currentPakItem.pIndexList = (XPackIndexInfo*)malloc(sizeof(XPackIndexInfo) * PACK_FILE_SHELL_MAX_SUPPORT_ELEM_FILE_NUM);
	if (currentPakItem.pIOFile->write(&(currentPakItem.Header), sizeof(currentPakItem.Header)) != sizeof(currentPakItem.Header))
		throw std::runtime_error("Failed to write pak header!");
	currentPakItem.Header.Data = sizeof(currentPakItem.Header);
	currentPakItem.nDataEndOffset = sizeof(currentPakItem.Header);
	currentPakItem.bModified = true;
	currentPakItem.bExcludeOfCheckId = (bExcludeOfCheckId != false);

	// Create pak part info
	KPackFilePartner filePartner;
	if (!filePartner.Init()) {
		throw std::runtime_error("Can not init KPackFilePartner");
	}

	if (currentPakItem.pIOFile != nullptr) {
		if (!currentPakItem.pIOFile->isOpen()) {
			throw std::runtime_error("Can not open file: '" + outputFileName + "' for writing objects!");
		}
	}
	else {
		throw std::runtime_error("Can not open file: '" + outputFileName + "' for writing objects!");
	}

	progress = 5;
	size_t fileCount = countRegualarFilesInFolder(inputPath);
	double progressPerFileAdded = (1.0 / fileCount) * 80;
	progressCallback(progress, "Start creating new pak item!");

	double totalAddFileProgress = 80;
	for (const auto& entry : fs::recursive_directory_iterator(inputPath)) {
		if (fs::is_regular_file(entry)) {
			std::string relativePath = fs::relative(entry.path(), inputPath.parent_path()).string();
			std::string absolutePath = entry.path().string();
			size_t ulen = absolutePath.length() - relativePath.length() - 1;
			const char* filePath = absolutePath.c_str();
			AddFileToPak(currentPakItem,
				filePartner,
				filePath,
				ulen,
				IPACK_FILE_SHELL_OPTION_SPR_SPLIT_FRAME_BALANCE_DEF
			);
			progress += progressPerFileAdded;
			progressCallback(progress, "Added an item to pak.");
		}
	}

	progressCallback(progress, "Saving to pak...");
	// Write block header 
	if (currentPakItem.pIOFile) {
		if (currentPakItem.bModified && currentPakItem.pIndexList) {
			int nLen = sizeof(XPackIndexInfo) * currentPakItem.Header.Count;
			currentPakItem.Header.CRC32 = Misc_CRC32(0, currentPakItem.pIndexList, nLen);
			currentPakItem.pIOFile->seek(currentPakItem.nDataEndOffset, SEEK_SET);
			currentPakItem.pIOFile->write(currentPakItem.pIndexList, nLen);
			currentPakItem.pIOFile->seek(0, SEEK_SET);
			currentPakItem.Header.Index = currentPakItem.nDataEndOffset;
			currentPakItem.Header.PakTime = GetCurrentLocalTimeSecond();
			currentPakItem.pIOFile->write(&currentPakItem.Header, sizeof(currentPakItem.Header));
			filePartner.Save(outputPartnerFileName.c_str(), currentPakItem.Header.PakTime, currentPakItem.Header.CRC32);
		}
	}

	currentPakItem.pIOFile->close();
	currentPakItem.pIOFile->release();
	SAFE_FREE(currentPakItem.pIndexList);
	filePartner.Clear();

	progress = 100;
	progressCallback(progress, "Done");

	Log::I("Hoàn thành việc ghi danh sách file vào ", outputFileName);
}
