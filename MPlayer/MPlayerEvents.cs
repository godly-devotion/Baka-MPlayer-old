/*
 * Custom events for mplayer
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using MPlayer.Info;

namespace MPlayer
{
    public class StdOutEventArgs : EventArgs
    {
        public string StdOut { get; private set; }

        public StdOutEventArgs(string stdOut)
        {
            StdOut = stdOut;
        }
    }

    public class StatusChangedEventArgs : EventArgs
    {
        public string Status { get; private set; }
        public bool AutoHide { get; private set; }

        public StatusChangedEventArgs(string status, bool autoHide)
        {
            Status = status;
            AutoHide = autoHide;
        }
    }

    public class PlayStateChangedEventArgs : EventArgs
    {
        public PlayStates PlayState { get; private set; }

        public PlayStateChangedEventArgs(PlayStates playState)
        {
            PlayState = playState;
        }
    }
}
