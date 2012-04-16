/****************************
* UltraID3Lib wrapper class *
* by Joshua Park            *
****************************/

using System.Drawing;
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
    private readonly UltraID3 tagReader = new UltraID3();

    public void Read(string url)
    {
        tagReader.Read(url);
    }

    public PictureTag GetAlbumPictureTag()
    {
        var frames = tagReader.ID3v2Tag.Frames.GetFrames(MultipleInstanceID3v2FrameTypes.ID3v23Picture);
        if (frames.Count > 0)
        {
            var image = (ID3v23PictureFrame)frames[0];
            return new PictureTag(image.Picture, image.GetPictureMIMEType());
        }
        return new PictureTag(null, null);
    }
}