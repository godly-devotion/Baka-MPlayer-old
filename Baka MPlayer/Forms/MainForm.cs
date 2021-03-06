﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Baka_MPlayer.GlobalKeyHook;
using Baka_MPlayer.Updates;
using Microsoft.WindowsAPICodePack.Taskbar;
using MPlayer;
using MPlayer.Info;
using MPlayer.Info.Track;
using Win32;

namespace Baka_MPlayer.Forms
{
    public partial class MainForm : Form
    {
        #region Private Fields

        private BlackForm blackForm;
        private InfoForm infoForm;

        private readonly IMPlayer mp;
        private KeyHookManager globalKeyHook;
        private VoiceCommandEngine voice;
        private Speech speech;

        // Win 7 thumbnail toolbar buttons
        private ThumbnailToolBarButton previousToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_previous, "Previous file");
        private ThumbnailToolBarButton playToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_play, "Play");
        private ThumbnailToolBarButton nextToolButton =
            new ThumbnailToolBarButton(Properties.Resources.tool_next, "Next file");

        // lastFile feature
        private bool firstFile = true;
        private string previousUrl = string.Empty;

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
            mp.Pause(true);
        }

        private void stopMenuItem_Click(object sender, EventArgs e)
        {
            mp.Stop();
        }

        private void rewindMenuItem_Click(object sender, EventArgs e)
        {
            mp.Rewind();
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
            if (e.Button != MouseButtons.Left || string.IsNullOrEmpty(mp.FileInfo.Url))
                return;

            mp.Pause(true);

            switch (mp.CurrentStatus.PlayState)
            {
                case PlayStates.Playing:
                    trayIcon.ShowBalloonTip(4000, "Paused", this.Text, ToolTipIcon.None);
                    break;
                case PlayStates.Paused:
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
            if (mp.FileInfo.IsOnline)
            {
                titleMenuItem.Text = string.Format("  {0}", Functions.String.AutoEllipsis(25, mp.FileInfo.MovieName));
                artistMenuItem.Text = "  Online Media";
                SetNotifyIconText(string.Format("{0}\n{1}", mp.FileInfo.MovieName, "Online Media"));
                return;
            }

            var lastPart = string.Format("(File {0} of {1})", playlist.GetPlayingItem.Index + 1, playlist.GetTotalItems);

            if (!string.IsNullOrEmpty(mp.FileInfo.Id3Tags.Title))
            {
                bool hasArtist = !string.IsNullOrEmpty(mp.FileInfo.Id3Tags.Artist);

                titleMenuItem.Text = string.Format("  {0}", Functions.String.AutoEllipsis(25, mp.FileInfo.Id3Tags.Title));
                artistMenuItem.Text = string.Format("  {0}",
                    hasArtist ? Functions.String.AutoEllipsis(25, mp.FileInfo.Id3Tags.Artist) : "Unknown Artist");

                SetNotifyIconText(string.Format("{0}\n{1}{2}", mp.FileInfo.Id3Tags.Title, hasArtist ? mp.FileInfo.Id3Tags.Artist + "\n" : "", lastPart));

                if (!mp.FileInfo.HasVideo && !hidePopupToolStripMenuItem.Checked)
                    trayIcon.ShowBalloonTip(4000, mp.FileInfo.Id3Tags.Title, (hasArtist ? mp.FileInfo.Id3Tags.Artist + "\n" : "") + lastPart, ToolTipIcon.None);

            }
            else
            {
                // no title & artist (no artist assumed)
                var fileName = Functions.String.AutoEllipsis(25, mp.FileInfo.MovieName);

                titleMenuItem.Text = string.Format("  {0}", fileName);
                artistMenuItem.Text = "  Unknown Artist";

                SetNotifyIconText(string.Format("{0}\n{1}", fileName, lastPart));

                if (!mp.FileInfo.HasVideo && !hidePopupToolStripMenuItem.Checked)
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
            var wpNewPosition = (WINDOWPOS)Marshal.PtrToStructure(LParam, typeof(WINDOWPOS));

            if ((wpNewPosition.flags & SWP.NoSize) == 0 || (wpNewPosition.flags & SWP.NoMove) == 0)
            {
                Rectangle rWorking = Screen.FromControl(this).WorkingArea;
                bool changed = false;

                if (Math.Abs(wpNewPosition.x - rWorking.X) <= SnapOffset)
                {
                    wpNewPosition.x = rWorking.X;
                    changed = true;
                }
                else if (Math.Abs(wpNewPosition.x + wpNewPosition.cx - rWorking.Right) <= SnapOffset)
                {
                    wpNewPosition.x = rWorking.Right - wpNewPosition.cx;
                    changed = true;
                }

                if (Math.Abs(wpNewPosition.y - rWorking.Y) <= SnapOffset)
                {
                    wpNewPosition.y = rWorking.Y;
                    changed = true;
                }
                else if (Math.Abs(wpNewPosition.y + wpNewPosition.cy - rWorking.Bottom) <= SnapOffset)
                {
                    wpNewPosition.y = rWorking.Bottom - wpNewPosition.cy;
                    changed = true;
                }

                // Marshal it back
                if (changed) Marshal.StructureToPtr(wpNewPosition, LParam, true);
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
            var fontFile = Path.Combine(Application.StartupPath, "LCD.ttf");

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

        private void KeyboardHook(object sender, KeyCodeEventArgs e)
        {
            if (!NotFocusedOnTextbox) return;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    if ((ModifierKeys & Keys.Shift) != 0) return;
                    if (mp.CurrentStatus.Duration - 5 > -1)
                        mp.Seek(mp.CurrentStatus.Duration - 5);
                    else //if (mplayer.currentPosition < 5)
                        mp.Seek(0);
                    break;
                case Keys.Right:
                    if ((ModifierKeys & Keys.Shift) != 0) return;
                    if (mp.CurrentStatus.Duration + 5 < mp.CurrentStatus.TotalLength)
                        mp.Seek(mp.CurrentStatus.Duration + 5);
                    else
                        playlist.PlayNext();
                    break;
                case Keys.PageUp:
                    mp.PreviousChapter();
                    break;
                case Keys.PageDown:
                    mp.NextChapter();
                    break;
                case Keys.MediaNextTrack:
                    playlist.PlayNext();
                    break;
                case Keys.MediaPreviousTrack:
                    playlist.PlayPrevious();
                    break;
                case Keys.MediaStop:
                    mp.Stop();
                    break;
                case Keys.MediaPlayPause:
                    switch (mp.CurrentStatus.PlayState)
                    {
                        case PlayStates.Playing:
                            mp.Pause(false);
                            break;
                        case PlayStates.Paused:
                        case PlayStates.Stopped:
                            mp.Pause(true);
                            break;
                    }
                    break;
            }
        }

        #endregion
        #region File Drag & Drop

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var fileDirs = (string[])e.Data.GetData(DataFormats.FileDrop);
            string fileDir = fileDirs.GetValue(0).ToString();

            if (File.Exists(fileDir))
                mp.OpenFile(fileDir);
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
            mp.Pause(true);
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
            get { return !(string.IsNullOrEmpty(mp.FileInfo.Url) || playlist.searchTextBox.Focused || inputTextbox.Focused); }
        }

        #endregion
        #region Voice Command

        private bool IsVoiceCommandEnabled;

        private void EnableVoiceCommand()
        {
            try
            {
                if (IsVoiceCommandEnabled) return;

                // if call name isn't set, ask for one
                if (string.IsNullOrEmpty(Properties.Settings.Default.CallName.Trim()))
                {
                    var voiceForm = new VoiceForm();
                    if (voiceForm.ShowDialog() == DialogResult.OK)
                    {
                        if (voiceForm.GetName != Properties.Settings.Default.CallName)
                        {
                            Properties.Settings.Default.CallName = voiceForm.GetName;
                            Properties.Settings.Default.Save();

                            // if voice command is running, restart it
                            if (voice != null)
                            {
                                IsVoiceCommandEnabled = false;
                                voice.SpeechRecognizedEvent += voice_SpeechRecognizedEvent;
                                voice.AudioLevelUpdatedEvent += voice_AudioLevelUpdatedEvent;
                                voice.Dispose();
                                voice = null;
                            }
                            SetStatusMsg("Voice Command: Name changed to " + Properties.Settings.Default.CallName, true);
                        }
                    }
                    voiceForm.Dispose();
                }

                if (voice == null)
                {
                    voice = new VoiceCommandEngine(Properties.Settings.Default.CallName);
                    voice.SpeechRecognizedEvent += voice_SpeechRecognizedEvent;
                    voice.AudioLevelUpdatedEvent += voice_AudioLevelUpdatedEvent;
                }
                voice.StartListening();
                speechButton.Image = Properties.Resources.enabled_mic;
                IsVoiceCommandEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "There was a problem while starting voice command.\nPlease make sure your mic is plugged in.",
                    "Voice Command", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                speechButton.Image = Properties.Resources.disabled_mic;

                if (voice != null)
                    voice.StopListening();
                IsVoiceCommandEnabled = false;
            }
        }
        private void DisableVoiceCommand()
        {
            if (!IsVoiceCommandEnabled) return;

            voice.StopListening();
            speechButton.Image = Properties.Resources.disabled_mic;
            IsVoiceCommandEnabled = false;
        }

        private void voice_SpeechRecognizedEvent(object sender, VoiceCommandEvents.SpeechRecognizedEventArgs e)
        {
            SetStatusMsg("Voice Command: " + Functions.String.ToTitleCase(e.Command), true);

            switch (e.Command)
            {
                case "open":
                case "open file":
                    OpenFile();
                    break;
                case "mute":
                    mp.Mute(true);
                    break;
                case "unmute":
                    mp.Mute(false);
                    break;
                case "increase volume":
                case "raise volume":
                case "volume up":
                    if (mp.Volume >= 90)
                        SetVolume(100);
                    else
                        SetVolume(mp.Volume + 10);
                    break;
                case "decrease volume":
                case "lower volume":
                case "volume down":
                    if (mp.Volume <= 10)
                        SetVolume(0);
                    else
                        SetVolume(mp.Volume - 10);
                    break;
                case "hide":
                    HidePlayer();
                    break;
                case "show":
                    this.WindowState = FormWindowState.Normal;
                    this.Focus();
                    break;
                case "help":
                    voiceCommandHelpToolStripMenuItem_Click(this, EventArgs.Empty);
                    break;
                case "stop listening":
                    DisableVoiceCommand();
                    break;
                case "close":
                    Application.Exit();
                    break;
            }

            // check if a file is opened before allowing playback commands
            if (!mp.PlayerIsRunning() || string.IsNullOrEmpty(mp.FileInfo.Url))
                return;

            switch (e.Command)
            {
                case "play":
                    mp.Play();
                    break;
                case "pause":
                    mp.Pause(false);
                    break;
                case "rewind":
                    mp.Rewind();
                    break;
                case "stop":
                    mp.Stop();
                    break;
                case "next chapter":
                case "skip chapter":
                    mp.NextChapter();
                    break;
                case "previous chapter":
                    mp.PreviousChapter();
                    break;
                case "next":
                case "next file":
                    playlist.PlayNext();
                    break;
                case "previous":
                case "previous file":
                    playlist.PlayPrevious();
                    break;
                case "fullscreen":
                case "view fullscreen":
                case "go fullscreen":
                    FullScreen = true;
                    break;
                case "exit fullscreen":
                case "leave fullscreen":
                    FullScreen = false;
                    break;
                case "whats playing":
                    if (speech == null)
                        speech = new Speech(mp.FileInfo);
                    speech.SayMedia();
                    break;
            }
        }

        private void voice_AudioLevelUpdatedEvent(object sender, VoiceCommandEvents.AudioLevelUpdatedEventArgs e)
        {
            audioLevelBar.Value = e.AudioLevel;
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

            var currentPos = seekBar.Value * mp.CurrentStatus.TotalLength / seekBar.Maximum;

            if (Properties.Settings.Default.ShowTimeRemaining)
                timeLeftLabel.Text = string.Format("-{0}", Functions.Time.ConvertSecondsToTime(mp.CurrentStatus.TotalLength - currentPos));
            else
                timeLeftLabel.Text = Functions.Time.ConvertSecondsToTime(mp.CurrentStatus.TotalLength);

            durationLabel.Text = Functions.Time.ConvertSecondsToTime(currentPos);
        }

        private void seekBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (seekBar_IsMouseDown)
            {
                mp.SeekPercent((double)seekBar.Value / seekBar.Maximum * 100.0);
                seekBar_IsMouseDown = false;
            }
        }

        private void timeLeftLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(mp.FileInfo.Url))
                return;

            var value = !Properties.Settings.Default.ShowTimeRemaining;
            Properties.Settings.Default.ShowTimeRemaining = value;
            Properties.Settings.Default.Save();
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
                    if (!string.IsNullOrEmpty(mp.FileInfo.Url))
                    {
                        var jumpForm = new JumpForm(mp.CurrentStatus);
                        if (jumpForm.ShowDialog(this) == DialogResult.OK)
                        {
                            mp.Seek(jumpForm.GetNewTime);
                            jumpForm.Dispose();
                        }
                    }
                    break;
                case MouseButtons.Right:
                    var webForm = new UrlForm(mp.FileInfo);
                    if (webForm.ShowDialog(this) == DialogResult.OK)
                    {
                        mp.OpenFile(webForm.Url);
                        webForm.Dispose();
                    }
                    break;
            }
        }

        // Voice Command Button
        private void speechButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (IsVoiceCommandEnabled)
                DisableVoiceCommand();
            else
                EnableVoiceCommand();
        }
        private void speechButton_MouseDown(object sender, MouseEventArgs e)
        {
            speechButton.Image = Properties.Resources.down_mic;
        }
        private void speechButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsVoiceCommandEnabled)
                speechButton.Image = Properties.Resources.enabled_mic;
            else
                speechButton.Image = Properties.Resources.disabled_mic;
        }

        // RewindButton
        private void rewindButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (mp.CurrentStatus.Duration < 3)
                mp.Stop();
            else switch (mp.CurrentStatus.PlayState)
            {
                case PlayStates.Playing:
                    mp.Rewind();
                    break;
                case PlayStates.Ended:
                    mp.OpenFile(mp.FileInfo.Url);
                    break;
                default:
                    mp.Stop();
                    break;
            }
        }
        private void rewindButton_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mp.Stop();
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

            using (var drawFont = new Font("Segoe UI", 8f))
            {
                var displayNum = playlist.GetPlayingItem.Index.ToString(CultureInfo.InvariantCulture);
                var stringSize = new SizeF(e.Graphics.MeasureString(displayNum, drawFont));
                // the additional values added to the x & y are paddings specific to the font
                var x = ((previousButton.Width - stringSize.Width) / 2) + 3;
                var y = ((previousButton.Height - stringSize.Height) / 2) + 1;
                e.Graphics.DrawString(displayNum, drawFont, new SolidBrush(controlPanel.BackColor), x, y);
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

            using (var drawFont = new Font("Segoe UI", 8f))
            {
                var displayNum = (playlist.GetPlayingItem.Index + 2).ToString(CultureInfo.InvariantCulture);
                var stringSize = new SizeF(e.Graphics.MeasureString(displayNum, drawFont));
                // the additional values added to the x & y are paddings specific to the font
                var x = ((nextButton.Width - stringSize.Width) / 2) - 5;
                var y = ((nextButton.Height - stringSize.Height) / 2) + 1;
                e.Graphics.DrawString(displayNum, drawFont, new SolidBrush(controlPanel.BackColor), x, y);
            }
        }

        // PlayButton
        private void playButton_EnabledChanged(object sender, EventArgs e)
        {
            if (playButton.Enabled)
            {
                if (mp.CurrentStatus.PlayState == PlayStates.Playing)
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
                if (mp.CurrentStatus.PlayState == PlayStates.Playing)
                    playButton.Image = Properties.Resources.down_pause;
                else
                    playButton.Image = Properties.Resources.down_play;
            }
        }
        private void playButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && playButton.Enabled)
            {
                mp.Pause(true);

                if (mp.CurrentStatus.PlayState == PlayStates.Playing)
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

        private void mouseHandler_MouseMoved(Point cursorPos)
        {
            if (!FullScreen) return;

            // convert cursorPos location relative to MainForm
            cursorPos = this.PointToClient(cursorPos);

            if (VO_State.LastCursorPos != cursorPos)
            {
                VO_State.LastCursorPos = cursorPos;

                if (cursorPos.Y > this.Bounds.Height - (seekPanel.Height + controlPanel.Height + 10))
                {
                    cursorTimer.Stop();

                    seekPanel.Show();
                    controlPanel.Show();
                }
                else if (!seekBar_IsMouseDown)
                {
                    // do not hide controls if seeking with mouse

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
                this.dimDesktopToolStripMenuItem.PerformClick();

            Process.Start(Application.ExecutablePath);
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void openURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var webForm = new UrlForm(mp.FileInfo);

            if (webForm.ShowDialog(this) == DialogResult.OK)
                mp.OpenFile(webForm.Url);

            webForm.Dispose();
        }

        private void openLocationFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var clipText = Clipboard.GetText().Replace("\"", string.Empty);

            if (Functions.IO.IsValidPath(clipText))
            {
                mp.OpenFile(clipText);
            }
            else
            {
                MessageBox.Show(string.Format("The location \"{0}\" cannot be opened.", clipText),
                    "Error Opening Location", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openLastFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastFile = Properties.Settings.Default.LastFile;
            if (File.Exists(lastFile))
                mp.OpenFile(lastFile);
        }

        private void showInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mp.FileInfo.IsOnline)
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "explorer.exe",
                        Arguments = string.Format("/select,\"{0}\"", mp.FileInfo.Url),
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
            if (!mp.FileInfo.IsOnline)
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "explorer.exe",
                        Arguments = string.Format("/select,\"{0}\"", mp.FileInfo.Url),
                        WindowStyle = ProcessWindowStyle.Normal
                    }
                };
                process.Start();
            }
            else
            {
                Process.Start(string.Format("http://{0}", new Uri(mp.FileInfo.Url).Host));
            }
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
        #region View

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullScreen = !FullScreen;
        }

        private void takeSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // hide distractions
            takeSnapshotToolStripMenuItem.HideDropDown();
            HideStatusLabel();

            // get which screen the player is on
            //Screen scrn = Screen.FromControl(this);
            var bmpSnapshot = new Bitmap(mplayerPanel.Width, mplayerPanel.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap
            var gfxScreenshot = Graphics.FromImage(bmpSnapshot);

            // Take the screenshot from the upper left corner to the right bottom
            var screenPoint = this.PointToScreen(new Point(mplayerPanel.Location.X, mplayerPanel.Location.Y + mainMenuStrip.Height));
            gfxScreenshot.CopyFromScreen(screenPoint, new Point(0, 0), mplayerPanel.Size, CopyPixelOperation.SourceCopy);

            var snapshotForm = new SnapshotForm(bmpSnapshot, mp.FileInfo);
            snapshotForm.ShowDialog(this);
            snapshotForm.Dispose();
        }

        private void SetFitWindowToolStripMenuItems(bool enable)
        {
            for (var i = 0; i < fitWindowToolStripMenuItem.DropDownItems.Count; i++)
            {
                fitWindowToolStripMenuItem.DropDownItems[i].Enabled = enable;
            }
        }
        private void currentSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FullScreen)
                return;

            var clientTargetSize = new Size()
            {
                Width = mplayerPanel.Width,
                Height = mplayerPanel.Height + mainMenuStrip.Height + seekPanel.Height + controlPanel.Height
            };

            clientTargetSize.Width += ShowPlaylist ?
                bodySplitContainer.Width - bodySplitContainer.SplitterDistance : 0;
            clientTargetSize.Height += ShowConsole ?
                mplayerSplitContainer.Height - mplayerSplitContainer.SplitterDistance : 0;

            this.ClientSize = clientTargetSize;
            SetStatusMsg("Fit Window: To Current Size", true);
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FitWindow(0.5, true);
            SetStatusMsg("Fit Window: 50%", true);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FitWindow(0.75, true);
            SetStatusMsg("Fit Window: 75%", true);
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FitWindow(1, true);
            SetStatusMsg("Fit Window: 100%", true);
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            FitWindow(2, true);
            SetStatusMsg("Fit Window: 200%", true);
        }
        private void FitWindow(double scale, bool considerPlaylist)
        {
            if (FullScreen)
                return;

            int w = Convert.ToInt32(mp.FileInfo.VideoWidth * scale);
            if (considerPlaylist && ShowPlaylist)
                w += bodySplitContainer.Width - bodySplitContainer.SplitterDistance;

            int h = Convert.ToInt32(mp.FileInfo.VideoHeight * scale);
            h += mainMenuStrip.Height + seekPanel.Height + controlPanel.Height;
            if (ShowConsole)
                h += mplayerSplitContainer.Height - mplayerSplitContainer.SplitterDistance;

            // add the caption and border size
            w += this.Width - this.ClientSize.Width;
            h += this.Height - this.ClientSize.Height;

            var scrn = Screen.FromControl(this);
            var workArea = scrn.WorkingArea;

            // fit within the screen's working area
            w = Math.Min(w, workArea.Right - workArea.Left);
            h = Math.Min(h, workArea.Bottom - workArea.Top);

            // center window
            var loc = new Point
            {
                Y = workArea.Y + (workArea.Height - h) / 2
            };

            if (w > this.MinimumSize.Width)
                loc.X = workArea.X + (workArea.Width - w) / 2;
            else
                loc.X = workArea.X + (workArea.Width - this.MinimumSize.Width) / 2;

            this.Location = loc;
            this.Size = new Size(w, h);
        }

        private void SetAspectRatioToolStripMenuItems(bool enable)
        {
            for (var i = 0; i < aspectRatioToolStripMenuItem.DropDownItems.Count; i++)
            {
                aspectRatioToolStripMenuItem.DropDownItems[i].Enabled = enable;
            }
        }
        private void autodetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VO_State.PanelAspectRatio = Math.Round((double)mp.FileInfo.VideoWidth / mp.FileInfo.VideoHeight, 5);
            ResizeMplayerPanel();
        }
        private void force43ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VO_State.PanelAspectRatio = 1.3333;
            ResizeMplayerPanel();
        }
        private void force169ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VO_State.PanelAspectRatio = 1.7778;
            ResizeMplayerPanel();
        }
        private void force2351ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VO_State.PanelAspectRatio = 2.35;
            ResizeMplayerPanel();
        }

        private void showSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.SetSubtitleVisibility(showSubtitlesToolStripMenuItem.Checked);
        }

        private void addSubtitleFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Subtitles|*.sub;*.srt;*.ass;*.ssa|All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
                mp.AddSubtitle(ofd.FileName);

            SetSubToolStripMenuItems();
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // increase size
            mp.SendCommand("add sub-scale 0.2");
        }
        private void sizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // decrease size
            mp.SendCommand("add sub-scale -0.2");
        }
        private void resetSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.SendCommand("set sub-scale 1");
        }
        private void subtitleMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                // (1) audio track index starts from 1
                // (2) two menuitems precede the first audio track menuitem
                mp.SetSubtitleTrack(index - 1);
            }
        }

        private void SetSubToolStripMenuItems()
        {
            subtitleTrackToolStripMenuItem.DropDownItems.Clear();

            // re-add "Add Subtitle File..." menu item
            var addSubItem = new ToolStripMenuItem("&Add Subtitle File...", null, addSubtitleFileToolStripMenuItem_Click);
            subtitleTrackToolStripMenuItem.DropDownItems.Add(addSubItem);
            subtitleTrackToolStripMenuItem.DropDownItems.Add("-");

            if (mp.FileInfo.Subs.Count > 0)
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

                var noItem = new ToolStripMenuItem("[ none ]", null) { Enabled = false };
                subtitleTrackToolStripMenuItem.DropDownItems.Add(noItem);
                return;
            }

            foreach (Subtitle sub in mp.FileInfo.Subs)
            {
                var text = string.Format("{0}: {1} ({2})", sub.TrackID, sub.Name, sub.Lang);
                var subItem = new ToolStripMenuItem(text, null, subtitleMenuItem_Click);
                subtitleTrackToolStripMenuItem.DropDownItems.Add(subItem);
            }
        }

        private void mediaInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (infoForm == null || infoForm.IsDisposed)
                infoForm = new InfoForm(mp.GetProcessId());
            infoForm.UpdateInfo(mp.FileInfo, mp.CurrentStatus.TotalLength);
            infoForm.Show();
        }

        #endregion
        #region Playback

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.Pause(true);
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.Stop();
        }
        private void rewindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.Rewind();
        }
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.Restart();
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

        private void SetAudioTrackMenuItems()
        {
            audioTracksToolStripMenuItem.DropDownItems.Clear();

            foreach (AudioTrack track in mp.FileInfo.AudioTracks)
            {
                string text;

                if (string.IsNullOrEmpty(track.Name) && string.IsNullOrEmpty(track.Lang))
                    text = string.Format("{0}: [ unnamed track ]", track.ID);
                else
                    text = string.Format("{0}: {1} ({2})", track.ID, track.Name, track.Lang);

                var item = new ToolStripMenuItem(text, null, audioTracksMenuItem_Click);
                audioTracksToolStripMenuItem.DropDownItems.Add(item);
            }

            if (mp.FileInfo.AudioTracks.Count.Equals(1))
            {
                audioTracksToolStripMenuItem.DropDownItems[0].Enabled = false;
            }
            else if (mp.FileInfo.AudioTracks.Count.Equals(0))
            {
                var item = new ToolStripMenuItem("[ none ]") { Enabled = false };
                audioTracksToolStripMenuItem.DropDownItems.Add(item);
            }
        }
        private void audioTracksMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                mp.SetAudioTrack(index + 1);
            }
        }

        private void increaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mp.Volume >= 95)
                SetVolume(100);
            else
                SetVolume(mp.Volume + 5);
        }
        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mp.Volume <= 5)
                SetVolume(0);
            else
                SetVolume(mp.Volume - 5);
        }
        private void volumeToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var newVol = Functions.TryParse.ToInt(volumeToolStripTextBox.Text);

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (volumeToolStripTextBox.Text.ToLower().Equals("mute") || newVol.Equals(0))
                    {
                        SetVolume(0);
                        playbackToolStripMenuItem.HideDropDown();
                    }
                    else if (newVol > 0 && newVol <= 100)
                    {
                        SetVolume(newVol);
                        playbackToolStripMenuItem.HideDropDown();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a value that is between 1 - 100.",
                            "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        SetVolume(mp.Volume);
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
        #region Navigate

        private void nextChapterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.NextChapter();
        }
        private void previousChapterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.PreviousChapter();
        }

        private void chaptersMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                int index = (item.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(item);
                mp.SetChapter(index);
            }
        }

        private void SetChapterToolStripMenuItems()
        {
            chaptersToolStripMenuItem.DropDownItems.Clear();

            if (mp.FileInfo.Chapters.Count.Equals(0))
            {
                var item = new ToolStripMenuItem("[ none ]", null);
                item.Enabled = false;
                chaptersToolStripMenuItem.DropDownItems.Add(item);
                return;
            }

            for (int i = 0; i < mp.FileInfo.Chapters.Count; i++)
            {
                var text = string.Format("{0}: {1}", i + 1, mp.FileInfo.Chapters[i].ChapterName);
                var item = new ToolStripMenuItem(text, null, chaptersMenuItem_Click);
                if (i < 9)
                    item.ShortcutKeys = Keys.Control | KeysClass.GetNumKey(i + 1);
                chaptersToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void frameStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.SendCommand("frame_step");
        }
        private void frameBackStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.SendCommand("frame_back_step");
        }

        private void jumpToTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jumpForm = new JumpForm(mp.CurrentStatus);

            if (jumpForm.ShowDialog(this) == DialogResult.OK)
                mp.Seek(jumpForm.GetNewTime);

            jumpForm.Dispose();
        }

        #endregion
        #region Settings

        private void showPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPlaylist = !ShowPlaylist;
        }

        private void hideAlbumArtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideAlbumArt = !HideAlbumArt;
        }

        private void dimDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (blackForm == null)
            {
                blackForm = new BlackForm(this);
                blackForm.SetTitle(Functions.Url.DecodeUrl(mp.FileInfo.MovieName));
            }

            if (dimDesktopToolStripMenuItem.Checked)
            {
                blackForm.Show();
                this.TopLevel = true;
            }
            else
            {
                blackForm.Hide();
            }
        }

        private void showCommandLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConsole = !ShowConsole;
        }

        private void alwaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alwaysToolStripMenuItem.Checked = true;
            whenPlayingToolStripMenuItem.Checked = false;
            neverToolStripMenuItem.Checked = false;
            SetOnTop();
        }
        private void whenPlayingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            whenPlayingToolStripMenuItem.Checked = true;
            alwaysToolStripMenuItem.Checked = false;
            neverToolStripMenuItem.Checked = false;
            SetOnTop();
        }
        private void neverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            neverToolStripMenuItem.Checked = true;
            alwaysToolStripMenuItem.Checked = false;
            whenPlayingToolStripMenuItem.Checked = false;
            SetOnTop();
        }
        private void SetOnTop()
        {
            if (whenPlayingToolStripMenuItem.Checked)
                this.TopMost = (mp.CurrentStatus.PlayState == PlayStates.Playing);
            else if (alwaysToolStripMenuItem.Checked)
                this.TopMost = true;
            else if (neverToolStripMenuItem.Checked)
                this.TopMost = false;
        }

        private void showIconInTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showIconInTrayToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.ShowIcon = true;
                trayIcon.Visible = true;
            }
            else
            {
                Properties.Settings.Default.ShowIcon = false;
                trayIcon.Visible = false;
            }
            Properties.Settings.Default.Save();
        }

        private void hidePopupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.HidePopup = hidePopupToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        #endregion
        #region Help

        private void bakaMPlayerHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://bakamplayer.u8sand.net/help.php");
        }

        private void voiceCommandHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var voiceForm = new VoiceForm();
            if (voiceForm.ShowDialog() == DialogResult.OK)
            {
                if (voiceForm.GetName != Properties.Settings.Default.CallName)
                {
                    Properties.Settings.Default.CallName = voiceForm.GetName;
                    Properties.Settings.Default.Save();

                    // if voice command is running, restart it
                    if (voice != null)
                    {
                        var voiceCommandState = IsVoiceCommandEnabled;

                        IsVoiceCommandEnabled = false;
                        voice.SpeechRecognizedEvent += voice_SpeechRecognizedEvent;
                        voice.AudioLevelUpdatedEvent += voice_AudioLevelUpdatedEvent;
                        voice.Dispose();
                        voice = null;

                        // if voice command was enabled, re-enable it
                        if (voiceCommandState)
                            EnableVoiceCommand();
                    }
                    SetStatusMsg("Voice Command: Name changed to " + Properties.Settings.Default.CallName, true);
                }
            }
            voiceForm.Dispose();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var checker = new UpdateEngine();
            checker.CheckForUpdates(false);
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
            globalKeyHook = new KeyHookManager();
            globalKeyHook.KeyDownEvent += KeyboardHook;

            // message filter for mouse events
            var mouseHandler = new GlobalMouseHandler();
            mouseHandler.MouseMoved += mouseHandler_MouseMoved;
            mouseHandler.XButtonDown += mouseHandler_XButtonDown;
            Application.AddMessageFilter(mouseHandler);

            InitializeComponent();

            mp = new mpv.mpv(mplayerPanel.Handle.ToInt32());
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // check for mpv.exe
            if (!File.Exists(Path.Combine(Application.StartupPath, "mpv.exe")))
            {
                MessageBox.Show("mpv.exe was not found!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }

            this.SuspendLayout(); // -- begin layout changes

            RegisterMPlayerEvents();
            playlist.Init(this, mp);
            trayIcon.ContextMenu = trayContextMenu;
            this.MouseWheel += MainForm_MouseWheel;
            this.MinimumSize = new Size(this.Width, this.Height - this.ClientSize.Height
                + mainMenuStrip.Height + seekPanel.Height + controlPanel.Height);
            folderToolStripMenuItem.Text = "Build " + Program.GetVersion();

            SetLCDFont(); // Embbeding Font (LCD.ttf)
            ShowConsole = false;
            ShowPlaylist = false;

            LoadSettings();

            this.ResumeLayout(); // -- end layout changes

            // check for updates
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var week = dfi.Calendar.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            var lastCheck = Properties.Settings.Default.LastUpdateCheck;
            if (week != lastCheck)
            {
                var checker = new UpdateEngine();
                checker.CheckForUpdates(true);

                Properties.Settings.Default.LastUpdateCheck = week;
                Properties.Settings.Default.Save();
            }

            // check for command line args
            var arg = Environment.GetCommandLineArgs();
            for (int i = 1; i < arg.Length; i++)
            {
                if (arg[i].Equals("-lastfile", StringComparison.OrdinalIgnoreCase))
                {
                    var lastFile = Properties.Settings.Default.LastFile;
                    if (File.Exists(lastFile))
                    {
                        mp.OpenFile(lastFile);
                    }
                    else
                    {
                        MessageBox.Show("Either there is no previous file or the previous file does not exist anymore.",
                            "Cannot Open Last File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                }
                if (File.Exists(arg[i]))
                {
                    // Note: opening only one file at a time is supported
                    mp.OpenFile(arg[i]);
                    break;
                }
            }
        }
        private void RegisterMPlayerEvents()
        {
            mp.StdOutEvent += mp_StdOutEvent;
            mp.StatusChangedEvent += mp_StatusChangedEvent;
            mp.FileOpenedEvent += mp_FileOpenedEvent;
            mp.PlayStateChangedEvent += mp_PlayStateChangedEvent;
            mp.DurationChangedEvent += mp_DurationChangedEvent;
            mp.MPlayerQuitEvent += mp_MPlayerQuitEvent;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Functions.OS.RunningOnWin7)
                SetThumbnailToolbars();
        }
        private void LoadSettings()
        {
            // load volume
            SetVolume(Properties.Settings.Default.Volume);

            // tray icon
            if (!Properties.Settings.Default.ShowIcon)
            {
                trayIcon.Visible = false;
                showIconInTrayToolStripMenuItem.Checked = false;
            }
            hidePopupToolStripMenuItem.Checked = Properties.Settings.Default.HidePopup;

            // set last file
            var lastFile = Properties.Settings.Default.LastFile;
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
                        mp.Pause(true);
                    break;
                case Keys.Escape:
                    if (FullScreen)
                    {
                        if (seekBar_IsMouseDown)
                            seekBar_IsMouseDown = false;
                        else
                            FullScreen = false;
                    }
                    else
                    {
                        HidePlayer();
                    }
                    break;
            }
        }
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                // scroll up (increase volume)
                if (mp.Volume >= 95)
                    SetVolume(100);
                else
                    SetVolume(mp.Volume + 5);
            }
            else
            {
                // scroll down (decrease volume)
                if (mp.Volume <= 5)
                    SetVolume(0);
                else
                    SetVolume(mp.Volume - 5);
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mp.Quit() && mp.PlayerIsRunning())
            {
                e.Cancel = true;
                return;
            }

            // save settings
            if (!string.IsNullOrEmpty(mp.FileInfo.Url))
                Properties.Settings.Default.LastFile = mp.FileInfo.Url;
            Properties.Settings.Default.Save();

            if (voice != null)
            {
                IsVoiceCommandEnabled = false;
                voice.SpeechRecognizedEvent -= voice_SpeechRecognizedEvent;
                voice.AudioLevelUpdatedEvent -= voice_AudioLevelUpdatedEvent;
                voice.Dispose();
            }
            globalKeyHook.Dispose();
            UnloadTray();
        }

        #endregion
        #region MPlayer Events

        private void mp_StdOutEvent(object sender, StdOutEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.StdOut.Equals("[ACTION] CLEAR_OUTPUT"))
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

        private void mp_StatusChangedEvent(object sender, StatusChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.Status.Equals("[ACTION] HIDE_STATUS_LABEL"))
                {
                    HideStatusLabel();
                    return;
                }
                if (e.Status.Equals("[ERROR] FAILED_TO_OPEN"))
                {
                    MessageBox.Show("Baka MPlayer couldn't open this file.\nThis can happen if the file is not supported, incomplete, or inaccessible.",
                        "Cannot open file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    SetStatusMsg("Open a file to begin playing", true);
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

        private void mp_FileOpenedEvent(object sender, EventArgs e)
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
                    Properties.Settings.Default.LastFile = mp.FileInfo.Url;
                }
                else
                {
                    Properties.Settings.Default.LastFile = previousUrl;
                    openLastFileToolStripMenuItem.ToolTipText = Path.GetFileName(previousUrl);
                    openLastFileToolStripMenuItem.Enabled = true;
                }

                if (mp.FileInfo.HasVideo)
                {
                    if (File.Exists(previousUrl) && !mp.FileInfo.IsOnline)
                    {
                        // auto fit window if playing directory changes
                        if (!Functions.IO.IsDirectorySame(previousUrl, mp.FileInfo.Url))
                        {
                            FitWindow(1, false);
                        }
                    }
                    else
                    {
                        // file is online so just auto fit window
                        FitWindow(1, false);
                    }
                }

                Properties.Settings.Default.Save();
                previousUrl = mp.FileInfo.Url;

                // set form's info

                this.Text = mp.FileInfo.IsOnline ? mp.FileInfo.MovieName : mp.FileInfo.FullFileName;

                if (mp.FileInfo.IsOnline)
                    folderToolStripMenuItem.Text = new Uri(mp.FileInfo.Url).Host;
                else
                    folderToolStripMenuItem.Text = Functions.String.AutoEllipsis(32, Functions.IO.GetFolderName(mp.FileInfo.Url));

                if (blackForm != null)
                    blackForm.SetTitle(Functions.Url.DecodeUrl(mp.FileInfo.MovieName));

                if (infoForm != null && !infoForm.IsDisposed)
                    infoForm.UpdateInfo(mp.FileInfo, mp.CurrentStatus.TotalLength);

                // set menu strips

                showInWindowsExplorerToolStripMenuItem.Enabled = !mp.FileInfo.IsOnline;

                if (mp.FileInfo.HasVideo)
                {
                    // video file
                    albumArtPicbox.Visible = false;
                    mplayerPanel.Visible = true;

                    frameStepToolStripMenuItem.Enabled = true;
                    frameBackStepToolStripMenuItem.Enabled = true;
                    addSubtitleFileToolStripMenuItem.Enabled = true;
                    HideAlbumArt = false;
                    hideAlbumArtToolStripMenuItem.Enabled = false;
                    takeSnapshotToolStripMenuItem.Enabled = true;
                    SetFitWindowToolStripMenuItems(true);
                    SetAspectRatioToolStripMenuItems(true);
                }
                else
                {
                    // music file
                    mplayerPanel.Visible = false;
                    albumArtPicbox.Visible = true;

                    frameStepToolStripMenuItem.Enabled = false;
                    frameBackStepToolStripMenuItem.Enabled = false;
                    addSubtitleFileToolStripMenuItem.Enabled = false;
                    hideAlbumArtToolStripMenuItem.Enabled = true;
                    takeSnapshotToolStripMenuItem.Enabled = false;
                    SetFitWindowToolStripMenuItems(false);
                    SetAspectRatioToolStripMenuItems(false);

                    // show album art (if it exists)
                    if (mp.FileInfo.Id3Tags.AlbumArtTag != null)
                        albumArtPicbox.Image = mp.FileInfo.Id3Tags.AlbumArtTag.AlbumArt;
                    else
                        albumArtPicbox.Image = Properties.Resources.music_128;

                    albumArtPicbox_SizeChanged(this, EventArgs.Empty);
                }
                ResizeMplayerPanel();

                if (mp.FileInfo.Chapters.Count > 0)
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
                SetAudioTrackMenuItems();
                SetChapterToolStripMenuItems();
                SetSubToolStripMenuItems();

                mp.Play();
            });
        }

        private void mp_PlayStateChangedEvent(object sender, PlayStateChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                SetOnTop();
                SetPlayState(e.PlayState);
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
                mp.OpenFile(mp.FileInfo.Url);
            }
            else if (offToolStripMenuItem.Checked)
            {
                // repeat off (default)
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
            SetStatusMsg("Reached the end of the playlist", true);
        }

        private void mp_DurationChangedEvent(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                UpdateTrayIconInfo();

                if (seekBar_IsMouseDown)
                    return;

                durationLabel.Text = Functions.Time.ConvertSecondsToTime(mp.CurrentStatus.Duration);

                // check if file is seekable
                if (mp.CurrentStatus.TotalLength > 0.0)
                {
                    seekBar.Value = Convert.ToInt32(mp.CurrentStatus.PercentPos * (seekBar.Maximum / 100.0), CultureInfo.InvariantCulture);
                }
                else
                {
                    seekBar.Value = 0;
                    timeLeftLabel.Text = "UNKNOWN";
                    return;
                }

                if (Properties.Settings.Default.ShowTimeRemaining)
                {
                    timeLeftLabel.Text = string.Format(CultureInfo.InvariantCulture, "-{0}",
                        Functions.Time.ConvertSecondsToTime(Math.Abs(mp.CurrentStatus.TotalLength - mp.CurrentStatus.Duration)));
                }
                else
                {
                    timeLeftLabel.Text = Functions.Time.ConvertSecondsToTime(mp.CurrentStatus.TotalLength);
                }
            });
        }

        private void mp_MPlayerQuitEvent(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
        
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM.WINDOWPOSCHANGING:
                    SnapToDesktopBorder(m.LParam);
                    break;
                case WM.SYSCOMMAND:
                    if (m.WParam.ToInt32() == SC.MINIMIZE)
                    {
                        // this.minimize event
                        if (blackForm != null)
                            blackForm.Hide();
                        dimDesktopToolStripMenuItem.Checked = false;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void OpenFile()
        {
            var ofd = new OpenFileDialog
            {
                Filter = string.Format("Multimedia|{0}|Video Files|{1}|Audio Files|{2}|All Files (*.*)|*.*",
                    Properties.Resources.VideoFiles + "; " + Properties.Resources.AudioFiles,
                    Properties.Resources.VideoFiles, Properties.Resources.AudioFiles)
            };

            if (mp.FileInfo.IsOnline)
            {
                ofd.FileName = string.Empty;
            }
            else
            {
                ofd.InitialDirectory = mp.FileInfo.GetDirectoryName;
                ofd.FileName = mp.FileInfo.FullFileName;
            }

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
                mp.OpenFile(ofd.FileName);
        }

        private void HidePlayer()
        {
            if (seekBar_IsMouseDown)
            {
                // if seeking, release focus from seekBar
                seekBar_IsMouseDown = false;
                return;
            }
            
            if (mp.PlayerIsRunning())
                mp.Pause(false);

            if (FullScreen)
                FullScreen = false;
            if (blackForm != null)
                blackForm.Hide();
            dimDesktopToolStripMenuItem.Checked = false;

            // hack required to bypass Windows' hide animation
            this.Opacity = 0.0;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Opacity = 1.0;
        }

        private void SetControls(bool enable, bool lastFile)
        {
            seekBar.Enabled = mp.CurrentStatus.TotalLength > 0.0 && enable;

            stopToolStripMenuItem.Enabled = enable;
            jumpToTimeToolStripMenuItem.Enabled = enable;

            fullScreenToolStripMenuItem.Enabled = enable;
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
            if (mp.FileInfo.Chapters.Count != 0)
            {
                var marks = new List<long>();

                foreach (var c in mp.FileInfo.Chapters)
                    marks.Add(c.StartTime / 1000);

                // add chapter marks on seekBar
                seekBar.AddMarks(marks, mp.CurrentStatus.TotalLength);
            }
            else
            {
                seekBar.ClearMarks();
            }
        }

        private void mouseHandler_XButtonDown(MouseButtons button)
        {
            if (!mp.PlayerIsRunning())
                return;

            switch (button)
            {
                case MouseButtons.XButton1: // seek backwards
                    if (mp.CurrentStatus.Duration - 5 > -1)
                        mp.Seek(mp.CurrentStatus.Duration - 5);
                    else //if (mplayer.currentPosition < 5)
                        mp.Seek(0);
                    break;
                case MouseButtons.XButton2: // seek forwards
                    if (mp.CurrentStatus.Duration + 5 < mp.CurrentStatus.TotalLength)
                        mp.Seek(mp.CurrentStatus.Duration + 5);
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
            if (mp == null || string.IsNullOrEmpty(mp.FileInfo.Url) || !mp.FileInfo.HasVideo)
                return;

            var currentRatio = (double)mplayerSplitContainer.Panel1.Width / mplayerSplitContainer.Panel1.Height;
            var ratio = VO_State.PanelAspectRatio;

            if (ratio.Equals(0.0))
                ratio = (double)mp.FileInfo.VideoWidth / mp.FileInfo.VideoHeight;
            
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

        private void UpdateTrayIconInfo()
        {
            switch(mp.CurrentStatus.PlayState)
            {
                case PlayStates.Playing:
                    nowPlayingMenuItem.Text = string.Format("Now Playing ({0})",
                        Functions.Time.ConvertSecondsToTime(mp.CurrentStatus.Duration));
                    break;
                case PlayStates.Paused:
                    nowPlayingMenuItem.Text = string.Format("Paused ({0})",
                        Functions.Time.ConvertSecondsToTime(mp.CurrentStatus.Duration));
                    break;
                case PlayStates.Stopped:
                    nowPlayingMenuItem.Text = "Stopped";
                    break;
                default:
                    nowPlayingMenuItem.Text = "Now Playing (0:00:00)";
                    break;
            }
        }

        private void SetVolume(int newVol)
        {
            if (newVol < 0 || newVol > 100)
                return;

            mp.SetVolume(newVol);
            Properties.Settings.Default.Volume = newVol;

            if (newVol.Equals(0))
            { // mute
                if (mp.PlayerIsRunning())
                    mp.Mute(true);
                volumeToolStripTextBox.Text = "Mute";

                volumeBar.ThumbFirstColor = Color.DarkGray;
                volumeBar.ThumbSecondColor = Color.DimGray;
                volumeBar.Value = 0;
            }
            else
            { // not mute
                if (mp.PlayerIsRunning())
                    mp.Mute(false);
                volumeToolStripTextBox.Text = newVol.ToString(CultureInfo.InvariantCulture);

                volumeBar.ThumbFirstColor = Color.Silver;
                volumeBar.ThumbSecondColor = Color.DarkGray;
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
                    case "CLR":
                        outputTextbox.Clear();
                        break;
                    case "EXIT":
                        Application.Exit();
                        break;
                    default:
                        mp.SendCommand(inputTextbox.Text);
                        break;
                }
                inputTextbox.SelectionStart = 0;
                inputTextbox.SelectionLength = inputTextbox.TextLength;
            }
        }

        private void mplayerPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(mp.FileInfo.Url) && e.Button == MouseButtons.Right)
                mp.Pause(true);
        }

        private void mplayerPanel_MouseDoubleClickFixed(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                FullScreen = !FullScreen;
        }

        private void mplayerSplitContainer_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(mp.FileInfo.Url) && e.Button == MouseButtons.Right)
                mp.Pause(true);
        }

        private void albumArtPicbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(mp.FileInfo.Url) && e.Button == MouseButtons.Right)
                mp.Pause(true);
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
