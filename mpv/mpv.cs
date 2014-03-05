/*
 * mpv wrapper
 * 
 * Copyright (c) 2014, Joshua Park & u8sand
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using MPlayer;
using MPlayer.Info;
using MPlayer.Info.Track;

namespace mpv
{
    public class mpv : IMPlayer
    {
        #region Private Fields

        private Process mpvProcess;
        private StreamWriter stdinWriter;
        private readonly MPlayer.TagLib_Sharp.Id3Tag id3Tag = new MPlayer.TagLib_Sharp.Id3Tag();
        private readonly int wid;
        private bool cachingFonts;
        private bool parsingClipInfo;
        private bool ignoreStatusMsg;
        private string externalSub;

        #endregion

        private Status _status = new Status();
        public Status CurrentStatus
        {
            get { return _status; }
            set { _status = value; }
        }

        private IFileInfo _fileInfo = new MPlayer.Info.FileInfo();
        public IFileInfo FileInfo
        {
            get { return _fileInfo; }
            set { _fileInfo = value; }
        }
        public int Volume { get; set; }

        public mpv(int wid)
        {
            this.wid = wid;
        }

        public bool OpenFile(string url)
        {
            ignoreStatusMsg = false;
            FileInfo.ResetInfo();
            OnStdOut(new StdOutEventArgs("[ACTION] CLEAR_OUTPUT"));
            OnStatusChanged(new StatusChangedEventArgs("Loading file...", false));

            if (!PlayerIsRunning())
                InitPlayer();

            return SendCommand("loadfile \"{0}\"", url.Replace("\\", "\\\\"));
        }
        public bool OpenFile(string url, string subFile)
        {
            externalSub = subFile;
            return OpenFile(url);
        }
        public bool SendCommand(string command)
        {
            try
            {
                if (!PlayerIsRunning())
                    throw new Exception("mpv is not running!");                
                
                stdinWriter.WriteLine(command);
                stdinWriter.Flush();
            }
            catch (Exception) { return false; }
            return true;
        }
        /// <summary>
        /// Send command using String.Format(command, value)
        /// </summary>
        public bool SendCommand(string command, object value)
        {
            return SendCommand(string.Format(CultureInfo.InvariantCulture, command, value));
        }
        public bool PlayerIsRunning()
        {
            return mpvProcess != null && !mpvProcess.HasExited;
        }
        public string GetProcessState()
        {
            string strResp = mpvProcess.Responding ? "" : " - not responding";
            return string.Format("mpv's process ID: {0}{1}", mpvProcess.Id, strResp);
        }
        public bool Quit()
        {
            try
            {
                if (!PlayerIsRunning())
                    return true;

                SendCommand("quit");
                stdinWriter.Close();
                mpvProcess.CancelOutputRead();
                mpvProcess.CancelErrorRead();
            }
            catch (Exception) { return false; }
            return true;
        }

        public bool Play()
        {
            ignoreStatusMsg = false;
            if (CurrentStatus.PlayState == PlayStates.Ended)
                return OpenFile(FileInfo.Url);
            return SendCommand("set pause no");
        }
        public bool Pause(bool toggle)
        {
            ignoreStatusMsg = false;
            if (CurrentStatus.PlayState == PlayStates.Ended)
                return OpenFile(FileInfo.Url);
            return SendCommand(toggle ? "cycle pause" : "set pause yes");
        }
        public bool Stop()
        {
            ignoreStatusMsg = true;
            SetPlayState(PlayStates.Stopped);
            return SendCommand("pausing seek 0 absolute");
        }
        public bool Restart()
        {
            return Rewind() && Play();
        }
        public bool Rewind()
        {
            return SendCommand("seek 0 absolute");
        }

        public bool Seek(double sec)
        {
            ignoreStatusMsg = false;
            return SendCommand("seek {0} absolute", sec);
        }
        public bool SeekPercent(double percent)
        {
            ignoreStatusMsg = false;
            return SendCommand("seek {0} absolute-percent", percent);
        }

        /// <summary>
        /// Sets chapter
        /// </summary>
        /// <param name="index">The chapter index (zero based)</param>
        public bool SetChapter(int index)
        {
            OnStatusChanged(new StatusChangedEventArgs(
                string.Format("Chapter {0}: \"{1}\"", index + 1, FileInfo.Chapters[index].ChapterName), true));
            return SendCommand("set chapter {0}", index);
        }
        public bool SkipChapter(int jump)
        {
            return SendCommand("add chapter {0}", jump);
        }
        public bool PreviousChapter()
        {
            return SkipChapter(-1);
        }
        public bool NextChapter()
        {
            return SkipChapter(1);
        }

        public bool Mute(bool mute)
        {
            return SendCommand("set mute {0}", mute ? "yes" : "no");
        }
        public bool SetVolume(int volume)
        {
            Volume = volume;
            if (PlayerIsRunning())
                return volume > -1 && SendCommand("set volume {0}", volume);
            return true;
        }

        /// <summary>
        /// Switches the audio track to the index specified
        /// </summary>
        /// <param name="trackId">The audio index (index starts from 1)</param>
        public bool SetAudioTrack(int trackId)
        {
            OnStatusChanged(new StatusChangedEventArgs(
                string.Format("Audio {0}: \"{1} ({2})\"",
                    FileInfo.AudioTracks[trackId - 1].ID, FileInfo.AudioTracks[trackId - 1].Name, FileInfo.AudioTracks[trackId - 1].Lang), true));
            return SendCommand("set audio {0}", trackId);
        }
        public bool SetVideoTrack(int trackId)
        {
            return SendCommand("set vid {0}", trackId);
        }
        /// <summary>
        /// Set subtitle track
        /// </summary>
        /// <param name="trackId">The subtitle index (index start from 1)</param>
        public bool SetSubtitleTrack(int trackId)
        {
            OnStatusChanged(new StatusChangedEventArgs(
                string.Format("Sub {0}: \"{1} ({2})\"",
                    FileInfo.Subs[trackId - 1].TrackID, FileInfo.Subs[trackId - 1].Name, FileInfo.Subs[trackId - 1].Lang), true));
            return SendCommand("set sub {0}", trackId);
        }
        public bool SetSubtitleVisibility(bool showSubs)
        {
            OnStatusChanged(new StatusChangedEventArgs("Subs " + (showSubs ? "shown" : "hidden"), true));
            return SendCommand("set sub-visibility {0}", showSubs ? "yes" : "no");
        }

        #region Functions

        private bool InitPlayer()
        {
            try
            {
                // instructs fontconfig to show debug messages regarding font caching
                Environment.SetEnvironmentVariable("FC_DEBUG", "128");

                const string statusMsg = "PAUSED=${=pause};PERCENT=${=percent-pos};AV=${=time-pos};WIDTH=${=dwidth};HEIGHT=${=dheight}";

                // set mplayer options (see mpv\config for more)
                var args = new StringBuilder();
                args.Append(" -slave-broken");
                args.Append(" -no-consolecontrols");// prevents the player from reading key events from standard input
                args.Append(" -idle");              // wait insead of quit
                args.Append(" -volstep=5");
                args.Append(" -msglevel=cplayer=v");// needed for EOF message
                args.Append(" -identify");          // needed for ID_* info
                args.Append(" -osd-level=0");       // do not show volume + seek on OSD
                args.Append(" -no-keepaspect");     // doesn't keep window aspect ratio when resizing windows
                args.Append(" -no-autosub");        // do not auto load subs (via filename)
                args.Append(" -cursor-autohide=no");
                args.Append(" -playing-msg=PLAYING_FILE:${media-title}");
                args.AppendFormat(" -status-msg=status:{0}", statusMsg);
                args.AppendFormat(" -volume={0}", Volume); // set previous volume
                args.AppendFormat(" -wid={0}", wid); // output handle

                mpvProcess = new Process
                {
                    EnableRaisingEvents = true,
                    StartInfo =
                    {
                        FileName = "mpv.exe",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8,

                        Arguments = args.ToString()
                    }
                };
                mpvProcess.Start();

                stdinWriter = new StreamWriter(mpvProcess.StandardInput.BaseStream, new UTF8Encoding(false))
                {
                    // use LF instead of CRLF
                    NewLine = "\n"
                };

                mpvProcess.OutputDataReceived += OutputDataReceived;
                mpvProcess.ErrorDataReceived += ErrorDataReceived;
                mpvProcess.Exited += Exited;
                mpvProcess.BeginOutputReadLine();
                mpvProcess.BeginErrorReadLine();
            }
            catch (Exception) { return false; }
            return true;
        }

        private void SetPlayState(PlayStates newState)
        {
            if (CurrentStatus.PlayState == newState)
                return;

            CurrentStatus.PlayState = newState;
            OnPlayStateChanged(new PlayStateChangedEventArgs(newState));
        }

        #endregion

        #region Events

        public event EventHandler<StdOutEventArgs> StdOutEvent;

        protected virtual void OnStdOut(StdOutEventArgs e)
        {
            if (StdOutEvent != null)
            {
                StdOutEvent(this, e);
            }
        }

        public event EventHandler<StatusChangedEventArgs> StatusChangedEvent;

        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
        {
            if (StatusChangedEvent != null)
            {
                StatusChangedEvent(this, e);
            }
        }

        public event EventHandler<EventArgs> FileOpenedEvent;

        protected virtual void OnFileOpened(EventArgs e)
        {
            if (FileOpenedEvent != null)
            {
                FileOpenedEvent(this, e);
            }
        }

        public event EventHandler<PlayStateChangedEventArgs> PlayStateChangedEvent;

        protected virtual void OnPlayStateChanged(PlayStateChangedEventArgs e)
        {
            if (PlayStateChangedEvent != null)
            {
                PlayStateChangedEvent(this, e);
            }
        }

        public event EventHandler<EventArgs> DurationChangedEvent;

        protected virtual void OnDurationChanged(EventArgs e)
        {
            if (DurationChangedEvent != null)
            {
                DurationChangedEvent(this, e);
            }
        }

        public event EventHandler<EventArgs> MPlayerQuitEvent;

        protected virtual void OnMPlayerQuit(EventArgs e)
        {
            if (MPlayerQuitEvent != null)
            {
                MPlayerQuitEvent(this, e);
            }
        }

        private void Exited(object sender, EventArgs e)
        {
            if (mpvProcess.ExitCode != 0)
            {
                throw new Exception("mpv quit unexpectedly!");
            }
            OnMPlayerQuit(e);
        }

        #endregion

        #region Standard Streams

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine("#err:" + e.Data);

            if (e.Data == null || ignoreStatusMsg)
                return;
            
            if (e.Data.StartsWith("status:"))
            {
                ParseStatusMsg(e.Data.Substring(7)); // parse out 'status:'
                return;
            }

            //[fontconfig] Scanning dir C:/Windows/Fonts (must set FC_DEBUG to show)
            if (!cachingFonts && e.Data.StartsWith("[fontconfig]"))
            {
                cachingFonts = true;
                OnStatusChanged(new StatusChangedEventArgs("Caching fonts...", false));
            }
            else if (cachingFonts && e.Data.StartsWith("["))
            {
                if (e.Data.StartsWith("[fontconfig]"))
                {
                    cachingFonts = false;
                    OnStatusChanged(new StatusChangedEventArgs("Fonts finished caching", true));
                }
                else
                {
                    OnStatusChanged(new StatusChangedEventArgs("Caching fonts: " + e.Data, false));
                }
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine("#out:" + e.Data);

            if (e.Data == null)
                return;

            // show output
            OnStdOut(new StdOutEventArgs(e.Data));

            // reached end of file
            if (e.Data.StartsWith("[cplayer] EOF code: 1") ||
                e.Data.StartsWith("[cplayer] EOF code: 2"))
            {
                ignoreStatusMsg = true;
                SetPlayState(PlayStates.Ended);
                return;
            }

            if (e.Data.Equals("Clip info:")) // ** RENAME TO 'File tags:' in future builds
            {
                parsingClipInfo = true;
                return;
            }
            if (parsingClipInfo)
            {
                ParseClipInfo(e.Data);
                return;
            }

            if (e.Data.StartsWith("ID_") && !e.Data.StartsWith("ID_PAUSED"))
            {
                // Parsing "ID_*"
                var i = e.Data.IndexOf('=');
                var key = e.Data.Substring(0, i);
                var value = e.Data.Substring(i + 1);

                ProcessDetails(key, value);
                FileInfo.IdInfos.Add(new IdInfo(key, value));
                return;
            }

            if (e.Data.Trim().StartsWith("**** Audio/Video desynchronisation detected! ****"))
            {
                OnStatusChanged(new StatusChangedEventArgs("Playback lag detected!", true));
                return;
            }

            if (e.Data.StartsWith("Cache fill:"))
            {
                OnStatusChanged(new StatusChangedEventArgs(e.Data.Trim(), true));
                return;
            }
            if (e.Data.StartsWith("[cache] Cache is not responding - slow/stuck network connection?") ||
                e.Data.StartsWith("[cache] Cache keeps not responding."))
            {
                OnStatusChanged(new StatusChangedEventArgs("Your network is slow or stuck, please wait a bit", true));
                return;
            }

            if (e.Data.StartsWith("Detected file format: "))
            {
                FileInfo.FileFormat = e.Data.Substring(22);
                return;
            }
            if (e.Data.StartsWith("Failed to recognize file format.") ||
                e.Data.StartsWith("Failed to open "))
            {
                OnStatusChanged(new StatusChangedEventArgs("[ERROR] FAILED_TO_OPEN", true));
                return;
            }

            if (e.Data.StartsWith("Video: no video"))
            {
                FileInfo.HasVideo = false;
                return;
            }

            if (e.Data.StartsWith("PLAYING_FILE:"))
            {
                OnStatusChanged(new StatusChangedEventArgs("[ACTION] HIDE_STATUS_LABEL", false));

                // parse the string after 'PLAYING_FILE:' (e.g. the media name or Youtube title)
                FileInfo.MovieName = e.Data.Substring(13);

                // load external sub if requested
                if (!string.IsNullOrEmpty(externalSub))
                {
                    SendCommand("sub_add \"{0}\"", externalSub.Replace("\\", "\\\\"));
                    FileInfo.Subs.Add(new Subtitle(FileInfo.Subs.Count.ToString(CultureInfo.InvariantCulture), "[ External Sub ]", "★"));
                    SetSubtitleTrack(FileInfo.Subs.Count);

                    externalSub = string.Empty;
                }

                // get album picture tag
                if (!FileInfo.IsOnline)
                {
                    FileInfo.Id3Tags.AlbumArtTag = id3Tag.GetAlbumPictureTag(FileInfo.Url);
                    if (FileInfo.Id3Tags.AlbumArtTag != null)
                        FileInfo.HasVideo = false;
                }

                // tell mainform that new file was opened
                OnFileOpened(new EventArgs());
            }
        }

        private void ParseStatusMsg(string line)
        {
            //PAUSED=no;PERCENT=123.456789;AV=123.456789;WIDTH=1920;HEIGHT=1080;
            string[] info = line.Split(';');

            foreach (var s in info)
            {
                var i = s.IndexOf('=');
                var value = s.Substring(i + 1, s.Length - i - 1);
                switch (s.Substring(0, i))
                {
                    case "PAUSED":
                        SetPlayState(value == "yes" ? PlayStates.Paused : PlayStates.Playing);
                        break;
                    case "PERCENT":
                        CurrentStatus.PercentPos = Helper.TryParse.ToDouble(value);
                        break;
                    case "AV":
                        ProcessProgress(Helper.TryParse.ToDouble(value));
                        break;
                    case "WIDTH":
                        FileInfo.VideoWidth = Helper.TryParse.ToInt(value);
                        break;
                    case "HEIGHT":
                        FileInfo.VideoHeight = Helper.TryParse.ToInt(value);
                        break;
                }
            }
        }
        private void ProcessProgress(double sec)
        {
            if (sec > 0.0 && Math.Abs(CurrentStatus.Duration - sec) > 0.000001)
            {
                CurrentStatus.Duration = sec;
                OnDurationChanged(new EventArgs());
            }
        }

        private bool ProcessDetails(string key, string value)
        {
            switch (key)
            {
                case "ID_FILENAME":
                    FileInfo.Url = value;
                    FileInfo.IsOnline = !File.Exists(FileInfo.Url);
                    FileInfo.FullFileName = Path.GetFileName(Helper.Url.DecodeUrl(value));
                    FileInfo.FileName = Path.GetFileNameWithoutExtension(FileInfo.FullFileName);
                    FileInfo.GetDirectoryName = Helper.IO.GetDirectoryName(value);
                    break;
                case "ID_LENGTH":
                    CurrentStatus.TotalLength = Helper.TryParse.ToDouble(value);
                    break;
                default:
                    if (key.StartsWith("ID_CHAPTER_ID")) // Chapters
                    {
                        FileInfo.Chapters.Add(new Chapter());
                    }
                    else if (key.StartsWith("ID_CHAPTER_"))
                    {
                        if (key.Contains("_START"))
                        {
                            FileInfo.Chapters[FileInfo.Chapters.Count - 1].StartTime = Helper.TryParse.ToLong(value);
                        }
                        else if (key.Contains("_NAME"))
                        {
                            FileInfo.Chapters[FileInfo.Chapters.Count - 1].ChapterName = value;
                        }
                        return true;
                    }
				
                    else if (key.StartsWith("ID_SID_ID")) // Subtitles
                    {
                        FileInfo.Subs.Add(new Subtitle(value));
                    }
                    else if (key.StartsWith("ID_SID_"))
                    {
                        if (key.Contains("_NAME"))
                        {
                            FileInfo.Subs[FileInfo.Subs.Count - 1].Name = value;
                        }
                        else if (key.Contains("_LANG"))
                        {
                            FileInfo.Subs[FileInfo.Subs.Count - 1].Lang = value;
                        }
                        return true;
                    }
                
                    else if (key.StartsWith("ID_AID_ID")) // Audio tracks
                    {
                        FileInfo.AudioTracks.Add(new AudioTrack(value));
                    }
                    else if (key.StartsWith("ID_AID_"))
                    {
                        if (key.Contains("_NAME"))
                        {
                            FileInfo.AudioTracks[FileInfo.AudioTracks.Count - 1].Name = value;
                        }
                        else if (key.Contains("_LANG"))
                        {
                            FileInfo.AudioTracks[FileInfo.AudioTracks.Count - 1].Lang = value;
                        }
                        return true;
                    }
                    return false;
            }
            return true;
        }

        private void ParseClipInfo(string data)
        {
            data = data.Trim();

            var i = data.IndexOf(": ", StringComparison.Ordinal);

            if (i < 1)
            {
                if (data.StartsWith("ID_CLIP_INFO_N="))
                    parsingClipInfo = false;
                return;
            }

            var s = data.Substring(i+2);
            data = data.ToLower();

            if (data.StartsWith("title:"))
                FileInfo.Id3Tags.Title = s;
            else if (data.StartsWith("artist:"))
                FileInfo.Id3Tags.Artist = s;
            else if (data.StartsWith("album:"))
                FileInfo.Id3Tags.Album = s;
            else if (data.StartsWith("date:"))
                FileInfo.Id3Tags.Date = s;
            else if (data.StartsWith("track:"))
                FileInfo.Id3Tags.Track = s;
            else if (data.StartsWith("genre:"))
                FileInfo.Id3Tags.Genre = s;
            else if (data.StartsWith("description:"))
                FileInfo.Id3Tags.Description = s;
            else if (data.StartsWith("comment:"))
                FileInfo.Id3Tags.Comment = s;
            else if (data.StartsWith("album_artist:"))
                FileInfo.Id3Tags.AlbumArtist = s;
            else if (data.StartsWith("encoder:"))
                FileInfo.Id3Tags.Encoder = s;
            else if (data.StartsWith("disk:"))
                FileInfo.Id3Tags.Disc = Helper.TryParse.ToInt(s);
        }

        #endregion
    }
}
