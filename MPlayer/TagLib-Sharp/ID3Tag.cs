/*
 * taglib-sharp wrapper class
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using System.Drawing;
using System.IO;

namespace MPlayer.TagLib_Sharp
{
    public class Id3Tag
    {
        public PictureTag GetAlbumPictureTag(string url)
        {
            if (!File.Exists(url)) return null;

            try
            {
                using (TagLib.File f = TagLib.File.Create(url))
                {
                    if (f.Tag.Pictures.Length > 0)
                    {
                        var pic = f.Tag.Pictures[0];
                        using (var albumArt = Image.FromStream(new MemoryStream(pic.Data.Data)))
                            return new PictureTag(albumArt, pic.MimeType);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
    }
}
