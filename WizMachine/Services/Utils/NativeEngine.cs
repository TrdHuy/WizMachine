using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using WizMachine.Data;
using WizMachine.Utils;
using static WizMachine.Services.Utils.NativeEngine;

namespace WizMachine.Services.Utils
{
    public static class NativeEngine
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
        public static extern void MyCppFunc(
            string filePath,
            [MarshalAs(UnmanagedType.LPArray,
            ArraySubType = UnmanagedType.Struct,
            SizeParamIndex = 2)] out NColor[] ros,
            out int length);

        [DllImport("engine.dll")]
        public static extern void ExportToSPRFile(string filePath,
            NSPRFileHead fileHead,
            NColor[] palette,
            int paletteSize,
            NFrameData[] frame);

        [DllImport("engine.dll")]
        public static extern IntPtr LoadSPRFile2(string filePath, out IntPtr colorPtr);

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
        public static extern int Multiply(int a, int b);

        [return: MarshalAs(UnmanagedType.BStr)]
        [DllImport("engine.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern string GetCalcOptions();
    }

    public static class NativeAPIAdapter
    {
        public static bool LoadSPRFile(string filePath,
            out SprFileHead sprFileHead,
            out Palette palette,
            out int frameDataBeginPos,
            out FrameRGBA[] frameRGBA)
        {
            NColor[] nativePalette;
            int nativePaletteLength;
            NFrameData[] nativeFrameData;

            int frameCount;
            NativeEngine.LoadSPRFile(filePath,
                out NSPRFileHead nativeFileHead,
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

        private static SprFileHead ConvertNativeSprFileHeadToAppData(NSPRFileHead nativeData)
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

        private static Palette ConvertNativeColorPaletteToAppData(NColor[] nativeData, int nativeDataLength)
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

        private static FrameRGBA ConvertAndFreeNativeFrameDataToAppData(NFrameData nativeData)
        {
            var frameRawData = new byte[(int)nativeData.DecodedLength];
            Marshal.Copy(nativeData.DecodedFrameData,
                frameRawData,
                0,
                (int)nativeData.DecodedLength);

            FreeArrData(nativeData.DecodedFrameData);
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

            FreeArrData(nativeData.ColorMap);
            nativeData.ColorMap = IntPtr.Zero;

            return new FrameRGBA
            {
                frameHeight = nativeData.FrameInfo.Height,
                frameWidth = nativeData.FrameInfo.Width,
                frameOffX = (short)nativeData.FrameInfo.OffX,
                frameOffY = (short)nativeData.FrameInfo.OffY,
                originDecodedBGRAData = frameRawData,
            }.Also(it => it.modifiedFrameRGBACache.PaletteIndexToPixelIndexMap = colorDics);
        }
    }
}
