/*
 * IMPlayer interface
 * contains common playback functions
 * 
 * Copyright (c) 2014, Joshua Park
 */

using System;
using MPlayer.Info;
using MPlayer.Info.Track;

namespace MPlayer
{
    public interface IMPlayer //: IDisposable
    {
        Status CurrentStatus { get; }
        IFileInfo FileInfo { get; }
        int Volume { get; }

        bool OpenFile(string url);
        bool OpenFile(string url, string subFile);
        bool SendCommand(string command);
        bool SendCommand(string command, object value);
        bool PlayerIsRunning();
        string GetProcessState();
        bool Quit();

        bool Play();
        bool Pause(bool toggle);
        bool Stop();
        bool Restart();
        bool Rewind();

        bool Seek(double sec);
        bool SeekPercent(double percent);
        bool SetChapter(int index);
        bool SkipChapter(int jump);
        bool PreviousChapter();
        bool NextChapter();

        bool Mute(bool mute);
        bool SetVolume(int vol);

        bool SetAudioTrack(int trackId);
        bool SetVideoTrack(int trackId);
        bool AddSubtitle(string subPath);
        bool SetSubtitleTrack(int trackId);
        bool SetSubtitleVisibility(bool showSubs);

        event EventHandler<StdOutEventArgs> StdOutEvent;
        event EventHandler<StatusChangedEventArgs> StatusChangedEvent;
        event EventHandler<EventArgs> FileOpenedEvent;
        event EventHandler<PlayStateChangedEventArgs> PlayStateChangedEvent;
        event EventHandler<EventArgs> DurationChangedEvent;
        event EventHandler<EventArgs> MPlayerQuitEvent;
    }
}
