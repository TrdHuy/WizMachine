using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizMachine.Data;
using WizMachine.Services.Base;
using WizMachine.Services.Utils;

namespace WizMachine.Services.Impl
{
    internal class PakWorkManager : IPakWorkManager
    {
        bool IPakWorkManager.CompressFolderToPakFile(string pakFilePath, string outputRootPath)
        {
            return NativeAPIAdapter.CompressFolderToPakFile(pakFilePath, outputRootPath);
        }

        bool IPakWorkManager.ExtractPakFile(string pakFilePath, string pakInfoPath, string outputRootPath)
        {
            return NativeAPIAdapter.ExtractPakFile(pakFilePath, pakInfoPath, outputRootPath);
        }

        bool IPakWorkManager.ExtractPakFile(string pakFilePath, string outputRootPath)
        {
            return NativeAPIAdapter.ExtractPakFile(pakFilePath, outputRootPath);
        }

        PakInfo IPakWorkManager.ParsePakInfoFile(string pakInfoPath)
        {
            return NativeAPIAdapter.ParsePakInfoFile(pakInfoPath);
        }
    }
}
