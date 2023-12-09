using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WizMachine.Data;
using System.Diagnostics;
using System.Linq;

namespace WizMachine.Utils
{
    internal static class BitmapUtil
    {
        public static PaletteColor[] SelectMostUsePaletteColorFromCountableColorSource(
            int colorDifferenceDelta,
            uint amount = 256,
            params Dictionary<Color, long>[] countableSources)
        {
            return SelectMostUseColorFromCountableColorSource(colorDifferenceDelta,
                amount,
                countableSources)
                .Select(it => new PaletteColor(it.B, it.G, it.R, it.A))
                .ToArray();
        }

        public static List<Color> SelectMostUseColorFromCountableColorSource(
            int colorDifferenceDelta,
            uint amount = 256,
            params Dictionary<Color, long>[] countableSources)
        {
            // Hợp 2 bộ màu vào làm 1
            var combinedSource = new Dictionary<Color, int>();
            var combinedSource2 = new List<(Color, long)>();

            int i = 0;

            foreach (var source in countableSources)
            {
                foreach (var kp in source)
                {
                    if (combinedSource.ContainsKey(kp.Key))
                    {
                        var index = combinedSource[kp.Key];
                        var item = combinedSource2[index];
                        item.Item2 += kp.Value;
                        combinedSource2[index] = item;
                    }
                    else
                    {
                        combinedSource.Add(kp.Key, i++);
                        combinedSource2.Add((kp.Key, kp.Value));
                    }
                }
            }

            return SelectMostUseColorFromCountableColorSource(combinedSource2,
                        colorDifferenceDelta,
                        amount,
                        out _,
                        out _,
                        out _,
                        out _);
        }

        public static List<Color> SelectMostUseColorFromCountableColorSource(
            List<(Color, long)> countableSource,
            int colorDifferenceDelta,
            uint colorSize,
            out int rgbPaletteColorCount,
            out List<Color> selectedColors,
            out List<Color> selectedAlphaColors,
            out List<Color> expectedRGBColors,
            bool isUsingAlpha = false,
            Color? backgroundForBlendColor = null,
            int colorDifferenceDeltaForCalculatingAlpha = 10,
            int deltaDistanceForNewARGBColor = 10,
            int deltaForAlphaAverageDeviation = 3)
        {
            // Sắp xếp theo số lượng sử dụng nhiều nhất
            var orderedSource = countableSource
                .OrderByDescending(kp => kp.Item2)
                .ToList();

            return SelectMostUseColorFromOrderedDescendingColorSource(
                orderedSource,
                colorDifferenceDelta,
                colorSize,
                out rgbPaletteColorCount,
                out selectedColors,
                out selectedAlphaColors,
                out expectedRGBColors,
                isUsingAlpha,
                backgroundForBlendColor,
                colorDifferenceDeltaForCalculatingAlpha,
                deltaDistanceForNewARGBColor,
                deltaForAlphaAverageDeviation);
        }

        private static List<Color> SelectMostUseColorFromOrderedDescendingColorSource(
          List<(Color, long)> orderedSource,
          int colorDifferenceDelta,
          uint colorSize,
          out int rgbPaletteColorCount,
          out List<Color> selectedColors,
          out List<Color> selectedAlphaColors,
          out List<Color> expectedRGBColors,
          bool isUsingAlpha,
          Color? backgroundForBlendColor,
          int colorDifferenceDeltaForCalculatingAlpha,
          int deltaDistanceForNewARGBColor,
          int deltaForAlphaAvarageDeviation)
        {
            var start = DateTime.Now;
            var selectedColorList = new List<Color>();

            // TODO: Dynamic this
            var selectedAlphaColorsList = new List<Color>();
            var combinedRGBList = new List<Color>();
            var expectedRGBList = new List<Color>();

            // Optimize color palette
            while (selectedColorList.Count < colorSize && orderedSource.Count > 0 && colorDifferenceDelta >= 0)
            {
                for (int i = 0; i < orderedSource.Count; i++)
                {
                    // For performance issue, do not use ElementAt to access the value with index
                    // use indexer instead
                    var expectedColor = orderedSource[i].Item1;
                    var shouldAdd = true;
                    foreach (var selectedColor in selectedColorList)
                    {
                        var distance = ColorUtil.CalculateEuclideanDistance(expectedColor, selectedColor);
                        if (distance < colorDifferenceDelta)
                        {
                            if (isUsingAlpha && distance < colorDifferenceDeltaForCalculatingAlpha)
                            {
                                var bg = backgroundForBlendColor ?? Colors.White;
                                var alpha = ColorUtil.FindAlphaColors(selectedColor, bg, expectedColor, out byte averageAbsoluteDeviation);
                                var newRGBColor = ColorUtil.BlendColors(Color.FromArgb(alpha, selectedColor.R, selectedColor.G, selectedColor.B), bg);
                                var distanceNewRGBColor = ColorUtil.CalculateEuclideanDistance(newRGBColor, expectedColor);
                                if (averageAbsoluteDeviation <= deltaForAlphaAvarageDeviation && distanceNewRGBColor <= deltaDistanceForNewARGBColor)
                                {
                                    expectedRGBList.Add(expectedColor);
                                    combinedRGBList.Add(newRGBColor);
                                    selectedAlphaColorsList.Add(Color.FromArgb(alpha, selectedColor.R, selectedColor.G, selectedColor.B));
                                    orderedSource.RemoveAt(i);
                                    i--;
                                }
                            }
                            shouldAdd = false;
                            break;
                        }
                    }
                    if (shouldAdd)
                    {
                        selectedColorList.Add(expectedColor);
                        orderedSource.RemoveAt(i);
                        i--;
                    }

                    if (selectedColorList.Count >= colorSize) break;
                }
                colorDifferenceDelta -= 2;
                //var newDelta = (int)((colorDifferenceDelta * selectedColorList.Count) / colorSize);
                //colorDifferenceDelta = newDelta == colorDifferenceDelta ? newDelta - 2 : newDelta;
            }

            //Combine RGB and ARGB color to selected list
            rgbPaletteColorCount = selectedColorList.Count;
            var combinedColorList = selectedColorList.ToList().Also((it) => it.AddRange(combinedRGBList));

            //reduce same combined color
            combinedColorList = combinedColorList.ReduceSameItem().ToList();

            selectedColors = combinedColorList;
            selectedAlphaColors = selectedAlphaColorsList;
            expectedRGBColors = expectedRGBList;

#if DEBUG
            // Bởi vì combinedRGBList và selectedAlphaColors chỉ được tính gần đúng
            // nên sẽ khác nhau về độ chênh lệch màu
            // Điều kiện assert dưới chỉ đúng khi 2 màu phải bằng nhau tuyệt đối
            // hay không có độ chênh lệch khi tính màu dựa trên kênh alpha
            if (colorDifferenceDeltaForCalculatingAlpha <= 1)
            {
                Debug.Assert(selectedColors.Count == selectedAlphaColors.Count + colorSize);
                Debug.Assert(expectedRGBColors.Count == selectedAlphaColors.Count);
            }
#endif

            Debug.WriteLine($"SelectMostUseColorFromOrderedDescendingColorSource: {(DateTime.Now - start).TotalMilliseconds}ms");
            return combinedColorList;
        }


        public static Dictionary<Color, long> CountColors(PaletteColor[] pixelArray,
            out long argbCount,
            out long rgbCount,
            out Dictionary<Color, long> argbSrc,
            out Dictionary<Color, long> rgbSrc)
        {
            Dictionary<Color, long> aRGBColorSet = new Dictionary<Color, long>();
            Dictionary<Color, long> rGBColorSet = new Dictionary<Color, long>();

            for (int i = 0; i < pixelArray.Length; i++)
            {
                byte blue = pixelArray[i].Blue;
                byte green = pixelArray[i].Green;
                byte red = pixelArray[i].Red;
                byte alpha = pixelArray[i].Alpha;

                Color colorARGB = Color.FromArgb(alpha, red, green, blue);
                if (!aRGBColorSet.ContainsKey(colorARGB))
                {
                    aRGBColorSet[colorARGB] = 1;
                }
                else
                {
                    aRGBColorSet[colorARGB]++;
                }

                Color colorRGB = Color.FromRgb(red, green, blue);
                if (!rGBColorSet.ContainsKey(colorRGB))
                {
                    rGBColorSet[colorRGB] = 1;
                }
                else
                {
                    rGBColorSet[colorRGB]++;
                }
            }
            argbCount = aRGBColorSet.Count;
            rgbCount = rGBColorSet.Count;
            argbSrc = aRGBColorSet;
            rgbSrc = rGBColorSet;
            return argbSrc;
        }

        public static void CountColors(BitmapSource bitmap,
           out long argbCount,
           out long rgbCount,
           out Dictionary<Color, long> argbSrc,
           out HashSet<Color> rgbSrc)
        {
            Dictionary<Color, long> aRGBColorSet = new Dictionary<Color, long>();
            HashSet<Color> rGBColorSet = new HashSet<Color>();

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = (width * bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[stride * height];

            bitmap.CopyPixels(pixelData, stride, 0);

            var isIncludedAlphaChannel = bitmap.Format.BitsPerPixel / 8 == 4;
            for (int i = 0; i < pixelData.Length; i += bitmap.Format.BitsPerPixel / 8)
            {
                byte blue = pixelData[i];
                byte green = pixelData[i + 1];
                byte red = pixelData[i + 2];
                if (isIncludedAlphaChannel)
                {
                    byte alpha = pixelData[i + 3];
                    Color colorARGB = Color.FromArgb(alpha, red, green, blue);
                    if (!aRGBColorSet.ContainsKey(colorARGB))
                    {
                        aRGBColorSet[colorARGB] = 1;
                    }
                    else
                    {
                        aRGBColorSet[colorARGB]++;
                    }
                }

                Color colorRGB = Color.FromRgb(red, green, blue);
                rGBColorSet.Add(colorRGB);
            }
            argbCount = aRGBColorSet.Count;
            rgbCount = rGBColorSet.Count;
            argbSrc = aRGBColorSet;
            rgbSrc = rGBColorSet;
        }

        #region ConvertBitmapSourceToPaletteColorArray
        public static PaletteColor[] ConvertBitmapSourceToPaletteColorArray(BitmapSource bitmapSource)
        {
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = (width * bitmapSource.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[height * stride];

            bitmapSource.CopyPixels(pixelData, stride, 0);
            PaletteColor[] paletteColors = new PaletteColor[width * height];
            for (int i = 0; i < width * height; i++)
            {
                if (bitmapSource.Format == PixelFormats.Bgr32 ||
                    bitmapSource.Format == PixelFormats.Bgra32)
                {
                    int offset = i * 4;
                    paletteColors[i] = new PaletteColor(blue: pixelData[offset],
                        green: pixelData[offset + 1],
                        red: pixelData[offset + 2],
                        alpha: pixelData[offset + 3]);
                }
                else if (bitmapSource.Format == PixelFormats.Rgb24)
                {
                    int offset = i * 3;
                    paletteColors[i] = new PaletteColor(blue: pixelData[offset + 2],
                        green: pixelData[offset + 1],
                        red: pixelData[offset],
                        alpha: 255);
                }
                else
                {
                    throw new Exception("ConvertBitmapSourceToPaletteColorArray: Invaild format!");
                }

            }

            return paletteColors;
        }

        public static PaletteColor[] ConvertBitmapSourceToPaletteColorArray(BitmapSource bitmapSource,
            out Dictionary<Color, long> argbCountableSource,
            out Dictionary<Color, long> rgbCountableSource,
            out Palette palette,
            out byte[] bgraBytesData,
            out Dictionary<int, List<long>> paletteColorIndexToPixelIndexMap)
        {
            paletteColorIndexToPixelIndexMap = new Dictionary<int, List<long>>();
            var colorPixelIndexMap = new Dictionary<Color, List<long>>();
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = (width * bitmapSource.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[height * stride];
            var bgraCounter = 0;
            if (bitmapSource.Format == PixelFormats.Bgr32 ||
                   bitmapSource.Format == PixelFormats.Bgra32)
            {
                bgraBytesData = pixelData;
            }
            else
            {
                bgraBytesData = new byte[height * width * 4];
            }
            bitmapSource.CopyPixels(pixelData, stride, 0);

            PaletteColor[] paletteColors = new PaletteColor[width * height];
            argbCountableSource = new Dictionary<Color, long>();
            var rgbSource = new Dictionary<Color, long>();
            for (int i = 0; i < width * height; i++)
            {
                var argbColor = Colors.Transparent;
                var rgbColor = Colors.White;
                if (bitmapSource.Format == PixelFormats.Bgr32 ||
                    bitmapSource.Format == PixelFormats.Bgra32)
                {
                    int offset = i * 4;
                    paletteColors[i] = new PaletteColor(blue: pixelData[offset],
                        green: pixelData[offset + 1],
                        red: pixelData[offset + 2],
                        alpha: pixelData[offset + 3]);
                    argbColor = Color.FromArgb(pixelData[offset + 3],
                        pixelData[offset + 2],
                        pixelData[offset + 1],
                        pixelData[offset]);
                    rgbColor = Color.FromRgb(
                        pixelData[offset + 2],
                        pixelData[offset + 1],
                        pixelData[offset]);
                }
                else if (bitmapSource.Format == PixelFormats.Rgb24)
                {
                    int offset = i * 3;
                    paletteColors[i] = new PaletteColor(blue: pixelData[offset + 2],
                        green: pixelData[offset + 1],
                        red: pixelData[offset],
                        alpha: 255);
                    argbColor = Color.FromArgb(255,
                        pixelData[offset],
                        pixelData[offset + 1],
                        pixelData[offset + 2]);
                    rgbColor = Color.FromRgb(
                        pixelData[offset + 2],
                        pixelData[offset + 1],
                        pixelData[offset]);

                    bgraBytesData[bgraCounter] = pixelData[offset + 2];
                    bgraBytesData[bgraCounter] = pixelData[offset + 1];
                    bgraBytesData[bgraCounter] = pixelData[offset];
                    bgraBytesData[bgraCounter] = 255;
                }
                else
                {
                    throw new Exception("ConvertBitmapSourceToPaletteColorArray: Invaild format!");
                }

                if (argbCountableSource.ContainsKey(argbColor))
                {
                    argbCountableSource[argbColor] += 1;
                }
                else
                {
                    argbCountableSource.Add(argbColor, 1);
                }

                if (rgbSource.ContainsKey(rgbColor))
                {
                    colorPixelIndexMap[rgbColor].Add(i);
                    rgbSource[rgbColor]++;
                }
                else
                {
                    colorPixelIndexMap.Add(rgbColor, new List<long> { i });
                    rgbSource.Add(rgbColor, 1);
                }
            }
            palette = new Palette(rgbSource.Count);
            int j = 0;
            foreach (var color in rgbSource.Keys)
            {
                palette.Data[j] = new PaletteColor(color.B, color.G, color.R, 255);
                paletteColorIndexToPixelIndexMap.Add(j, colorPixelIndexMap[color]);
                j++;
            }
            rgbCountableSource = rgbSource;
            return paletteColors;
        }
        #endregion

        public static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            // Nếu mảng có chiều dài khác nhau, chúng không giống nhau
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                // So sánh từng phần tử của hai mảng
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            // Nếu không có phần tử nào khác nhau, chúng giống nhau
            return true;
        }
    }
}
