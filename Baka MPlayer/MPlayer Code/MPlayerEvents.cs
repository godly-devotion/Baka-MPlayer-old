/*************************************
* Custom events for MPlayer.cs class *
*************************************/
using System;

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