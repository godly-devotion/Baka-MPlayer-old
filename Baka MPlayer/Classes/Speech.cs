using System;
using System.Speech.Synthesis;
using System.Windows.Forms;
using MPlayer.Info;

public class Speech
{
    private readonly SpeechSynthesizer synth = new SpeechSynthesizer();
    private readonly IFileInfo fileInfo;

    public Speech(IFileInfo fileInfo)
    {
        this.fileInfo = fileInfo;
    }

    public void SayMedia()
    {
        if (string.IsNullOrEmpty(fileInfo.Url))
            return;

        try
        {
            var title = fileInfo.Id3Tags.Title;
            var artist = fileInfo.Id3Tags.Artist;

            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(artist))
                    Speak(title + ", by " + artist);
                else
                    Speak(title);
            }
            else Speak(fileInfo.MovieName);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    public void Speak(string speech)
    {
        try
        {
            if (!string.IsNullOrEmpty(speech))
            {
                synth.SpeakAsyncCancelAll();
                synth.SpeakAsync(speech.ToLower());
            }
            else synth.SpeakAsyncCancelAll();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error Speaking", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}