/*
 * Helper Functions
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Helper
{
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

    public static class IO
    {
        public static string GetDirectoryName(string f)
        {
            try
            {
                return f.Substring(0, f.LastIndexOf(Path.DirectorySeparatorChar));
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
                return "n/a";
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
}
