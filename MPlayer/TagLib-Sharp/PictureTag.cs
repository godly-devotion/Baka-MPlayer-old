/*
 * PictureTag.cs
 * stores album art information
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using System.Drawing;

namespace MPlayer.TagLib_Sharp
{
    public class PictureTag
    {
        public Image AlbumArt;
        public string Type;

        public PictureTag(Image albumArt, string type)
        {
            this.AlbumArt = new Bitmap(albumArt);
            this.Type = type;
        }

        /// <summary>
        /// Gets the file extension for the album picture (excluding the period)
        /// </summary>
        public string GetPictureExt()
        {
            int i = Type.IndexOf('/');
            return Type.Substring(i + 1, Type.Length - i - 1);
        }
    }
}
