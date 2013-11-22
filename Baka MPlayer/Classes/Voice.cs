﻿using Baka_MPlayer.Forms;
using System;
using System.Media;
using System.Speech.Recognition;

public enum VoiceState
{
    SpeechRecognized,
    SpeechDetected,
    SpeechRejected,
    SpeechCompleted
}

public class Voice : IDisposable
{
    private readonly SpeechRecognitionEngine engine;
    private readonly SoundPlayer sfx = new SoundPlayer();
    private readonly MainForm mainForm;
    public string callName;

    public Voice(MainForm mainForm, string callName)
    {
        this.mainForm = mainForm;
        this.callName = callName;

        // Create an in-process speech recognizer.
        engine = new SpeechRecognitionEngine();

        // Configure input to the speech recognizer.
        engine.SetInputToDefaultAudioDevice();
        
        var choice = new Choices(new[] {
            "open",
            "play",
            "pause",
            "rewind",
            "stop",
            "mute",
            "unmute",
            "increase volume",
            "volume up",
            "decrease volume",
            "volume down",
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
            "hide",
            "show",
            "whats playing",
            "help",
            "stop listening",
            "close"
        });

        var grammarBuilder = new GrammarBuilder(callName);
        grammarBuilder.Append(choice.ToGrammarBuilder());
        
        // Add the grammars
        engine.LoadGrammarAsync(new Grammar(grammarBuilder));

        // Adds handlers for the grammar's speech recognized event.
        engine.SpeechRecognized += recognizer_SpeechRecognized;
        engine.SpeechDetected += engine_SpeechDetected;
        engine.SpeechRecognitionRejected += engine_SpeechRecognitionRejected;
        engine.RecognizeCompleted += engine_RecognizeCompleted;
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

        sfx.SoundLocation = @"C:\Windows\Media\Speech Sleep.wav";
        sfx.Play();
    }

    // Handle the SpeechRecognized event.
    public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        if (e.Result.Confidence > 0.8F)
        {
            mainForm.CallStateChanged(VoiceState.SpeechRecognized);
            mainForm.CallTakeAction(e.Result.Text.ToUpperInvariant().Substring(callName.Length + 1));
            return;
        }
        mainForm.CallStateChanged(VoiceState.SpeechRejected);
    }

    private void engine_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
    {
        mainForm.CallStateChanged(VoiceState.SpeechCompleted);
    }

    private void engine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
    {
        mainForm.CallStateChanged(VoiceState.SpeechRejected);
    }

    private void engine_SpeechDetected(object sender, SpeechDetectedEventArgs e)
    {
        mainForm.CallStateChanged(VoiceState.SpeechDetected);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
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
