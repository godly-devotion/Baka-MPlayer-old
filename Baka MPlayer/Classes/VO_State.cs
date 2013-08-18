using System.Drawing;

public static class VO_State
{
    // Note: WM_MOUSEMOVE is sometimes triggered even if mouse wasn't moved
    // http://blogs.msdn.com/b/oldnewthing/archive/2003/10/01/55108.aspx
    public static Point LastCursorPos = new Point(1, 1);

    /// <summary>
    /// Used as the global bool for full screen
    /// </summary>
    public static bool FullScreen { get; set; }
}