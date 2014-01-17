/*
 * IFileInfo.cs
 * stores media info of a file
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System.Collections.Generic;
using MPlayer.Info.Track;

namespace MPlayer.Info
{
    public interface IFileInfo
    {
        string Url { get; set; }
        /// <summary>
        /// Returns the internal movie name (not necessarily the file name)
        /// </summary>
        string MovieName { get; set; }
        /// <summary>
        /// Returns file name without extension
        /// </summary>
        string FileName { get; set; }
        /// <summary>
        /// Returns file name with extension
        /// </summary>
        string FullFileName { get; set; }
        /// <summary>
        /// Returns MovieName if IsOnline else FullFileName
        /// </summary>
        string GetName { get; }
        /// <summary>
        /// Note: Returns 'null' if root dir ("C:\")
        /// </summary>
        string GetDirectoryName { get; set; } // Path.GetDirectoryName(...)
        /// <summary>
        /// Returns mplayer's detected file format
        /// </summary>
        string FileFormat { get; set; }
        /// <summary>
        /// Returns file size in kilobytes
        /// </summary>
        int FileSize { get; set; }
        /// <summary>
        /// Returns whether or not the file is online
        /// </summary>
        bool IsOnline { get; set; }

        bool HasVideo { get; set; }
        int VideoWidth { get; set; }
        int VideoHeight { get; set; }

        Id3Tag Id3Tags { get; set; }
        List<IdInfo> IdInfos { get; set; }

        // Tracks
        List<Subtitle> Subs { get; }
        List<Chapter> Chapters { get; }
        List<AudioTrack> AudioTracks { get; }

        void ResetInfo();
    }
}
