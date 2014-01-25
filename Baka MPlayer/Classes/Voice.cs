using System;
using System.Media;
using System.Speech.Recognition;
using Baka_MPlayer.Forms;

public class Voice : IDisposable
{
    private readonly SpeechRecognitionEngine engine;
    private readonly SoundPlayer sfx = new SoundPlayer();
    private readonly MainForm mainForm;
    private readonly string callName;

    public Voice(MainForm mainForm, string callName)
    {
        this.mainForm = mainForm;
        this.callName = callName;

        engine = new SpeechRecognitionEngine();
        engine.SetInputToDefaultAudioDevice();
        
        var choice = new Choices(new[] {
            "open",
            "open file",
            "mute",
            "unmute",
            "increase volume",
            "raise volume",
            "volume up",
            "decrease volume",
            "lower volume",
            "volume down",
            "hide",
            "show",
            "help",
            "stop listening",
            "close",
            "play",
            "pause",
            "rewind",
            "stop",
            "next chapter",
            "skip chapter",
            "previous chapter",
            "next",
            "next file",
            "previous",
            "previous file",
            "fullscreen",
            "view fullscreen",
            "go fullscreen",
            "exit fullscreen",
            "leave fullscreen",
            "whats playing"
        });

        var grammarBuilder = new GrammarBuilder(callName);
        grammarBuilder.Append(choice.ToGrammarBuilder());
        
        // add the grammars
        engine.LoadGrammarAsync(new Grammar(grammarBuilder));

        // adds handlers for the grammar's speech recognized event.
        engine.SpeechRecognized += recognizer_SpeechRecognized;
        engine.AudioLevelUpdated += engine_AudioLevelUpdated;
    }

    public void StartListening()
    {
        engine.RecognizeAsync(RecognizeMode.Multiple);

        sfx.SoundLocation = @"C:\Windows\Media\Speech On.wav";
        sfx.Play();
    }

    public void StopListening()
    {
        engine.RecognizeAsyncStop();
        mainForm.CallUpdateAudioLevel(0);

        sfx.SoundLocation = @"C:\Windows\Media\Speech Sleep.wav";
        sfx.Play();
    }

    private void PlayRecognizedCommandSound()
    {
        sfx.SoundLocation = @"C:\Windows\Media\ding.wav";
        sfx.Play();
    }

    private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        if (e.Result.Confidence > 0.8F)
        {
            PlayRecognizedCommandSound();
            mainForm.CallTakeAction(e.Result.Text.ToLowerInvariant().Substring(callName.Length + 1));
        }
    }

    public void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
    {
        if (!mainForm.IsDisposed)
            mainForm.CallUpdateAudioLevel(e.AudioLevel);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            engine.RecognizeAsyncStop();

            // dispose managed resources
            engine.Dispose();
            sfx.Dispose();
        }
        // free native resources
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
