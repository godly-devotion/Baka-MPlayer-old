/**************************
* mpv wrapper             *
* by Joshua Park & u8sand *
**************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Baka_MPlayer.Forms;

public class MPlayer
{
    #region Variables

    private Process mplayer;
    private readonly MainForm mainForm;
    private readonly ID3Tag id3Tag = new ID3Tag();
    private readonly string execName;
    private bool parsingClipInfo;
    private bool cachingFonts;
    private bool ignoreStatus;
    private bool killParent;

    #endregion

    #region Constructor

    public MPlayer(MainForm mainForm, string execName)
    {
        this.mainForm = mainForm;
        this.execName = execName;
    }

    #endregion

    #region Functions

    public bool OpenFile(string url)
    {
        try
        {
            Info.ResetInfo();
            mainForm.CallSetStatus("Loading file...", true);

            if (MPlayerIsRunning())
            {
                SendCommand("loadfile \"{0}\"", url.Replace("\\", "\\\\")); // open file
                mainForm.ClearOutput();
                return true;
            }
            // instructs fontconfig to show debug messages regarding font caching
            Environment.SetEnvironmentVariable("FC_DEBUG", "128");

            // mplayer is not running, so start mplayer then load url
            var args = new StringBuilder();
            args.AppendFormat("-vo={0} -ao={1}", "direct3d", "dsound");
            args.Append(" -slave-broken");         		 		// switch on slave mode for frontend
            args.Append(" -idle");                 		        // wait insead of quit
            args.Append(" -volstep=5");			  		 		// change volume step
            args.Append(" -msglevel identify=6:global=6");      // set msglevel
            args.Append(" -no-mouseinput");         		 	// disable mouse input events
            args.Append(" -ass");                  		 		// enable .ass subtitle support
            args.Append(" -no-keepaspect");         		 	// doesn't keep window aspect ratio when resizing windows
            args.Append(" -framedrop=yes");                     // enables soft framedrop
            //args.Append(" -no-cache");                        // disables caching
            args.Append(" -status-msg=status:PAUSED=${=pause};AV=${=time-pos};WIDTH=${=width};HEIGHT=${=height}");
            args.AppendFormat(" -volume={0}", Info.Current.Volume); // retrieves last volume
            args.AppendFormat(" -wid={0}", mainForm.mplayerPanel.Handle); // output handle
            
            mplayer = new Process
            {
                StartInfo =
                {
                    FileName = execName,
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
    /// Opens file with optional args
    /// </summary>
    public bool OpenFile(string url, string optionalArgs)
    {
        try
        {
            Info.ResetInfo();
            mainForm.CallSetStatus("Loading file...", true);

            if (MPlayerIsRunning())
            {
                SendCommand("loadfile \"{0}\"", url.Replace("\\", "\\\\")); // open file
                mainForm.ClearOutput();
                return true;
            }
            // instructs fontconfig to show debug messages regarding font caching
            Environment.SetEnvironmentVariable("FC_DEBUG", "128");

            // mplayer is not running, so start mplayer then load url
            var args = new StringBuilder();
            args.AppendFormat("-vo={0} -ao={1}", "direct3d", "dsound");
            args.Append(" -slave-broken");         		 		// switch on slave mode for frontend
            args.Append(" -idle");                 		        // wait insead of quit
            args.Append(" -volstep=5");			  		 		// change volume step
            args.Append(" -msglevel identify=6:global=6");      // set msglevel
            args.Append(" -no-mouseinput");         		 	// disable mouse input events
            args.Append(" -ass");                  		 		// enable .ass subtitle support
            args.Append(" -no-keepaspect");         		 	// doesn't keep window aspect ratio when resizing windows
            args.Append(" -framedrop=yes");                     // enables soft framedrop
            //args.Append(" -no-cache");                        // disables caching
            args.Append(" -status-msg=status:PAUSED=${=pause};AV=${=time-pos};WIDTH=${=width};HEIGHT=${=height}");
            args.AppendFormat(" -volume={0}", Info.Current.Volume); // retrieves last volume
            args.AppendFormat(" -wid={0}", mainForm.mplayerPanel.Handle); // output handle

            if (!string.IsNullOrEmpty(optionalArgs))
                args.Append(optionalArgs);

            mplayer = new Process
            {
                StartInfo =
                {
                    FileName = execName,
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
            if (mplayer == null || mplayer.HasExited)
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
            if (mplayer == null || mplayer.HasExited)
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
        return mplayer != null;
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
            mainForm.CallPlayStateChanged();
    }

    public bool Close(bool alsoKillParent)
    {
        try
        {
            if (mplayer == null || mplayer.HasExited)
                return true;

            killParent = alsoKillParent;
            mplayer.CancelOutputRead();
            mplayer.CancelErrorRead();
            mplayer.Kill();
        }
        catch (Exception) { return false; }
        return true;
    }

    #endregion

    #region API

    public bool Play()
    {
        return SendCommand("set pause no");
    }
    public bool Pause(bool toggle)
    {
        if (toggle && Info.Current.PlayState == PlayStates.Paused)
            return SendCommand("set pause no");
        return SendCommand("set pause yes");
    }
    public bool Stop()
    {
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
    public bool Mute(bool mute) // 1 = mute, 0 = unmute
    {
        return SendCommand("set mute {0}", mute ? "yes" : "no");
    }
    public bool SkipChapter(bool ahead) // true = skip ahead, false = skip back
    {
        return SendCommand("seek_chapter {0} 0", ahead ? 1 : -1);
    }
    public bool Seek(double sec)
    {
        ignoreStatus = true;
        return SendCommand("seek {0} 2 0", (int)sec);
    }
    public bool SeekFrame(double frame)
    {
        ignoreStatus = true;
        // seek <value> [type] [hr-seek] <- force precise seek if possible
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
    /// <param name="index">Based on zero index</param>
    public bool SetAudioTrack(int index)
    {
        return SendCommand("set audio {0}", index);
    }
    /// <summary>
    /// Shows or hides subs.
    /// </summary>
    public bool ShowSubs(bool show)
    {
        return SendCommand("set sub-visibility {0}", show ? "yes" : "no");
    }
    /// <summary>
    /// Set subtitle track.
    /// </summary>
    public bool SetSubs(int index)
    {
        return SendCommand("set sub {0}", index);
    }
    /// <summary>
    /// Sets chapter
    /// </summary>
    public bool SetChapter(int index)
    {
        return SendCommand("set chapter {0}", index);
    }
    /// <summary>
    /// Shows [text] on the OSD (on screen display)
    /// </summary>
    public bool ShowStatus(string text)
    {
        return SendCommand(string.Format("show_text {0} {1} {2}", text, "4000", '1'));
    }

    #endregion

    #region Events

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

        //[fontconfig] Scanning dir C:/Windows/Fonts (must set FC_DEBUG to show)
        if (!cachingFonts && e.Data.StartsWith("[fontconfig]"))
		{
            cachingFonts = true;
            mainForm.CallSetStatus("Caching fonts...", true);
		}
		else if (cachingFonts && e.Data.StartsWith("["))
		{
            if (e.Data.StartsWith("[fontconfig]"))
            {
                cachingFonts = false;
                mainForm.CallSetStatus("Fonts finished caching", false);
            }
            else
                mainForm.CallSetStatus("Caching fonts: " + e.Data, true);
		}
    }

    private void OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        Debug.WriteLine(e.Data);

        if (e.Data.StartsWith("get_path(")) // ignore get_path(...) (result from msglevel global=6)
            return;

        // show output
        mainForm.SetOutput(e.Data);
        
        if (e.Data.StartsWith("EOF code: 1")) // reached end of file
        {
            Info.Current.PlayState = PlayStates.Ended;
            mainForm.CallMediaEnded();
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

        if (e.Data.StartsWith("Video: no video"))
            Info.VideoInfo.HasVideo = false;

        if ((e.Data.StartsWith("AO: [dsound]") && !Info.VideoInfo.HasVideo) || e.Data.StartsWith("VO: [direct3d]"))
        {
            mainForm.CallHideStatusLabel();

            // get album picture tag
            id3Tag.Read(Info.URL);
            Info.ID3Tags.AlbumArtTag = id3Tag.GetAlbumPictureTag();
            if (Info.ID3Tags.AlbumArtTag.AlbumArt != null)
                Info.VideoInfo.HasVideo = false;

            // tell mainform that new file was opened
            mainForm.CallMediaOpened();
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
            mainForm.CallDurationChanged();
        }
    }

    private static bool ProcessDetails(string key, string value)
    {
        switch (key)
        {
            case "ID_FILENAME":
                Info.URL = value;
                Info.FullFileName = Path.GetFileName(value);
                Info.GetDirectoryName = Functions.IO.GetDirectoryName(value);
                Info.FileExists = File.Exists(value);
                break;
            case "ID_VIDEO_WIDTH":
                Info.VideoInfo.Width = Functions.TryParse.ParseInt(value);
                break;
            case "ID_VIDEO_HEIGHT":
                Info.VideoInfo.Height = Functions.TryParse.ParseInt(value);
                break;
            case "ID_VIDEO_FPS":
                Info.VideoInfo.FPS = Functions.TryParse.ParseDouble(value);
                break;
            case "ID_VIDEO_ASPECT":
                Info.VideoInfo.AspectRatio = Functions.TryParse.ParseDouble(value);
                break;
            case "ID_LENGTH":
                Info.Current.TotalLength = Functions.TryParse.ParseDouble(value);
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
                        Info.Chapters[Info.Chapters.Count - 1].StartFrame = Functions.TryParse.ParseLong(value);
                    }
                    else if (key.Contains("_NAME"))
                    {
                        Info.Chapters[Info.Chapters.Count - 1].ChapterName = value;
                    }
                    return true;
                }
				
				else if (key.StartsWith("ID_SUBTITLE_ID")) // Subtitles
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
                
				else if (key.StartsWith("ID_AUDIO_ID")) // Audio tracks
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
    private void Exited(object sender, EventArgs e)
    {
        if (killParent)
            Application.Exit();
        killParent = false;
    }

    #endregion
}
