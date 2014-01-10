/*
 * ID3Tag.cs
 * 
 * Copyright (c) 2014, Joshua Park
 */

using MPlayer.TagLib_Sharp;

namespace MPlayer.Info
{
    public class ID3Tag
    {
        // ID_CLIP_INFO
        public string Album_Artist;
        public string Encoder;
        public string Artist;
        public string Genre;
        public string Track;
        public int Disc;
        public string Title;
        public string Album;
        public string Date;
        public string Comment;
        public string Description;

        // Album Picture tag
        public PictureTag AlbumArtTag;
    }
}
