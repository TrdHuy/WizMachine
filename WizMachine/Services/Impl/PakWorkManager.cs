using System.Collections.Generic;
using WizMachine.Data;
using WizMachine.Services.Base;
using WizMachine.Services.Utils;
using WizMachine.Utils;

namespace WizMachine.Services.Impl
{
    internal class PakWorkManager : IPakWorkManager
    {
        private const string TAG = "PakWorkManager";
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


        private Dictionary<string, PakInfo> mSessionToPakInfoMap = new Dictionary<string, PakInfo>();
        private Dictionary<string, string> mPakFilePathToSession = new Dictionary<string, string>();
        private Dictionary<ulong, (string, CompressedFileInfo)> mBlockIdToSessionAndCompressFileInfoMap = new Dictionary<ulong, (string, CompressedFileInfo)>();
        bool IPakWorkManager.LoadPakFileToWorkManager(string pakFilePath, ProgressChangedCallback? progressChangedCallback)
        {

            if (mPakFilePathToSession.ContainsKey(pakFilePath))
            {
                Logger.Raw.I($"{TAG}: Already exist this pak in work manager. pakFilePath={pakFilePath}");
                return false;
            }
            float loadPakToWMWorkLoad = 0.8f;
            string sessionToken = NativeAPIAdapter.LoadPakFileToWorkManager(pakFilePath, out PakInfo pakFileInfo, (p, m) =>
            {
                progressChangedCallback?.Invoke((int)(p * loadPakToWMWorkLoad), m);
            });

            progressChangedCallback?.Invoke((int)(loadPakToWMWorkLoad * 100), "Mapping block...");

            if (!string.IsNullOrEmpty(sessionToken) && sessionToken.Length == 16)
            {
                mSessionToPakInfoMap.Add(sessionToken, pakFileInfo);
                mPakFilePathToSession.Add(pakFilePath, sessionToken);
                foreach (var pair in pakFileInfo.fileMap)
                {
                    var id = pair.Key;
                    var blockInfo = pair.Value;
                    mBlockIdToSessionAndCompressFileInfoMap[id] = (sessionToken, blockInfo);
                }
                Logger.Raw.I($"{TAG}: Added new pak to work manager. pakFilePath={pakFilePath}");

                progressChangedCallback?.Invoke(100, "Done!");
                return true;
            }
            return false;
        }

        void IPakWorkManager.ResetPakWorkManager()
        {
            foreach (var token in mSessionToPakInfoMap.Keys)
            {
                NativeAPIAdapter.CloseSession(token);
            }
            mSessionToPakInfoMap.Clear();
            mBlockIdToSessionAndCompressFileInfoMap.Clear();
            mPakFilePathToSession.Clear();
            Logger.Raw.I($"{TAG}: reset pak work manager!");
        }

        bool IPakWorkManager.RemovePakFileFromWorkManager(string pakFilePath)
        {
            var result = mPakFilePathToSession.Remove(pakFilePath);
            if (!result)
            {
                Logger.Raw.E($"{TAG}: pak file {pakFilePath} not existed!");
            }
            else
            {
                Logger.Raw.I($"{TAG}: removed pak file {pakFilePath}!");
            }
            return result;
        }

        bool IPakWorkManager.IsBlockExistByPath(string blockPath)
        {
            ulong blockId = NativeAPIAdapter.GetBlockIdFromPath(blockPath);
            return mBlockIdToSessionAndCompressFileInfoMap.ContainsKey(blockId);
        }

        bool IPakWorkManager.ExtractBlockByPath(string blockPath, string outputPath)
        {
            ulong blockId = NativeAPIAdapter.GetBlockIdFromPath(blockPath);
            if (mBlockIdToSessionAndCompressFileInfoMap.ContainsKey(blockId))
            {
                string sessionToken = mBlockIdToSessionAndCompressFileInfoMap[blockId].Item1;
                var blockInfo = mBlockIdToSessionAndCompressFileInfoMap[blockId].Item2;
                NativeAPIAdapter.ExtractFileFromPak(sessionToken, blockInfo.index, outputPath);
                return true;
            }
            return false;
        }
    }
}
