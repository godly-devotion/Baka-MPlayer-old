/*****************************
* taglib-sharp wrapper class *
* by Joshua Park             *
*****************************/
using System.Drawing;
using System.IO;

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

public class ID3Tag
{
    public PictureTag GetAlbumPictureTag(string url)
    {
        if (File.Exists(url))
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
        return null;
    }
}