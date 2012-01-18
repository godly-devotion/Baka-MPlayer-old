using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Baka_MPlayer.Forms;

public class MPlayer
{
#region Variables
	private Process mplayer;
	private readonly MainForm mainForm;
	private bool parsingHeader;
    private bool parsingClipInfo = false;
	private bool parsingSubsTrack = false;
	private bool parsingAudioTrack = false;
	
	// For parsing mplayer output
	private readonly Regex HeaderParsing = new Regex(
		@"^([a-z_]+)=([a-z0-9_\.\,]+)",
		RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
#endregion
	
#region Constructor
	public MPlayer(MainForm mainForm)
	{
		this.mainForm = mainForm;
	}
#endregion

#region Functions
	public bool OpenFile(string url)
	{
		try
		{
			if (mplayer != null)
			{
				SendCommand(string.Format("loadfile \"{0}\"", url)); // open file
				mainForm.CallMediaOpened(); // tells mainform that new media has been opened
			}
			// mplayer is not running, so start mplayer then load url
			var cmdArgs = string.Format(" -vo {0} -ao {1}", "direct3d" /*directx, gl, gl2*/, "dsound" /*win32*/);
			cmdArgs += " -slave";                		 			// switch on slave mode for frontend
			cmdArgs += " -idle";                 		 			// wait insead of quit
			cmdArgs += " -utf8";                 		 			// handles the subtitle file as UTF-8
			cmdArgs += " -volstep 5";			  		 			// change volume step
			cmdArgs += " -msglevel identify=6:global=6"; 			// set msglevel
			cmdArgs += " -nomouseinput";         		 			// disable mouse input events
			cmdArgs += " -ass";                  		 			// enable .ass subtitle support
			cmdArgs += " -nokeepaspect";         		 			// doesn't keep window aspect ratio when resizing windows
			cmdArgs += string.Format(" -wid {0}", mainForm.mplayerPanel.Handle); // output handle

			mplayer = new Process
			{
				StartInfo =
				{
					FileName = "mplayer2.exe",
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					Arguments = cmdArgs + string.Format(" \"{0}\"", url)
				}
			};
			
			parsingHeader = true;
			mplayer.Start();
			
			mplayer.EnableRaisingEvents = true;

			mplayer.OutputDataReceived += OutputDataReceived;
			mplayer.ErrorDataReceived += ErrorDataReceived;
			mplayer.Exited += Exited;
			mplayer.BeginOutputReadLine();
			mplayer.BeginErrorReadLine();
			
			// tell mainform that new file was opened
			mainForm.CallMediaOpened();
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
		
			mplayer.StandardInput.WriteLine(command);
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
		
			mplayer.StandardInput.WriteLine(String.Format(command, value));
			mplayer.StandardInput.Flush();
		}
		catch (Exception) { return false; }
		return true;
	}
	public bool Close()
	{
		try
		{
            if (mplayer == null || mplayer.HasExited)
				throw new Exception();
			
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
		if (PlayingFile.Current.PlayState == PlayStates.Playing)
			return SendCommand("pause"); // pause command toggles between pause and play
		return false;
	}
	public bool Pause(bool toggle)
	{
		if (toggle || PlayingFile.Current.PlayState == PlayStates.Playing)
			return SendCommand("pause"); // pause command toggles between pause and play
		return false;
	}
	public bool Stop()
	{
		return SendCommand("pausing seek 0 2");
	}
	public bool Restart()
	{
		return Rewind() && Play();
	}
	public bool Rewind()
	{
		return SendCommand("seek 0 2");
	}
	public bool Mute(bool mute) // true = mute, false = unmute
	{
		return SendCommand("mute {0}", mute ? 1 : 0);
	}
	public bool SkipChapter(bool ahead) // true = skip ahead, false = skip back
	{
 		return SendCommand("seek_chapter {0} 0", ahead ? 1 : -1);
	}
	public bool Seek(int sec)
	{
		return SendCommand("seek {0} 2", sec); // set position to time specified
	}
	public bool SeekFrame(double frame)
	{
		// seek <value> [type] [hr-seek] <- force precise seek if possible
		return SendCommand("seek {0} 2 1", frame / PlayingFile.VideoInfo.FPS);
	}
	
	public bool SetPlayRate(float ratio) // 1 = 100%, normal speed. .5 = 50% speed, 2 = 200% double speed.
	{
		return ratio > 0 && SendCommand("speed_mult {0}", ratio); // set the play rate
	}
	public bool SetVolume(int volume)
	{
		return volume >= 0 && SendCommand("volume {0} 1", volume);
	}
	public bool ShowSubs(bool show)
	{
		return SendCommand("sub_visibility {0}", show ? 1 : 0); // sub_visibility [value]
	}
#endregion

#region Events
	private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
	{
		//Debug.WriteLine(e.Data);
	}

	private void OutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (parsingHeader)
		{
			if (e.Data.StartsWith("get_path('")) // ignore get_path(...) (result from msglevel global=6)
				return;

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

			if (parsingSubsTrack)
			{
				PlayingFile.MiscInfo.Subs[PlayingFile.MiscInfo.Subs.Count-1].TrackID = e.Data;
				parsingSubsTrack = false;
				return;
			}
			if (parsingAudioTrack)
			{
				PlayingFile.AudioInfo.AudioTracks[PlayingFile.AudioInfo.AudioTracks.Count-1].ID = e.Data;
				parsingAudioTrack = false;
				return;
			}
		
			Match match = HeaderParsing.Match(e.Data);
			if (match.Groups.Count == 3)
			{
				// Parsing "ID_*"
				if(ProcessDetails(match.Groups[1].Value, match.Groups[2].Value))
					return;
				// information that was not handled
				PlayingFile.MiscInfo.OtherInfo.Add(new Info(match.Groups[1].Value, match.Groups[2].Value));
			}
			
			if (e.Data.StartsWith("Video: no video"))
			{
				PlayingFile.VideoInfo.HasVideo = false;
				return;
			}
			
			if (e.Data.Equals("Starting playback..."))
			{
				parsingHeader = false;
				mainForm.CallMediaOpened();
				return;
			}
		}
		else
		{
            if (e.Data.StartsWith("A:"))
            {				
                ProcessProgress(e.Data);
                return;
            }
			ProcessOther(e.Data);
			return;
		}
	}
	
	private void ProcessProgress(string time)
	{
		if (PlayingFile.Current.PlayState == PlayStates.Paused)
		{
			PlayingFile.Current.PlayState = PlayStates.Playing;
			mainForm.CallPlayStateChanged();
		}
		
		int sec;
		int.TryParse(time.Substring(2, time.IndexOf('.')-2).Trim(), out sec);
		PlayingFile.Current.Duration = sec;
		
		mainForm.CallDurationChanged();
	}
	
	private bool ProcessDetails(string key, string value)
	{
		switch (key)
		{
			case "ID_FILENAME":
				PlayingFile.URL = value;
				PlayingFile.FileName = Path.GetFileName(value);
				break;
			case "ID_VIDEO_FPS":
				double fps;
				double.TryParse(value, out fps);
				PlayingFile.VideoInfo.FPS = fps;
				break;
		    case "ID_VIDEO_ASPECT":
                double ratio;
				double.TryParse(value, out ratio);
                PlayingFile.VideoInfo.AspectRatio = ratio;
		        break;
		    case "ID_LENGTH":
                int length;
                int.TryParse(value, out length);
                PlayingFile.Current.TotalLength = length;
		        break;
			default:
				if (key.StartsWith("ID_CHAPTER_")) // Chapters
				{
					if (key.Contains("_START"))
					{
						long frame;
						long.TryParse(value, out frame);
						PlayingFile.MiscInfo.Chapters.Add(new Chapter());
						PlayingFile.MiscInfo.Chapters[PlayingFile.MiscInfo.Chapters.Count-1].StartFrame = frame;
					}
					else if (key.Contains("_NAME"))
					{
						PlayingFile.MiscInfo.Chapters[PlayingFile.MiscInfo.Chapters.Count-1].ChapterName = value;
					}
					return true;
				}
				else if (key.StartsWith("ID_SID_")) // Subtitles
				{
					if (key.Contains("_NAME"))
					{
						PlayingFile.MiscInfo.Subs.Add(new Sub());
						PlayingFile.MiscInfo.Subs[PlayingFile.MiscInfo.Subs.Count-1].Name = value;
					}
					else if (key.Contains("_LANG"))
					{
						PlayingFile.MiscInfo.Subs[PlayingFile.MiscInfo.Subs.Count-1].Lang = value;
						parsingSubsTrack = true;
					}
					return true;
				}
				else if(key.Contains("ID_AID_")) // Audio tracks
				{
					if (key.Contains("_NAME"))
					{
						PlayingFile.AudioInfo.AudioTracks.Add(new AudioTrack());
						PlayingFile.AudioInfo.AudioTracks[PlayingFile.AudioInfo.AudioTracks.Count-1].Name = value;
					}
					else if (key.Contains("_LANG"))
					{
						PlayingFile.AudioInfo.AudioTracks[PlayingFile.AudioInfo.AudioTracks.Count-1].Lang = value;
						parsingAudioTrack = true;
					}
					return true;
				}
				return false;
		}
		return true;
	}
	
	private void ProcessOther(string output)
	{
		if (output.StartsWith("ID_PAUSED"))
		{
			PlayingFile.Current.PlayState = PlayStates.Paused;
			mainForm.CallPlayStateChanged();
		}
		else
		{
			// Other information while playing
		}
	}
	private void ParseClipInfo(string data)
	{
		if (data.StartsWith(" album_artist:"))
			PlayingFile.ID3Tags.Album_Artist = data.Substring(data.IndexOf(": ")+1);
		else if (data.StartsWith(" encoder:"))
			PlayingFile.ID3Tags.Encoder = data.Substring(data.IndexOf(": ")+1);
		else if (data.StartsWith(" artist:"))
			PlayingFile.ID3Tags.Artist = data.Substring(data.IndexOf(": ")+1);
		else if (data.StartsWith(" genre:"))
			PlayingFile.ID3Tags.Genre = data.Substring(data.IndexOf(": ")+1);
        else if (data.StartsWith(" track:"))
        {
            int track;
            int.TryParse(data.Substring(data.IndexOf(": ") + 1), out track);
            PlayingFile.ID3Tags.Track = track;
        }
        else if (data.StartsWith(" disk:"))
        {
            int disk;
            int.TryParse(data.Substring(data.IndexOf(": ") + 1), out disk);
            PlayingFile.ID3Tags.Disc = disk;
        }
        else if (data.StartsWith(" title:"))
            PlayingFile.ID3Tags.Title = data.Substring(data.IndexOf(": ") + 1);
        else if (data.StartsWith(" album:"))
            PlayingFile.ID3Tags.Album = data.Substring(data.IndexOf(": ") + 1);
        else if (data.StartsWith(" date:"))
            PlayingFile.ID3Tags.Date = data.Substring(data.IndexOf(": ") + 1);
        else if (data.StartsWith("ID_CLIP_INFO_N"))
            parsingClipInfo = false;
	}
	private void Exited(object sender, EventArgs e)
	{
		Application.Exit();
	}
#endregion
}
