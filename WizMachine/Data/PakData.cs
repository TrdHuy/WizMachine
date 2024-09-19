using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizMachine.Data
{
    public struct CompressedFileInfo
    {
        public int index;
        public string id;
        public ulong idValue;
        public string time;
        public string fileName;
        public int size;
        public int inPakSize;
        public int comprFlag;
        public string crc;
    }

    public struct PakInfo
    {
        public int totalFiles;
        public string pakTime;
        public string pakTimeSave;
        public string crc;

        public Dictionary<ulong, CompressedFileInfo> fileMap;  // Map chứa các file dựa trên idValue
    }
}
