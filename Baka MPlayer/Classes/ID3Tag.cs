/****************************
* UltraID3Lib wrapper class *
* by Joshua Park            *
****************************/

using System.Diagnostics;
using System.Drawing;
using System.IO;
using HundredMilesSoftware.UltraID3Lib;

public class PictureTag
{
    public Bitmap AlbumArt;
    public string Type;

    public PictureTag(Bitmap albumArt, string type)
    {
        this.AlbumArt = albumArt;
        this.Type = type;
    }

    public string GetPictureExt()
    {
        int i = Type.IndexOf('/');
        return "." + Type.Substring(i + 1, Type.Length - i - 1);
    }
}

public class ID3Tag
{
    private UltraID3 tagReader;

    public void Read(string url)
    {
        if (File.Exists(url))
        {
            try
            {
                tagReader = new UltraID3();
                tagReader.Read(url);
            }
            catch (IOException ex)
            {
                // unload tagReader
                tagReader = null;

                Debug.WriteLine("Error at ID3Tag class: " + ex.Message);
            }
        }
    }

    public PictureTag GetAlbumPictureTag()
    {
        if (tagReader == null)
            return new PictureTag(null, null);

        var frames = tagReader.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23Picture);
        if (frames.Count > 0)
        {
            var image = (ID3v23PictureFrame)frames[0];
            return new PictureTag(image.Picture, image.GetPictureMIMEType());
        }
        return new PictureTag(null, null);
    }
}