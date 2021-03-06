﻿/*
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
        /// Selected audio codec
        /// </summary>
        public string AudioCodec { get; set; }
        /// <summary>
        /// Selected video codec
        /// </summary>
        public string VideoCodec { get; set; }
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

        public Id3Tag Id3Tags { get; set; }

        private List<IdInfo> _idInfos = new List<IdInfo>();
        public List<IdInfo> IdInfos
        {
            get { return _idInfos; }
            set { _idInfos = value; }
        }

        // Tracks
        private List<Subtitle> _subs = new List<Subtitle>();
        public List<Subtitle> Subs
        {
            get { return _subs; }
            set { _subs = value; }
        }

        private List<Chapter> _chapters = new List<Chapter>();
        public List<Chapter> Chapters
        {
            get { return _chapters; }
            set { _chapters = value; }
        }

        private List<AudioTrack> _audioTracks = new List<AudioTrack>();
        public List<AudioTrack> AudioTracks
        {
            get { return _audioTracks; }
            set { _audioTracks = value; }
        }

        /// <summary>
        /// Reset all info (always call before playing next file)
        /// </summary>
        public void ResetInfo()
        {
            AudioCodec = string.Empty;
            VideoCodec = string.Empty;

            // VideoInfo
            HasVideo = true;
            VideoWidth = 0;
            VideoHeight = 0;

            Id3Tags = new Id3Tag
            {
                AlbumArtist = string.Empty,
                Encoder = string.Empty,
                Artist = string.Empty,
                Genre = string.Empty,
                Track = string.Empty,
                Disc = 0,
                Title = string.Empty,
                Album = string.Empty,
                Date = string.Empty,
                Comment = string.Empty,
                Description = string.Empty,

                AlbumArtTag = null
            };

            IdInfos.Clear();
            
            // Tracks
            Subs.Clear();
            Chapters.Clear();
            AudioTracks.Clear();
        }
    }
}

