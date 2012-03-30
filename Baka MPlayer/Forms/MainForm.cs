using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Baka_MPlayer.Forms
{
    public partial class MainForm : Form
    {
        #region DLL Import

        // used to get the virtual keys
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        // global key hook
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private readonly HookProc myCallbackDelegate;
        static IntPtr hHook;

        [DllImport("user32.dll")]
        protected static extern IntPtr SetWindowsHookEx(int code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        #endregion
        #region Global Variables

        private BlackForm blackForm;
        private InfoForm infoForm;
        // class instances
        private MPlayer mplayer;
        private Voice voice;
        private readonly Settings settings = new Settings();

        // Win 7 thumbnail toolbar buttons
        private ThumbnailToolBarButton previousToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_previous, "Previous file");
        private ThumbnailToolBarButton playToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_play, "Play");
        private ThumbnailToolBarButton nextToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_next, "Next file");

        // lastFile
        private bool firstFile = true;
        private string tempURL = string.Empty;

        private bool seekBar_IsMouseDown;

        #endregion

        #region Tray Icon

        // Right click context menu
        private void showMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
                ToggleToTaskbar(false);
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Focus();
            }
        }

        private void playMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.Pause(true);
        }

        private void stopMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.Stop();
        }

        private void rewindMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.Rewind();
        }

        private void nextMenuItem_Click(object sender, EventArgs e)
        {
            playlist.PlayNextFile();
        }

        private void previousMenuItem_Click(object sender, EventArgs e)
        {
            playlist.PlayPreviousFile();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || string.IsNullOrEmpty(Info.URL))
                return;

            mplayer.Pause(true);

            switch (Info.Current.PlayState)
            {
                case PlayStates.Playing:
                    trayIcon.ShowBalloonTip(4000, "Paused", this.Text, ToolTipIcon.None);
                    break;
                case PlayStates.Paused:
                    trayIcon.ShowBalloonTip(4000, "Playing", this.Text, ToolTipIcon.None);
                    break;
                case PlayStates.Stopped:
                    trayIcon.ShowBalloonTip(4000, "Playing", this.Text, ToolTipIcon.None);
                    break;
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Normal;
                this.Focus();
            }
            else
            {
                ToggleToTaskbar(false);
            }
        }

        // notification tray code
        private void SetSystemTray()
        {
            var lastPart = string.Format("(File {0} of {1})", playlist.GetPlaylistIndex + 1, playlist.GetTotalItems);

            if (!string.IsNullOrEmpty(Info.ID3Tags.Title))
            {
                bool hasArtist = !string.IsNullOrEmpty(Info.ID3Tags.Artist);

                titleMenuItem.Text = string.Format("  {0}", Functions.AutoEllipsis(25, Info.ID3Tags.Title));
                artistMenuItem.Text = string.Format("  {0}",
                    hasArtist ? Functions.AutoEllipsis(25, Info.ID3Tags.Artist) : "Unknown Artist");

                SetNotifyIconText(string.Format("{0}\n{1}{2}", Info.ID3Tags.Title, hasArtist ? Info.ID3Tags.Artist + '\n' : "", lastPart));

                if (!Info.VideoInfo.HasVideo && !hidePopupToolStripMenuItem.Checked)
                    trayIcon.ShowBalloonTip(4000, Info.ID3Tags.Title, (hasArtist ? Info.ID3Tags.Artist + '\n' : "") + lastPart, ToolTipIcon.None);

            }
            else
            {
                // no title & artist (no artist assumed)
                var fullFileName = Path.GetFileNameWithoutExtension(Info.URL);

                titleMenuItem.Text = string.Format("  {0}", Functions.AutoEllipsis(25, fullFileName));
                artistMenuItem.Text = "  Unknown Artist";

                SetNotifyIconText(Functions.AutoEllipsis(25, fullFileName) + '\n' + lastPart);

                if (!Info.VideoInfo.HasVideo && !hidePopupToolStripMenuItem.Checked)
                    trayIcon.ShowBalloonTip(4000, fullFileName, lastPart, ToolTipIcon.None);
            }
        }

        private void SetNotifyIconText(string text)
        {
            if (text.Length > 127)
            {
                //throw new ArgumentOutOfRangeException("Text limited to 127 characters");
                text = text.Substring(0, 127);
            }
            var t = typeof(NotifyIcon);
            const BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
            t.GetField("text", hidden).SetValue(trayIcon, text);
            if ((bool)t.GetField("added", hidden).GetValue(trayIcon))
                t.GetMethod("UpdateIcon", hidden).Invoke(trayIcon, new object[] { true });
        }

        private void ToggleToTaskbar(bool minimize)
        {
            this.WindowState = minimize ?
                FormWindowState.Minimized : FormWindowState.Normal;
            this.Visible = !minimize;
        }

        private void UnloadTray()
        {
            // release mainNotifyIcon's resources
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        #endregion
        #region Set Status

        public void CallSetStatus(string status, bool noAutoHide)
        {
            Invoke((MethodInvoker)(() => SetStatus(status, noAutoHide)));
        }
        private void SetStatus(string status, bool noAutoHide)
        {
            statusLabel.Text = status;
            statusLabel.Show();
            if (!noAutoHide)
                statusTimer.Enabled = true;
        }
        public void CallHideStatusLabel()
        {
            Invoke((MethodInvoker)HideStatusLabel);
        }
        private void HideStatusLabel()
        {
            statusLabel.Hide();
            statusTimer.Enabled = false;
        }

        private void statusLabel_MouseClick(object sender, MouseEventArgs e)
        {
            HideStatusLabel();
        }
        private void statusTimer_Tick(object sender, EventArgs e)
        {
            HideStatusLabel();
        }

        #endregion
        #region CLI Code

        private void inputTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            var input = inputTextbox.Text.Trim().ToLower();
            if (input.Length > 0)
            {
                switch (input)
                {
                    case "clear":
                        ClearOutput();
                        break;
                    case "exit":
                        Application.Exit();
                        break;
                    default:
                        mplayer.SendCommand(inputTextbox.Text);
                        break;
                }
                inputTextbox.SelectionStart = 0;
                inputTextbox.SelectionLength = inputTextbox.TextLength;
            }
        }

        public void SetOutput(string output)
        {
            Invoke((MethodInvoker)delegate
            {
                outputTextbox.AppendText("\n" + output);
                // auto scroll to end
                outputTextbox.SelectionStart = outputTextbox.TextLength;
                outputTextbox.ScrollToCaret();
            });
        }

        public void ClearOutput()
        {
            Invoke((MethodInvoker)delegate
            {
                outputTextbox.Text = string.Empty;
            });
        }

        #endregion
        #region Snap-to-Border + Minimize to Tray

        private const int SnapOffset = 10; // pixels
        private const int WM_WINDOWPOSCHANGING = 0x46;
        // minimize to notification area constants
        private const Int32 WM_SYSCOMMAND = 0x112;
        private const Int32 SC_MINIMIZE = 0xf020;
        //const Int32 SC_RESTORE = 0xf120;

        // snap to border constants
        public struct WINDOWPOS
        {
            public IntPtr hwnd; public IntPtr hwndInsertAfter;
            public int x; public int y; public int cx; public int cy; public SWP flags;
        }

        [Flags]
        public enum SWP
        {
            Normal = 0,
            NoSize = 0x1,
            NoMove = 0x2,
            NoZOrder = 0x4,
            NoRedraw = 0x8,
            NoActivate = 0x10,
            FrameChanged = 0x20,
            ShowWindow = 0x40,
            HideWindow = 0x80,
            NoCopyBits = 0x100,
            NoOwnerZOrder = 0x200,
            NoSendChanging = 0x400,
            DrawFrame = FrameChanged,
            NoReposition = NoOwnerZOrder,
            DeferErase = 0x2000,
            AsyncWindowPos = 0x4000
        }

        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages
            switch (m.Msg)
            {
                case WM_WINDOWPOSCHANGING:
                    // Snap to desktop border
                    SnapToDesktopBorder(m.LParam);
                    break;
                case WM_SYSCOMMAND:
                    if (m.WParam.ToInt32() == SC_MINIMIZE)
                    {
                        // mainForm minimize
                        if (blackForm != null)
                            blackForm.Hide();
                        dimLightsToolStripMenuItem.Checked = false;
                        if (minimizeToTrayToolStripMenuItem.Enabled && minimizeToTrayToolStripMenuItem.Checked)
                            ToggleToTaskbar(true);
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void SnapToDesktopBorder(IntPtr LParam)
        {
            // Snap client to the top, left, bottom or right desktop border
            // as the form is moved near that border.

            // Marshal the LPARAM value which is a WINDOWPOS struct
            var WPNewPosition = (WINDOWPOS)Marshal.PtrToStructure(LParam, typeof(WINDOWPOS));

            if ((WPNewPosition.flags & SWP.NoSize) == 0 || (WPNewPosition.flags & SWP.NoMove) == 0)
            {
                var RWorking = Screen.FromControl(this).WorkingArea;
                var Changed = false;

                if (Math.Abs(WPNewPosition.x - RWorking.X) <= SnapOffset)
                {
                    WPNewPosition.x = RWorking.X;
                    Changed = true;
                }
                else if (Math.Abs(WPNewPosition.x + WPNewPosition.cx - RWorking.Right) <= SnapOffset)
                {
                    WPNewPosition.x = RWorking.Right - WPNewPosition.cx;
                    Changed = true;
                }

                if (Math.Abs(WPNewPosition.y - RWorking.Y) <= SnapOffset)
                {
                    WPNewPosition.y = RWorking.Y;
                    Changed = true;
                }
                else if (Math.Abs(WPNewPosition.y + WPNewPosition.cy - RWorking.Bottom) <= SnapOffset)
                {
                    WPNewPosition.y = RWorking.Bottom - WPNewPosition.cy;
                    Changed = true;
                }

                // Marshal it back
                if (Changed) Marshal.StructureToPtr(WPNewPosition, LParam, true);
            }
        }

        #endregion
        #region Embbed Font

        private static System.Drawing.Text.PrivateFontCollection fonts;
        private static FontFamily NewFont_FF;

        private static Font CreateFont(string name, FontStyle style, float size, GraphicsUnit unit)
        {
            // create a new font collection
            fonts = new System.Drawing.Text.PrivateFontCollection();
            // add the font file to the new font
            // "name" is the qualified path to your font file
            fonts.AddFontFile(name);
            // retrieve your new font
            NewFont_FF = fonts.Families[0];

            return new Font(NewFont_FF, size, style, unit);
        }

        private void SetLCDFont()
        {
            var fontFile = Application.StartupPath + @"\LCD.ttf";

            if (!File.Exists(fontFile))
                File.WriteAllBytes(fontFile, Properties.Resources.LCD);

            var fontLCD = CreateFont(fontFile, FontStyle.Bold, 11.25f, GraphicsUnit.Point);

            // set fonts
            durationLabel.Font = fontLCD;
            timeLeftLabel.Font = fontLCD;
        }

        #endregion
        #region Draggable Form

        private void DraggableWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !FullScreen)
            {
                var control = (Control)sender;
                control.Capture = false;
                // Create and send a WM_NCLBUTTONDOWN message.
                const int WM_NCLBUTTONDOWN = 0xa1;
                const int HTCAPTION = 2;
                var msg = Message.Create(this.Handle, WM_NCLBUTTONDOWN, new IntPtr(HTCAPTION), IntPtr.Zero);
                this.DefWndProc(ref msg);
            }
        }

        #endregion
        #region Keyboard Hook

        private int callbackFunction_KeyboardHook(int code, IntPtr wParam, IntPtr lParam)
        {
            if (!(code.Equals(3) && Convert.ToString(lParam.ToInt64(), 2).StartsWith("10"))
                || playlist.searchTextBox.Focused || string.IsNullOrEmpty(Info.URL) || code < 0)
            {
                // you need to call CallNextHookEx without further processing
                // and return the value returned by CallNextHookEx
                return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            }

            // Note: the values are shown in hex, must convert to int
            // http://msdn.microsoft.com/en-us/library/dd375731(v=vs.85).aspx
            switch (wParam.ToInt32())
            {
                case 37:
                    // left arrow key
                    if (Info.Current.Duration - 5 > -1)
                    {
                        mplayer.Seek(Info.Current.Duration - 5);
                        mplayer.ignoreUpdate = true;
                    }
                    else //if (mplayer.currentPosition < 5)
                        mplayer.Seek(0);
                    break;
                case 39:
                    // right arrow key
                    if (Info.Current.Duration + 5 < Info.Current.TotalLength)
                    {
                        mplayer.Seek(Info.Current.Duration + 5);
                        mplayer.ignoreUpdate = true;
                    }
                    else
                        playlist.PlayNextFile();
                    break;
                case 161:
                    // RShiftKey - VoiceOver
                    Speech.SayMedia();
                    break;
                case 176:
                    // media next button
                    playlist.PlayNextFile();
                    break;
                case 177:
                    // media previous button
                    playlist.PlayPreviousFile();
                    break;
                case 178:
                    // media stop button
                    mplayer.Stop();
                    break;
                case 179:
                    // media play pause button
                    switch (Info.Current.PlayState)
                    {
                        case PlayStates.Playing:
                            mplayer.Pause(false);
                            break;
                        case PlayStates.Paused:
                        case PlayStates.Stopped:
                            mplayer.Pause(true);
                            break;
                    }
                    return -1;
            }

            // return the value returned by CallNextHookEx
            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        #endregion
        #region Gradient Paint
        private void controlPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            var gradientBrush = new LinearGradientBrush(controlPanel.ClientRectangle,
                Color.FromArgb(255, 30, 30, 30), Color.Black, LinearGradientMode.Vertical);
            formGraphics.FillRectangle(gradientBrush, controlPanel.ClientRectangle);
        }
        #endregion
        #region Drag & Drop Support

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var fileDirs = (string[])e.Data.GetData(DataFormats.FileDrop);
            string fileDir = fileDirs.GetValue(0).ToString();

            if (File.Exists(fileDir))
                mplayer.OpenFile(fileDir);
            else
            {
                MessageBox.Show(string.Format("Error: \"{0}\" does not exist.", Path.GetFileName(fileDir)),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion
        #region Win 7 Thumbnail Toolbars

        private void setThumbnailToolbars()
        {
            playToolButton = new ThumbnailToolBarButton(Properties.Resources.tool_play, "Play") { Enabled = false };
            playToolButton.Click += playToolButton_Click;

            nextToolButton = new ThumbnailToolBarButton(Properties.Resources.tool_next, "Next file") { Enabled = false };
            nextToolButton.Click += nextToolButton_Click;

            previousToolButton = new ThumbnailToolBarButton(Properties.Resources.tool_previous, "Previous file") { Enabled = false };
            previousToolButton.Click += previousToolButton_Click;
            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(this.Handle, previousToolButton, playToolButton, nextToolButton);
        }

        private void playToolButton_Click(Object sender, EventArgs e)
        {
            mplayer.Pause(true);
        }

        private void nextToolButton_Click(Object sender, EventArgs e)
        {
            playlist.PlayNextFile();
        }

        private void previousToolButton_Click(Object sender, EventArgs e)
        {
            playlist.PlayPreviousFile();
        }

        #endregion

        #region Accessors

        public void SetShuffleCheckState(bool check)
        {
            Invoke((MethodInvoker)(() => shuffleToolStripMenuItem.Checked = check));
        }

        public void SetPlaylistButtonEnable(bool enable)
        {
            Invoke((MethodInvoker)(() => playlistButton.Enabled = enable));
        }

        #endregion
        #region Property's

        private bool _voiceEnabled;
        private bool VoiceEnabled
        {
            get { return _voiceEnabled; }
            set
            {
                if (value)
                {
                    if (voice == null)
                        voice = new Voice(this, "baka");
                    voice.StartListening();
                    _voiceEnabled = true;
                }
                else
                {
                    voice.StopListening();
                    _voiceEnabled = false;
                }
            }
        }

        public bool ShowPlaylist
        {
            get { return !mplayerSplitContainer.Panel2Collapsed; }
            set
            {
                if (value)
                {
                    mplayerSplitContainer.Panel2Collapsed = false;
                    showPlaylistToolStripMenuItem.Checked = true;

                    playlist.DisableInteraction = false;
                    mplayerSplitContainer.IsSplitterFixed = false;
                }
                else
                {
                    mplayerSplitContainer.Panel2Collapsed = true;
                    showPlaylistToolStripMenuItem.Checked = false;
                    hideAlbumArtToolStripMenuItem.Checked = false;

                    playlist.DisableInteraction = true;
                    mplayerSplitContainer.IsSplitterFixed = true;
                }
            }
        }

        private bool HideAlbumArt
        {
            get { return mplayerSplitContainer.Panel1Collapsed; }
            set { mplayerSplitContainer.Panel1Collapsed = value; }
        }

        private bool ShowConsole
        {
            get { return !bodySplitContainer.Panel2Collapsed; }
            set { bodySplitContainer.Panel2Collapsed = !value; }
        }

        #endregion
        #region Speech Code

        public void CallStateChanged(VoiceState e)
        {
            //Invoke((MethodInvoker)(() => stateChanged(e)));
        }
        private void stateChanged(VoiceState e)
        {
            switch (e)
            {
                case VoiceState.SpeechDetected:
                    break;
                case VoiceState.SpeechRecognized:
                    break;
                case VoiceState.SpeechRejected:
                    break;
                case VoiceState.SpeechCompleted:
                    break;
            }
        }

        public void CallTakeAction(string speechCommand)
        {
            Invoke((MethodInvoker)(() => takeAction(speechCommand)));
        }
        private void takeAction(string speechCommand)
        {
            SetStatus("Voice Command: " + Functions.ToTitleCase(speechCommand), false);

            switch (speechCommand)
            {
                case "open":
                    OpenFile();
                    break;
                case "play":
                    mplayer.Play();
                    break;
                case "pause":
                    mplayer.Pause(false);
                    break;
                case "stop":
                    mplayer.Stop();
                    break;
                case "mute":
                    mplayer.Mute(true);
                    break;
                case "unmute":
                    mplayer.Mute(false);
                    break;
                case "next":
                case "next file":
                    playlist.PlayNextFile();
                    break;
                case "previous":
                case "previous file":
                    playlist.PlayPreviousFile();
                    break;
                case "hide":
                    HidePlayer();
                    break;
                case "whats playing":
                    Speech.SayMedia();
                    break;
            }
        }

        #endregion
        #region SeekPanel Code

        private void seekBar_MouseDown(object sender, MouseEventArgs e)
        {
            seekBar_IsMouseDown = true;
        }

        private void seekBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!seekBar_IsMouseDown)
                return;

            var currentPos = seekBar.Value * Info.Current.TotalLength / seekBar.Maximum;

            if (settings.GetBoolValue("ShowTimeRemaining"))
                timeLeftLabel.Text = string.Format("-{0}", Functions.ConvertTimeFromSeconds(Info.Current.TotalLength - currentPos));
            else
                timeLeftLabel.Text = Functions.ConvertTimeFromSeconds(Info.Current.TotalLength);

            durationLabel.Text = Functions.ConvertTimeFromSeconds(currentPos);
        }

        private void seekBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (seekBar_IsMouseDown)
            {
                var newTime = (seekBar.Value * Info.Current.TotalLength) / seekBar.Maximum;
                mplayer.Seek(newTime);
                mplayer.ignoreUpdate = true;
                seekBar_IsMouseDown = false;
            }
        }

        private void timeLeftLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(Info.URL))
                return;

            var setting = !settings.GetBoolValue("ShowTimeRemaining");
            settings.SetConfig(setting, "ShowTimeRemaining");
            settings.SaveConfig();
        }

        #endregion
        #region Control Buttons

        // QuickButton
        private void quickButton_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    OpenFile();
                    break;
                case MouseButtons.Middle:
                    if (!string.IsNullOrEmpty(Info.URL))
                    {
                        var jumpForm = new JumpForm();
                        if (jumpForm.ShowDialog(this) == DialogResult.OK)
                        {
                            mplayer.Seek(jumpForm.GetNewTime);
                            jumpForm.Dispose();
                        }
                    }
                    break;
                case MouseButtons.Right:
                    var webForm = new WebForm();
                    if (webForm.ShowDialog(this) == DialogResult.OK)
                    {
                        mplayer.OpenFile(webForm.URL);
                        webForm.Dispose();
                    }
                    break;
            }
        }

        private void quickButton_MouseDown(object sender, MouseEventArgs e)
        {
            quickButton.Image = Properties.Resources.down_open;
        }

        private void quickButton_MouseUp(object sender, MouseEventArgs e)
        {
            quickButton.Image = Properties.Resources.default_open;
        }

        // Speech Button
        private void speechButton_MouseClick(object sender, MouseEventArgs e)
        {
            VoiceEnabled = !VoiceEnabled;
        }

        private void speechButton_MouseDown(object sender, MouseEventArgs e)
        {
            speechButton.Image = Properties.Resources.down_mic;
        }

        private void speechButton_MouseUp(object sender, MouseEventArgs e)
        {
            speechButton.Image = VoiceEnabled ?
                Properties.Resources.enabled_mic : Properties.Resources.disabled_mic;
        }

        // RewindButton
        private void rewindButton_EnabledChanged(object sender, EventArgs e)
        {
            rewindButton.Image = rewindButton.Enabled ?
                Properties.Resources.default_reverse : Properties.Resources.disabled_reverse;
        }

        private void rewindButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (Info.Current.Duration < 3)
                mplayer.Stop();
            else if (Info.Current.PlayState == PlayStates.Playing)
                mplayer.Rewind();
            else if (Info.Current.PlayState == PlayStates.Ended)
                mplayer.OpenFile(Info.URL);
            else
                mplayer.Stop();
        }

        private void rewindButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (rewindButton.Enabled && e.Button == MouseButtons.Left)
                rewindButton.Image = Properties.Resources.down_reverse;
        }

        private void rewindButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (rewindButton.Enabled)
                rewindButton.Image = Properties.Resources.default_reverse;
            else
                rewindButton.Image = Properties.Resources.disabled_reverse;
        }

        // PreviousButton
        private void previousButton_EnabledChanged(object sender, EventArgs e)
        {
            previousButton.Image = previousButton.Enabled ?
                Properties.Resources.default_previous : Properties.Resources.disabled_previous;
        }

        private void previousButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                playlist.PlayPreviousFile();
        }

        private void previousButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (previousButton.Enabled && e.Button == MouseButtons.Left)
                previousButton.Image = Properties.Resources.down_previous;
        }

        private void previousButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (previousButton.Enabled)
                previousButton.Image = Properties.Resources.default_previous;
            else
                previousButton.Image = Properties.Resources.disabled_previous;
        }

        private void previousButton_Paint(object sender, PaintEventArgs e)
        {
            if (!previousButton.Enabled) return;
            var drawFont = new Font("Segoe UI", 10f);
            var stringSize = new SizeF(e.Graphics.MeasureString(playlist.GetPlaylistIndex.ToString(), drawFont));
            var x = ((previousButton.Width - stringSize.Width) / 2) + 4;
            var y = (previousButton.Height - stringSize.Height) / 2;
            e.Graphics.DrawString(playlist.GetPlaylistIndex.ToString(), drawFont, new SolidBrush(Color.Black), x, y);
        }

        // NextButton
        private void nextButton_EnabledChanged(object sender, EventArgs e)
        {
            nextButton.Image = nextButton.Enabled ?
                Properties.Resources.default_next : Properties.Resources.disabled_next;
        }

        private void nextButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                playlist.PlayNextFile();
        }

        private void nextButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (nextButton.Enabled && e.Button == MouseButtons.Left)
                nextButton.Image = Properties.Resources.down_next;
        }

        private void nextButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (nextButton.Enabled)
                nextButton.Image = Properties.Resources.default_next;
            else
                nextButton.Image = Properties.Resources.disabled_next;
        }

        private void nextButton_Paint(object sender, PaintEventArgs e)
        {
            if (!nextButton.Enabled) return;
            var drawFont = new Font("Segoe UI", 10f);
            var stringSize = new SizeF(e.Graphics.MeasureString((playlist.GetPlaylistIndex + 2).ToString(), drawFont));
            float x = ((nextButton.Width - stringSize.Width) / 2) - 4;
            float y = (nextButton.Height - stringSize.Height) / 2;
            e.Graphics.DrawString((playlist.GetPlaylistIndex + 2).ToString(), drawFont, new SolidBrush(Color.Black), x, y);
        }

        // PlayButton
        private void playButton_EnabledChanged(object sender, EventArgs e)
        {
            if (playButton.Enabled)
            {
                if (Info.Current.PlayState == PlayStates.Playing)
                    playButton.Image = Properties.Resources.default_pause;
                else
                    playButton.Image = Properties.Resources.default_play;
            }
            else
            {
                playButton.Image = Properties.Resources.disabled_play;
            }
        }

        private void playButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && playButton.Enabled)
            {
                if (Info.Current.PlayState == PlayStates.Playing)
                    playButton.Image = Properties.Resources.down_pause;
                else
                    playButton.Image = Properties.Resources.down_play;
            }
        }

        private void playButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && playButton.Enabled)
            {
                mplayer.Pause(true);

                if (Info.Current.PlayState == PlayStates.Playing)
                    playButton.Image = Properties.Resources.default_play;
                else
                    playButton.Image = Properties.Resources.default_pause;
            }
        }

        // PlaylistButton
        private void playlistButton_EnabledChanged(object sender, EventArgs e)
        {
            playlistButton.Image = playlistButton.Enabled ?
                Properties.Resources.default_playlist : Properties.Resources.disabled_playlist;
        }

        private void playlistButton_MouseClick(object sender, MouseEventArgs e)
        {
            ShowPlaylist = !ShowPlaylist;
            showPlaylistToolStripMenuItem.Checked = !mplayerSplitContainer.Panel2Collapsed;
            hideAlbumArtToolStripMenuItem.Checked = false;
        }

        private void playlistButton_MouseDown(object sender, MouseEventArgs e)
        {
            playlistButton.Image = Properties.Resources.down_playlist;
        }

        private void playlistButton_MouseUp(object sender, MouseEventArgs e)
        {
            playlistButton.Image = Properties.Resources.default_playlist;
        }

        // VolumeBar
        private void volumeBar_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateVolume(volumeBar.Value);
        }

        #endregion
        #region Full Screen Mode

        private bool FullScreen
        {
            get
            {
                return fullScreenToolStripMenuItem.Checked;
            }
            set
            {
                if (value)
                {
                    // hide controls
                    ShowPlaylist = false;
                    ShowConsole = false;
                    mainMenuStrip.Hide();
                    seekPanel.Hide();
                    controlPanel.Hide();

                    this.ControlBox = false;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    this.TopMost = true;
                }
                else
                {
                    // show controls
                    mainMenuStrip.Show();
                    seekPanel.Show();
                    controlPanel.Show();

                    this.ControlBox = true;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                    this.TopMost = false;
                }
            }
        }

        private bool IsCursorShown = true;
        private bool CursorShown
        {
            get { return IsCursorShown; }
            set
            {
                if (value != IsCursorShown)
                {
                    IsCursorShown = value;
                    if (value)
                        Cursor.Show();
                    else
                        Cursor.Hide();
                }
            }
        }

        private void MouseMoved()
        {
            if (FullScreen)
            {
                var scrn = Screen.FromControl(this);

                CursorShown = true;
                cursorTimer.Start();

                if (Cursor.Position.Y > scrn.Bounds.Height - (seekPanel.Height + controlPanel.Height + 10))
                {
                    seekPanel.Show();
                    controlPanel.Show();
                }
                else
                {
                    seekPanel.Hide();
                    controlPanel.Hide();
                }
            }
        }

        private void cursorTimer_Tick(object sender, EventArgs e)
        {
            CursorShown = false;
            cursorTimer.Stop();
        }

        #endregion

        #region File

        private void newPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (blackForm != null && blackForm.Visible)
                this.dimLightsToolStripMenuItem.PerformClick();

            Process.Start(Application.ExecutablePath);
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void openURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var webForm = new WebForm();

            if (webForm.ShowDialog(this) == DialogResult.OK)
            {
                mplayer.OpenFile(webForm.URL);
                webForm.Dispose();
            }
        }

        private void playDVDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.OpenFile("dvdnav://");
        }

        private void cDDVDISOToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void specifyDriveLetterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*var inputForm = new InputForm("Type in the drive letter that the CD/DVD disc is in:", "Specify Drive Letter", "D");
            if (inputForm.ShowDialog(this) == DialogResult.OK)
            {
                openDVDDrive(inputForm.GetInputText.ToUpper());
                inputForm.Dispose();
            }*/
        }

        private void openDVDDrive(string letter)
        {
            //mplayer.SendCommand("dvd://1 -dvd-device {0}:\\", letter);
        }

        private void specifyBlurayDriveLetterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*var inputForm = new InputForm("Type in the drive letter that the Blu-ray disc is in:", "Specify Drive Letter", "D");
            if (inputForm.ShowDialog(this) == DialogResult.OK)
            {
                openBluRayDisc(inputForm.GetInputText.ToUpper());
                inputForm.Dispose();
            }*/
        }

        private void openBluRayDisc(string letter)
        {
            //mplayer.SendCommand("br:// -bluray-device {0}:\\", letter);
        }

        private void openLocationFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clipText = Clipboard.GetText();

            if (File.Exists(clipText) || Functions.ValidateURL(clipText))
                mplayer.OpenFile(clipText);
            else
                MessageBox.Show(string.Format("The location \"{0}\" cannot be openend.", clipText),
                    "Error Opening Location", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void openLastFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastFile = settings.GetStringValue("LastFile");
            if (File.Exists(lastFile))
                mplayer.OpenFile(lastFile);
        }

        private void saveMediaAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Info.URL))
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "explorer.exe",
                        Arguments = string.Format("/select,\"{0}\"", Info.URL),
                        WindowStyle = ProcessWindowStyle.Normal
                    }
                };
                process.Start();
            }
            else
                MessageBox.Show("Possible Reasons:\n1. File is located on the internet.\n2. The file may have been moved or deleted.",
                    "Error Opening Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Info.URL))
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "explorer.exe",
                        Arguments = string.Format("/select,\"{0}\"", Info.URL),
                        WindowStyle = ProcessWindowStyle.Normal
                    }
                };
                process.Start();
            }
            else
                Process.Start(string.Format("http://{0}", new Uri(Info.URL).Host));
        }

        private void playNextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlist.PlayNextFile();
        }

        private void playPreviousFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlist.PlayPreviousFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
        #region Playback
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.Pause(true);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.Stop();
        }

        private void rewindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.Rewind();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.Restart();
        }

        private void shuffleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shuffleToolStripMenuItem.Checked)
            {
                playlist.Shuffle();
                SetBackForwardControls();
            }
            else
            {
                playlist.refreshRequired = true;
                playlist.SetPlaylist();
            }
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offToolStripMenuItem.Checked = true;
            playlistToolStripMenuItem.Checked = false;
            thisFileToolStripMenuItem.Checked = false;
        }

        private void playlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistToolStripMenuItem.Checked = true;
            offToolStripMenuItem.Checked = false;
            thisFileToolStripMenuItem.Checked = false;
        }

        private void thisFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thisFileToolStripMenuItem.Checked = true;
            offToolStripMenuItem.Checked = false;
            playlistToolStripMenuItem.Checked = false;
        }

        private void frameStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.SendCommand("frame_step");
        }

        private void jumpToTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jumpForm = new JumpForm();
            if (jumpForm.ShowDialog(this) == DialogResult.OK)
            {
                mplayer.Seek(jumpForm.GetNewTime);
                jumpForm.Dispose();
            }
        }
        #endregion
        #region Media

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullScreen = fullScreenToolStripMenuItem.Checked;
        }

        private void fitToVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Info.VideoInfo.HasVideo)
            {
                this.Size = this.MinimumSize;
                return;
            }

            int playlistWidth = ShowPlaylist ?
                mplayerSplitContainer.Width - mplayerSplitContainer.SplitterDistance : 0;
            int consoleHeight = ShowConsole ?
                bodySplitContainer.Height - bodySplitContainer.SplitterDistance : 0;

            this.ClientSize = new Size(mplayerPanel.Width + playlistWidth,
                mainMenuStrip.Height + mplayerPanel.Height + consoleHeight + seekPanel.Height + controlPanel.Height);
        }

        private void audioTracksMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                mplayer.SetAudioTrack(index);
            }
        }

        private void SetAudioTracks()
        {
            audioTracksToolStripMenuItem.DropDownItems.Clear();

            if (Info.AudioInfo.AudioTracks.Count < 2)
            {
                var item = new ToolStripMenuItem("0: [ main ]", null);
                item.Enabled = false;
                audioTracksToolStripMenuItem.DropDownItems.Add(item);
                return;
            }

            foreach (AudioTrack track in Info.AudioInfo.AudioTracks)
            {
                var text = string.Format("{0}: {1} ({2})", track.ID, track.Name, track.Lang);
                var item = new ToolStripMenuItem(text, null, audioTracksMenuItem_Click);
                audioTracksToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void chaptersMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                mplayer.SetChapter(index);
            }
        }

        private void SetChapters()
        {
            chaptersToolStripMenuItem.DropDownItems.Clear();

            if (Info.MiscInfo.Chapters.Count.Equals(0))
            {
                var item = new ToolStripMenuItem("[ none ]", null);
                item.Enabled = false;
                chaptersToolStripMenuItem.DropDownItems.Add(item);
                return;
            }

            for (int i = 0; i < Info.MiscInfo.Chapters.Count; i++)
            {
                var text = string.Format("{0}: {1}", i+1, Info.MiscInfo.Chapters[i].ChapterName);
                var item = new ToolStripMenuItem(text, null, chaptersMenuItem_Click);
                if (i < 9)
                    item.ShortcutKeys = Keys.Control | KeysClass.GetNumKey(i+1);
                chaptersToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void monoAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void increaseVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Info.Current.Volume >= 95)
                UpdateVolume(100);
            else
                UpdateVolume(Info.Current.Volume + 5);
        }

        private void decreaseVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Info.Current.Volume <= 5)
                UpdateVolume(0);
            else
                UpdateVolume(Info.Current.Volume - 5);
        }

        private void volumeToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            int newVol;
            int.TryParse(volumeToolStripTextBox.Text, out newVol);

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (volumeToolStripTextBox.Text.ToLower().Equals("mute") || newVol.Equals(0))
                    {
                        UpdateVolume(0);
                        optionsToolStripMenuItem.HideDropDown();
                    }
                    else if (newVol > 0 && newVol <= 100)
                    {
                        UpdateVolume(newVol);
                        optionsToolStripMenuItem.HideDropDown();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a value that is between 1 - 100.",
                            "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        UpdateVolume(Info.Current.Volume);
                    }
                    break;
                case Keys.Up:
                    if (newVol >= 0 && newVol < 100)
                        volumeToolStripTextBox.Text = (newVol + 1).ToString();
                    break;
                case Keys.Down:
                    if (newVol > 0 && newVol <= 100)
                        volumeToolStripTextBox.Text = (newVol - 1).ToString();
                    break;
            }
        }

        #endregion
        #region Subtitles

        private void showSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.SetSubs(showSubtitlesToolStripMenuItem.Checked ? 0 : -1);
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // increase size
            mplayer.SendCommand("sub_scale +.1 0");
        }

        private void sizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // decrease size
            mplayer.SendCommand("sub_scale -.1 0");
        }

        private void resetSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.SendCommand("sub_scale 1 1");
        }

        private void subtitleMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                mplayer.SetSubs(index);
            }
        }

        private void SetSubs()
        {
            subtitleTrackToolStripMenuItem.DropDownItems.Clear();

            if (Info.MiscInfo.Subs.Count > 0)
            {
                showSubtitlesToolStripMenuItem.Enabled = true;
                showSubtitlesToolStripMenuItem.Checked = true;
                
                sizeToolStripMenuItem.Enabled = true;
                sizeToolStripMenuItem1.Enabled = true;
                resetSizeToolStripMenuItem.Enabled = true;
            }
            else
            {
                showSubtitlesToolStripMenuItem.Enabled = false;
                showSubtitlesToolStripMenuItem.Checked = false;

                sizeToolStripMenuItem.Enabled = false;
                sizeToolStripMenuItem1.Enabled = false;
                resetSizeToolStripMenuItem.Enabled = false;

                var item = new ToolStripMenuItem("[ none ]", null);
                item.Enabled = false;
                subtitleTrackToolStripMenuItem.DropDownItems.Add(item);
                return;
            }

            foreach (Sub sub in Info.MiscInfo.Subs)
            {
                var text = string.Format("{0}: {1} ({2})", sub.TrackID, sub.Name, sub.Lang);
                var item = new ToolStripMenuItem(text, null, subtitleMenuItem_Click);
                subtitleTrackToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        #endregion
        #region Tools

        private void showCommandLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConsole = showCommandLineToolStripMenuItem.Checked;
        }

        private void takeSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //fitToVideoToolStripMenuItem_Click(null, null);
            // hide osd
            HideStatusLabel();
            mplayer.ShowStatus(string.Empty);

            // get which screen the player is on
            //Screen scrn = Screen.FromControl(this);
            var bmpSnapshot = new Bitmap(mplayerPanel.Width, mplayerPanel.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap
            var gfxScreenshot = Graphics.FromImage(bmpSnapshot);

            // Take the screenshot from the upper left corner to the right bottom
            var screenPoint = this.PointToScreen(new Point(mplayerPanel.Location.X, mplayerPanel.Location.Y + mainMenuStrip.Height));
            gfxScreenshot.CopyFromScreen(screenPoint, new Point(0, 0), mplayerPanel.Size, CopyPixelOperation.SourceCopy);
            var SaveSnapshot = new SnapshotForm(bmpSnapshot);
            SaveSnapshot.ShowDialog(this);
        }

        private void sayMediaNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Speech.SayMedia();
        }

        private void mediaInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (infoForm == null || infoForm.IsDisposed)
                infoForm = new InfoForm(mplayer.GetMPlayerInfo());
            infoForm.Show();
        }

        #endregion
        #region Options

        private void showPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPlaylist = showPlaylistToolStripMenuItem.Checked;
        }

        private void hideAlbumArtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hideAlbumArtToolStripMenuItem.Checked)
            {
                HideAlbumArt = true;
                showPlaylistToolStripMenuItem.Checked = true;
                showPlaylistToolStripMenuItem_Click(null, null);
            }
            else
            {
                HideAlbumArt = false;
            }
        }

        private void dimLightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (blackForm == null)
            {
                blackForm = new BlackForm(this);
                blackForm.SetTitle = Path.GetFileNameWithoutExtension(Functions.DecodeURL(Info.URL));
            }

            if (dimLightsToolStripMenuItem.Checked)
            {
                blackForm.Show();
                this.TopLevel = true;
            }
            else
                blackForm.Hide();
        }

        private void alwaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alwaysToolStripMenuItem.Checked = true;
            whenPlayingToolStripMenuItem.Checked = false;
            neverToolStripMenuItem.Checked = false;
            setOnTop();
        }

        private void whenPlayingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            whenPlayingToolStripMenuItem.Checked = true;
            alwaysToolStripMenuItem.Checked = false;
            neverToolStripMenuItem.Checked = false;
            setOnTop();
        }

        private void neverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            neverToolStripMenuItem.Checked = true;
            alwaysToolStripMenuItem.Checked = false;
            whenPlayingToolStripMenuItem.Checked = false;
            setOnTop();
        }

        private void setOnTop()
        {
            if (whenPlayingToolStripMenuItem.Checked)
                this.TopMost = (Info.Current.PlayState == PlayStates.Playing);
            else if (alwaysToolStripMenuItem.Checked)
                this.TopMost = true;
            else if (neverToolStripMenuItem.Checked)
                this.TopMost = false;
        }

        private void showIconInTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showIconInTrayToolStripMenuItem.Checked)
            {
                minimizeToTrayToolStripMenuItem.Enabled = true;
                settings.SetConfig(true, "ShowIcon");
                trayIcon.Visible = true;
            }
            else
            {
                minimizeToTrayToolStripMenuItem.Enabled = false;
                settings.SetConfig(false, "ShowIcon");
                trayIcon.Visible = false;
            }
            settings.SaveConfig();
        }

        private void minimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.SetConfig(minimizeToTrayToolStripMenuItem.Checked, "MinimizeToTray");
            settings.SaveConfig();
        }

        private void hidePopupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.SetConfig(hidePopupToolStripMenuItem.Checked, "HidePopup");
            settings.SaveConfig();
        }

        private void allOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var optionsForm = new OptionsForm();
            optionsForm.ShowDialog(this);
        }

        #endregion
        #region Help

        private void bakaMPlayerHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpForm = new HelpForm();
            helpForm.Show();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var checker = new UpdateChecker();
            checker.Check(false);
        }

        private void aboutBakaMPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        #endregion

        #region MainForm Code

        public MainForm()
        {
            // set keyboard hook
            //  initialize our delegate
            this.myCallbackDelegate = this.callbackFunction_KeyboardHook;
            //  setup a keyboard hook
            hHook = SetWindowsHookEx(2, this.myCallbackDelegate, IntPtr.Zero, AppDomain.GetCurrentThreadId());

            // set mouse hook
            var mouseHandler = new GlobalMouseHandler();
            mouseHandler.TheMouseMoved += MouseMoved;
            Application.AddMessageFilter(mouseHandler);

            InitializeComponent();

            this.MouseWheel += MainForm_MouseWheel;
            mplayer = new MPlayer(this);
            playlist.Init(this, mplayer);

            folderToolStripMenuItem.Text = "Build " + Application.ProductVersion;
            trayIcon.ContextMenu = trayContextMenu;
            SetLCDFont(); // Embbeding Font (LCD)
            ShowConsole = false;
            ShowPlaylist = false;

            LoadSettings();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // check for mplayer2
            if (!File.Exists(Application.StartupPath + @"\mplayer2.exe"))
            {
                MessageBox.Show("Baka MPlayer cannot load without \"mplayer2.exe\"!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }

            this.MinimumSize = new Size(this.Width,
                this.Height - this.ClientSize.Height + mainMenuStrip.Height + seekPanel.Height + controlPanel.Height);

            // check for command line args
            var cmdLine = Environment.GetCommandLineArgs();
            for (int i = 1; i < cmdLine.Length; i++)
            {
                if (cmdLine[i].Equals("-lastfile"))
                {
                    if (File.Exists(settings.GetStringValue("LastFile")))
                        mplayer.OpenFile(settings.GetStringValue("LastFile"));
                    else
                        MessageBox.Show("Either there is no previous file or the previous file does not exist anymore.",
                            "No Previous File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }
                if (File.Exists(cmdLine[i]))
                {
                    // Note: only one file is supported for now
                    mplayer.OpenFile(cmdLine[i]);
                    break;
                }
            }

            // check for updates
            var checker = new UpdateChecker();
            checker.Check(true);
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Functions.RunningOnWin7)
                setThumbnailToolbars();
        }
        private void LoadSettings()
        {
            // load volume
            UpdateVolume(settings.GetIntValue("Volume"));

            if (!settings.GetBoolValue("ShowIcon"))
            {
                trayIcon.Visible = false;
                showIconInTrayToolStripMenuItem.Checked = false;
            }
            if (settings.GetBoolValue("MinimizeToTray"))
            {
                minimizeToTrayToolStripMenuItem.Enabled = true;
                minimizeToTrayToolStripMenuItem.Checked = true;
            }
            hidePopupToolStripMenuItem.Checked = settings.GetBoolValue("HidePopup");

            // set previous file
            var lastFile = settings.GetStringValue("LastFile");
            if (File.Exists(lastFile))
            {
                openLastFileToolStripMenuItem.ToolTipText = Path.GetFileName(lastFile);
                openLastFileToolStripMenuItem.Enabled = true;
            }
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                HidePlayer();

            // make sure its not focused on anything else
            if (string.IsNullOrEmpty(Info.URL) || playlist.searchTextBox.Focused)
                return;

            switch (e.KeyCode)
            {
                case Keys.Space: // play or pause
                    mplayer.Pause(true);
                    break;
            }
        }
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                // scroll up (increase volume)
                if (Info.Current.Volume >= 95)
                    UpdateVolume(100);
                else
                    UpdateVolume(Info.Current.Volume + 5);
            }
            else
            {
                // scroll down (decrease volume)
                if (Info.Current.Volume <= 5)
                    UpdateVolume(0);
                else
                    UpdateVolume(Info.Current.Volume - 5);
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // unhook windows keyboard hook
            UnhookWindowsHookEx(hHook);
            UnloadTray();

            // save LastFile
            if (!string.IsNullOrEmpty(Info.URL))
                settings.SetConfig(Info.URL, "LastFile");
            settings.SaveConfig();
            mplayer.Close();
        }

        #endregion

        #region Media Opened

        public void CallMediaOpened()
        {
            Invoke((MethodInvoker)MediaOpened);
        }
        private void MediaOpened()
        {
            if (firstFile)
            {
                firstFile = false;
                playlist.refreshRequired = true;
                settings.SetConfig(Info.URL, "LastFile");
            }
            else
            {
                settings.SetConfig(tempURL, "LastFile");
                openLastFileToolStripMenuItem.ToolTipText = Path.GetFileName(tempURL);
                openLastFileToolStripMenuItem.Enabled = true;
                playlist.refreshRequired = Path.GetDirectoryName(tempURL) != Path.GetDirectoryName(Info.URL);
            }
            settings.SaveConfig();
            tempURL = Info.URL;

            // set file name texts
            this.Text = Path.GetFileName(Functions.DecodeURL(Info.URL));

            folderToolStripMenuItem.Text = File.Exists(Info.URL) ?
                Functions.AutoEllipsis(32, Functions.GetFolderName(Info.URL)) : new Uri(Info.URL).Host;

            if (blackForm != null)
                blackForm.SetTitle = Path.GetFileNameWithoutExtension(Functions.DecodeURL(Info.URL));

            if (infoForm != null && !infoForm.IsDisposed)
                infoForm.RefreshInfo();

            if (Info.VideoInfo.HasVideo)
            {
                // video file
                albumArtPicbox.Visible = false;
                mplayerPanel.Visible = true;

                frameStepToolStripMenuItem.Enabled = true;
                fullScreenToolStripMenuItem.Enabled = true;
                hideAlbumArtToolStripMenuItem.Checked = false;
                hideAlbumArtToolStripMenuItem_Click(null, null);
                hideAlbumArtToolStripMenuItem.Enabled = false;
                takeSnapshotToolStripMenuItem.Enabled = true;
            }
            else
            {
                // music file
                mplayerPanel.Visible = false;
                albumArtPicbox.Visible = true;

                frameStepToolStripMenuItem.Enabled = false;
                fullScreenToolStripMenuItem.Enabled = false;
                hideAlbumArtToolStripMenuItem.Enabled = true;
                takeSnapshotToolStripMenuItem.Enabled = false;

                // show album art (if it exists);
                albumArtPicbox.Image = Info.ID3Tags.AlbumArt ?? Properties.Resources.Music_128;
            }
            bodySplitContainer_Panel1_SizeChanged(null, null);

            // check if media is online
            if (File.Exists(Info.URL))
            {
                saveMediaAsToolStripMenuItem.Enabled = false;
                showInWindowsExplorerToolStripMenuItem.Enabled = true;
            }
            else
            {
                saveMediaAsToolStripMenuItem.Enabled = true;
                showInWindowsExplorerToolStripMenuItem.Enabled = false;
            }
            // set previous volume (output drivers fault for not saving volume)
            UpdateVolume(Info.Current.Volume);

            // call other methods
            playlist.SetPlaylist();
            SetSystemTray();
            enableControls();
            SetBackForwardControls();

            // create menu items
            SetAudioTracks();
            SetChapters();
            SetSubs();

            // play video
            mplayer.SendCommand("pausing get_property pause");
            mplayer.Play();
        }

        #endregion
        #region Media Ended

        public void CallMediaEnded()
        {
            Invoke((MethodInvoker)MediaEnded);
        }
        private void MediaEnded()
        {
            if (stopAftercurrentToolStripMenuItem.Checked)
            {
                LastFile();
                return;
            }

            if (playlistToolStripMenuItem.Checked)
            {
                if (playlist.GetPlaylistIndex.Equals(playlist.GetTotalItems - 1))
                {
                    // end of playlist, repeat from beginning of playlist
                    playlist.PlayFile(0);
                }
                else
                {
                    // next file
                    playlist.PlayNextFile();
                }
            }
            else if (thisFileToolStripMenuItem.Checked)
            {
                // repeat this file
                mplayer.OpenFile(Info.URL);
            }
            else if (offToolStripMenuItem.Checked)
            {
                // repeat off/default
                if (playlist.GetPlaylistIndex < playlist.GetTotalItems - 1)
                    playlist.PlayFile(playlist.GetPlaylistIndex + 1);
                else
                    LastFile();
            }
        }

        private void LastFile()
        {
            seekBar.Enabled = false;
            previousButton.Enabled = false;
            playButton.Enabled = false;
            nextButton.Enabled = false;
        }

        #endregion
        #region PlayState Changed

        public void CallPlayStateChanged()
        {
            Invoke((MethodInvoker)PlayStateChanged);
        }
        private void PlayStateChanged()
        {
            setOnTop();

            switch (Info.Current.PlayState)
            {
                case PlayStates.Unidentified:
                    seekBar.Enabled = false;
                    rewindButton.Enabled = false;
                    playButton.Enabled = false;
                    playToolButton.Enabled = false;
                    break;
                case PlayStates.Playing:
                    playButton.Image = Properties.Resources.default_pause;
                    playToolButton.Icon = Properties.Resources.tool_pause;

                    playToolStripMenuItem.Text = "&Pause";
                    playMenuItem.Text = "&Pause";
                    playToolButton.Tooltip = "Pause";
                    break;
                case PlayStates.Paused:
                    playButton.Image = Properties.Resources.default_play;
                    playToolButton.Icon = Properties.Resources.tool_play;

                    playToolStripMenuItem.Text = "&Play";
                    playMenuItem.Text = "&Play";
                    playToolButton.Tooltip = "Play";
                    break;
                case PlayStates.Stopped:
                    seekBar.Value = 0;
                    playButton.Image = Properties.Resources.default_play;
                    playToolButton.Icon = Properties.Resources.tool_play;

                    playToolStripMenuItem.Text = "&Play";
                    playMenuItem.Text = "&Play";
                    playToolButton.Tooltip = "Play";
                    durationLabel.Text = "STOPPED";
                    break;
            }
        }

        #endregion
        #region Duration Changed

        public void CallDurationChanged()
        {
            try
            {
                Invoke((MethodInvoker)DurationChanged);
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void DurationChanged()
        {
            UpdateNowPlayingInfo();

            if (seekBar_IsMouseDown)
                return;
            
            if (Info.Current.TotalLength > 0)
            {
                seekBar.Value = Convert.ToInt32((Info.Current.Duration * 1000000) / Info.Current.TotalLength); // %
                durationLabel.Text = Functions.ConvertTimeFromSeconds(Info.Current.Duration);
            }

            if (settings.GetBoolValue("ShowTimeRemaining"))
                timeLeftLabel.Text = string.Format("-{0}", Functions.ConvertTimeFromSeconds(Info.Current.TotalLength - Info.Current.Duration));
            else
                timeLeftLabel.Text = Functions.ConvertTimeFromSeconds(Info.Current.TotalLength);
        }

        #endregion

        private void OpenFile()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = string.Format("Multimedia|{0}|Video Files|{1}|Audio Files|{2}|All Files (*.*)|*.*",
                Properties.Resources.VideoFiles + "; " + Properties.Resources.AudioFiles,
                Properties.Resources.VideoFiles, Properties.Resources.AudioFiles);

            if (File.Exists(Info.URL))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Info.URL);
                ofd.FileName = Path.GetFileName(Info.URL);
            }
            else
                ofd.FileName = string.Empty;

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                mplayer.OpenFile(ofd.FileName);
                ofd.Dispose();
            }
        }

        private void HidePlayer()
        {
            if (seekBar_IsMouseDown)
            { // stop mouse movement
                seekBar_IsMouseDown = false;
                return;
            }

            // boss mode
            mplayer.Pause(false);

            if (FullScreen)
                fullScreenToolStripMenuItem.PerformClick();
            if (blackForm != null)
                blackForm.Hide();
            dimLightsToolStripMenuItem.Checked = false;
            if (minimizeToTrayToolStripMenuItem.Enabled && minimizeToTrayToolStripMenuItem.Checked)
                ToggleToTaskbar(true);

            // code required to bypass windows hide animation
            this.Opacity = 0.0;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Opacity = 1.0;
        }

        private void enableControls()
        {
            // control panel controls
            seekBar.Enabled = true;
            rewindButton.Enabled = true;
            playButton.Enabled = true;
            playlistButton.Enabled = true;
            
            // menu strips
            playToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = true;
            rewindToolStripMenuItem.Enabled = true;
            restartToolStripMenuItem.Enabled = true;
            jumpToTimeToolStripMenuItem.Enabled = true;

            fitToVideoToolStripMenuItem.Enabled = true;
            sayMediaNameToolStripMenuItem.Enabled = true;
            mediaInfoToolStripMenuItem.Enabled = true;

            showPlaylistToolStripMenuItem.Enabled = true;

            folderToolStripMenuItem.Enabled = true;

            // tray context menu
            showMenuItem.Enabled = true;
            playMenuItem.Enabled = true;
            stopMenuItem.Enabled = true;
            rewindMenuItem.Enabled = true;

            // thumbnail toolbar button
            playToolButton.Enabled = true;
        }

        private void albumArtPicbox_SizeChanged(object sender, EventArgs e)
        {
            if (!albumArtPicbox.Visible) return;

            if (albumArtPicbox.Width < albumArtPicbox.Image.Width ||
                albumArtPicbox.Height < albumArtPicbox.Image.Height)
                albumArtPicbox.SizeMode = PictureBoxSizeMode.Zoom;
            else
                albumArtPicbox.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void bodySplitContainer_Panel1_SizeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Info.URL))
            {
                //mplayerPanel.Size = bodySplitContainer.Panel1.Size;
                return;
            }

            if (Info.VideoInfo.HasVideo)
            {
                var now_aspect = (double)bodySplitContainer.Panel1.Width / bodySplitContainer.Panel1.Height;
                int newWid, newHei;

                var ratio = Info.VideoInfo.AspectRatio;
                if (ratio.Equals(0.0))
                    ratio = (double)Info.VideoInfo.Width / Info.VideoInfo.Height;

                if (now_aspect < ratio)
                {
                    newWid = bodySplitContainer.Panel1.Width;
                    newHei = (int)(newWid / ratio);
                }
                else
                {
                    newHei = bodySplitContainer.Panel1.Height;
                    newWid = (int)(newHei * ratio);
                }

                // update Size : Width, Height
                if (Info.VideoInfo.Width != newWid || Info.VideoInfo.Height != newHei)
                    mplayerPanel.Size = new Size(newWid, newHei);

                // update location : Top, Left
                newWid = (bodySplitContainer.Panel1.Width - newWid) / 2;
                newHei = (bodySplitContainer.Panel1.Height - newHei) / 2;

                if (mplayerPanel.Left != newWid || mplayerPanel.Top != newHei)
                    mplayerPanel.Location = new Point(newWid, newHei);
            }
        }

        private void UpdateNowPlayingInfo()
        {
            switch(Info.Current.PlayState)
            {
                case PlayStates.Playing:
                    nowPlayingMenuItem.Text = string.Format("Now Playing ({0})", Functions.ConvertTimeFromSeconds(Info.Current.Duration));
                    break;
                case PlayStates.Paused:
                    nowPlayingMenuItem.Text = string.Format("Paused ({0})", Functions.ConvertTimeFromSeconds(Info.Current.Duration));
                    break;
                case PlayStates.Stopped:
                    nowPlayingMenuItem.Text = "Stopped";
                    break;
                default:
                    nowPlayingMenuItem.Text = "Now Playing (00:00:00)";
                    break;
            }
        }

        private void UpdateVolume(int newVol)
        {
            if (newVol < 0 || newVol > 100)
                return;

            if (mplayer.MPlayerIsRunning())
                mplayer.SetVolume(newVol);
            Info.Current.Volume = newVol;
            settings.SetConfig(newVol, "Volume");

            if (newVol.Equals(0))
            { // mute
                if (mplayer.MPlayerIsRunning())
                    mplayer.Mute(true);
                volumeToolStripTextBox.Text = "Mute";

                volumeBar.ThumbInnerColor = Color.DimGray;
                volumeBar.ThumbOuterColor = Color.DarkGray;
                volumeBar.ThumbPenColor = Color.DimGray;
                volumeBar.Value = 0;
            }
            else
            { // not mute
                if (mplayer.MPlayerIsRunning())
                    mplayer.Mute(false);
                volumeToolStripTextBox.Text = newVol.ToString();

                volumeBar.ThumbInnerColor = Color.DarkGray;
                volumeBar.ThumbOuterColor = Color.Silver;
                volumeBar.ThumbPenColor = Color.Gray;
                volumeBar.Value = newVol;
            }
        }

        public void CallSetBackForwardControls()
        {
            Invoke((MethodInvoker)SetBackForwardControls);
        }
        private void SetBackForwardControls()
        {
            if (playlist.GetTotalItems > 1)
            {
                if (playlist.GetPlaylistIndex.Equals(0))
                {
                    previousButton.Enabled = false;
                    nextButton.Enabled = true;
                    playPreviousFileToolStripMenuItem.Enabled = false;
                    playNextFileToolStripMenuItem.Enabled = true;
                    // ThumbnailButtons
                    previousToolButton.Enabled = false;
                    nextToolButton.Enabled = true;
                    // notification area
                    previousMenuItem.Enabled = false;
                    nextMenuItem.Enabled = true;
                }
                else if (playlist.GetPlaylistIndex + 1 == playlist.GetTotalItems)
                {
                    previousButton.Enabled = true;
                    nextButton.Enabled = false;
                    playPreviousFileToolStripMenuItem.Enabled = true;
                    playNextFileToolStripMenuItem.Enabled = false;
                    // ThumbnailButtons
                    previousToolButton.Enabled = true;
                    nextToolButton.Enabled = false;
                    // notification area
                    previousMenuItem.Enabled = true;
                    nextMenuItem.Enabled = false;
                }
                else
                {
                    previousButton.Enabled = true;
                    nextButton.Enabled = true;
                    playPreviousFileToolStripMenuItem.Enabled = true;
                    playNextFileToolStripMenuItem.Enabled = true;
                    // ThumbnailButtons
                    previousToolButton.Enabled = true;
                    nextToolButton.Enabled = true;
                    // notification area
                    previousMenuItem.Enabled = true;
                    nextMenuItem.Enabled = true;
                }
            }
            else
            {
                previousButton.Enabled = false;
                nextButton.Enabled = false;
                previousButton.Refresh();
                nextButton.Refresh();
                playPreviousFileToolStripMenuItem.Enabled = false;
                playNextFileToolStripMenuItem.Enabled = false;
                // ThumbnailButtons
                previousToolButton.Enabled = false;
                nextToolButton.Enabled = false;
                // notification area
                previousMenuItem.Enabled = false;
                nextMenuItem.Enabled = false;
                return;
            }
            previousButton.Refresh();
            nextButton.Refresh();
        }
    }
}
