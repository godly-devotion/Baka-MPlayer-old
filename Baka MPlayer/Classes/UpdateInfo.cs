/*
 * UpdateInfo
 * 
 * Copyright (c) 2014, Joshua Park
 */

public class UpdateInfo
{
    public bool UpdateAvailable;
    public string LatestVer;
    public string Date;
    public string BugFixes;

    public UpdateInfo(bool updateAvailable, string latestVer, string date, string bugFixes)
    {
        this.UpdateAvailable = updateAvailable;
        this.LatestVer = latestVer;
        this.Date = date;
        this.BugFixes = bugFixes;
    }
}
