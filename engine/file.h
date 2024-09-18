#pragma once
#include "utils.h"

#ifndef _ENGINE_FILE_H_
#define _ENGINE_FILE_H_

static int s_nEngineFindFilePackFirst = 1;

class IFile
{
public:
	//Read file data
	virtual unsigned long read(void* Buffer, unsigned long ReadBytes) = 0;
	//Write file data
	virtual unsigned long write(const void* Buffer, unsigned long WriteBytes) = 0;
	// Obtain the file content Buffer. The iFile interface is responsible for releasing this buffer. External applications cannot release it by themselves.
	virtual void* getBuffer() = 0;
	//Move the file pointer position, Origin -> Initial position: SEEK_CUR SEEK_END SEEK_SET
	virtual long	seek(long Offset, int Origin) = 0;
	// Get the file length, and return 0 on failure.
	virtual unsigned long	size() = 0;
	// Close the open file
	virtual void	close() = 0;
	//Interface object destroyed
	virtual void	release() = 0;
	// Get the file pointer position, and return -1 on failure.
	virtual long	tell() = 0;
	
	virtual char* getFullPath() = 0;
	
	virtual bool isOpen() = 0;
	//// Determine whether the opened file is a file in the package and return a Boolean value
	//virtual int		IsFileInPak() = 0;
	////Determine whether the file is compressed in blocks
	//virtual int		IsPackedByFragment() = 0;
	////Get the number of file chunks
	//virtual int		GetFragmentCount() = 0;
	////Get the size of the chunk
	//virtual unsigned int	GetFragmentSize(int nFragmentIndex) = 0;
	////Read a file block, and the incoming pBuffer is the target buffer. If the incoming buffer pointer is empty, a new buffer will be allocated internally and the pointer will be passed out. The external part needs to be responsible for destroying it.
	////The size of the buffer needs to be able to accommodate the contents of the fragments. You can know the data size of each fragment through GetFragmentCount. The buffer must be at least this large.
	////The return value indicates the actual read data size of the block. If the operation fails (including the block does not exist, the sub-file is not stored in blocks, etc.), 0 is returned.
	//virtual unsigned long	ReadFragment(int nFragmentIndex, void*& pBuffer) = 0;

	virtual ~IFile() {};
};


IFile* g_OpenFile(const char* FileName, int ForceSingleFile/* = false*/, int ForWrite/* = false*/, int ForceCreate /* = false*/);

#endif  //#ifndef _ENGINE_FILE_H_