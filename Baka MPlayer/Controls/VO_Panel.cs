using System;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class VO_Panel : Panel
    {
        public VO_Panel()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!VO_State.FullScreen) return;
            switch (m.Msg)
            {
                case WM.SETCURSOR:
                    if (VO_State.LastCursorPos == Cursor.Position)
                    {
                        Cursor.Current = null;
                        m.Result = (IntPtr)1;
                    }
                    break;
            }
        }
    }
}
