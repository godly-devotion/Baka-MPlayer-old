using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Baka_MPlayer.Forms
{
    public partial class MainForm : Form
    {
        #region DLL Imports

        // global key hook
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private readonly HookProc myCallbackDelegate;
        private static IntPtr hHook;

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

        private readonly MPlayer mplayer;
        private readonly Settings settings = new Settings();
        private Voice voice;

        // Win 7 thumbnail toolbar buttons
        private ThumbnailToolBarButton previousToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_previous, "Previous file");
        private ThumbnailToolBarButton playToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_play, "Play");
        private ThumbnailToolBarButton nextToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_next, "Next file");

        // lastFile feature
        private bool firstFile = true;
        private string tempURL = string.Empty;

        private bool seekBar_IsMouseDown;

        #endregion

        #region Tray Icon

        // Right click context menu
        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Focus();
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
            playlist.PlayNext();
        }

        private void previousMenuItem_Click(object sender, EventArgs e)
        {
            playlist.PlayPrevious();
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
            this.WindowState = FormWindowState.Normal;
            this.Focus();
        }

        // notify icon code
        private void SetSystemTray()
        {
            if (Info.IsOnline)
            {
                titleMenuItem.Text = string.Format("  {0}", Functions.String.AutoEllipsis(25, Info.MovieName));
                artistMenuItem.Text = "  Online Media";
                SetNotifyIconText(string.Format("{0}\n{1}", Info.MovieName, "Online Media"));
                return;
            }

            var lastPart = string.Format("(File {0} of {1})", playlist.GetPlayingItem.Index + 1, playlist.GetTotalItems);

            if (!string.IsNullOrEmpty(Info.ID3Tags.Title))
            {
                bool hasArtist = !string.IsNullOrEmpty(Info.ID3Tags.Artist);

                titleMenuItem.Text = string.Format("  {0}", Functions.String.AutoEllipsis(25, Info.ID3Tags.Title));
                artistMenuItem.Text = string.Format("  {0}",
                    hasArtist ? Functions.String.AutoEllipsis(25, Info.ID3Tags.Artist) : "Unknown Artist");

                SetNotifyIconText(string.Format("{0}\n{1}{2}", Info.ID3Tags.Title, hasArtist ? Info.ID3Tags.Artist + '\n' : "", lastPart));

                if (!Info.VideoInfo.HasVideo && !hidePopupToolStripMenuItem.Checked)
                    trayIcon.ShowBalloonTip(4000, Info.ID3Tags.Title, (hasArtist ? Info.ID3Tags.Artist + '\n' : "") + lastPart, ToolTipIcon.None);

            }
            else
            {
                // no title & artist (no artist assumed)
                var fileName = Functions.String.AutoEllipsis(25, Info.MovieName);

                titleMenuItem.Text = string.Format("  {0}", fileName);
                artistMenuItem.Text = "  Unknown Artist";

                SetNotifyIconText(string.Format("{0}\n{1}", fileName, lastPart));

                if (!Info.VideoInfo.HasVideo && !hidePopupToolStripMenuItem.Checked)
                    trayIcon.ShowBalloonTip(4000, fileName, lastPart, ToolTipIcon.None);
            }
        }

        private void SetNotifyIconText(string text)
        {
            // max text length is 127 characters
            if (text.Length > 127)
                text = text.Substring(0, 127);

            var t = typeof(NotifyIcon);
            const BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;

            t.GetField("text", hidden).SetValue(trayIcon, text);
            if ((bool)t.GetField("added", hidden).GetValue(trayIcon))
                t.GetMethod("UpdateIcon", hidden).Invoke(trayIcon, new object[] { true });
        }

        private void UnloadTray()
        {
            // release trayIcon's resources
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        #endregion
        #region Snap-to-Border

        private const int SnapOffset = 10; // pixels

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
            fonts.AddFontFile(name);
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
            if (code.Equals(3) && Convert.ToString(lParam.ToInt64(), 2).StartsWith("10") && NotFocusedOnTextbox)
            {
                switch ((Keys)wParam.ToInt32())
                {
                    case Keys.Left:
                        if (Info.Current.Duration - 5 > -1)
                            mplayer.Seek(Info.Current.Duration - 5);
                        else //if (mplayer.currentPosition < 5)
                            mplayer.Seek(0);
                        break;
                    case Keys.Right:
                        if (Info.Current.Duration + 5 < Info.Current.TotalLength)
                            mplayer.Seek(Info.Current.Duration + 5);
                        else
                            playlist.PlayNext();
                        break;
                    case Keys.PageUp:
                        mplayer.SkipChapter(false);
                        break;
                    case Keys.PageDown:
                        mplayer.SkipChapter(true);
                        break;
                    case Keys.MediaNextTrack:
                        playlist.PlayNext();
                        break;
                    case Keys.MediaPreviousTrack:
                        playlist.PlayPrevious();
                        break;
                    case Keys.MediaStop:
                        mplayer.Stop();
                        break;
                    case Keys.MediaPlayPause:
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
            }

            // return the value returned by CallNextHookEx
            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        #endregion
        #region File Drag & Drop

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

        private void SetThumbnailToolbars()
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
            playlist.PlayNext();
        }

        private void previousToolButton_Click(Object sender, EventArgs e)
        {
            playlist.PlayPrevious();
        }

        #endregion

        #region Accessors

        private bool voiceEnabled;
        private bool EnableVoiceCommand
        {
            get { return voiceEnabled; }
            set
            {
                try
                {
                    if (value)
                    {
                        if (voice == null)
                        {
                            var name = settings.GetStringValue(SettingEnum.CallName).Trim();
                            voice = new Voice(this, string.IsNullOrEmpty(name) ? "baka" : name);
                        }
                        voice.StartListening();
                        voiceEnabled = true;
                    }
                    else
                    {
                        voice.StopListening();
                        voiceEnabled = false;
                    }
                }
                catch (Exception)
                {
                    SetStatusMsg("There was a problem starting the voice command system. Check to see if your mic is plugged in.", true);
                    
                    if (voice != null)
                        voice.StopListening();
                    voiceEnabled = false;
                }
            }
        }

        public bool ShowPlaylist
        {
            get { return !bodySplitContainer.Panel2Collapsed; }
            set
            {
                if (value)
                {
                    bodySplitContainer.Panel2Collapsed = false;
                    showPlaylistToolStripMenuItem.Checked = true;

                    playlist.DisableInteraction = false;
                }
                else
                {
                    bodySplitContainer.Panel2Collapsed = true;
                    showPlaylistToolStripMenuItem.Checked = false;
                    hideAlbumArtToolStripMenuItem.Checked = false;

                    playlist.DisableInteraction = true;
                    seekBar.Focus();
                }
            }
        }

        private bool HideAlbumArt
        {
            get { return bodySplitContainer.Panel1Collapsed; }
            set
            {
                if (value)
                    ShowPlaylist = true;

                bodySplitContainer.Panel1Collapsed = value;
                hideAlbumArtToolStripMenuItem.Checked = value;
            }
        }

        private bool ShowConsole
        {
            get { return !mplayerSplitContainer.Panel2Collapsed; }
            set
            {
                if (!value)
                    seekBar.Focus();

                mplayerSplitContainer.Panel2Collapsed = !value;
                showCommandLineToolStripMenuItem.Checked = value;
            }
        }

        public bool CheckShuffleToolStripMenuItem
        {
            set { Invoke((MethodInvoker)(() => shuffleToolStripMenuItem.Checked = value)); }
        }

        public bool EnablePlaylistButton
        {
            set { Invoke((MethodInvoker)(() => playlistButton.Enabled = value)); }
        }

        private bool NotFocusedOnTextbox
        {
            get { return !(string.IsNullOrEmpty(Info.URL) || playlist.searchTextBox.Focused || inputTextbox.Focused); }
        }

        #endregion
        #region Speech Code

        public void CallStateChanged(VoiceState e)
        {
            //Invoke((MethodInvoker)(() => StateChanged(e)));
        }
        private void StateChanged(VoiceState e)
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
            Invoke((MethodInvoker)(() => TakeAction(speechCommand)));
        }
        private void TakeAction(string speechCommand)
        {
            SetStatusMsg("Voice Command: " + Functions.String.ToTitleCase(speechCommand), true);

            switch (speechCommand)
            {
                case "OPEN":
                    OpenFile();
                    break;
                case "PLAY":
                    mplayer.Play();
                    break;
                case "PAUSE":
                    mplayer.Pause(false);
                    break;
                case "STOP":
                    mplayer.Stop();
                    break;
                case "MUTE":
                    mplayer.Mute(true);
                    break;
                case "UNMUTE":
                    mplayer.Mute(false);
                    break;
                case "NEXT":
                case "NEXT FILE":
                    playlist.PlayNext();
                    break;
                case "PREVIOUS":
                case "PREVIOUS FILE":
                    playlist.PlayPrevious();
                    break;
                case "HIDE":
                    HidePlayer();
                    break;
                case "WHATS PLAYING":
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

            if (settings.GetBoolValue(SettingEnum.ShowTimeRemaining))
                timeLeftLabel.Text = string.Format("-{0}", Functions.Time.ConvertTimeFromSeconds(Info.Current.TotalLength - currentPos));
            else
                timeLeftLabel.Text = Functions.Time.ConvertTimeFromSeconds(Info.Current.TotalLength);

            durationLabel.Text = Functions.Time.ConvertTimeFromSeconds(currentPos);
        }

        private void seekBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (seekBar_IsMouseDown)
            {
                mplayer.Seek((seekBar.Value * Info.Current.TotalLength) / seekBar.Maximum);
                seekBar_IsMouseDown = false;
            }
        }

        private void timeLeftLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(Info.URL))
                return;

            var setting = !settings.GetBoolValue(SettingEnum.ShowTimeRemaining);
            settings.SetConfig(setting, SettingEnum.ShowTimeRemaining);
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
                    var webForm = new UrlForm();
                    if (webForm.ShowDialog(this) == DialogResult.OK)
                    {
                        mplayer.OpenFile(webForm.URL);
                        webForm.Dispose();
                    }
                    break;
            }
        }

        // Speech Button
        private void speechButton_MouseClick(object sender, MouseEventArgs e)
        {
            EnableVoiceCommand = !EnableVoiceCommand;
        }
        private void speechButton_MouseDown(object sender, MouseEventArgs e)
        {
            speechButton.Image = Properties.Resources.down_mic;
        }
        private void speechButton_MouseUp(object sender, MouseEventArgs e)
        {
            speechButton.Image = EnableVoiceCommand ?
                Properties.Resources.enabled_mic : Properties.Resources.disabled_mic;
        }

        // RewindButton
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
        private void rewindButton_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mplayer.Stop();
        }

        // PreviousButton
        private void previousButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                playlist.PlayPrevious();
        }
        private void previousButton_Paint(object sender, PaintEventArgs e)
        {
            if (!previousButton.Enabled)
                return;

            using (var drawFont = new Font("Segoe UI", 10f))
            {
                var stringSize = new SizeF(e.Graphics.MeasureString(playlist.GetPlayingItem.Index.ToString(), drawFont));
                var x = ((previousButton.Width - stringSize.Width) / 2) + 5;
                var y = (previousButton.Height - stringSize.Height) / 2;
                e.Graphics.DrawString(playlist.GetPlayingItem.Index.ToString(), drawFont, new SolidBrush(Color.Black), x, y);
            }
        }

        // NextButton
        private void nextButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                playlist.PlayNext();
        }
        private void nextButton_Paint(object sender, PaintEventArgs e)
        {
            if (!nextButton.Enabled)
                return;

            using (var drawFont = new Font("Segoe UI", 10f))
            {
                var stringSize = new SizeF(e.Graphics.MeasureString((playlist.GetPlayingItem.Index + 2).ToString(), drawFont));
                var x = ((nextButton.Width - stringSize.Width) / 2) - 5;
                var y = (nextButton.Height - stringSize.Height) / 2;
                e.Graphics.DrawString((playlist.GetPlayingItem.Index + 2).ToString(), drawFont, new SolidBrush(Color.Black), x, y);
            }
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
        private void playlistButton_MouseClick(object sender, MouseEventArgs e)
        {
            ShowPlaylist = !ShowPlaylist;
        }

        // VolumeBar
        private void volumeBar_Scroll(object sender, ScrollEventArgs e)
        {
            SetVolume(volumeBar.Value);
        }

        #endregion
        #region Full Screen Mode

        private bool FullScreen
        {
            get
            {
                return VO_State.FullScreen;
            }
            set
            {
                VO_State.FullScreen = value;
                fullScreenToolStripMenuItem.Checked = value;

                if (value)
                {
                    ShowPlaylist = false;
                    ShowConsole = false;

                    mainMenuStrip.Hide();
                    seekPanel.Hide();
                    controlPanel.Hide();

                    this.ControlBox = false;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    this.TopMost = true;
                    
                    VO_State.LastCursorPos = Cursor.Position;
                    Cursor.Current = null;
                }
                else
                {
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

        private void mouseHandler_MouseMoved()
        {
            if (!FullScreen) return;
            if (VO_State.LastCursorPos != Cursor.Position)
            {
                VO_State.LastCursorPos = Cursor.Position;

                var scrn = Screen.FromControl(this);

                if (Cursor.Position.Y > scrn.Bounds.Height - (seekPanel.Height + controlPanel.Height + 10))
                {
                    cursorTimer.Stop();

                    seekPanel.Show();
                    controlPanel.Show();
                }
                else
                {
                    cursorTimer.Start();

                    seekPanel.Hide();
                    controlPanel.Hide();
                }
            }
        }

        private void cursorTimer_Tick(object sender, EventArgs e)
        {
            Cursor.Current = null;
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

        private void openFileWithExternalSubsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var subForm = new OpenSubForm();

            if (subForm.ShowDialog(this) == DialogResult.OK)
            {
                mplayer.ExternalSub = subForm.SubFile;
                mplayer.OpenFile(subForm.MediaFile);
            }

            subForm.Dispose();
        }

        private void openURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var webForm = new UrlForm();

            if (webForm.ShowDialog(this) == DialogResult.OK)
                mplayer.OpenFile(webForm.URL);

            webForm.Dispose();
        }

        private void openLocationFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clipText = Clipboard.GetText();

            if (File.Exists(clipText) || Functions.URL.IsValidURL(clipText))
                mplayer.OpenFile(clipText);
            else
                MessageBox.Show(string.Format("The location \"{0}\" cannot be opened.", clipText),
                    "Error Opening Location", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void openLastFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastFile = settings.GetStringValue(SettingEnum.LastFile);
            if (File.Exists(lastFile))
                mplayer.OpenFile(lastFile);
        }

        private void showInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Info.IsOnline)
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
            {
                MessageBox.Show("Possible Reasons:\n1. File is located on the internet.\n2. The file may have been moved or deleted.",
                    "Problem Opening Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Info.IsOnline)
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
            playlist.PlayNext();
        }

        private void playPreviousFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlist.PlayPrevious();
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
            playlist.Shuffle = shuffleToolStripMenuItem.Checked;
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

        private void frameBackStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.SendCommand("frame_back_step");
        }

        private void jumpToTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jumpForm = new JumpForm();

            if (jumpForm.ShowDialog(this) == DialogResult.OK)
                mplayer.Seek(jumpForm.GetNewTime);

            jumpForm.Dispose();
        }
        #endregion
        #region Media

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullScreen = !FullScreen;
        }

        private void fitToVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Info.VideoInfo.HasVideo)
            {
                this.Size = this.MinimumSize;
                return;
            }

            int playlistWidth = ShowPlaylist ?
                bodySplitContainer.Width - bodySplitContainer.SplitterDistance : 0;
            int consoleHeight = ShowConsole ?
                mplayerSplitContainer.Height - mplayerSplitContainer.SplitterDistance : 0;

            this.ClientSize = new Size(mplayerPanel.Width + playlistWidth,
                mainMenuStrip.Height + mplayerPanel.Height + consoleHeight + seekPanel.Height + controlPanel.Height);
        }

        private void previousChapterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.SkipChapter(false);
        }

        private void nextChapterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.SkipChapter(true);
        }

        private void autodetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info.VideoInfo.AspectRatio = Math.Round((double) Info.VideoInfo.Width/Info.VideoInfo.Height, 5);
            ResizeMplayerPanel();
        }

        private void force43ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info.VideoInfo.AspectRatio = 1.3333;
            ResizeMplayerPanel();
        }

        private void force169ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info.VideoInfo.AspectRatio = 1.7778;
            ResizeMplayerPanel();
        }

        private void force2351ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info.VideoInfo.AspectRatio = 2.35;
            ResizeMplayerPanel();
        }

        private void audioTracksMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                mplayer.SetAudioTrack(index + 1);
            }
        }

        private void SetAudioTracks()
        {
            audioTracksToolStripMenuItem.DropDownItems.Clear();

            foreach (AudioTrack track in Info.AudioTracks)
            {
                string text;

                if (string.IsNullOrEmpty(track.Name) && string.IsNullOrEmpty(track.Lang))
                    text = string.Format("{0}: [ main ]", track.ID);
                else
                    text = string.Format("{0}: {1} ({2})", track.ID, track.Name, track.Lang);

                var item = new ToolStripMenuItem(text, null, audioTracksMenuItem_Click);
                audioTracksToolStripMenuItem.DropDownItems.Add(item);
            }

            if (Info.AudioTracks.Count < 2)
                audioTracksToolStripMenuItem.DropDownItems[0].Enabled = false;
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

            if (Info.Chapters.Count.Equals(0))
            {
                var item = new ToolStripMenuItem("[ none ]", null);
                item.Enabled = false;
                chaptersToolStripMenuItem.DropDownItems.Add(item);
                return;
            }

            for (int i = 0; i < Info.Chapters.Count; i++)
            {
                var text = string.Format("{0}: {1}", i+1, Info.Chapters[i].ChapterName);
                var item = new ToolStripMenuItem(text, null, chaptersMenuItem_Click);
                if (i < 9)
                    item.ShortcutKeys = Keys.Control | KeysClass.GetNumKey(i+1);
                chaptersToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void increaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Info.Current.Volume >= 95)
                SetVolume(100);
            else
                SetVolume(Info.Current.Volume + 5);
        }

        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Info.Current.Volume <= 5)
                SetVolume(0);
            else
                SetVolume(Info.Current.Volume - 5);
        }

        private void volumeToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var newVol = Functions.TryParse.ParseInt(volumeToolStripTextBox.Text);

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (volumeToolStripTextBox.Text.ToLower().Equals("mute") || newVol.Equals(0))
                    {
                        SetVolume(0);
                        optionsToolStripMenuItem.HideDropDown();
                    }
                    else if (newVol > 0 && newVol <= 100)
                    {
                        SetVolume(newVol);
                        optionsToolStripMenuItem.HideDropDown();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a value that is between 1 - 100.",
                            "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        SetVolume(Info.Current.Volume);
                    }
                    break;
                case Keys.Up:
                    if (newVol >= 0 && newVol < 100)
                        volumeToolStripTextBox.Text = (newVol + 1).ToString(CultureInfo.InvariantCulture);
                    break;
                case Keys.Down:
                    if (newVol > 0 && newVol <= 100)
                        volumeToolStripTextBox.Text = (newVol - 1).ToString(CultureInfo.InvariantCulture);
                    break;
            }
        }

        #endregion
        #region Subtitles

        private void showSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.ShowSubs(showSubtitlesToolStripMenuItem.Checked);
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // increase size
            mplayer.SendCommand("add sub-scale 0.2");
        }

        private void sizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // decrease size
            mplayer.SendCommand("add sub-scale -0.2");
        }

        private void resetSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mplayer.SendCommand("set sub-scale 1");
        }

        private void subtitleMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                mplayer.SetSubs(index + 1);
            }
        }

        private void SetSubs()
        {
            subtitleTrackToolStripMenuItem.DropDownItems.Clear();

            if (Info.Subs.Count > 0)
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

            foreach (Sub sub in Info.Subs)
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
            ShowConsole = !ShowConsole;
        }

        private void takeSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // hide distractions
            takeSnapshotToolStripMenuItem.HideDropDown();
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
            
            var snapshotForm = new SnapshotForm(bmpSnapshot);
            snapshotForm.ShowDialog(this);
            snapshotForm.Dispose();
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
            ShowPlaylist = !ShowPlaylist;
        }

        private void hideAlbumArtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideAlbumArt = !HideAlbumArt;
        }

        private void dimLightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (blackForm == null)
            {
                blackForm = new BlackForm(this);
                blackForm.RefreshTitle();
            }

            if (dimLightsToolStripMenuItem.Checked)
            {
                blackForm.Show();
                this.TopLevel = true;
            }
            else
            {
                blackForm.Hide();
            }
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
                settings.SetConfig(true, SettingEnum.ShowIcon);
                trayIcon.Visible = true;
            }
            else
            {
                settings.SetConfig(false, SettingEnum.ShowIcon);
                trayIcon.Visible = false;
            }
            settings.SaveConfig();
        }

        private void hidePopupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.SetConfig(hidePopupToolStripMenuItem.Checked, SettingEnum.HidePopup);
            settings.SaveConfig();
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
            aboutForm.Dispose();
        }

        #endregion

        #region MainForm Code

        public MainForm()
        {
            // set keyboard hook
            this.myCallbackDelegate = this.callbackFunction_KeyboardHook;
            hHook = SetWindowsHookEx(WH.KEYBOARD, this.myCallbackDelegate, IntPtr.Zero, AppDomain.GetCurrentThreadId());

            // message filter for mouse events
            var mouseHandler = new GlobalMouseHandler();
            mouseHandler.MouseMoved += mouseHandler_MouseMoved;
            mouseHandler.XButtonDown += mouseHandler_XButtonDown;
            Application.AddMessageFilter(mouseHandler);

            InitializeComponent();

            mplayer = new MPlayer(mplayerPanel.Handle.ToInt32());
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // check for mpv.exe
            if (!File.Exists(Application.StartupPath + "\\mpv.exe"))
            {
                MessageBox.Show("Baka MPlayer cannot load without mpv.exe!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }

            this.SuspendLayout(); // -- begin layout changes

            RegisterMPlayerEvents();
            playlist.Init(this, mplayer);
            trayIcon.ContextMenu = trayContextMenu;
            this.MouseWheel += MainForm_MouseWheel;
            this.MinimumSize = new Size(this.Width, this.Height - this.ClientSize.Height
                + mainMenuStrip.Height + seekPanel.Height + controlPanel.Height);
            folderToolStripMenuItem.Text = "Build " + Application.ProductVersion;

            SetLCDFont(); // Embbeding Font (LCD.ttf)
            ShowConsole = false;
            ShowPlaylist = false;

            LoadSettings();

            this.ResumeLayout(); // -- end layout changes

            // check for updates
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var week = dfi.Calendar.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            var lastCheck = settings.GetIntValue(SettingEnum.LastUpdateCheck);
            if (week != lastCheck)
            {
                var checker = new UpdateChecker();
                checker.Check(true);

                settings.SetConfig(week, SettingEnum.LastUpdateCheck);
                settings.SaveConfig();
            }

            // check for command line args
            var arg = Environment.GetCommandLineArgs();
            for (int i = 1; i < arg.Length; i++)
            {
                if (arg[i].Equals("-lastfile", StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(settings.GetStringValue(SettingEnum.LastFile)))
                        mplayer.OpenFile(settings.GetStringValue(SettingEnum.LastFile));
                    else
                        MessageBox.Show("Either there is no previous file or the previous file does not exist anymore.",
                            "No Previous File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }
                if (File.Exists(arg[i]))
                {
                    // Note: opening only one file is supported for now
                    mplayer.OpenFile(arg[i]);
                    break;
                }
            }
        }
        private void RegisterMPlayerEvents()
        {
            mplayer.StdOutEvent += mplayer_StdOutEvent;
            mplayer.StatusChangedEvent += mplayer_StatusChangedEvent;
            mplayer.FileOpenedEvent += mplayer_FileOpenedEvent;
            mplayer.PlayStateChangedEvent += mplayer_PlayStateChangedEvent;
            mplayer.DurationChangedEvent += mplayer_DurationChangedEvent;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Functions.OS.RunningOnWin7)
                SetThumbnailToolbars();
        }
        private void LoadSettings()
        {
            // load volume
            SetVolume(settings.GetIntValue(SettingEnum.Volume));

            // tray icon
            if (!settings.GetBoolValue(SettingEnum.ShowIcon))
            {
                trayIcon.Visible = false;
                showIconInTrayToolStripMenuItem.Checked = false;
            }
            hidePopupToolStripMenuItem.Checked = settings.GetBoolValue(SettingEnum.HidePopup);

            // set previous file
            var lastFile = settings.GetStringValue(SettingEnum.LastFile);
            if (File.Exists(lastFile))
            {
                openLastFileToolStripMenuItem.ToolTipText = Path.GetFileName(lastFile);
                openLastFileToolStripMenuItem.Enabled = true;
            }
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space: // play or pause
                    if (NotFocusedOnTextbox)
                        mplayer.Pause(true);
                    break;
                case Keys.Escape:
                    if (FullScreen)
                        FullScreen = false;
                    else
                        HidePlayer();
                    break;
            }
        }
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                // scroll up (increase volume)
                if (Info.Current.Volume >= 95)
                    SetVolume(100);
                else
                    SetVolume(Info.Current.Volume + 5);
            }
            else
            {
                // scroll down (decrease volume)
                if (Info.Current.Volume <= 5)
                    SetVolume(0);
                else
                    SetVolume(Info.Current.Volume - 5);
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mplayer.MPlayerIsRunning())
                e.Cancel = true;
            else
                return;

            // save LastFile
            if (!string.IsNullOrEmpty(Info.URL))
                settings.SetConfig(Info.URL, SettingEnum.LastFile);
            settings.SaveConfig();

            UnhookWindowsHookEx(hHook);
            UnloadTray();

            // This function will kill the mplayer process, which in turn will
            // signal the Application to exit thus causing a one time recursion
            mplayer.Kill();
        }

        #endregion
        #region MPlayer Events

        private void mplayer_StdOutEvent(object sender, StdOutEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.StdOut.Equals("[Baka_MPlayer] CLEAR_OUTPUT", StringComparison.Ordinal))
                {
                    outputTextbox.Clear();
                    return;
                }

                outputTextbox.AppendText("\n" + e.StdOut);
                // auto scroll to end
                outputTextbox.SelectionStart = outputTextbox.TextLength;
                outputTextbox.ScrollToCaret();
            });
        }

        private void mplayer_StatusChangedEvent(object sender, StatusChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.Status.Equals("[Baka_MPlayer] HIDE_STATUS_LABEL", StringComparison.Ordinal))
                {
                    HideStatusLabel();
                    return;
                }

                SetStatusMsg(e.Status, e.AutoHide);
            });
        }
        private void SetStatusMsg(string message, bool autoHide)
        {
            statusLabel.Text = message;
            statusLabel.Show();

            if (autoHide)
                statusTimer.Enabled = true;
        }

        private void mplayer_FileOpenedEvent(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                SetControls(true, false);

                // lose focus from other controls (e.g. playlist)
                seekBar.Focus();

                // save last file information
                if (firstFile)
                {
                    firstFile = false;
                    settings.SetConfig(Info.URL, SettingEnum.LastFile);
                }
                else
                {
                    settings.SetConfig(tempURL, SettingEnum.LastFile);
                    openLastFileToolStripMenuItem.ToolTipText = Path.GetFileName(tempURL);
                    openLastFileToolStripMenuItem.Enabled = true;
                }
                settings.SaveConfig();
                tempURL = Info.URL;

                // set form's info

                this.Text = Info.IsOnline ? Info.MovieName : Info.FullFileName;

                folderToolStripMenuItem.Text = Info.IsOnline ?
                    new Uri(Info.URL).Host : Functions.String.AutoEllipsis(32, Functions.IO.GetFolderName(Info.URL));

                if (blackForm != null)
                    blackForm.RefreshTitle();

                if (infoForm != null && !infoForm.IsDisposed)
                    infoForm.RefreshInfo();

                // set menu strips

                showInWindowsExplorerToolStripMenuItem.Enabled = !Info.IsOnline;

                if (Info.VideoInfo.HasVideo)
                {
                    // video file
                    albumArtPicbox.Visible = false;
                    mplayerPanel.Visible = true;

                    frameStepToolStripMenuItem.Enabled = true;
                    frameBackStepToolStripMenuItem.Enabled = true;
                    HideAlbumArt = false;
                    hideAlbumArtToolStripMenuItem.Enabled = false;
                    takeSnapshotToolStripMenuItem.Enabled = true;

                    for (var i = 0; i < aspectRatioToolStripMenuItem.DropDownItems.Count; i++)
                    {
                        aspectRatioToolStripMenuItem.DropDownItems[i].Enabled = true;
                    }
                }
                else
                {
                    // music file
                    mplayerPanel.Visible = false;
                    albumArtPicbox.Visible = true;

                    frameStepToolStripMenuItem.Enabled = false;
                    frameBackStepToolStripMenuItem.Enabled = false;
                    hideAlbumArtToolStripMenuItem.Enabled = true;
                    takeSnapshotToolStripMenuItem.Enabled = false;

                    // show album art (if it exists);
                    albumArtPicbox.Image = Info.ID3Tags.AlbumArtTag.AlbumArt ?? Properties.Resources.Music_128;
                    albumArtPicbox_SizeChanged(null, null);

                    for (var i = 0; i < aspectRatioToolStripMenuItem.DropDownItems.Count; i++)
                    {
                        aspectRatioToolStripMenuItem.DropDownItems[i].Enabled = false;
                    }
                }
                ResizeMplayerPanel();

                if (Info.Chapters.Count > 0)
                {
                    previousChapterToolStripMenuItem.Enabled = true;
                    nextChapterToolStripMenuItem.Enabled = true;
                }
                else
                {
                    previousChapterToolStripMenuItem.Enabled = false;
                    nextChapterToolStripMenuItem.Enabled = false;
                }

                // call other methods
                playlist.RefreshPlaylist(false);
                SetChapterMarks();
                SetSystemTray();

                // create menu items
                SetAudioTracks();
                SetChapters();
                SetSubs();

                mplayer.Play();
            });
        }

        private void mplayer_PlayStateChangedEvent(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                setOnTop();
                SetPlayState(Info.Current.PlayState);
            });
        }
        private void SetPlayState(PlayStates newPlayState)
        {
            switch (newPlayState)
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
                case PlayStates.Ended:
                    MediaEnded();
                    break;
            }
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
                if (playlist.GetPlayingItem.Index.Equals(playlist.GetTotalItems - 1))
                {
                    // end of playlist, repeat from beginning of playlist
                    playlist.PlayFile(0);
                }
                else
                {
                    // next file
                    playlist.PlayNext();
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
                if (playlist.GetPlayingItem.Index < playlist.GetTotalItems - 1)
                    playlist.PlayNext();
                else
                    LastFile();
            }
        }
        private void LastFile()
        {
            SetControls(false, true);
            SetPlayState(PlayStates.Stopped);
            SetStatusMsg("Reached the end", true);
        }

        private void mplayer_DurationChangedEvent(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                UpdateNowPlayingInfo();

                if (seekBar_IsMouseDown)
                    return;

                if (Info.Current.TotalLength > 0.0)
                {
                    seekBar.Value = Convert.ToInt32((Info.Current.Duration * seekBar.Maximum) / Info.Current.TotalLength); // %
                    durationLabel.Text = Functions.Time.ConvertTimeFromSeconds(Info.Current.Duration);
                }
                
                if (settings.GetBoolValue(SettingEnum.ShowTimeRemaining))
                    timeLeftLabel.Text = string.Format("-{0}", Functions.Time.ConvertTimeFromSeconds(Info.Current.TotalLength - Info.Current.Duration));
                else
                    timeLeftLabel.Text = Functions.Time.ConvertTimeFromSeconds(Info.Current.TotalLength);
            });
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM.WINDOWPOSCHANGING:
                    // snap to desktop border
                    SnapToDesktopBorder(m.LParam);
                    break;
                case WM.SYSCOMMAND:
                    if (m.WParam.ToInt32() == SC.MINIMIZE)
                    {
                        // this.minimize event
                        if (blackForm != null)
                            blackForm.Hide();
                        dimLightsToolStripMenuItem.Checked = false;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void OpenFile()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = string.Format("Multimedia|{0}|Video Files|{1}|Audio Files|{2}|All Files (*.*)|*.*",
                Properties.Resources.VideoFiles + "; " + Properties.Resources.AudioFiles,
                Properties.Resources.VideoFiles, Properties.Resources.AudioFiles);

            if (Info.IsOnline)
                ofd.FileName = string.Empty;
            else
            {
                ofd.InitialDirectory = Info.GetDirectoryName;
                ofd.FileName = Info.FullFileName;
            }

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
                mplayer.OpenFile(ofd.FileName);
        }

        private void HidePlayer()
        {
            if (seekBar_IsMouseDown)
            { // stop mouse movement
                seekBar_IsMouseDown = false;
                return;
            }
            
            if (mplayer.MPlayerIsRunning())
                mplayer.Pause(false);

            if (FullScreen)
                FullScreen = false;
            if (blackForm != null)
                blackForm.Hide();
            dimLightsToolStripMenuItem.Checked = false;

            // hack required to bypass Windows' hide animation
            this.Opacity = 0.0;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Opacity = 1.0;
        }

        private void SetControls(bool enable, bool lastFile)
        {
            seekBar.Enabled = enable;

            stopToolStripMenuItem.Enabled = enable;
            jumpToTimeToolStripMenuItem.Enabled = enable;

            fullScreenToolStripMenuItem.Enabled = enable;
            fitToVideoToolStripMenuItem.Enabled = enable;
            sayMediaNameToolStripMenuItem.Enabled = enable;
            mediaInfoToolStripMenuItem.Enabled = enable;

            stopMenuItem.Enabled = enable;

            if (!lastFile)
            {
                // control panel controls
                rewindButton.Enabled = enable;
                playButton.Enabled = enable;
                playlistButton.Enabled = enable;

                // menu strips
                playToolStripMenuItem.Enabled = enable;
                rewindToolStripMenuItem.Enabled = enable;
                restartToolStripMenuItem.Enabled = enable;

                // tray context menu
                showMenuItem.Enabled = enable;
                playMenuItem.Enabled = enable;
                rewindMenuItem.Enabled = enable;

                // thumbnail toolbar button
                playToolButton.Enabled = enable;

                showPlaylistToolStripMenuItem.Enabled = enable;
                folderToolStripMenuItem.Enabled = enable;
            }
        }

        private void SetChapterMarks()
        {
            // set chapter marks on seekBar
            if (Info.Chapters.Count != 0)
            {
                var marks = new List<long>();

                foreach (var c in Info.Chapters)
                    marks.Add(c.StartTime / 1000);

                seekBar.AddMarks(marks, Info.Current.TotalLength);
            }
            else
            {
                seekBar.ClearMarks();
            }
        }

        private void mouseHandler_XButtonDown(MouseButtons button)
        {
            if (!mplayer.MPlayerIsRunning())
                return;

            switch (button)
            {
                case MouseButtons.XButton1: // seek backwards
                    if (Info.Current.Duration - 5 > -1)
                        mplayer.Seek(Info.Current.Duration - 5);
                    else //if (mplayer.currentPosition < 5)
                        mplayer.Seek(0);
                    break;
                case MouseButtons.XButton2: // seek forwards
                    if (Info.Current.Duration + 5 < Info.Current.TotalLength)
                        mplayer.Seek(Info.Current.Duration + 5);
                    else
                        playlist.PlayNext();
                    break;
            }
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

        private void mplayerSplitContainer_Panel1_SizeChanged(object sender, EventArgs e)
        {
            ResizeMplayerPanel();
        }

        private void ResizeMplayerPanel()
        {
            if (string.IsNullOrEmpty(Info.URL) && !Info.VideoInfo.HasVideo)
                return;

            var currentRatio = (double)mplayerSplitContainer.Panel1.Width / mplayerSplitContainer.Panel1.Height;
            var ratio = Info.VideoInfo.AspectRatio;

            if (ratio.Equals(0.0))
                ratio = (double)Info.VideoInfo.Width / Info.VideoInfo.Height;

            int width, height;

            if (currentRatio < ratio)
            {
                width = mplayerSplitContainer.Panel1.Width;
                height = (int)(width / ratio);
            }
            else
            {
                height = mplayerSplitContainer.Panel1.Height;
                width = (int)(height * ratio);
            }

            int x = (mplayerSplitContainer.Panel1.Width - width) / 2;
            int y = (mplayerSplitContainer.Panel1.Height - height) / 2;

            mplayerPanel.SetBounds(x, y, width, height);
        }

        private void UpdateNowPlayingInfo()
        {
            switch(Info.Current.PlayState)
            {
                case PlayStates.Playing:
                    nowPlayingMenuItem.Text = string.Format("Now Playing ({0})", Functions.Time.ConvertTimeFromSeconds(Info.Current.Duration));
                    break;
                case PlayStates.Paused:
                    nowPlayingMenuItem.Text = string.Format("Paused ({0})", Functions.Time.ConvertTimeFromSeconds(Info.Current.Duration));
                    break;
                case PlayStates.Stopped:
                    nowPlayingMenuItem.Text = "Stopped";
                    break;
                default:
                    nowPlayingMenuItem.Text = "Now Playing (00:00:00)";
                    break;
            }
        }

        private void SetVolume(int newVol)
        {
            if (newVol < 0 || newVol > 100)
                return;

            if (mplayer.MPlayerIsRunning())
                mplayer.SetVolume(newVol);
            Info.Current.Volume = newVol;
            settings.SetConfig(newVol, SettingEnum.Volume);

            if (newVol.Equals(0))
            { // mute
                if (mplayer.MPlayerIsRunning())
                    mplayer.Mute(true);
                volumeToolStripTextBox.Text = "Mute";

                volumeBar.ThumbSecondColor = Color.DimGray;
                volumeBar.ThumbFirstColor = Color.DarkGray;
                volumeBar.ThumbBorderColor = Color.DimGray;
                volumeBar.Value = 0;
            }
            else
            { // not mute
                if (mplayer.MPlayerIsRunning())
                    mplayer.Mute(false);
                volumeToolStripTextBox.Text = newVol.ToString(CultureInfo.InvariantCulture);

                volumeBar.ThumbSecondColor = Color.DarkGray;
                volumeBar.ThumbFirstColor = Color.Silver;
                volumeBar.ThumbBorderColor = Color.Gray;
                volumeBar.Value = newVol;
            }
        }

        public void CallSetBackForwardControls()
        {
            Invoke((MethodInvoker)SetBackForwardControls);
        }
        private void SetBackForwardControls()
        {
            // previous buttons
            if (playlist.GetPlayingItem.Index > 0)
            {
                previousButton.Enabled = true;
                playPreviousFileToolStripMenuItem.Enabled = true;
                previousToolButton.Enabled = true;
                previousMenuItem.Enabled = true;
            }
            else
            {
                previousButton.Enabled = false;
                playPreviousFileToolStripMenuItem.Enabled = false;
                previousToolButton.Enabled = false;
                previousMenuItem.Enabled = false;
            }

            // next buttons
            if (playlist.GetPlayingItem.Index < playlist.GetTotalItems - 1)
            {
                nextButton.Enabled = true;
                playNextFileToolStripMenuItem.Enabled = true;
                nextToolButton.Enabled = true;
                nextMenuItem.Enabled = true;
            }
            else
            {
                nextButton.Enabled = false;
                playNextFileToolStripMenuItem.Enabled = false;
                nextToolButton.Enabled = false;
                nextMenuItem.Enabled = false;
            }

            previousButton.Refresh();
            nextButton.Refresh();
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

        private void inputTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            var input = inputTextbox.Text.Trim().ToUpperInvariant();
            if (input.Length > 0)
            {
                switch (input)
                {
                    case "CLEAR":
                        outputTextbox.Clear();
                        break;
                    case "EXIT":
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

        private void mplayerPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(Info.URL) && e.Button == MouseButtons.Right)
                mplayer.Pause(true);
        }

        private void mplayerPanel_MouseDoubleClickFixed(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                FullScreen = !FullScreen;
        }

        private void mplayerSplitContainer_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(Info.URL) && e.Button == MouseButtons.Right)
                mplayer.Pause(true);
        }

        private void albumArtPicbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(Info.URL) && e.Button == MouseButtons.Right)
                mplayer.Pause(true);
        }

        private void bodySplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // revert focus to overall program
            seekBar.Focus();
        }

        private void mplayerSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // revert focus to overall program
            seekBar.Focus();
        }
    }
}
