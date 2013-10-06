using System;
using System.Speech.Synthesis;
using System.Windows.Forms;

public static class Speech
{
    private static readonly SpeechSynthesizer synth = new SpeechSynthesizer();

    public static void SayMedia()
    {
        if (string.IsNullOrEmpty(Info.URL))
            return;

        try
        {
            var title = Info.ID3Tags.Title;
            var artist = Info.ID3Tags.Artist;

            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(artist))
                    Speak(title + ", by " + artist);
                else
                    Speak(title);
            }
            else
                Speak(Info.MovieName);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    public static void Speak(string speech)
    {
        try
        {
            if (speech != string.Empty)
            {
                synth.SpeakAsyncCancelAll();
                synth.SpeakAsync(speech.ToLower());
            }
            else
                synth.SpeakAsyncCancelAll();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error Speaking", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}