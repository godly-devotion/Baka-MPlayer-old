// http://stackoverflow.com/questions/2063974/how-do-i-capture-the-mouse-mouse-move-event-in-my-winform-application

using System.Drawing;
using System.Windows.Forms;
using Win32;

public delegate void MouseMovedEvent(Point cursorPos);
public delegate void XButtonDownEvent(MouseButtons button);

public class GlobalMouseHandler : IMessageFilter
{
    public event MouseMovedEvent MouseMoved;
    public event XButtonDownEvent XButtonDown;

    public bool PreFilterMessage(ref Message m)
    {
        switch (m.Msg)
        {
            case WM.MOUSEMOVE:
                if (MouseMoved != null)
                {
                    MouseMoved(CursorPosition.GetCursorPosition());
                }
                break;
            case WM.XBUTTONDOWN:
                if (XButtonDown != null)
                {
                    var button = WindowMacro.HighWord(m.WParam.ToInt32());

                    if (button == 0x0001)
                        XButtonDown(MouseButtons.XButton1);
                    if (button == 0x0002)
                        XButtonDown(MouseButtons.XButton2);
                }
                break;
        }

        // always allow message to continue to the next filter control
        return false;
    }
}