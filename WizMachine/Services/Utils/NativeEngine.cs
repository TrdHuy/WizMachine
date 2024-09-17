using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using WizMachine.Data;
using WizMachine.Utils;

namespace WizMachine.Services.Utils
{
   
    public static class NativeAPIAdapter
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


            [DllImport("engine.dll", CallingConvention = CallingConvention.StdCall)]
            public static extern void ExtractPakFile(string pakFilePath,
                string pakInfoPath,
                string outputRootPath);

            [DllImport("engine.dll", CallingConvention = CallingConvention.StdCall)]
            public static extern void CompressFolderToPakFile(string pakFilePath,
                string outputRootPath);

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
        }


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

        public static bool ExtractPakFile(string pakFilePath,
            string pakInfoPath,
            string outputRootPath)
        {
            NativeEngine.ExtractPakFile(pakFilePath, pakInfoPath, outputRootPath);
            return true;
        }

        public static bool CompressFolderToPakFile(string pakFilePath,
           string outputRootPath)
        {
            NativeEngine.CompressFolderToPakFile(pakFilePath, outputRootPath);
            return true;
        }

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
    }
}
