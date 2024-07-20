#include "pch.h"
#include "file_alone.h"

IFile* g_OpenFile(const char* FileName, int ForceSingleFile/* = false*/, int ForWrite/* = false*/)
{
	if (FileName == NULL || FileName[0] == 0)
		return 0;

	int	TryTurn[2] = { 0, 0 }; //0-Do not try, 1-read independent files, 2-read packaged files ---? Why so divided?
	if (ForceSingleFile || ForWrite)
	{
		TryTurn[0] = 1;
	}
	else if (s_nEngineFindFilePackFirst)
	{
		TryTurn[0] = 2;
		TryTurn[1] = 1;
	}
	else
	{
		TryTurn[0] = 1;
		TryTurn[1] = 2;
	}

	IFile* pFile = NULL;
	for (int nTry = 0; nTry < 2; nTry++)
	{
		if (TryTurn[nTry] == 2)
		{
			//TODO : porting KPackFile for read case
			/*KPackFile	pak;
			if (pak.Open(FileName))
			{
				pFile = pak.Deprive();
				break;
			}*/
		}
		else if (TryTurn[nTry] == 1)
		{
			AloneFile* alone = new AloneFile();
			if (alone->open(FileName, ForWrite))
			{
				return alone;
				break;
			}
		}
	}
	return pFile;
}