/*
 * Chapter.cs
 * stores info on each chapter
 * 
 * Copyright (c) 2014, Joshua Park
 */

namespace MPlayer.Info.Track
{
    /// <summary>
    /// Stores chapter's start time and name
    /// </summary>
    public class Chapter
    {
        /// <summary>
        /// In miliseconds
        /// </summary>
        public long StartTime;
        public string ChapterName;

        public Chapter() { }
        public Chapter(long startTime, string chapterName)
        {
            this.StartTime = startTime;
            this.ChapterName = chapterName;
        }
    }
}
