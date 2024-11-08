using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using WizMachine.Data;
using WizMachine.Utils;

namespace WizMachine.Services.Utils
{

    // Cần attr này để truyền callback xuống c++ không bị lỗi memory access violation 
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ProgressChangedCallback(int progress, string message);


    internal static class NativeAPIAdapter
    {
        private const string TAG = "NativeAPIAdapter";

        private class NativeEngine
        {
            public enum ErrorCode
            {
                Success = 0,        // Không có lỗi
                InvalidArgument,    // Tham số không hợp lệ
                OutOfMemory,        // Không đủ bộ nhớ
                NotFound,           // Không tìm thấy
                InternalError,      // Lỗi nội bộ
                UnknownError,        // Lỗi không xác định
                SecurityError,
                ShouldNeverHappen
            };
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct APIResult
            {
                public ErrorCode errorCode;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
                public string errorMessage;
            }

            private const string ENGINE_DLL = "engine.dll";

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

            #region SPR
            [DllImport(ENGINE_DLL)]
            public static extern APIResult ExportToSPRFile(string filePath,
                NSPRFileHead fileHead,
                NColor[] palette,
                int paletteSize,
                NFrameData[] frame);

            /// <summary>
            /// Tải dữ liệu SPR (sprite) từ một file và khởi tạo thông tin metadata cùng dữ liệu khung hình liên quan.
            /// </summary>
            /// <param name="filePath">Đường dẫn tới file chứa dữ liệu SPR cần tải.</param>
            /// <param name="fileHead">Tham chiếu đến cấu trúc <see cref="NSPRFileHead"/> để lưu thông tin tiêu đề của file SPR.</param>
            /// <param name="palette">Con trỏ đầu ra tới mảng bảng màu, được cấp phát dựa trên dữ liệu trong file SPR.</param>
            /// <param name="paletteLength">Giá trị đầu ra cho biết số lượng màu trong mảng bảng màu.</param>
            /// <param name="frameDataBeginPos">Giá trị đầu ra cho biết vị trí bắt đầu của dữ liệu khung hình trong file SPR.</param>
            /// <param name="frame">Con trỏ đầu ra tới mảng <see cref="FrameData"/> đại diện cho dữ liệu của từng khung hình.</param>
            /// <param name="frameCount">Giá trị đầu ra chỉ định số lượng khung hình trong dữ liệu SPR.</param>
            /// <remarks>
            /// Hàm này đọc dữ liệu từ file SPR trên ổ đĩa, cấp phát bộ nhớ cho bảng màu và dữ liệu khung hình tương ứng.
            /// Đảm bảo giải phóng bộ nhớ đã cấp phát cho <paramref name="palette"/> và <paramref name="frame"/> sau khi sử dụng để tránh rò rỉ bộ nhớ.
            /// </remarks>
            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult LoadSPRFile(string filePath,
                ref NSPRFileHead fileHead,
                out IntPtr palette,
                out int paletteLength,
                out int frameDataBeginPos,
                out IntPtr frame,
                out int frameCount);

            /// <summary>
            /// Tải dữ liệu SPR (sprite) từ bộ nhớ và khởi tạo thông tin metadata cùng dữ liệu khung hình liên quan.
            /// </summary>
            /// <param name="data">Mảng byte chứa dữ liệu thô của SPR.</param>
            /// <param name="dataLength">Độ dài của mảng byte dữ liệu SPR.</param>
            /// <param name="fileHead">Tham chiếu đến cấu trúc <see cref="NSPRFileHead"/> để lưu thông tin tiêu đề của file SPR.</param>
            /// <param name="palette">Con trỏ đầu ra tới mảng bảng màu, được cấp phát dựa trên dữ liệu SPR.</param>
            /// <param name="paletteLength">Giá trị đầu ra cho biết số lượng màu trong mảng bảng màu.</param>
            /// <param name="frameDataBeginPos">Giá trị đầu ra cho biết vị trí bắt đầu của dữ liệu khung hình trong dữ liệu SPR.</param>
            /// <param name="frame">Con trỏ đầu ra tới mảng <see cref="FrameData"/> đại diện cho dữ liệu của từng khung hình.</param>
            /// <param name="frameCount">Giá trị đầu ra chỉ định số lượng khung hình trong dữ liệu SPR.</param>
            /// <remarks>
            /// Hàm này giao tiếp trực tiếp với dữ liệu SPR ở cấp độ hệ thống, và cấp phát bộ nhớ cho bảng màu và dữ liệu khung hình.
            /// Đảm bảo giải phóng bộ nhớ đã cấp phát cho <paramref name="palette"/> và <paramref name="frame"/> sau khi sử dụng để tránh rò rỉ bộ nhớ.
            /// </remarks>
            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult LoadSPRMemory(
                byte[] data,
                int dataLength,
                ref NSPRFileHead fileHead,
                out IntPtr palette,
                out int paletteLength,
                out int frameDataBeginPos,
                out IntPtr frame,
                out int frameCount
            );

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult FreeSPRMemory(
            IntPtr palette,
                IntPtr frameData, int frameCount);

            #endregion

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
            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult GetBlockIdFromPath(string blockPath, out uint blockId);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult ExtractBlockFromPakFile(string sessionString, int subFileIndex, string outputPath);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult LoadPakFileToWorkManager(string filePath,
                ref PakInfo pakInfo,
                ProgressChangedCallback progressChangedCallback,
                out IntPtr sessionTokenPtr);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult ClosePakFileSession(string sessionString);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult FreeBuffer(IntPtr buffer);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult ReadBlockFromPakFile(string sessionToken,
                int subFileIndex,
                out ulong subFileSize,
                out IntPtr blockData);

            [DllImport(ENGINE_DLL, CharSet = CharSet.Ansi)]
            public static extern APIResult FreePakInfo(ref PakInfo pakInfo);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult ParsePakInfoFile(string pakInfoPath,
                ref PakInfo pakInfo);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult ExtractPakFile(string pakFilePath,
                string? pakInfoPath,
                string outputRootPath,
                ProgressChangedCallback progressChangedCallback);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult CompressFolderToPakFile(string pakFilePath,
                string outputRootPath,
                bool bExcludeOfCheckId,
                ProgressChangedCallback progressChangedCallback);
            #endregion

            #region CERT
            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern APIResult ForceCheckCertPermission(WizMachine.Data.CertInfo certinfo);

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

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult GetCertificateInfo(string filePath, ref CertInfo certInfo);

            [DllImport(ENGINE_DLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern APIResult FreeCertInfo(ref CertInfo certInfo);
            #endregion
        }

        #region CERT
        public static void ForceCheckCertPermission(WizMachine.Data.CertInfo certinfo)
        {
            var apiRes = NativeEngine.ForceCheckCertPermission(certinfo);
            if (apiRes.errorCode == NativeEngine.ErrorCode.SecurityError)
            {
                throw new Exception("Certificate has no permission!");
            }
        }

        public static WizMachine.Data.CertInfo GetSignedCertInfoFromFile(string filePath)
        {
            WizMachine.Data.CertInfo cert = new WizMachine.Data.CertInfo();
            NativeEngine.CertInfo unsafeCertInfo = new NativeEngine.CertInfo();
            var result = NativeEngine.GetCertificateInfo(filePath, ref unsafeCertInfo);

            if (result.errorCode == NativeEngine.ErrorCode.Success)
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
                throw new Exception("Failed to retrieve certificate information. Error code: " + result.errorCode +
                    "; message: " + result.errorMessage +
                    "; filePath: " + filePath);
            }
            return cert;
        }
        #endregion

        #region PAK

        public static byte[] ReadBlockFromPak(string sessionToken, int blockIndex)
        {
            ulong blockSize;
            var apiResult = NativeEngine.ReadBlockFromPakFile(sessionToken,
                blockIndex,
                out blockSize,
                out IntPtr bufferPtr);

            if (apiResult.errorCode != NativeEngine.ErrorCode.Success)
            {
                throw new Exception(apiResult.errorMessage);
            }

            if (bufferPtr == IntPtr.Zero)
            {
                throw new Exception("Failed to read sub-file data.");
            }


            // Chuyển đổi IntPtr thành mảng byte
            byte[] buffer = ConvertPointerToSharpBuffer(blockSize, bufferPtr);

            // Giải phóng bộ nhớ trong C++
            NativeEngine.FreeBuffer(bufferPtr);

            return buffer;


        }

        public static uint GetBlockIdFromPath(string blockPath)
        {
            var apiRes = NativeEngine.GetBlockIdFromPath(blockPath, out uint blockId);
            if (apiRes.errorCode != NativeEngine.ErrorCode.Success)
            {
                throw new Exception(apiRes.errorMessage);
            }
            return blockId;
        }

        public static string LoadPakFileToWorkManager(string filePath,
            out PakInfo pakFileInfo,
            ProgressChangedCallback? progressChangedCallback = null)
        {
            NativeEngine.PakInfo nPakInfo = new NativeEngine.PakInfo();
            var apiRes = NativeEngine.LoadPakFileToWorkManager(filePath, ref nPakInfo, progressChangedCallback: (progress, message) =>
            {
                progressChangedCallback?.Invoke(progress, message);
            }, out IntPtr sessionPtr);

            if (apiRes.errorCode != NativeEngine.ErrorCode.Success)
            {
                throw new Exception(apiRes.errorMessage);
            }
            if (sessionPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to load .pak file.");
            }
            pakFileInfo = ConvertNativePakInfo(nPakInfo);

            string sessionToken = Marshal.PtrToStringAnsi(sessionPtr) ?? "";
            NativeEngine.FreePakInfo(ref nPakInfo);
            NativeEngine.FreeBuffer(sessionPtr);
            return sessionToken;
        }

        public static bool ExtractFileFromPak(string sessionToken, int subFileIndex, string outputPath)
        {
            NativeEngine.ExtractBlockFromPakFile(sessionToken, subFileIndex, outputPath).Apply(it =>
            {
                if (it.errorCode != NativeEngine.ErrorCode.Success)
                {
                    throw new Exception(it.errorMessage);
                }
            });
            return true;
        }

        public static bool CloseSession(string sessionString)
        {
            return NativeEngine.ClosePakFileSession(sessionString).Let(it =>
            {
                if (it.errorCode != NativeEngine.ErrorCode.Success)
                {
                    return false;
                }
                return true;
            });
        }


        public static PakInfo ParsePakInfoFile(string pakInfoPath)
        {
            NativeEngine.PakInfo nPakInfo = new NativeEngine.PakInfo();
            var apiRes = NativeEngine.ParsePakInfoFile(pakInfoPath, ref nPakInfo);
            if (apiRes.errorCode == NativeEngine.ErrorCode.Success)
            {
                PakInfo appLayerPakInfo = ConvertNativePakInfo(nPakInfo);
                NativeEngine.FreePakInfo(ref nPakInfo);
                // Mặc dù nPakInfo được khai báo trên C# (bộ nhớ của nPakInfo do C# quản lý)
                // nhưng các element trong nPakInfo lại được khai báo và khởi tạo dưới C++
                // vì vậy vẫn cần phải giải phóng bộ nhớ để tránh memory leak
                return appLayerPakInfo;

            }
            else
            {
                NativeEngine.FreePakInfo(ref nPakInfo);
                throw new Exception(apiRes.errorMessage);
            }
        }

        public static bool ExtractPakFile(string pakFilePath,
            string pakInfoPath,
            string outputRootPath,
            ProgressChangedCallback? progressChangedCallback = null)
        {
            var apiRes = NativeEngine.ExtractPakFile(pakFilePath, pakInfoPath, outputRootPath, (p, m) =>
            {
                progressChangedCallback?.Invoke(p, m);
            });

            if (apiRes.errorCode != NativeEngine.ErrorCode.Success)
            {
                throw new Exception(apiRes.errorMessage);
            }

            return true;
        }

        public static bool ExtractPakFile(string pakFilePath,
           string outputRootPath,
           ProgressChangedCallback? progressChangedCallback = null)
        {
            var apiRes = NativeEngine.ExtractPakFile(pakFilePath, null, outputRootPath, (p, m) =>
            {
                progressChangedCallback?.Invoke(p, m);
            });
            if (apiRes.errorCode != NativeEngine.ErrorCode.Success)
            {
                throw new Exception(apiRes.errorMessage);
            }
            return true;
        }

        public static bool CompressFolderToPakFile(string pakFilePath,
           string outputRootPath,
           ProgressChangedCallback? progressChangedCallback = null)
        {
            var apiRes = NativeEngine.CompressFolderToPakFile(pakFilePath, outputRootPath, false, (p, m) =>
            {
                progressChangedCallback?.Invoke(p, m);
            });
            if (apiRes.errorCode == NativeEngine.ErrorCode.Success)
            {
                return true;
            }
            else
            {
                throw new Exception(apiRes.errorMessage);
            }
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
            var nFileHead = new NativeEngine.NSPRFileHead();
            IntPtr palettePtr, framePtr;
            int nativePaletteLength, frameCount;


            var apiResult = NativeEngine.LoadSPRFile(filePath,
                 ref nFileHead,
                 out palettePtr,
                 out nativePaletteLength,
                 out frameDataBeginPos,
                 out framePtr,
                 out frameCount);
            if (apiResult.errorCode == NativeEngine.ErrorCode.Success)
            {
                NativeEngine.NColor[] nPalette = new NativeEngine.NColor[nativePaletteLength];
                int structSize = Marshal.SizeOf(typeof(NativeEngine.NColor));
                for (int i = 0; i < nativePaletteLength; i++)
                {
                    IntPtr colorPtr = IntPtr.Add(palettePtr, i * structSize);
                    nPalette[i] = Marshal.PtrToStructure<NativeEngine.NColor>(colorPtr);
                }
                palette = ConvertNativeColorPaletteToAppData(nPalette, nativePaletteLength);
                sprFileHead = ConvertNativeSprFileHeadToAppData(nFileHead);

                NativeEngine.NFrameData[] nFrameData = new NativeEngine.NFrameData[frameCount];
                structSize = Marshal.SizeOf(typeof(NativeEngine.NFrameData));
                frameRGBA = new FrameRGBA[frameCount];
                for (int i = 0; i < frameCount; i++)
                {
                    IntPtr framDataPtr = IntPtr.Add(framePtr, i * structSize);
                    nFrameData[i] = Marshal.PtrToStructure<NativeEngine.NFrameData>(framDataPtr);
                    frameRGBA[i] = ConvertFrameDataToAppData(nFrameData[i]);
                }
                NativeEngine.FreeSPRMemory(palettePtr, framePtr, frameCount);
                return true;
            }
            else
            {
                throw new Exception($"Failed to load SPR file in native: {apiResult.errorMessage}");
            }

        }

        public static bool LoadSPRFromMemory(byte[] sprData,
            out SprFileHead sprFileHead,
            out Palette palette,
            out int frameDataBeginPos,
            out FrameRGBA[] frameRGBA)
        {
            var nFileHead = new NativeEngine.NSPRFileHead();
            IntPtr palettePtr, framePtr;
            int nativePaletteLength, frameCount;


            NativeEngine.LoadSPRMemory(sprData,
                sprData.Length,
                ref nFileHead,
                out palettePtr,
                out nativePaletteLength,
                out frameDataBeginPos,
                out framePtr,
                out frameCount);

            NativeEngine.NColor[] nPalette = new NativeEngine.NColor[nativePaletteLength];
            int structSize = Marshal.SizeOf(typeof(NativeEngine.NColor));
            for (int i = 0; i < nativePaletteLength; i++)
            {
                IntPtr colorPtr = IntPtr.Add(palettePtr, i * structSize);
                nPalette[i] = Marshal.PtrToStructure<NativeEngine.NColor>(colorPtr);
            }
            palette = ConvertNativeColorPaletteToAppData(nPalette, nativePaletteLength);
            sprFileHead = ConvertNativeSprFileHeadToAppData(nFileHead);

            NativeEngine.NFrameData[] nFrameData = new NativeEngine.NFrameData[frameCount];
            structSize = Marshal.SizeOf(typeof(NativeEngine.NFrameData));
            frameRGBA = new FrameRGBA[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                IntPtr framDataPtr = IntPtr.Add(framePtr, i * structSize);
                nFrameData[i] = Marshal.PtrToStructure<NativeEngine.NFrameData>(framDataPtr);
                frameRGBA[i] = ConvertFrameDataToAppData(nFrameData[i]);
            }


            NativeEngine.FreeSPRMemory(palettePtr, framePtr, frameCount);

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

        private static FrameRGBA ConvertFrameDataToAppData(NativeEngine.NFrameData nativeData)
        {
            var frameRawData = new byte[(int)nativeData.DecodedLength];
            Marshal.Copy(nativeData.DecodedFrameData,
                frameRawData,
                0,
                (int)nativeData.DecodedLength);

            //NativeEngine.FreeArrData(nativeData.DecodedFrameData);
            //nativeData.DecodedFrameData = IntPtr.Zero;

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

            //NativeEngine.FreeArrData(nativeData.ColorMap);
            //nativeData.ColorMap = IntPtr.Zero;

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

        private static byte[] ConvertPointerToSharpBuffer(ulong bufferSize, IntPtr bufferPtr)
        {
            // Xử lý trường hợp nếu buffer size quá lớn (lớn hơn int MaxValue)
            // Cần phải chia nhỏ buffer ra để copy.
            byte[] buffer = new byte[bufferSize];
            ulong remainingSize = bufferSize;
            const int chunkSize = int.MaxValue;
            ulong offset = 0;

            while (remainingSize > 0)
            {
                int bytesToCopy = (int)Math.Min(chunkSize, remainingSize); // Lấy số byte để sao chép trong lần này
                IntPtr currentPointer = IntPtr.Add(bufferPtr, (int)offset); // Calculate the current pointer position

                Marshal.Copy(currentPointer, buffer, (int)offset, bytesToCopy); // Copy to buffer
                offset += (ulong)bytesToCopy;
                remainingSize -= (ulong)bytesToCopy;
            }

            return buffer;
        }
    }
}
