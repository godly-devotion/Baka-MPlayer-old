using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class VO_Panel : Panel
    {
        [Description("Occurs when the control is double clicked by the mouse.")]
        [Category("Action")]
        public event MouseEventHandler MouseDoubleClickFixed;
        
        private readonly Timer clickTimer = new Timer();

        public VO_Panel()
        {
            InitializeComponent();

            // setup timer
            clickTimer.Tick += clickTimer_Tick;
            clickTimer.Interval = SystemInformation.DoubleClickTime;
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

        private void VO_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (clickTimer.Enabled)
            {
                if (MouseDoubleClickFixed != null)
                    MouseDoubleClickFixed(sender, e);
            }
            else
            {
                clickTimer.Enabled = true;
            }
        }

        private void clickTimer_Tick(object sender, EventArgs e)
        {
            clickTimer.Stop();
        }
    }
}
