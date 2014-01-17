/*
 * Subtitle.cs
 * stores info on each subtitle
 * 
 * Copyright (c) 2014, Joshua Park
 */

namespace MPlayer.Info.Track
{
    /// <summary>
    /// Stores subtitle info
    /// </summary>
    public class Subtitle
    {
        public string TrackID;
        public string Name;
        public string Lang; // language (jpn, eng, ...)

        public Subtitle() { }
        public Subtitle(string trackId)
        {
            this.TrackID = trackId;
        }
        public Subtitle(string trackId, string name, string lang)
        {
            this.TrackID = trackId;
            this.Name = name;
            this.Lang = lang;
        }
    }
}
