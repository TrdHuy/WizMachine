using WizMachine.Data;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WizMachineTest.Utils
{
    internal static class TestUtil
    {
        public static BitmapSource? LoadBitmapFromFile(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();

            try
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(filePath);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải tệp hình ảnh: {ex.Message}");
                return null;
            }

            if (bitmapImage.IsDownloading)
            {
                bitmapImage.DownloadCompleted += (sender, e) =>
                {
                    // Bitmap đã được tải xong, bạn có thể sử dụng nó ở đây
                    // Ví dụ: Image.Source = bitmapImage;
                };
            }
            else
            {
                // Bitmap đã được tải xong, bạn có thể sử dụng nó ở đây
                // Ví dụ: Image.Source = bitmapImage;
            }
            return bitmapImage;
        }

        public static string FullPath(this string relativePath)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            return fullPath;
        }

        public static byte[] ConvertPaletteColorArrayToByteArray(PaletteColor[] colors)
        {
            int colorSize = Marshal.SizeOf(typeof(PaletteColor));
            byte[] byteArray = new byte[colors.Length * colorSize];
            for (int i = 0; i < colors.Length; i++)
            {
                byte[] colorBytes = new byte[colorSize];
                IntPtr ptr = Marshal.AllocHGlobal(colorSize);
                try
                {
                    Marshal.StructureToPtr(colors[i], ptr, false);
                    Marshal.Copy(ptr, colorBytes, 0, colorSize);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
                Buffer.BlockCopy(colorBytes, 0, byteArray, i * colorSize, colorSize);
            }
            return byteArray;
        }

        public static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static byte[]? ReadBytesFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    return data;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
