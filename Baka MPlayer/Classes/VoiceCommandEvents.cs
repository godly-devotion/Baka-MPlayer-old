/*
 * Custom events for VoiceCommandEngine
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;

namespace VoiceCommandEvents
{
    public class SpeechRecognizedEventArgs : EventArgs
    {
        public string Command { get; private set; }

        public SpeechRecognizedEventArgs(string command)
        {
            Command = command;
        }
    }

    public class AudioLevelUpdatedEventArgs : EventArgs
    {
        public int AudioLevel { get; private set; }

        public AudioLevelUpdatedEventArgs(int audioLevel)
        {
            AudioLevel = audioLevel;
        }
    }
}
