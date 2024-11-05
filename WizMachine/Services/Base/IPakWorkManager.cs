using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizMachine.Data;
using WizMachine.Services.Utils;

namespace WizMachine.Services.Base
{
    public interface IPakWorkManager
    {
        public PakInfo ParsePakInfoFile(string pakInfoPath);

        public bool ExtractPakFile(string pakFilePath,
            string pakInfoPath,
            string outputRootPath);

        public bool ExtractPakFile(string pakFilePath,
            string outputRootPath);

        public bool CompressFolderToPakFile(string pakFilePath,
           string outputRootPath);


        #region WorkManager
        public bool LoadPakFileToWorkManager(string pakFilePath, ProgressChangedCallback? progressChangedCallback = null);
        public void ResetPakWorkManager();
        public bool RemovePakFileFromWorkManager(string pakFilePath);
        public bool IsBlockExistByPath(string blockPath);
        public bool ExtractBlockByPath(string blockPath, string outputPath);
        #endregion
    }
}
