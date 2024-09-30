using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using WizMachine.Data;
using WizMachine.Utils;

namespace WizMachine.Services.Utils
{

    internal static class NativeAPIAdapter
    {
        private class NativeEngine
        {
            public struct NFrameInfo
            {
                public ushort Width;
                public ushort Height;
                public ushort OffX;
                public ushort OffY;
            };

            public struct NFrameData
            {
                public FrameInfo FrameInfo;
                public uint EncryptedFrameDataOffset;
                public uint EncryptedLength;
                public uint DecodedLength;
                public IntPtr DecodedFrameData;
                public IntPtr ColorMap;
            };

            public struct NSPRFileHead
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] VersionInfo;
                public ushort GlobalWidth;
                public ushort GlobalHeight;
                public short OffX;
                public short OffY;
                public ushort FrameCounts;
                public ushort ColorCounts;
                public ushort DirectionCount;
                public ushort Interval;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
                public byte[] Reserved;
            }

            public struct NColor
            {
                public byte R;
                public byte G;
                public byte B;
            };

            [DllImport("engine.dll")]
            public static extern void ExportToSPRFile(string filePath,
                NSPRFileHead fileHead,
                NColor[] palette,
                int paletteSize,
                NFrameData[] frame);

            [DllImport("engine.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void LoadSPRFile(string filePath,
                out NSPRFileHead fileHead,
                [MarshalAs(UnmanagedType.LPArray,
            ArraySubType = UnmanagedType.Struct,
            SizeParamIndex = 3)] out NColor[] palette,
                out int paletteLength,
                out int frameDataBeginPos,
                [MarshalAs(UnmanagedType.LPArray,
            ArraySubType = UnmanagedType.Struct,
            SizeParamIndex = 6)] out NFrameData[] frameData,
                out int frameCount);

            [DllImport("engine.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeArrData(IntPtr arrPtr);

            #region PAK
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct CompressedFileInfo
            {
                public int index;
                public IntPtr id;
                public ulong idValue;
                public IntPtr time;
                public IntPtr fileName;
                public int size;
                public int inPakSize;
                public int comprFlag;
                public IntPtr crc;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct PakInfo
            {
                public int totalFiles;
                public IntPtr pakTime;
                public IntPtr pakTimeSave;
                public IntPtr crc;

                public IntPtr files;  // Con trỏ đến mảng CompressedFileInfo
                public int fileCount;
            }

            [DllImport("engine.dll", CharSet = CharSet.Ansi)]
            public static extern void FreePakInfo(ref PakInfo pakInfo);

            [DllImport("engine.dll", CallingConvention = CallingConvention.StdCall)]
            public static extern void ParsePakInfoFile(string pakInfoPath,
                ref PakInfo pakInfo);

            [DllImport("engine.dll", CallingConvention = CallingConvention.StdCall)]
            public static extern void ExtractPakFile(string pakFilePath,
                string? pakInfoPath,
                string outputRootPath);

            [DllImport("engine.dll", CallingConvention = CallingConvention.StdCall)]
            public static extern void CompressFolderToPakFile(string pakFilePath,
                string outputRootPath);
            #endregion 

            #region CERT
            [DllImport("engine.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void ForceCheckCertPermission(WizMachine.Data.CertInfo certinfo);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct CertInfo
            {
                public IntPtr Subject;
                public IntPtr Issuer;
                public long ValidFrom;
                public long ValidTo;
                public IntPtr Thumbprint;
                public IntPtr SerialNumber;
            }

            [DllImport("engine.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetCertificateInfo(string filePath, ref CertInfo certInfo);

            [DllImport("engine.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeCertInfo(ref CertInfo certInfo);
            #endregion
        }

        #region CERT
        public static void ForceCheckCertPermission(WizMachine.Data.CertInfo certinfo)
        {
            NativeEngine.ForceCheckCertPermission(certinfo);
        }

        public static WizMachine.Data.CertInfo GetSignedCertInfoFromFile(string filePath)
        {
            WizMachine.Data.CertInfo cert = new WizMachine.Data.CertInfo();
            NativeEngine.CertInfo unsafeCertInfo = new NativeEngine.CertInfo();
            int result = NativeEngine.GetCertificateInfo(filePath, ref unsafeCertInfo);

            if (result == 0)
            {
                cert.Subject = Marshal.PtrToStringAnsi(unsafeCertInfo.Subject) ?? "";
                cert.Issuer = Marshal.PtrToStringAnsi(unsafeCertInfo.Issuer) ?? "";
                cert.ValidFrom = unsafeCertInfo.ValidFrom;
                cert.ValidTo = unsafeCertInfo.ValidTo;
                cert.Thumbprint = Marshal.PtrToStringAnsi(unsafeCertInfo.Thumbprint) ?? "";
                cert.SerialNumber = Marshal.PtrToStringAnsi(unsafeCertInfo.SerialNumber) ?? "";

                // Free the memory allocated by C++
                NativeEngine.FreeCertInfo(ref unsafeCertInfo);
            }
            else
            {
                Console.WriteLine("Failed to retrieve certificate information. Error code: " + result);
            }
            return cert;
        }
        #endregion

        #region PAK
        public static PakInfo ParsePakInfoFile(string pakInfoPath)
        {
            NativeEngine.PakInfo nPakInfo = new NativeEngine.PakInfo();
            NativeEngine.ParsePakInfoFile(pakInfoPath, ref nPakInfo);
            PakInfo appLayerPakInfo = ConvertNativePakInfo(nPakInfo);

            // Mặc dù nPakInfo được khai báo trên C# (bộ nhớ của nPakInfo do C# quản lý)
            // nhưng các element trong nPakInfo lại được khai báo và khởi tạo dưới C++
            // vì vậy vẫn cần phải giải phóng bộ nhớ để tránh memory leak
            NativeEngine.FreePakInfo(ref nPakInfo);
            return appLayerPakInfo;
        }

        public static bool ExtractPakFile(string pakFilePath,
            string pakInfoPath,
            string outputRootPath)
        {
            NativeEngine.ExtractPakFile(pakFilePath, pakInfoPath, outputRootPath);
            return true;
        }

        public static bool ExtractPakFile(string pakFilePath,
           string outputRootPath)
        {
            NativeEngine.ExtractPakFile(pakFilePath, null, outputRootPath);
            return true;
        }

        public static bool CompressFolderToPakFile(string pakFilePath,
           string outputRootPath)
        {
            NativeEngine.CompressFolderToPakFile(pakFilePath, outputRootPath);
            return true;
        }

        private static PakInfo ConvertNativePakInfo(NativeEngine.PakInfo pakInfo)
        {
            PakInfo pakInfo2 = new PakInfo
            {
                totalFiles = pakInfo.totalFiles,
                pakTime = Marshal.PtrToStringAnsi(pakInfo.pakTime) ?? "",
                pakTimeSave = Marshal.PtrToStringAnsi(pakInfo.pakTimeSave) ?? "",
                crc = Marshal.PtrToStringAnsi(pakInfo.crc) ?? "",
                fileMap = new Dictionary<ulong, CompressedFileInfo>()
            };

            if (pakInfo.fileCount > 0 && pakInfo.files != IntPtr.Zero)
            {
                int structSize = Marshal.SizeOf(typeof(NativeEngine.CompressedFileInfo));

                for (int i = 0; i < pakInfo.fileCount; i++)
                {
                    IntPtr currentPtr = new IntPtr(pakInfo.files.ToInt64() + i * structSize);
                    NativeEngine.CompressedFileInfo nFileInfo = Marshal.PtrToStructure<NativeEngine.CompressedFileInfo>(currentPtr);
                    var cFileInfo = new CompressedFileInfo()
                    {
                        index = nFileInfo.index,
                        id = Marshal.PtrToStringAnsi(nFileInfo.id) ?? "",
                        idValue = nFileInfo.idValue,
                        time = Marshal.PtrToStringAnsi(nFileInfo.time) ?? "",
                        fileName = Marshal.PtrToStringAnsi(nFileInfo.fileName) ?? "",
                        size = nFileInfo.size,
                        inPakSize = nFileInfo.inPakSize,
                        comprFlag = nFileInfo.comprFlag,
                        crc = Marshal.PtrToStringAnsi(nFileInfo.crc) ?? ""
                    };
                    pakInfo2.fileMap[cFileInfo.idValue] = cFileInfo;
                }
            }

            return pakInfo2;
        }
        #endregion

        #region SPR
        public static bool LoadSPRFile(string filePath,
            out SprFileHead sprFileHead,
            out Palette palette,
            out int frameDataBeginPos,
            out FrameRGBA[] frameRGBA)
        {
            NativeEngine.NColor[] nativePalette;
            int nativePaletteLength;
            NativeEngine.NFrameData[] nativeFrameData;

            int frameCount;
            NativeEngine.LoadSPRFile(filePath,
                out NativeEngine.NSPRFileHead nativeFileHead,
                out nativePalette,
                out nativePaletteLength,
                out frameDataBeginPos,
                out nativeFrameData,
                out frameCount);

            frameRGBA = new FrameRGBA[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                frameRGBA[i] = ConvertAndFreeNativeFrameDataToAppData(nativeFrameData[i]);
            }

            sprFileHead = ConvertNativeSprFileHeadToAppData(nativeFileHead);
            palette = ConvertNativeColorPaletteToAppData(nativePalette, nativePaletteLength);

            return true;
        }

        private static SprFileHead ConvertNativeSprFileHeadToAppData(NativeEngine.NSPRFileHead nativeData)
        {
            return new SprFileHead(nativeData.VersionInfo,
                nativeData.GlobalWidth,
                nativeData.GlobalHeight,
                nativeData.OffX,
                nativeData.OffY,
                nativeData.FrameCounts,
                nativeData.ColorCounts,
                nativeData.DirectionCount,
                nativeData.Interval,
                nativeData.Reserved);
        }

        private static Palette ConvertNativeColorPaletteToAppData(NativeEngine.NColor[] nativeData, int nativeDataLength)
        {
            var paletteAppData = new PaletteColor[nativeDataLength];
            for (int i = 0; i < nativeDataLength; i++)
            {
                paletteAppData[i] = new PaletteColor(blue: nativeData[i].B,
                    green: nativeData[i].G,
                    red: nativeData[i].R,
                    alpha: 255);
            }

            return new Palette(paletteAppData);
        }

        private static FrameRGBA ConvertAndFreeNativeFrameDataToAppData(NativeEngine.NFrameData nativeData)
        {
            var frameRawData = new byte[(int)nativeData.DecodedLength];
            Marshal.Copy(nativeData.DecodedFrameData,
                frameRawData,
                0,
                (int)nativeData.DecodedLength);

            NativeEngine.FreeArrData(nativeData.DecodedFrameData);
            nativeData.DecodedFrameData = IntPtr.Zero;

            var colorMap = new int[(int)nativeData.DecodedLength / 4];
            Marshal.Copy(nativeData.ColorMap,
               colorMap,
               0,
               (int)nativeData.DecodedLength / 4);
            var colorDics = new Dictionary<int, List<long>>();
            for (int j = 0; j < nativeData.DecodedLength / 4; j++)
            {

                if (!colorDics.ContainsKey(colorMap[j]))
                {
                    colorDics.Add(colorMap[j], new List<long> { j });
                }
                else
                {
                    colorDics[colorMap[j]].Add(j);
                }
            }

            NativeEngine.FreeArrData(nativeData.ColorMap);
            nativeData.ColorMap = IntPtr.Zero;

            var frameData = new FrameRGBA
            {
                frameHeight = nativeData.FrameInfo.Height,
                frameWidth = nativeData.FrameInfo.Width,
                frameOffX = (short)nativeData.FrameInfo.OffX,
                frameOffY = (short)nativeData.FrameInfo.OffY,
                originDecodedBGRAData = frameRawData,
            };
            frameData.modifiedFrameRGBACache.PaletteIndexToPixelIndexMap = colorDics;
            return frameData;
        }
        #endregion
    }
}
