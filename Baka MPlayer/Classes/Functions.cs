﻿/*****************************
* Functions (by Joshua Park) *
*****************************/
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public static class Functions
{
    public static class Time
    {
        public static void CalculateTimeFromSeconds(double time, out int hour, out int min, out int sec)
        {
            hour = (int)(time / 3600);
            min = (int)((time % 3600) / 60);
            sec = (int)((time % 3600) % 60);
        }
        public static string ConvertTime(int hour, int min, int sec)
        {
            if (hour > 0)
                return string.Format("{0}:{1}:{2}", hour.ToString("#0"), min.ToString("00"), sec.ToString("00"));
            return string.Format("{0}:{1}", min.ToString("#0"), sec.ToString("00"));
        }
        public static string ConvertTimeFromSeconds(double totalSec)
        {
            int hour, min, sec;
            CalculateTimeFromSeconds(totalSec, out hour, out min, out sec);
            return ConvertTime(hour, min, sec);
        }
    }

    public static class URL
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long PathIsURL(string pszPath);

        public static bool ValidateURL(string url)
        {
            return PathIsURL(url).Equals(1);
        }
        public static string DecodeURL(string input)
        {
            return System.Web.HttpUtility.UrlDecode(input);
        }
    }

    public static class String
    {
        public static string AutoEllipsis(int max, string toUse)
        {
            if (max < toUse.Length)
                return toUse.Remove(max, toUse.Length - max) + "...";
            return toUse;
        }
        public static bool IsAlphanumeric(string text)
        {
            var r = new Regex("[a-zA-Z0-9]");
            return r.IsMatch(text);
        }
        public static string ToTitleCase(string input)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }
    }

    public static class IO
    {
        public static string GetDirectoryName(string f)
        {
            try
            {
                return f.Substring(0, f.LastIndexOf('\\'));
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string GetFileSize(string FilePath, int RoundTo)
        {
            try
            {
                var FileProperties = new System.IO.FileInfo(FilePath);
                if (FileProperties.Length < 1024) {
                    // Bytes
                    return (FileProperties.Length + " B");
                } if (FileProperties.Length >= 1024 && FileProperties.Length < 1048576) {
                    // Kilobytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1024, RoundTo) + "kB";
                } if (FileProperties.Length >= 1048576 && FileProperties.Length < 1073741824) {
                    // Megabytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1048576, RoundTo) + " MB";
                } if (FileProperties.Length >= 1073741824 && FileProperties.Length < 1099511627776L) {
                    // Gigabytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1073741824, RoundTo) + " GB";
                } if (FileProperties.Length >= 1099511627776L && FileProperties.Length < 1099511627776L) {
                    // Terabytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1099511627776L, RoundTo) + " TB";
                }
                return "Not Available";
            }
            catch (Exception ex)
            {
                return ("Error: " + ex.Message);
            }
        }
        public static string GetFolderName(string fileLocation)
        {
            try
            {
                var objInfo = new System.IO.FileInfo(fileLocation);
                return objInfo.Directory.Name;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public static class OS
    {
        public static bool IsRunning64Bit()
        {
            // if (IntPtr.Size == 4) 32-bit
            // if (IntPtr.Size == 8) 64-bit

            return IntPtr.Size.Equals(8);
        }
        public static bool RunningOnWin7
        {
            get
            {
                return (Environment.OSVersion.Version.Major > 6) ||
                    (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1);
            }
        }
    }

    public static class Calculate
    {
        public static int GetGCD(int x, int y)
        {
            while (x != y)
            {
                if (x > y)
                    x = x - y;
                else
                    y = y - x;
            }
            return x;
        }
    }

    public static class CSharp_Wrappers
    {
        public static int GET_X_LPARAM(int lparam)
        { return LowWord(lparam); }

        public static int GET_Y_LPARAM(int lparam)
        { return HighWord(lparam); }

        public static int LowWord(int word)
        { return word & 0xFFFF; }

        public static int HighWord(int word)
        { return word >> 16; }
    }
}