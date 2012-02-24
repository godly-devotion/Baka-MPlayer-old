// from http://www.joejoe.org/forum/topic/9807-vista-textbox-c%23/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    [Description("CustomTextBox")]
    [ToolboxBitmap(typeof(TextBox))]
    public partial class CustomTextBox : TextBox
    {
        private const int ECM_FIRST = 0x1500;
        private const int EM_SETCUEBANNER = ECM_FIRST + 1;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        private string _CueText = "";

        [Category("Appearance")]
        [Description("The cue text associated with the control.")]
        public string CueText
        {
            get { return _CueText; }
            set { _CueText = value; SetCueText(); }
        }

        private void SetCueText()
        {
            if (Environment.OSVersion.Version.Major > 5)
                SendMessage(this.Handle, EM_SETCUEBANNER, IntPtr.Zero, _CueText);
        }
    }
}