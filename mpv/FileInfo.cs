/*
 * FileInfo.cs
 * stores media info of a file
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using System.Collections.Generic;
using MPlayer.Info.Track;

namespace MPlayer.Info
{
    /// <summary>
    /// Stores media info of a file
    /// </summary>
    public class FileInfo : IFileInfo
    {
        public string Url { set; get; }
        /// <summary>
        /// Returns the internal movie name (not necessarily the file name)
        /// </summary>
        public string MovieName { set; get; }
        /// <summary>
        /// Returns file name without extension
        /// </summary>
        public string FileName { set; get; }
        /// <summary>
        /// Returns file name with extension
        /// </summary>
        public string FullFileName { set; get; }
        /// <summary>
        /// Returns MovieName if IsOnline else FullFileName
        /// </summary>
        public string GetName
        {
            get { return IsOnline ? MovieName : FullFileName; }
        }
        /// <summary>
        /// Note: Returns 'null' if root dir ("C:\")
        /// </summary>
        public string GetDirectoryName { get; set; } // Path.GetDirectoryName(...)
        /// <summary>
        /// Returns mplayer's detected file format
        /// </summary>
        public string FileFormat { get; set; }
        /// <summary>
        /// Returns file size in kilobytes
        /// </summary>
        public int FileSize { get; set; }
        /// <summary>
        /// Returns whether or not the file is online
        /// </summary>
        public bool IsOnline { get; set; }

        public bool HasVideo { get; set; }
        public int VideoWidth { get; set; }
        public int VideoHeight { get; set; }

        public ID3Tag Id3Tags { get; set; }

        // MiscInfo
        public List<Subtitle> Subs { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<AudioTrack> AudioTracks { get; set; }
        public List<ID_Info> OtherInfos { get; set; }

        /// <summary>
        /// Always call before playing next file.
        /// </summary>
        public void ResetInfo()
        {
            // MiscInfo
            Subs = new List<Subtitle>();
            Chapters = new List<Chapter>();
            AudioTracks = new List<AudioTrack>();
            OtherInfos = new List<ID_Info>();

            // VideoInfo
            HasVideo = true;
            VideoWidth = 0;
            VideoHeight = 0;

            // ID3Tags
            Id3Tags = new ID3Tag
            {
                Album_Artist = String.Empty,
                Encoder = String.Empty,
                Artist = String.Empty,
                Genre = String.Empty,
                Track = String.Empty,
                Disc = 0,
                Title = String.Empty,
                Album = String.Empty,
                Date = String.Empty,
                Comment = String.Empty,
                Description = String.Empty,

                AlbumArtTag = null
            };
        }
    }
}

