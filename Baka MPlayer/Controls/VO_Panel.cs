using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Win32;

namespace Baka_MPlayer.Controls
{
    public partial class VO_Panel : Panel
    {
        [Description("Occurs when the control is double clicked by the mouse.")]
        [Category("Action")]
        public event MouseEventHandler MouseDoubleClickFixed;
        
        private readonly Timer _clickTimer = new Timer();
        private Point _startingClickPosition;

        public VO_Panel()
        {
            InitializeComponent();

            // setup timer
            _clickTimer.Tick += clickTimer_Tick;
            _clickTimer.Interval = SystemInformation.DoubleClickTime;
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
            if (_clickTimer.Enabled)
            {
                if (MouseDoubleClickFixed != null && _startingClickPosition == Cursor.Position)
                    MouseDoubleClickFixed(sender, e);
            }
            else
            {
                _startingClickPosition = Cursor.Position;
                _clickTimer.Enabled = true;
            }
        }

        private void clickTimer_Tick(object sender, EventArgs e)
        {
            _clickTimer.Stop();
        }
    }
}
