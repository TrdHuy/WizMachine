#include "pch.h"
#include "pak.h"

int ParsePakInfoFileInternal(const char* filename, PakInfoInternal& pakInfo) {
	std::ifstream file(filename);
	if (!file.is_open()) {
		std::cerr << "Error opening file." << std::endl;
		return -1;
	}

	std::string line;
	getline(file, line);
	std::istringstream iss(line);

	std::string part;
	while (std::getline(iss, part, '\t')) {
		std::istringstream partStream(part);
		std::string key;
		std::string value;
		std::getline(partStream, key, ':');
		std::getline(partStream, value);

		if (key == "TotalFile") {
			pakInfo.totalFiles = std::stoi(value);
		}
		else if (key == "PakTime") {
			pakInfo.pakTime = value;
		}
		else if (key == "PakTimeSave") {
			pakInfo.pakTimeSave = value;
		}
		else if (key == "CRC") {
			pakInfo.crc = value;
		}
	}

	// Skipping the header line of the column titles
	std::getline(file, line);
	std::regex lineFormat(R"((\d+)\t(\w+)\t(\d{4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2}:\d{1,2})\t(.+?)\t(\d+)\t(\d+)\t(\d)\t(\w+))");
	std::smatch matches;

	while (std::getline(file, line)) {
		if (std::regex_match(line, matches, lineFormat)) {
			CompressedFileInfoInternal fi;
			fi.index = std::stoi(matches[1].str());
			fi.id = matches[2].str();
			fi.time = matches[3].str();
			fi.fileName = matches[4].str();
			fi.size = std::stoi(matches[5].str());
			fi.inPakSize = std::stoi(matches[6].str());
			fi.comprFlag = std::stoi(matches[7].str());
			fi.crc = matches[8].str();
			fi.convertIdToValue();
			pakInfo.addFile(fi);
		}
	}

	file.close();
	return 0;
}

bool DecompressData(char* pSrcBuffer, unsigned nSrcLen, unsigned char* pDestBuffer, unsigned int uExtractSize)
{
	unsigned int uDestLen = 0;
	ucl_nrv2b_decompress_8((unsigned char*)pSrcBuffer, nSrcLen, pDestBuffer, &uDestLen, NULL);
	return (uDestLen == uExtractSize);
}

unsigned long GetCompressSize(const PakBlockHeader* header) {
	return (((unsigned long)header->Length[2]) << 16) | (((unsigned long)header->Length[1]) << 8) | header->Length[0];
}

bool ReadElemBufferFromPak(std::ifstream& pakFile, unsigned int offset, unsigned int storedSize,
	unsigned int pakMethod, char* buffer, unsigned int size) {
	pakFile.seekg(offset, std::ios::beg);

	if (pakMethod == XPACK_METHOD_NONE) {
		assert(storedSize == size);
		pakFile.read(buffer, size);
		return true;
	}
	else if (storedSize <= 4194304 && pakMethod == XPACK_METHOD_UCL) { // 4MB
		std::unique_ptr<char[]> compressBuffer(new char[storedSize]);
		pakFile.read(compressBuffer.get(), storedSize);
		return DecompressData(compressBuffer.get(), storedSize, reinterpret_cast<unsigned char*>(buffer), size);
	}
	return false;
}

std::unique_ptr<BYTE[]> ReadBlock(int block
	, int blockCount
	, int blockHeaderIndex
	, std::ifstream& file
	, int* decompressLenght
	, std::function<bool(PakBlockHeader)> blockHeaderPredicate = nullptr) {
	if (!file.is_open() || block < 0 || block >= blockCount) {
		return nullptr;
	}

	auto blockHeaderBuffer = std::make_unique<unsigned char[]>(sizeof(PakBlockHeader));
	int blockHeaderOffset = blockHeaderIndex + block * sizeof(PakBlockHeader);

	file.seekg(blockHeaderOffset, std::ios::beg);
	file.read(reinterpret_cast<char*>(blockHeaderBuffer.get()), sizeof(PakBlockHeader));
	auto* pBlockHeader = reinterpret_cast<PakBlockHeader*>(blockHeaderBuffer.get());

	if (pBlockHeader->ID <= 0 || pBlockHeader->RealLength <= 0 || pBlockHeader->Offset <= 0) {
		return nullptr;
	}

	if (blockHeaderPredicate != nullptr && !blockHeaderPredicate(*pBlockHeader)) {
		return nullptr;
	}

	*decompressLenght = pBlockHeader->RealLength;

	unsigned long size = GetCompressSize(pBlockHeader);
	auto blockBuffer = std::make_unique<unsigned char[]>(size);

	file.seekg(pBlockHeader->Offset, std::ios::beg);
	file.read(reinterpret_cast<char*>(blockBuffer.get()), size);

	XPackIndexInfo* xPackIndexInfo = nullptr;
	xPackIndexInfo = reinterpret_cast<XPackIndexInfo*>(pBlockHeader);
	bool bOk = true;
	auto decompressedBuffer = std::make_unique<unsigned char[]>(xPackIndexInfo->uSize);


	// Nếu không chứa cờ FRAGMENT
	if ((xPackIndexInfo->uCompressSizeFlag & XPACK_FLAG_FRAGMENT) == 0)
	{
		unsigned int pakMethod = xPackIndexInfo->uCompressSizeFlag & XPACK_METHOD_FILTER;
		unsigned int storedSize = xPackIndexInfo->uCompressSizeFlag & XPACK_COMPRESS_SIZE_FILTER;
		bOk = ReadElemBufferFromPak(file, xPackIndexInfo->uOffset, storedSize,
			pakMethod, (char*)decompressedBuffer.get(), xPackIndexInfo->uSize);
		return decompressedBuffer;
	}
	XPackFileFragmentElemHeader header;
	if (!ReadElemBufferFromPak(file, xPackIndexInfo->uOffset,
		sizeof(header), XPACK_METHOD_NONE, (char*)&header, sizeof(header)))
	{
		bOk = false;
		return nullptr;
	}
	unsigned uSize = 0;
	for (int i = 0; i < header.nNumFragment; i++)
	{
		XPackFileFragmentInfo fragment;
		if (!ReadElemBufferFromPak(file, xPackIndexInfo->uOffset + header.nFragmentInfoOffset + sizeof(fragment) * i,
			sizeof(fragment), XPACK_METHOD_NONE, (char*)&fragment, sizeof(fragment)))
		{
			bOk = false;
			break;
		}
		if (!ReadElemBufferFromPak(file, xPackIndexInfo->uOffset + fragment.uOffset, (fragment.uCompressSizeFlag & XPACK_COMPRESS_SIZE_FILTER),
			(fragment.uCompressSizeFlag & XPACK_METHOD_FILTER), (char*)decompressedBuffer.get() + uSize, fragment.uSize))
		{
			bOk = false;
			break;
		}
		uSize += fragment.uSize;
	}
	return decompressedBuffer;
}

int ExtractPakInternal(const char* pakfilePath,
	const char* outputRootPath,
	PakInfoInternal pakInfo,
	std::unique_ptr<PakHeader>& header) {
	std::ifstream file(pakfilePath, std::ios::binary | std::ios::ate);
	if (!file.is_open()) {
		return 0;
	}

	file.seekg(0, std::ios::beg);
	auto headerBuffer = std::make_unique<unsigned char[]>(sizeof(PakHeader));
	file.read(reinterpret_cast<char*>(headerBuffer.get()), sizeof(PakHeader));
	header.reset(reinterpret_cast<PakHeader*>(headerBuffer.release()));

	if (!file) {
		return 0;
	}

	int blockCount = header->Count;
	for (int block = 0; block < blockCount; ++block) {
		int decrompressLenght;
		CompressedFileInfoInternal compressInfo;
		bool infoFound = false;

		auto extractedBuffer = ReadBlock(block,
			blockCount,
			header->Index,
			file,
			&decrompressLenght,
			[&pakInfo, &compressInfo, &infoFound](PakBlockHeader header) {
				auto fileResult = pakInfo.findFileByIdValue(header.ID);
				if (fileResult) {
					compressInfo = *fileResult.get();
					infoFound = true;
				}
				return true;
			});
		if (extractedBuffer) {
			std::string rootPath = std::string(outputRootPath);
			std::string outFileName = "extracted_block_" + std::to_string(block) + ".bin";
			// TODO: hỗ trợ kiểm tra có phải file spr hay không trong trường hợp không tìm thấy extract file map
			if (infoFound && !compressInfo.fileName.empty()) {
				outFileName = compressInfo.fileName;
			}
			outFileName = rootPath + "\\" + outFileName;

			// Ensure the directory path exists
			fs::path filePath(outFileName);
			fs::create_directories(filePath.parent_path());

			std::ofstream outFile(filePath, std::ios::binary);
			if (!outFile.is_open()) {
				std::cerr << "Failed to open output file: " << outFileName << std::endl;
				continue;
			}
			outFile.write(reinterpret_cast<const char*>(extractedBuffer.get()), decrompressLenght);
			outFile.close();
		}
	}

	file.close();
	return blockCount;
}

int ExtractPakInternal(const char* pakfilePath,
	const char* outputRootPath,
	std::unique_ptr<PakHeader>& header) {
	//std::ifstream file(pakfilePath, std::ios::binary | std::ios::ate);
	//if (!file.is_open()) {
	//	return 0;
	//}

	//file.seekg(0, std::ios::beg);
	//auto headerBuffer = std::make_unique<unsigned char[]>(sizeof(PakHeader));
	//file.read(reinterpret_cast<char*>(headerBuffer.get()), sizeof(PakHeader));
	//header.reset(reinterpret_cast<PakHeader*>(headerBuffer.release()));

	//if (!file) {
	//	return 0;
	//}

	//int blockCount = header->Count;
	//for (int block = 0; block < blockCount; ++block) {
	//	int decrompressLenght;
	//	CompressedFileInfoInternal compressInfo;
	//	PakBlockHeader blockHeader;
	//	auto extractedBuffer = ReadBlock(block,
	//		blockCount,
	//		header->Index,
	//		file,
	//		&decrompressLenght,
	//		[&blockHeader](PakBlockHeader header) {
	//			blockHeader = header;
	//			return true;
	//		});
	//	if (extractedBuffer) {
	//		std::string rootPath = std::string(outputRootPath);
	//		std::string outFileName = "extracted_block_" + std::to_string(block) + ".bin";
	//		// TODO: hỗ trợ kiểm tra có phải file spr hay không trong trường hợp không tìm thấy extract file map
	//		if (infoFound && !compressInfo.fileName.empty()) {
	//			outFileName = compressInfo.fileName;
	//		}
	//		outFileName = rootPath + "\\" + outFileName;

	//		// Ensure the directory path exists
	//		fs::path filePath(outFileName);
	//		fs::create_directories(filePath.parent_path());

	//		std::ofstream outFile(filePath, std::ios::binary);
	//		if (!outFile.is_open()) {
	//			std::cerr << "Failed to open output file: " << outFileName << std::endl;
	//			continue;
	//		}
	//		outFile.write(reinterpret_cast<const char*>(extractedBuffer.get()), decrompressLenght);
	//		outFile.close();
	//	}
	//}

	//file.close();
	//return blockCount;
	return 0;
}