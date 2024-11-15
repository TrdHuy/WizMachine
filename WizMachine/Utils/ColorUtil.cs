﻿using System;
using System.Windows.Media;
using WizMachine.Data;

namespace WizMachine.Utils
{
    internal static class ColorUtil
    {
        public static double CalculateEuclideanDistance(Color thisColor, Color otherColor)
        {
            int deltaRed = thisColor.R - otherColor.R;
            int deltaGreen = thisColor.G - otherColor.G;
            int deltaBlue = thisColor.B - otherColor.B;
            return Math.Sqrt(deltaRed * deltaRed + deltaGreen * deltaGreen + deltaBlue * deltaBlue);
        }

        public static double CalculateRGBEuclideanDistance(byte B, byte G, byte R, PaletteColor otherColor)
        {
            int deltaRed = R - otherColor.Red;
            int deltaGreen = G - otherColor.Green;
            int deltaBlue = B - otherColor.Blue;
            return Math.Sqrt(deltaRed * deltaRed + deltaGreen * deltaGreen + deltaBlue * deltaBlue);
        }

        public static double CalculateRGBEuclideanDistance(PaletteColor thisColor, PaletteColor otherColor)
        {
            int deltaRed = thisColor.Red - otherColor.Red;
            int deltaGreen = thisColor.Green - otherColor.Green;
            int deltaBlue = thisColor.Blue - otherColor.Blue;
            return Math.Sqrt(deltaRed * deltaRed + deltaGreen * deltaGreen + deltaBlue * deltaBlue);
        }

        public static Color BlendColors(Color foreground, Color background)
        {
            byte alphaBackground = background.A;
            byte alphaForeground = foreground.A;

            byte redBackground = background.R;
            byte greenBackground = background.G;
            byte blueBackground = background.B;

            byte redForeground = foreground.R;
            byte greenForeground = foreground.G;
            byte blueForeground = foreground.B;

            byte alphaResult = (byte)(alphaBackground + alphaForeground * (255 - alphaBackground) / 255);
            byte redResult = (byte)((redForeground * alphaForeground / 255) + (redBackground * (255 - alphaForeground) / 255));
            byte greenResult = (byte)((greenForeground * alphaForeground / 255) + (greenBackground * (255 - alphaForeground) / 255));
            byte blueResult = (byte)((blueForeground * alphaForeground / 255) + (blueBackground * (255 - alphaForeground) / 255));

            return Color.FromArgb(alphaResult, redResult, greenResult, blueResult);
        }

        public static byte FindAlphaColors(Color foreground, Color background, Color combinedColor, out byte averageAbsoluteDeviation)
        {
            byte redBackground = background.R;
            byte greenBackground = background.G;
            byte blueBackground = background.B;

            byte redForeground = foreground.R;
            byte greenForeground = foreground.G;
            byte blueForeground = foreground.B;

            byte alphaR_Foreground = (byte)Math.Round(255d * (double)((double)(combinedColor.R - redBackground) / (double)(redForeground - redBackground)));
            byte alphaG_Foreground = (byte)Math.Round(255d * (double)((double)(combinedColor.G - greenBackground) / (double)(greenForeground - greenBackground)));
            byte alphaB_Foreground = (byte)Math.Round(255d * (double)((double)(combinedColor.B - blueBackground) / (double)(blueForeground - blueBackground)));

            byte alphaAvarage = (byte)(Math.Round((double)((double)(alphaR_Foreground + alphaG_Foreground + alphaB_Foreground) / 3d)));
            averageAbsoluteDeviation = (byte)((Math.Abs(alphaR_Foreground - alphaAvarage)
                + Math.Abs(alphaG_Foreground - alphaAvarage)
                + Math.Abs(alphaB_Foreground - alphaAvarage)) / 3);

            return alphaAvarage;
        }
    }
}
