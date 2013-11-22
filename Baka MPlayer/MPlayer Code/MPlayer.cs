﻿/**************************
* mpv wrapper             *
* by Joshua Park & u8sand *
**************************/
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

public class MPlayer
{
    #region Variables

    private Process mplayer;
    private readonly ID3Tag id3Tag = new ID3Tag();
    private readonly int wid;
    private bool cachingFonts;
    private bool parsingClipInfo;
    private bool ignoreStatus;
    public string ExternalSub { get; set; }

    #endregion

    #region Events

    public event EventHandler<StdOutEventArgs> StdOutEvent;

    protected virtual void OnStdOut(StdOutEventArgs e)
    {
        var handler = this.StdOutEvent;
        if (handler != null)
        {
            StdOutEvent(this, e);
        }
    }

    public event EventHandler<StatusChangedEventArgs> StatusChangedEvent;

    protected virtual void OnStatusChanged(StatusChangedEventArgs e)
    {
        var handler = this.StatusChangedEvent;
        if (handler != null)
        {
            StatusChangedEvent(this, e);
        }
    }

    public event EventHandler<EventArgs> FileOpenedEvent;

    protected virtual void OnFileOpened(EventArgs e)
    {
        var handler = this.FileOpenedEvent;
        if (handler != null)
        {
            FileOpenedEvent(this, e);
        }
    }

    public event EventHandler<EventArgs> PlayStateChangedEvent;

    protected virtual void OnPlayStateChanged(EventArgs e)
    {
        var handler = this.PlayStateChangedEvent;
        if (handler != null)
        {
            PlayStateChangedEvent(this, e);
        }
    }

    public event EventHandler<EventArgs> DurationChangedEvent;

    protected virtual void OnDurationChanged(EventArgs e)
    {
        var handler = this.DurationChangedEvent;
        if (handler != null)
        {
            DurationChangedEvent(this, e);
        }
    }

    #endregion

    #region Constructor

    public MPlayer(int wid)
    {
        this.wid = wid;
    }

    #endregion

    #region Functions

    public bool OpenFile(string url)
    {
        try
        {
            Info.ResetInfo();
            OnStatusChanged(new StatusChangedEventArgs("Loading file...", false));
            OnStdOut(new StdOutEventArgs("[Baka_MPlayer] CLEAR_OUTPUT"));

            if (MPlayerIsRunning())
            {
                SendCommand("loadfile \"{0}\"", url.Replace("\\", "\\\\")); // open file
                return true;
            }
            // instructs fontconfig to show debug messages regarding font caching
            Environment.SetEnvironmentVariable("FC_DEBUG", "128");

            // set mplayer options (see mpv\config)
            var args = new StringBuilder();
            args.Append(" -slave-broken");                      // switch on slave mode for frontend
            args.Append(" -idle");                              // wait insead of quit
            args.Append(" -volstep=5");                         // volume step
            args.Append(" -msglevel cplayer=6:identify=6");     // set msglevel
            args.Append(" -osd-level=0");                       // do not show volume + seek on OSD
            args.Append(" -no-keepaspect");                     // doesn't keep window aspect ratio when resizing windows
            args.Append(" -no-autosub");                        // do not auto load subs (via filename)
            args.Append(" -cursor-autohide=no");                // disable cursor autohide
            //args.Append(" -no-osc");                          // disable osc
            args.Append(" -playing-msg=PLAYING_FILE:${media-title}");
            args.Append(" -status-msg=status:PAUSED=${=pause};AV=${=time-pos};WIDTH=${=dwidth};HEIGHT=${=dheight}");
            args.AppendFormat(" -volume={0}", Info.Current.Volume); // sets previous volume
            args.AppendFormat(" -wid={0}", wid); // output handle
            
            mplayer = new Process
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

                    Arguments = args.AppendFormat(" \"{0}\"", url).ToString()
                }
            };
            mplayer.Start();
            mplayer.EnableRaisingEvents = true;
            // use LF instead of CRLF
            mplayer.StandardInput.NewLine = "\n";

            mplayer.OutputDataReceived += OutputDataReceived;
            mplayer.ErrorDataReceived += ErrorDataReceived;
            mplayer.Exited += Exited;
            mplayer.BeginOutputReadLine();
            mplayer.BeginErrorReadLine();
        }
        catch (Exception) { return false; }
        return true;
    }
    /// <summary>
    /// Send command to player
    /// </summary>
    public bool SendCommand(string command)
    {
        try
        {
            if (!MPlayerIsRunning())
                throw new Exception();

            byte[] buffer = Encoding.UTF8.GetBytes(command);
            mplayer.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
            mplayer.StandardInput.WriteLine();
            //mplayer.StandardInput.WriteLine(command);
            mplayer.StandardInput.Flush();
        }
        catch (Exception) { return false; }
        return true;
    }
    /// <summary>
    /// Send command to player (uses String.Format)
    /// </summary>
    public bool SendCommand(string command, object value)
    {
        try
        {
            if (!MPlayerIsRunning())
                throw new Exception();

            byte[] buffer = Encoding.UTF8.GetBytes(string.Format(command,value));
            mplayer.StandardInput.BaseStream.Write(buffer, 0, buffer.Length);
            mplayer.StandardInput.WriteLine();
            //mplayer.StandardInput.WriteLine(command, value);
            mplayer.StandardInput.Flush();
        }
        catch (Exception) { return false; }
        return true;
    }

    public bool MPlayerIsRunning()
    {
        return mplayer != null && !mplayer.HasExited;
    }
    public void WaitForExit()
    {
        mplayer.WaitForExit();
    }

    public string GetMPlayerInfo()
    {
        string strResp = mplayer.Responding ? "" : " - Not Responding";
        return string.Format("MPlayer's Process ID: {0}{1}", mplayer.Id, strResp);
    }

    private void SetPlayState(PlayStates newState, bool callPlayStateChanged)
    {
        if (Info.Current.PlayState == newState)
            return;

        Info.Current.PlayState = newState;
        if (callPlayStateChanged)
            OnPlayStateChanged(new EventArgs());
    }

    public bool Kill()
    {
        try
        {
            if (!MPlayerIsRunning())
                return true;

            mplayer.CancelOutputRead();
            mplayer.CancelErrorRead();
            mplayer.Kill();
        }
        catch (Exception) { return false; }
        return true;
    }
    private void Exited(object sender, EventArgs e)
    {
        Application.Exit();
    }

    #endregion

    #region API

    public bool Play()
    {
        if (Info.Current.PlayState == PlayStates.Ended)
            return OpenFile(Info.URL);
        return SendCommand("set pause no");
    }
    public bool Pause(bool toggle)
    {
        if (Info.Current.PlayState == PlayStates.Ended)
            return OpenFile(Info.URL);
        if (toggle && Info.Current.PlayState == PlayStates.Paused)
            return SendCommand("set pause no");
        return SendCommand("set pause yes");
    }
    public bool Stop()
    {
        ignoreStatus = true;
        SetPlayState(PlayStates.Stopped, true);
        return SendCommand("pausing seek 0 2 1");
    }
    public bool Restart()
    {
        return Rewind() && Play();
    }
    public bool Rewind()
    {
        return SendCommand("seek 0 2 1");
    }
    public bool Mute(bool mute)
    {
        return SendCommand("set mute {0}", mute ? "yes" : "no");
    }
    public bool SkipChapter(bool skipAhead)
    {
        return SendCommand("add chapter {0}", skipAhead ? 1 : -1);
    }
    public bool Seek(double sec)
    {
        ignoreStatus = true;
        return SendCommand("seek {0} 2 1", (int)sec);
    }
    public bool SeekFrame(double frame)
    {
        ignoreStatus = true;
        // seek <value> [type] [hr-seek]
        return SendCommand("seek {0} 2 1", frame / Info.VideoInfo.FPS);
    }

    public bool SetPlayRate(float ratio) // factor of 0.01-100
    {
        return ratio > 0 && SendCommand("set speed {0}", ratio); // set the play rate
    }
    public bool SetVolume(int volume)
    {
        return volume >= 0 && SendCommand("set volume {0}", volume);
    }
    /// <summary>
    /// Switches the audio track to the index specified
    /// </summary>
    /// <param name="i">The audio index (index starts from 1)</param>
    public bool SetAudioTrack(int i)
    {
        OnStatusChanged(new StatusChangedEventArgs(
            string.Format("Audio {0}: \"{1} ({2})\"",
            Info.AudioTracks[i-1].ID, Info.AudioTracks[i-1].Name, Info.AudioTracks[i-1].Lang), true));
        return SendCommand("set audio {0}", i);
    }
    /// <summary>
    /// Shows or hides subs
    /// </summary>
    public bool ShowSubs(bool show)
    {
        OnStatusChanged(new StatusChangedEventArgs("Subs " + (show ? "shown" : "hidden"), true));
        return SendCommand("set sub-visibility {0}", show ? "yes" : "no");
    }
    /// <summary>
    /// Set subtitle track
    /// </summary>
    /// <param name="i">The subtitle index (index start from 1)</param>
    public bool SetSubs(int i)
    {
        OnStatusChanged(new StatusChangedEventArgs(
            string.Format("Sub {0}: \"{1} ({2})\"",
            Info.Subs[i-1].TrackID, Info.Subs[i-1].Name, Info.Subs[i-1].Lang), true));
        return SendCommand("set sub {0}", i);
    }
    /// <summary>
    /// Sets chapter
    /// </summary>
    /// <param name="i">The chapter index (zero based)</param>
    public bool SetChapter(int i)
    {
        OnStatusChanged(new StatusChangedEventArgs(
            string.Format("Chapter {0}: \"{1}\"", i+1, Info.Chapters[i].ChapterName), true));
        return SendCommand("set chapter {0}", i);
    }
    /// <summary>
    /// Shows [text] on the OSD (on screen display)
    /// </summary>
    public bool ShowStatus(string text)
    {
        return SendCommand(string.Format("show_text {0} {1} {2}", text, "4000", '1'));
    }

    #endregion

    #region Standard Streams

    private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        Debug.WriteLine("-stderr:" + e.Data);

        if (ignoreStatus)
        {
            ignoreStatus = false;
            return;
        }

        if (e.Data.StartsWith("status:"))
        {
            ParseStatusMsg(e.Data.Substring(7)); // get rid of 'status:'
            return;
        }

        if (e.Data.StartsWith("Cache fill:"))
        {
            OnStatusChanged(new StatusChangedEventArgs(e.Data.Trim(), true));
            return;
        }
        if (e.Data.StartsWith("Cache is not responding - slow/stuck network connection?") ||
            e.Data.StartsWith("Cache keeps not responding."))
        {
            OnStatusChanged(new StatusChangedEventArgs("Your network is slow or stuck", true));
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
        // filter unwanted output (result of msglevel cplayer=6)
        if (e.Data.StartsWith("[cplayer] Sub:"))
            return;

        Debug.WriteLine(e.Data);

        // show output
        OnStdOut(new StdOutEventArgs(e.Data));

        // reached end of file
        if (e.Data.StartsWith("[cplayer] EOF code: 1") ||
            e.Data.StartsWith("[cplayer] EOF code: 2"))
        {
            Info.Current.PlayState = PlayStates.Ended;
            OnPlayStateChanged(new EventArgs());
            return;
        }

        if (e.Data.Equals("Clip info:"))
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
            Info.OtherInfo.Add(new ID_Info(key, value));
            return;
        }

        if (e.Data.StartsWith("Detected file format: "))
        {
            Info.FileFormat = e.Data.Substring(22);
            return;
        }
        if (e.Data.StartsWith("Failed to recognize file format."))
        {
            MessageBox.Show("Baka MPlayer couldn't open this file.\nThis can happen if the file is not supported, incomplete, or inaccessible.",
                            "Cannot open file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            OnStatusChanged(new StatusChangedEventArgs("Open a file to begin playing", true));
            return;
        }

        if (e.Data.StartsWith("Video: no video"))
        {
            Info.VideoInfo.HasVideo = false;
            return;
        }

        if (e.Data.StartsWith("PLAYING_FILE:"))
        {
            OnStatusChanged(new StatusChangedEventArgs("[Baka_MPlayer] HIDE_STATUS_LABEL", false));

            // sets appropriate movie name (e.g. internel file name or Youtube title)
            Info.MovieName = e.Data.Substring(13);

            // load external sub if requested
            if (!string.IsNullOrEmpty(ExternalSub))
            {
                SendCommand("sub_add \"{0}\"", ExternalSub.Replace("\\", "\\\\"));
                Info.Subs.Add(new Sub(Info.Subs.Count.ToString(CultureInfo.InvariantCulture), "[ External Sub ]", "★"));
                SetSubs(Info.Subs.Count);

                ExternalSub = string.Empty;
            }

            // get album picture tag
            if (!Info.IsOnline)
            {
                id3Tag.Read(Info.URL);
                Info.ID3Tags.AlbumArtTag = id3Tag.GetAlbumPictureTag();
                if (Info.ID3Tags.AlbumArtTag.AlbumArt != null)
                    Info.VideoInfo.HasVideo = false;
            }

            // tell mainform that new file was opened
            OnFileOpened(new EventArgs());
        }
    }

    private void ParseStatusMsg(string line)
    {
        //PAUSED=no;AV=123.456789;WIDTH=1920;HEIGHT=1080;
        string[] info = line.Split(';');

        foreach (var s in info)
        {
            var i = s.IndexOf('=');
            var value = s.Substring(i + 1, s.Length - i - 1);
            switch (s.Substring(0, i))
            {
                case "PAUSED":
                    if (value.Equals("yes"))
                        SetPlayState(PlayStates.Paused, true);
                    else
                        SetPlayState(PlayStates.Playing, true);
                    break;
                case "AV":
                    if (Info.Current.PlayState == PlayStates.Playing)
                        ProcessProgress(Functions.TryParse.ParseDouble(value));
                    break;
                case "WIDTH":
                    Info.VideoInfo.Width = Functions.TryParse.ParseInt(value);
                    break;
                case "HEIGHT":
                    Info.VideoInfo.Height = Functions.TryParse.ParseInt(value);
                    break;
            }
        }
    }
    private void ProcessProgress(double sec)
    {
        if (sec > 0.0)
        {
            Info.Current.Duration = sec;
            OnDurationChanged(new EventArgs());
        }
    }

    private static bool ProcessDetails(string key, string value)
    {
        switch (key)
        {
            case "ID_FILENAME":
                Info.URL = value;
                Info.IsOnline = !File.Exists(Info.URL);
                Info.FullFileName = Path.GetFileName(Functions.URL.DecodeURL(value));
                Info.FileName = Path.GetFileNameWithoutExtension(Info.FullFileName);
                Info.GetDirectoryName = Functions.IO.GetDirectoryName(value);
                break;
            case "ID_LENGTH":
                Info.Current.TotalLength = Functions.TryParse.ParseDouble(value);
                break;
            case "ID_VIDEO_FPS":
                Info.VideoInfo.FPS = Functions.TryParse.ParseDouble(value);
                break;
            case "ID_VIDEO_ASPECT":
                Info.VideoInfo.AspectRatio = Functions.TryParse.ParseDouble(value);
                break;
            default:
                if (key.StartsWith("ID_CHAPTER_ID")) // Chapters
                {
                    Info.Chapters.Add(new Chapter());
                }
                else if (key.StartsWith("ID_CHAPTER_")) // Chapters
                {
                    if (key.Contains("_START"))
                    {
                        Info.Chapters[Info.Chapters.Count - 1].StartTime = Functions.TryParse.ParseLong(value);
                    }
                    else if (key.Contains("_NAME"))
                    {
                        Info.Chapters[Info.Chapters.Count - 1].ChapterName = value;
                    }
                    return true;
                }
				
				else if (key.StartsWith("ID_SID_ID")) // Subtitles
                {
                    Info.Subs.Add(new Sub(value));
                }
                else if (key.StartsWith("ID_SID_")) // Subtitles
                {
                    if (key.Contains("_NAME"))
                    {
                        Info.Subs[Info.Subs.Count - 1].Name = value;
                    }
                    else if (key.Contains("_LANG"))
                    {
                        Info.Subs[Info.Subs.Count - 1].Lang = value;
                    }
                    return true;
                }
                
				else if (key.StartsWith("ID_AID_ID")) // Audio tracks
                {
                    Info.AudioTracks.Add(new AudioTrack(value));
                }
                else if (key.StartsWith("ID_AID_"))
                {
                    if (key.Contains("_NAME"))
                    {
                        Info.AudioTracks[Info.AudioTracks.Count - 1].Name = value;
                    }
                    else if (key.Contains("_LANG"))
                    {
                        Info.AudioTracks[Info.AudioTracks.Count - 1].Lang = value;
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

        var i = data.IndexOf(": ");

        if (i < 1)
        {
            if (data.StartsWith("ID_CLIP_INFO_N="))
                parsingClipInfo = false;
            return;
        }

        var s = data.Substring(i+2);
        data = data.ToLower();

        if (data.StartsWith("title:"))
            Info.ID3Tags.Title = s;
        else if (data.StartsWith("artist:"))
            Info.ID3Tags.Artist = s;
        else if (data.StartsWith("album:"))
            Info.ID3Tags.Album = s;
        else if (data.StartsWith("date:"))
            Info.ID3Tags.Date = s;
        else if (data.StartsWith("track:"))
            Info.ID3Tags.Track = s;
        else if (data.StartsWith("genre:"))
            Info.ID3Tags.Genre = s;
        else if (data.StartsWith("description:"))
            Info.ID3Tags.Description = s;
        else if (data.StartsWith("comment:"))
            Info.ID3Tags.Comment = s;
        else if (data.StartsWith("album_artist:"))
            Info.ID3Tags.Album_Artist = s;
        else if (data.StartsWith("encoder:"))
            Info.ID3Tags.Encoder = s;
        else if (data.StartsWith("disk:"))
            Info.ID3Tags.Disc = Functions.TryParse.ParseInt(s);
    }

    #endregion
}
