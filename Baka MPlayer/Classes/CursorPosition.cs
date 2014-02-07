using System.Drawing;
using System.Runtime.InteropServices;

public static class CursorPosition
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out Point lpPoint);

    /// <summary>
    /// Defines the x- and y-coordinates of a point
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public POINT(Point pt) : this(pt.X, pt.Y) { }

        public static implicit operator Point(POINT p)
        {
            return new Point(p.X, p.Y);
        }

        public static implicit operator POINT(Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }

    public static Point GetCursorPosition()
    {
        Point p;
        return GetCursorPos(out p) ? p : Point.Empty;
    }
}
