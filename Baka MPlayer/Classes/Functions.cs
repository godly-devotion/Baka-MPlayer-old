/*****************************
* Functions (by Joshua Park) *
*****************************/
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Functions
{
    public static class Time
    {
        /// <summary>
        /// Calculates the total seconds from hour, min, sec.
        /// </summary>
        public static void CalculateTimeFromSeconds(double time, out int hour, out int min, out int sec)
        {
            hour = (int)(time / 3600);
            min = (int)((time % 3600) / 60);
            sec = (int)((time % 3600) % 60);
        }
        /// <summary>
        /// Converts hour, min, sec to "0:00:00" format.
        /// </summary>
        public static string ConvertTime(int hour, int min, int sec)
        {
            if (hour > 0)
                return string.Format("{0}:{1}:{2}", hour.ToString("#0"), min.ToString("00"), sec.ToString("00"));
            return string.Format("{0}:{1}", min.ToString("#0"), sec.ToString("00"));
        }
        /// <summary>
        /// Converts total seconds to "0:00:00" format;
        /// </summary>
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

        public static bool IsValidURL(string url)
        {
            return PathIsURL(url).Equals(1);
        }
        public static string DecodeURL(string input)
        {
            return IsValidURL(input) ? System.Web.HttpUtility.UrlDecode(input) : input;
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
        /// <summary>
        /// NOTE: Conversion doesn't work on words with ALL CAPS
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToTitleCase(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
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
        public static string GetFileSize(string filePath, int roundTo)
        {
            try
            {
                var FileProperties = new System.IO.FileInfo(filePath);
                if (FileProperties.Length < 1024) {
                    // Bytes
                    return (FileProperties.Length + " B");
                } if (FileProperties.Length >= 1024 && FileProperties.Length < 1048576) {
                    // Kilobytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1024, roundTo) + " kB";
                } if (FileProperties.Length >= 1048576 && FileProperties.Length < 1073741824) {
                    // Megabytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1048576, roundTo) + " MB";
                } if (FileProperties.Length >= 1073741824 && FileProperties.Length < 1099511627776L) {
                    // Gigabytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1073741824, roundTo) + " GB";
                } if (FileProperties.Length >= 1099511627776L && FileProperties.Length < 1099511627776L) {
                    // Terabytes
                    return Math.Round(Convert.ToDecimal(FileProperties.Length) / 1099511627776L, roundTo) + " TB";
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

    public static class TryParse
    {
        /// <summary>
        /// Parses to double with InvariantCulture
        /// </summary>
        public static double ParseDouble(string value)
        {
            double result;
            double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
            return result;
        }
        /// <summary>
        /// Parses to int with InvariantCulture
        /// </summary>
        public static int ParseInt(string value)
        {
            int result;
            int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
            return result;
        }
        /// <summary>
        /// Parses to long with InvariantCulture
        /// </summary>
        public static long ParseLong(string value)
        {
            long result;
            long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
            return result;
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
}
