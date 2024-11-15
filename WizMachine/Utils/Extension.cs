﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WizMachine.Utils
{
    internal static class Extension
    {
        public static string GetCallingExecutablePathByStackTrace()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] frames = stackTrace.GetFrames();

            foreach (var frame in frames)
            {
                var method = frame.GetMethod();
                if (method != null)
                {
                    var assembly = method.DeclaringType?.Assembly;

                    if (assembly != null && assembly != Assembly.GetExecutingAssembly()) // Bỏ qua assembly của chính thư viện B
                    {
                        return assembly.Location; // Trả về đường dẫn đầy đủ của file EXE hoặc DLL
                    }
                }
            }

            return "Unknown";
        }

        public static IEnumerable<T> ReduceSameItem<T>(this IEnumerable<T> @in)
        {
            return @in.GroupBy(i => i).Select(i => i.First());
        }

        public static void CopyStructToList<T>(this T value, List<byte> list) where T : struct
        {
            int structSize = Marshal.SizeOf(typeof(T));
            byte[] byteArray = new byte[structSize];
            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(value, structPtr, false);
            Marshal.Copy(structPtr, byteArray, 0, structSize);
            Marshal.FreeHGlobal(structPtr);
            list.AddRange(byteArray);
        }

        public static T? BinToStruct<T>(this FileStream fs, long position = 0) where T : struct
        {
            int structSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[structSize];
            long oldPosition = fs.Position;
            fs.Position = position;

            int bytesRead = fs.Read(buffer, 0, structSize);
            if (bytesRead != structSize)
            {
                Console.WriteLine("Không thể đọc đủ dữ liệu từ tệp.");
                return null;
            }

            IntPtr ptr = Marshal.AllocHGlobal(structSize);
            Marshal.Copy(buffer, 0, ptr, structSize);
            var temp = (T?)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            fs.Position = oldPosition;
            return temp;
        }

        public static byte[] ToByteArray<T>(this T value) where T : struct
        {
            int structSize = Marshal.SizeOf(typeof(T));
            byte[] result = new byte[structSize];
            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(value, structPtr, false);
            Marshal.Copy(structPtr, result, 0, structSize);
            Marshal.FreeHGlobal(structPtr);
            return result;
        }

        public static void CopyStructToArray<T>(this T value, byte[] arr, int offset) where T : struct
        {
            int structSize = Marshal.SizeOf(typeof(T));
            if (offset + structSize > arr.Length) throw new Exception("Failed to copy struct to array!");
            IntPtr structPtr = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(value, structPtr, false);
            Marshal.Copy(structPtr, arr, offset, structSize);
            Marshal.FreeHGlobal(structPtr);
        }


    }

    internal static class SharpExtension
    {
        public static T? IfNullThenLet<T>(this T? self, Func<T?> block)
        {
            if (self == null)
            {
                return block();
            }
            return self;
        }

        public static void ApplyIfNotNull<T1, T2>(this (T1, T2) self, Action<T1, T2> block)
        {
            if (self.Item1 != null && self.Item2 != null)
            {
                block(self.Item1, self.Item2);
            }
        }

        public static void Apply<T>(this T self, Action<T> block)
        {
            block(self);
        }

        public static R Let<T, R>(this T self, Func<T, R> block)
        {
            return block(self);
        }

        public static T Also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }

        public static void FoEach<T>(this IEnumerable<T> self, Action<T> block)
        {
            foreach (var item in self)
            {
                block(item);
            }
        }

        public static void FoEach<T>(this IEnumerable<T> self, Action<int, T> block)
        {
            IEnumerator<T> enumerator = self.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                block(i++, enumerator.Current);
            }
            enumerator.Dispose();
        }

        public static void IfNullOrEmpty(this string? self, Action block)
        {
            if (string.IsNullOrEmpty(self))
            {
                block();
            }
        }

        public static void IfNotNullOrEmpty(this string? self, Action<string> block)
        {
            if (!string.IsNullOrEmpty(self))
            {
                block(self);
            }
        }

        public static void FoEach<T>(this IEnumerable<T> self, Func<int, T, bool> block)
        {
            IEnumerator<T> enumerator = self.GetEnumerator();
            int i = 0;
            var isContinue = true;
            while (enumerator.MoveNext() && isContinue)
            {
                isContinue = block(i++, enumerator.Current);
            }
            enumerator.Dispose();
        }

        public static bool FoEach<T>(this IEnumerable<T> self, Func<T, bool> block)
        {
            var res = true;
            foreach (var item in self)
            {
                res = res & block(item);
            }
            return res;
        }

        public static void For<T>(this int self, out T? result, Func<int, T?, T> block)
        {
            result = default;
            for (int i = 0; i < self; i++)
            {
                result = block(i, result);
            }
        }

        public static void ForEachNonEmptyLine(this string self, Action<string> block)
        {
            var lines = self.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                block(line);
            }
        }

        public static void ForEachLine(this string self, Action<string> block)
        {
            var lines = self.Split("\n");
            foreach (var line in lines)
            {
                block(line);
            }
        }

        public static bool IfFalseApply(this bool condition, Action<bool> block)
        {
            if (!condition)
            {
                block(condition);
            }
            return condition;
        }

        public static bool IfTrueApply(this bool condition, Action<bool> block)
        {
            if (condition)
            {
                block(condition);
            }
            return condition;
        }

        public static void ThrowIfNull<T>(this T self)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
        }

        public static T? IfNull<T>(this T? self, Action<T?> block)
        {
            if (self == null)
                block(self);
            return self;
        }

        public static void IfIs<T>(this object self, Action<T> block, Action? @else = null)
        {
            if (self is T e)
            {
                block(e);
            }
            else
            {
                @else?.Invoke();
            }
        }

        public static T? IfIsThenAlso<T>(this object self, Func<T, T> block)
        {
            if (self is T e)
            {
                return block(e);
            }
            return default(T);
        }

        public static R? IfIsThenLet<T, R>(this object self, Func<T, R> block)
        {
            if (self is T e)
            {
                return block(e);
            }
            return default(R);
        }


        public static T ThrowIfNull<T>(this T? self, Exception e)
        {
            return self ?? throw e;
        }
    }
}
