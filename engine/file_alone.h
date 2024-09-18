#ifndef _ENGINE_ALONE_FILE_H_
#define _ENGINE_ALONE_FILE_H_

#include <iostream>
#include <fstream>
#include <string>
#include <filesystem>
#include "file.h"

class AloneFile : public IFile
{
private:
	std::string fullFolderFilePath;
	std::fstream fileStream;
	void* contentBuffer = nullptr;
	unsigned long contentBufferSize = 0;

public:
	// Constructor with file path
	AloneFile() {
		contentBuffer = nullptr;
		contentBufferSize = 0;
	}

	void close() {
		if (fileStream.is_open()) {
			fileStream.close();
		}
	}

	bool isOpen() {
		return fileStream.is_open() && fileStream.good();
	}

	// Read from file
	unsigned long read(void* buffer, unsigned long readBytes) {
		if (fileStream.is_open() && fileStream.good()) {
			fileStream.read(static_cast<char*>(buffer), readBytes);
			return fileStream.gcount();
		}
		return 0;
	}
	// Write to file
	unsigned long write(const void* buffer, unsigned long writeBytes) {
		if (fileStream.is_open() && fileStream.good()) {
			fileStream.write(static_cast<const char*>(buffer), writeBytes);
			return fileStream.good() ? writeBytes : 0;
		}
		return 0;
	}

	long tell() {
		if (fileStream.is_open()) {
			return fileStream.tellg();
		}
		return 0;
	}

	long seek(long offset, int origin) {
		if (fileStream.is_open()) {
			// Set the new position based on origin
			std::ios_base::seekdir dir;
			switch (origin) {
			case std::ios::beg:
				dir = std::ios_base::beg;
				break;
			case std::ios::cur:
				dir = std::ios_base::cur;
				break;
			case std::ios::end:
				dir = std::ios_base::end;
				break;
			default:
				return -1;  // Invalid origin
			}

			fileStream.seekg(offset, dir);
			fileStream.seekp(offset, dir);

			if (fileStream.fail()) {
				fileStream.clear(); // Clear fail state
				return -1;
			}

			return fileStream.tellg();  // Return the new position
		}
		return -1;
	}

	unsigned long size() {
		if (fileStream.is_open()) {
			std::streampos currentPos = fileStream.tellg();  // Save current position
			fileStream.seekg(0, std::ios::end);
			unsigned long fileSize = fileStream.tellg();  // Get the file size
			fileStream.seekg(currentPos);  // Restore the position
			return fileSize;
		}
		return 0;
	}

	void* getBuffer() {
		if (contentBuffer == nullptr && fileStream.is_open()) {
			contentBufferSize = size();
			if (contentBufferSize > 0) {
				contentBuffer = malloc(contentBufferSize);
				if (contentBuffer) {
					seek(0, std::ios::beg);
					if (read(contentBuffer, contentBufferSize) != contentBufferSize) {
						free(contentBuffer);
						contentBuffer = nullptr;
					}
				}
			}
		}
		return contentBuffer;
	}

	void release() {
		close();  // Make sure the file is closed
		delete this;  // Self-delete the object
	}

	char* getFullPath() {
		if (fileStream.is_open()) {
			return fullFolderFilePath.data();
		}
		return nullptr;
	}

	// Destructor
	~AloneFile() {
		close();
	}

public:


	// Open file with automatic directory and file creation
	bool open(const char* fileName, bool writeSupport = false, bool forceCreate = false) {
		close();  // Ensure any previously open file is closed

		// Generate the full path for the file
		std::filesystem::path fullPath = std::filesystem::absolute(fileName);
		fullFolderFilePath = fullPath.string();

		std::string mode = "rb";  // Default read-only mode

		if (writeSupport) {
			if (!std::filesystem::exists(fullPath) || forceCreate) {
				mode = "w+b";  // Read-write mode (file will be created)
			}
			else {
				mode = "r+b";  // Read-write mode (file must exist)
			}
		}

		// Check and create directories if they do not exist
		std::filesystem::path dirPath = std::filesystem::path(fullPath).parent_path();
		if (!std::filesystem::exists(dirPath)) {
			std::filesystem::create_directories(dirPath);
		}

		// Convert std::string mode to std::ios_base::openmode
		std::ios_base::openmode fmode = std::ios::in;
		if (mode.find('w') != std::string::npos) {
			fmode = std::ios::out | std::ios::trunc;  // Create new or truncate
		}
		if (mode.find('r') != std::string::npos) {
			fmode |= std::ios::in;
		}
		if (mode.find('+') != std::string::npos) {
			fmode |= std::ios::in | std::ios::out;  // Read and write
		}
		if (mode.find('b') != std::string::npos) {
			fmode |= std::ios::binary;  // Binary mode
		}

		fileStream.open(fullPath, fmode);

		return fileStream.is_open();
	}
};

#endif //#ifndef _ENGINE_ALONE_FILE_H_