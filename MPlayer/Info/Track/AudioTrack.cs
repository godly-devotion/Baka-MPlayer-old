/*
 * AudioTracks.cs
 * stores info on each audio track
 * 
 * Copyright (c) 2014, Joshua Park
 */

namespace MPlayer.Info.Track
{
    /// <summary>
    /// Stores audio track info
    /// </summary>
    public class AudioTrack
    {
        public string ID;
        public string Name;
        public string Lang;

        public AudioTrack() { }
        public AudioTrack(string ID)
        {
            this.ID = ID;
        }
        public AudioTrack(string ID, string name, string lang)
        {
            this.ID = ID;
            this.Name = name;
            this.Lang = lang;
        }
    }
}
