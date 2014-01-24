/*
 * Common Static Functions
 * 
 * Copyright (c) 2014, Joshua Park
 */

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
        public static string ConvertToTime(int hour, int min, int sec)
        {
            var invC = CultureInfo.InvariantCulture;
            if (hour > 0)
                return string.Format("{0}:{1}:{2}", hour.ToString("#0", invC), min.ToString("00", invC), sec.ToString("00", invC));
            return string.Format("{0}:{1}", min.ToString("#0", invC), sec.ToString("00", invC));
        }
        /// <summary>
        /// Converts seconds to "0:00:00" format;
        /// </summary>
        public static string ConvertSecondsToTime(double totalSec)
        {
            int hour, min, sec;
            CalculateTimeFromSeconds(totalSec, out hour, out min, out sec);
            return ConvertToTime(hour, min, sec);
        }
    }
    
    public static class Url
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long PathIsURL(string pszPath);

        public static bool IsValidUrl(string url)
        {
            return PathIsURL(url).Equals(1);
        }
        public static string DecodeUrl(string input)
        {
            return IsValidUrl(input) ? System.Web.HttpUtility.UrlDecode(input) : input;
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
            var invC = CultureInfo.InvariantCulture;
            try
            {
                var fileProperties = new System.IO.FileInfo(filePath);
                if (fileProperties.Length < 1024)
                {
                    // Bytes
                    return (fileProperties.Length + " B");
                }
                if (fileProperties.Length >= 1024 && fileProperties.Length < 1048576)
                {
                    // Kilobytes
                    return Math.Round(Convert.ToDecimal(fileProperties.Length, invC) / 1024, roundTo) + " kB";
                }
                if (fileProperties.Length >= 1048576 && fileProperties.Length < 1073741824)
                {
                    // Megabytes
                    return Math.Round(Convert.ToDecimal(fileProperties.Length, invC) / 1048576, roundTo) + " MB";
                }
                if (fileProperties.Length >= 1073741824 && fileProperties.Length < 1099511627776L)
                {
                    // Gigabytes
                    return Math.Round(Convert.ToDecimal(fileProperties.Length, invC) / 1073741824, roundTo) + " GB";
                }
                if (fileProperties.Length >= 1099511627776L && fileProperties.Length < 1099511627776L)
                {
                    // Terabytes
                    return Math.Round(Convert.ToDecimal(fileProperties.Length, invC) / 1099511627776L, roundTo) + " TB";
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
        public static double ToDouble(string value)
        {
            double result;
            double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
            return result;
        }
        /// <summary>
        /// Parses to int with InvariantCulture
        /// </summary>
        public static int ToInt(string value)
        {
            int result;
            int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
            return result;
        }
        /// <summary>
        /// Parses to long with InvariantCulture
        /// </summary>
        public static long ToLong(string value)
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
