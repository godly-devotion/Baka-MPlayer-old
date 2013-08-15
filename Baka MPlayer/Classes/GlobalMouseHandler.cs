// http://stackoverflow.com/questions/2063974/how-do-i-capture-the-mouse-mouse-move-event-in-my-winform-application

using System.Drawing;
using System.Windows.Forms;

public delegate void MouseMovedEvent();

public class GlobalMouseHandler : IMessageFilter
{
    public event MouseMovedEvent TheMouseMoved;

    // Note: WM_MOUSEMOVE is sometimes triggered even if mouse wasn't moved
    // http://blogs.msdn.com/b/oldnewthing/archive/2003/10/01/55108.aspx
    private Point lastCursorPos = new Point(1, 1);

    public bool PreFilterMessage(ref Message m)
    {
        if (TheMouseMoved != null)
        {
            if (m.Msg == WM.MOUSEMOVE && lastCursorPos != Cursor.Position)
            {
                lastCursorPos = Cursor.Position;
                TheMouseMoved();
            }
        }

        // always allow message to continue to the next filter control
        return false;
    }
}