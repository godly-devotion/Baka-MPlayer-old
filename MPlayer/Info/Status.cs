/*
 * Status.cs
 * stores status message info from mplayer
 * 
 * Copyright (c) 2014, Joshua Park
 */

namespace MPlayer.Info
{
    public class Status
    {
        public double Duration;
        public double TotalLength;
        public PlayStates PlayState;

        public Status()
        {
            PlayState = PlayStates.Unidentified;
        }

        public Status(double duration, double totalLength, PlayStates playState)
        {
            this.Duration = duration;
            this.TotalLength = totalLength;
            this.PlayState = playState;
        }
    }
}
