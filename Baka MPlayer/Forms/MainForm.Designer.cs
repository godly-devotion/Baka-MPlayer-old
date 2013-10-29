using Baka_MPlayer.Controls;

namespace Baka_MPlayer.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileWithExternalSubsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLocationFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLastFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.playNextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playPreviousFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rewindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.shuffleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repeatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.playlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thisFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopAftercurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.frameStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frameBackStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.jumpToTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitToVideoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.aspectRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autodetectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.force43ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.force169ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.force2351ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chaptersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.increaseVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.volumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.subtitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSubtitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.subtitleTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.takeSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sayMediaNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCommandLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.mediaInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAlbumArtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dimLightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.onTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whenPlayingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.neverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showIconInTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hidePopupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bakaMPlayerHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutBakaMPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.quickButton = new Baka_MPlayer.Controls.MediaButton();
            this.nextButton = new Baka_MPlayer.Controls.MediaButton();
            this.previousButton = new Baka_MPlayer.Controls.MediaButton();
            this.rewindButton = new Baka_MPlayer.Controls.MediaButton();
            this.playlistButton = new Baka_MPlayer.Controls.MediaButton();
            this.speechButton = new System.Windows.Forms.PictureBox();
            this.volumeBar = new Baka_MPlayer.Controls.ColorSlider();
            this.playButton = new System.Windows.Forms.PictureBox();
            this.seekPanel = new System.Windows.Forms.Panel();
            this.seekBar = new Baka_MPlayer.Controls.ColorSlider();
            this.timeLeftLabel = new System.Windows.Forms.Label();
            this.durationLabel = new System.Windows.Forms.Label();
            this.mplayerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.bodySplitContainer = new System.Windows.Forms.SplitContainer();
            this.statusLabel = new System.Windows.Forms.Label();
            this.albumArtPicbox = new System.Windows.Forms.PictureBox();
            this.mplayerPanel = new Baka_MPlayer.Controls.VO_Panel();
            this.outputTextbox = new System.Windows.Forms.RichTextBox();
            this.inputTextbox = new Baka_MPlayer.Controls.CustomTextBox();
            this.playlist = new Baka_MPlayer.Controls.PlaylistControl();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenu = new System.Windows.Forms.ContextMenu();
            this.showMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.nowPlayingMenuItem = new System.Windows.Forms.MenuItem();
            this.titleMenuItem = new System.Windows.Forms.MenuItem();
            this.artistMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.playMenuItem = new System.Windows.Forms.MenuItem();
            this.stopMenuItem = new System.Windows.Forms.MenuItem();
            this.rewindMenuItem = new System.Windows.Forms.MenuItem();
            this.nextMenuItem = new System.Windows.Forms.MenuItem();
            this.previousMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.xToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusTimer = new System.Windows.Forms.Timer(this.components);
            this.cursorTimer = new System.Windows.Forms.Timer(this.components);
            this.mainMenuStrip.SuspendLayout();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quickButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previousButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rewindButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speechButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playButton)).BeginInit();
            this.seekPanel.SuspendLayout();
            this.mplayerSplitContainer.Panel1.SuspendLayout();
            this.mplayerSplitContainer.Panel2.SuspendLayout();
            this.mplayerSplitContainer.SuspendLayout();
            this.bodySplitContainer.Panel1.SuspendLayout();
            this.bodySplitContainer.Panel2.SuspendLayout();
            this.bodySplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.AutoSize = false;
            this.mainMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.mainMenuStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainMenuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.playbackToolStripMenuItem,
            this.mediaToolStripMenuItem,
            this.subtitlesToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.folderToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.mainMenuStrip.Size = new System.Drawing.Size(584, 20);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DraggableWindow_MouseDown);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPlayerToolStripMenuItem,
            this.toolStripSeparator1,
            this.openFileToolStripMenuItem,
            this.openFileWithExternalSubsToolStripMenuItem,
            this.openURLToolStripMenuItem,
            this.openLocationFromClipboardToolStripMenuItem,
            this.openLastFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.showInWindowsExplorerToolStripMenuItem,
            this.toolStripSeparator3,
            this.playNextFileToolStripMenuItem,
            this.playPreviousFileToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newPlayerToolStripMenuItem
            // 
            this.newPlayerToolStripMenuItem.Name = "newPlayerToolStripMenuItem";
            this.newPlayerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newPlayerToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.newPlayerToolStripMenuItem.Text = "&New Player";
            this.newPlayerToolStripMenuItem.Click += new System.EventHandler(this.newPlayerToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(268, 6);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.openFileToolStripMenuItem.Text = "&Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openFileWithExternalSubsToolStripMenuItem
            // 
            this.openFileWithExternalSubsToolStripMenuItem.Name = "openFileWithExternalSubsToolStripMenuItem";
            this.openFileWithExternalSubsToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.openFileWithExternalSubsToolStripMenuItem.Text = "Open File with External Subs";
            this.openFileWithExternalSubsToolStripMenuItem.Click += new System.EventHandler(this.openFileWithExternalSubsToolStripMenuItem_Click);
            // 
            // openURLToolStripMenuItem
            // 
            this.openURLToolStripMenuItem.Name = "openURLToolStripMenuItem";
            this.openURLToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.openURLToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.openURLToolStripMenuItem.Text = "Open &URL";
            this.openURLToolStripMenuItem.Click += new System.EventHandler(this.openURLToolStripMenuItem_Click);
            // 
            // openLocationFromClipboardToolStripMenuItem
            // 
            this.openLocationFromClipboardToolStripMenuItem.Name = "openLocationFromClipboardToolStripMenuItem";
            this.openLocationFromClipboardToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.openLocationFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.openLocationFromClipboardToolStripMenuItem.Text = "Open Location from &Clipboard";
            this.openLocationFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.openLocationFromClipboardToolStripMenuItem_Click);
            // 
            // openLastFileToolStripMenuItem
            // 
            this.openLastFileToolStripMenuItem.Enabled = false;
            this.openLastFileToolStripMenuItem.Name = "openLastFileToolStripMenuItem";
            this.openLastFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.openLastFileToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.openLastFileToolStripMenuItem.Text = "Open &Last File";
            this.openLastFileToolStripMenuItem.Click += new System.EventHandler(this.openLastFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(268, 6);
            // 
            // showInWindowsExplorerToolStripMenuItem
            // 
            this.showInWindowsExplorerToolStripMenuItem.Enabled = false;
            this.showInWindowsExplorerToolStripMenuItem.Name = "showInWindowsExplorerToolStripMenuItem";
            this.showInWindowsExplorerToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.showInWindowsExplorerToolStripMenuItem.Text = "Show in &File Explorer";
            this.showInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInWindowsExplorerToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(268, 6);
            // 
            // playNextFileToolStripMenuItem
            // 
            this.playNextFileToolStripMenuItem.Enabled = false;
            this.playNextFileToolStripMenuItem.Name = "playNextFileToolStripMenuItem";
            this.playNextFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.playNextFileToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.playNextFileToolStripMenuItem.Text = "Pla&y Next File";
            this.playNextFileToolStripMenuItem.Click += new System.EventHandler(this.playNextFileToolStripMenuItem_Click);
            // 
            // playPreviousFileToolStripMenuItem
            // 
            this.playPreviousFileToolStripMenuItem.Enabled = false;
            this.playPreviousFileToolStripMenuItem.Name = "playPreviousFileToolStripMenuItem";
            this.playPreviousFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
            this.playPreviousFileToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.playPreviousFileToolStripMenuItem.Text = "Play &Previous File";
            this.playPreviousFileToolStripMenuItem.Click += new System.EventHandler(this.playPreviousFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(268, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // playbackToolStripMenuItem
            // 
            this.playbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.rewindToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.toolStripSeparator5,
            this.shuffleToolStripMenuItem,
            this.repeatToolStripMenuItem,
            this.stopAftercurrentToolStripMenuItem,
            this.toolStripSeparator6,
            this.frameStepToolStripMenuItem,
            this.frameBackStepToolStripMenuItem,
            this.toolStripSeparator7,
            this.jumpToTimeToolStripMenuItem});
            this.playbackToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            this.playbackToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.playbackToolStripMenuItem.Text = "&Playback";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Enabled = false;
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.ShortcutKeyDisplayString = "Space";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.playToolStripMenuItem.Text = "&Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.stopToolStripMenuItem.Text = "&Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // rewindToolStripMenuItem
            // 
            this.rewindToolStripMenuItem.Enabled = false;
            this.rewindToolStripMenuItem.Name = "rewindToolStripMenuItem";
            this.rewindToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.rewindToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.rewindToolStripMenuItem.Text = "&Rewind";
            this.rewindToolStripMenuItem.Click += new System.EventHandler(this.rewindToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Enabled = false;
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.restartToolStripMenuItem.Text = "R&estart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(230, 6);
            // 
            // shuffleToolStripMenuItem
            // 
            this.shuffleToolStripMenuItem.CheckOnClick = true;
            this.shuffleToolStripMenuItem.Image = global::Baka_MPlayer.Properties.Resources.shuffle;
            this.shuffleToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.shuffleToolStripMenuItem.Name = "shuffleToolStripMenuItem";
            this.shuffleToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.shuffleToolStripMenuItem.Text = "Shuff&le";
            this.shuffleToolStripMenuItem.Click += new System.EventHandler(this.shuffleToolStripMenuItem_Click);
            // 
            // repeatToolStripMenuItem
            // 
            this.repeatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.offToolStripMenuItem,
            this.toolStripSeparator8,
            this.playlistToolStripMenuItem,
            this.thisFileToolStripMenuItem});
            this.repeatToolStripMenuItem.Image = global::Baka_MPlayer.Properties.Resources.repeat;
            this.repeatToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.repeatToolStripMenuItem.Name = "repeatToolStripMenuItem";
            this.repeatToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.repeatToolStripMenuItem.Text = "Re&peat";
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Checked = true;
            this.offToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.offToolStripMenuItem.Text = "&Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(112, 6);
            // 
            // playlistToolStripMenuItem
            // 
            this.playlistToolStripMenuItem.Name = "playlistToolStripMenuItem";
            this.playlistToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.playlistToolStripMenuItem.Text = "&Playlist";
            this.playlistToolStripMenuItem.Click += new System.EventHandler(this.playlistToolStripMenuItem_Click);
            // 
            // thisFileToolStripMenuItem
            // 
            this.thisFileToolStripMenuItem.Name = "thisFileToolStripMenuItem";
            this.thisFileToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.thisFileToolStripMenuItem.Text = "&This File";
            this.thisFileToolStripMenuItem.Click += new System.EventHandler(this.thisFileToolStripMenuItem_Click);
            // 
            // stopAftercurrentToolStripMenuItem
            // 
            this.stopAftercurrentToolStripMenuItem.CheckOnClick = true;
            this.stopAftercurrentToolStripMenuItem.Name = "stopAftercurrentToolStripMenuItem";
            this.stopAftercurrentToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.stopAftercurrentToolStripMenuItem.Text = "Stop after &current";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(230, 6);
            // 
            // frameStepToolStripMenuItem
            // 
            this.frameStepToolStripMenuItem.Enabled = false;
            this.frameStepToolStripMenuItem.Name = "frameStepToolStripMenuItem";
            this.frameStepToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.frameStepToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.frameStepToolStripMenuItem.Text = "&Frame Step";
            this.frameStepToolStripMenuItem.Click += new System.EventHandler(this.frameStepToolStripMenuItem_Click);
            // 
            // frameBackStepToolStripMenuItem
            // 
            this.frameBackStepToolStripMenuItem.Enabled = false;
            this.frameBackStepToolStripMenuItem.Name = "frameBackStepToolStripMenuItem";
            this.frameBackStepToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
            this.frameBackStepToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.frameBackStepToolStripMenuItem.Text = "Frame &Back Step";
            this.frameBackStepToolStripMenuItem.Click += new System.EventHandler(this.frameBackStepToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(230, 6);
            // 
            // jumpToTimeToolStripMenuItem
            // 
            this.jumpToTimeToolStripMenuItem.Enabled = false;
            this.jumpToTimeToolStripMenuItem.Name = "jumpToTimeToolStripMenuItem";
            this.jumpToTimeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.jumpToTimeToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.jumpToTimeToolStripMenuItem.Text = "&Jump To Time...";
            this.jumpToTimeToolStripMenuItem.Click += new System.EventHandler(this.jumpToTimeToolStripMenuItem_Click);
            // 
            // mediaToolStripMenuItem
            // 
            this.mediaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullScreenToolStripMenuItem,
            this.fitToVideoToolStripMenuItem,
            this.toolStripSeparator9,
            this.aspectRatioToolStripMenuItem,
            this.audioTracksToolStripMenuItem,
            this.chaptersToolStripMenuItem,
            this.toolStripSeparator10,
            this.increaseVolumeToolStripMenuItem,
            this.decreaseVolumeToolStripMenuItem,
            this.toolStripSeparator17,
            this.volumeToolStripMenuItem,
            this.volumeToolStripTextBox});
            this.mediaToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.mediaToolStripMenuItem.Name = "mediaToolStripMenuItem";
            this.mediaToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.mediaToolStripMenuItem.Text = "&Media";
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.Enabled = false;
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Return)));
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.fullScreenToolStripMenuItem.Text = "&Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // fitToVideoToolStripMenuItem
            // 
            this.fitToVideoToolStripMenuItem.Enabled = false;
            this.fitToVideoToolStripMenuItem.Name = "fitToVideoToolStripMenuItem";
            this.fitToVideoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+` (Tilde key)";
            this.fitToVideoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemtilde)));
            this.fitToVideoToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.fitToVideoToolStripMenuItem.Text = "Fit To &Video";
            this.fitToVideoToolStripMenuItem.Click += new System.EventHandler(this.fitToVideoToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(223, 6);
            // 
            // aspectRatioToolStripMenuItem
            // 
            this.aspectRatioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autodetectToolStripMenuItem,
            this.force43ToolStripMenuItem,
            this.force169ToolStripMenuItem,
            this.force2351ToolStripMenuItem});
            this.aspectRatioToolStripMenuItem.Name = "aspectRatioToolStripMenuItem";
            this.aspectRatioToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.aspectRatioToolStripMenuItem.Text = "Aspect &Ratio";
            // 
            // autodetectToolStripMenuItem
            // 
            this.autodetectToolStripMenuItem.Enabled = false;
            this.autodetectToolStripMenuItem.Name = "autodetectToolStripMenuItem";
            this.autodetectToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.autodetectToolStripMenuItem.Text = "&Autodetect";
            this.autodetectToolStripMenuItem.Click += new System.EventHandler(this.autodetectToolStripMenuItem_Click);
            // 
            // force43ToolStripMenuItem
            // 
            this.force43ToolStripMenuItem.Enabled = false;
            this.force43ToolStripMenuItem.Name = "force43ToolStripMenuItem";
            this.force43ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.force43ToolStripMenuItem.Text = "Force &4:3";
            this.force43ToolStripMenuItem.Click += new System.EventHandler(this.force43ToolStripMenuItem_Click);
            // 
            // force169ToolStripMenuItem
            // 
            this.force169ToolStripMenuItem.Enabled = false;
            this.force169ToolStripMenuItem.Name = "force169ToolStripMenuItem";
            this.force169ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.force169ToolStripMenuItem.Text = "Force 16:&9";
            this.force169ToolStripMenuItem.Click += new System.EventHandler(this.force169ToolStripMenuItem_Click);
            // 
            // force2351ToolStripMenuItem
            // 
            this.force2351ToolStripMenuItem.Enabled = false;
            this.force2351ToolStripMenuItem.Name = "force2351ToolStripMenuItem";
            this.force2351ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.force2351ToolStripMenuItem.Text = "Force &2.35:1";
            this.force2351ToolStripMenuItem.Click += new System.EventHandler(this.force2351ToolStripMenuItem_Click);
            // 
            // audioTracksToolStripMenuItem
            // 
            this.audioTracksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem});
            this.audioTracksToolStripMenuItem.Name = "audioTracksToolStripMenuItem";
            this.audioTracksToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.audioTracksToolStripMenuItem.Text = "Audio &Tracks";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.Enabled = false;
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.mainToolStripMenuItem.Text = "0: [ main ]";
            // 
            // chaptersToolStripMenuItem
            // 
            this.chaptersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem1});
            this.chaptersToolStripMenuItem.Name = "chaptersToolStripMenuItem";
            this.chaptersToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.chaptersToolStripMenuItem.Text = "&Chapters";
            // 
            // noneToolStripMenuItem1
            // 
            this.noneToolStripMenuItem1.Enabled = false;
            this.noneToolStripMenuItem1.Name = "noneToolStripMenuItem1";
            this.noneToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
            this.noneToolStripMenuItem1.Text = "[ none ]";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(223, 6);
            // 
            // increaseVolumeToolStripMenuItem
            // 
            this.increaseVolumeToolStripMenuItem.Name = "increaseVolumeToolStripMenuItem";
            this.increaseVolumeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.increaseVolumeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.increaseVolumeToolStripMenuItem.Text = "&Increase Volume";
            this.increaseVolumeToolStripMenuItem.Click += new System.EventHandler(this.increaseVolumeToolStripMenuItem_Click);
            // 
            // decreaseVolumeToolStripMenuItem
            // 
            this.decreaseVolumeToolStripMenuItem.Name = "decreaseVolumeToolStripMenuItem";
            this.decreaseVolumeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.decreaseVolumeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.decreaseVolumeToolStripMenuItem.Text = "&Decrease Volume";
            this.decreaseVolumeToolStripMenuItem.Click += new System.EventHandler(this.decreaseVolumeToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(223, 6);
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.Enabled = false;
            this.volumeToolStripMenuItem.Name = "volumeToolStripMenuItem";
            this.volumeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.volumeToolStripMenuItem.Text = "Volume:";
            // 
            // volumeToolStripTextBox
            // 
            this.volumeToolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.volumeToolStripTextBox.MaxLength = 3;
            this.volumeToolStripTextBox.Name = "volumeToolStripTextBox";
            this.volumeToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.volumeToolStripTextBox.Text = "50";
            this.volumeToolStripTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.volumeToolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.volumeToolStripTextBox_KeyDown);
            // 
            // subtitlesToolStripMenuItem
            // 
            this.subtitlesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSubtitlesToolStripMenuItem,
            this.toolStripSeparator11,
            this.subtitleTrackToolStripMenuItem,
            this.fontSizeToolStripMenuItem});
            this.subtitlesToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.subtitlesToolStripMenuItem.Name = "subtitlesToolStripMenuItem";
            this.subtitlesToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.subtitlesToolStripMenuItem.Text = "&Subtitles";
            // 
            // showSubtitlesToolStripMenuItem
            // 
            this.showSubtitlesToolStripMenuItem.CheckOnClick = true;
            this.showSubtitlesToolStripMenuItem.Enabled = false;
            this.showSubtitlesToolStripMenuItem.Name = "showSubtitlesToolStripMenuItem";
            this.showSubtitlesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.showSubtitlesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.showSubtitlesToolStripMenuItem.Text = "&Show Subtitles";
            this.showSubtitlesToolStripMenuItem.Click += new System.EventHandler(this.showSubtitlesToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(192, 6);
            // 
            // subtitleTrackToolStripMenuItem
            // 
            this.subtitleTrackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem});
            this.subtitleTrackToolStripMenuItem.Name = "subtitleTrackToolStripMenuItem";
            this.subtitleTrackToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.subtitleTrackToolStripMenuItem.Text = "Subtitle &Track";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Enabled = false;
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.noneToolStripMenuItem.Text = "[ none ]";
            // 
            // fontSizeToolStripMenuItem
            // 
            this.fontSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeToolStripMenuItem,
            this.sizeToolStripMenuItem1,
            this.resetSizeToolStripMenuItem});
            this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
            this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.fontSizeToolStripMenuItem.Text = "&Font Size";
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.Enabled = false;
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.sizeToolStripMenuItem.Text = "&Size +";
            this.sizeToolStripMenuItem.Click += new System.EventHandler(this.sizeToolStripMenuItem_Click);
            // 
            // sizeToolStripMenuItem1
            // 
            this.sizeToolStripMenuItem1.Enabled = false;
            this.sizeToolStripMenuItem1.Name = "sizeToolStripMenuItem1";
            this.sizeToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
            this.sizeToolStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.sizeToolStripMenuItem1.Text = "S&ize -";
            this.sizeToolStripMenuItem1.Click += new System.EventHandler(this.sizeToolStripMenuItem1_Click);
            // 
            // resetSizeToolStripMenuItem
            // 
            this.resetSizeToolStripMenuItem.Enabled = false;
            this.resetSizeToolStripMenuItem.Name = "resetSizeToolStripMenuItem";
            this.resetSizeToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.resetSizeToolStripMenuItem.Text = "&Reset Size";
            this.resetSizeToolStripMenuItem.Click += new System.EventHandler(this.resetSizeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.takeSnapshotToolStripMenuItem,
            this.sayMediaNameToolStripMenuItem,
            this.showCommandLineToolStripMenuItem,
            this.toolStripSeparator12,
            this.mediaInfoToolStripMenuItem});
            this.toolsToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // takeSnapshotToolStripMenuItem
            // 
            this.takeSnapshotToolStripMenuItem.Enabled = false;
            this.takeSnapshotToolStripMenuItem.Name = "takeSnapshotToolStripMenuItem";
            this.takeSnapshotToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.takeSnapshotToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.takeSnapshotToolStripMenuItem.Text = "Take &Snapshot";
            this.takeSnapshotToolStripMenuItem.Click += new System.EventHandler(this.takeSnapshotToolStripMenuItem_Click);
            // 
            // sayMediaNameToolStripMenuItem
            // 
            this.sayMediaNameToolStripMenuItem.Enabled = false;
            this.sayMediaNameToolStripMenuItem.Name = "sayMediaNameToolStripMenuItem";
            this.sayMediaNameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.sayMediaNameToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.sayMediaNameToolStripMenuItem.Text = "Say &Media Name";
            this.sayMediaNameToolStripMenuItem.Click += new System.EventHandler(this.sayMediaNameToolStripMenuItem_Click);
            // 
            // showCommandLineToolStripMenuItem
            // 
            this.showCommandLineToolStripMenuItem.Name = "showCommandLineToolStripMenuItem";
            this.showCommandLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.showCommandLineToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.showCommandLineToolStripMenuItem.Text = "Show &Command Line";
            this.showCommandLineToolStripMenuItem.Click += new System.EventHandler(this.showCommandLineToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(218, 6);
            // 
            // mediaInfoToolStripMenuItem
            // 
            this.mediaInfoToolStripMenuItem.Enabled = false;
            this.mediaInfoToolStripMenuItem.Name = "mediaInfoToolStripMenuItem";
            this.mediaInfoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mediaInfoToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.mediaInfoToolStripMenuItem.Text = "Media &Info...";
            this.mediaInfoToolStripMenuItem.Click += new System.EventHandler(this.mediaInfoToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showPlaylistToolStripMenuItem,
            this.hideAlbumArtToolStripMenuItem,
            this.dimLightsToolStripMenuItem,
            this.toolStripSeparator13,
            this.onTopToolStripMenuItem,
            this.trayIconToolStripMenuItem});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // showPlaylistToolStripMenuItem
            // 
            this.showPlaylistToolStripMenuItem.Enabled = false;
            this.showPlaylistToolStripMenuItem.Name = "showPlaylistToolStripMenuItem";
            this.showPlaylistToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.showPlaylistToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showPlaylistToolStripMenuItem.Text = "&Show Playlist";
            this.showPlaylistToolStripMenuItem.Click += new System.EventHandler(this.showPlaylistToolStripMenuItem_Click);
            // 
            // hideAlbumArtToolStripMenuItem
            // 
            this.hideAlbumArtToolStripMenuItem.Enabled = false;
            this.hideAlbumArtToolStripMenuItem.Name = "hideAlbumArtToolStripMenuItem";
            this.hideAlbumArtToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.hideAlbumArtToolStripMenuItem.Text = "&Hide Album Art";
            this.hideAlbumArtToolStripMenuItem.Click += new System.EventHandler(this.hideAlbumArtToolStripMenuItem_Click);
            // 
            // dimLightsToolStripMenuItem
            // 
            this.dimLightsToolStripMenuItem.CheckOnClick = true;
            this.dimLightsToolStripMenuItem.Name = "dimLightsToolStripMenuItem";
            this.dimLightsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.dimLightsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dimLightsToolStripMenuItem.Text = "&Dim Lights";
            this.dimLightsToolStripMenuItem.Click += new System.EventHandler(this.dimLightsToolStripMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(177, 6);
            // 
            // onTopToolStripMenuItem
            // 
            this.onTopToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysToolStripMenuItem,
            this.whenPlayingToolStripMenuItem,
            this.toolStripSeparator16,
            this.neverToolStripMenuItem});
            this.onTopToolStripMenuItem.Name = "onTopToolStripMenuItem";
            this.onTopToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.onTopToolStripMenuItem.Text = "&On Top";
            // 
            // alwaysToolStripMenuItem
            // 
            this.alwaysToolStripMenuItem.Name = "alwaysToolStripMenuItem";
            this.alwaysToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.alwaysToolStripMenuItem.Text = "&Always";
            this.alwaysToolStripMenuItem.Click += new System.EventHandler(this.alwaysToolStripMenuItem_Click);
            // 
            // whenPlayingToolStripMenuItem
            // 
            this.whenPlayingToolStripMenuItem.Name = "whenPlayingToolStripMenuItem";
            this.whenPlayingToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.whenPlayingToolStripMenuItem.Text = "When &Playing";
            this.whenPlayingToolStripMenuItem.Click += new System.EventHandler(this.whenPlayingToolStripMenuItem_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(142, 6);
            // 
            // neverToolStripMenuItem
            // 
            this.neverToolStripMenuItem.Checked = true;
            this.neverToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.neverToolStripMenuItem.Name = "neverToolStripMenuItem";
            this.neverToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.neverToolStripMenuItem.Text = "&Never";
            this.neverToolStripMenuItem.Click += new System.EventHandler(this.neverToolStripMenuItem_Click);
            // 
            // trayIconToolStripMenuItem
            // 
            this.trayIconToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showIconInTrayToolStripMenuItem,
            this.hidePopupToolStripMenuItem});
            this.trayIconToolStripMenuItem.Name = "trayIconToolStripMenuItem";
            this.trayIconToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.trayIconToolStripMenuItem.Text = "&Tray Icon";
            // 
            // showIconInTrayToolStripMenuItem
            // 
            this.showIconInTrayToolStripMenuItem.Checked = true;
            this.showIconInTrayToolStripMenuItem.CheckOnClick = true;
            this.showIconInTrayToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showIconInTrayToolStripMenuItem.Name = "showIconInTrayToolStripMenuItem";
            this.showIconInTrayToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.showIconInTrayToolStripMenuItem.Text = "&Show Icon In Tray";
            this.showIconInTrayToolStripMenuItem.Click += new System.EventHandler(this.showIconInTrayToolStripMenuItem_Click);
            // 
            // hidePopupToolStripMenuItem
            // 
            this.hidePopupToolStripMenuItem.CheckOnClick = true;
            this.hidePopupToolStripMenuItem.Name = "hidePopupToolStripMenuItem";
            this.hidePopupToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.hidePopupToolStripMenuItem.Text = "&Hide Popup";
            this.hidePopupToolStripMenuItem.Click += new System.EventHandler(this.hidePopupToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bakaMPlayerHelpToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripSeparator18,
            this.aboutBakaMPlayerToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // bakaMPlayerHelpToolStripMenuItem
            // 
            this.bakaMPlayerHelpToolStripMenuItem.Name = "bakaMPlayerHelpToolStripMenuItem";
            this.bakaMPlayerHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.bakaMPlayerHelpToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.bakaMPlayerHelpToolStripMenuItem.Text = "Baka MPlayer &Help";
            this.bakaMPlayerHelpToolStripMenuItem.Click += new System.EventHandler(this.bakaMPlayerHelpToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "&Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(185, 6);
            // 
            // aboutBakaMPlayerToolStripMenuItem
            // 
            this.aboutBakaMPlayerToolStripMenuItem.Name = "aboutBakaMPlayerToolStripMenuItem";
            this.aboutBakaMPlayerToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.aboutBakaMPlayerToolStripMenuItem.Text = "&About Baka MPlayer";
            this.aboutBakaMPlayerToolStripMenuItem.Click += new System.EventHandler(this.aboutBakaMPlayerToolStripMenuItem_Click);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.folderToolStripMenuItem.Enabled = false;
            this.folderToolStripMenuItem.ForeColor = System.Drawing.Color.DarkGray;
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.folderToolStripMenuItem.Text = "Build 0.0.0.0";
            this.folderToolStripMenuItem.Click += new System.EventHandler(this.folderToolStripMenuItem_Click);
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.controlPanel.Controls.Add(this.quickButton);
            this.controlPanel.Controls.Add(this.nextButton);
            this.controlPanel.Controls.Add(this.previousButton);
            this.controlPanel.Controls.Add(this.rewindButton);
            this.controlPanel.Controls.Add(this.playlistButton);
            this.controlPanel.Controls.Add(this.speechButton);
            this.controlPanel.Controls.Add(this.volumeBar);
            this.controlPanel.Controls.Add(this.playButton);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlPanel.Location = new System.Drawing.Point(0, 341);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(584, 50);
            this.controlPanel.TabIndex = 1;
            this.controlPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.controlPanel_Paint);
            this.controlPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DraggableWindow_MouseDown);
            // 
            // quickButton
            // 
            this.quickButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quickButton.BackColor = System.Drawing.Color.Transparent;
            this.quickButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quickButton.DefaultImage = global::Baka_MPlayer.Properties.Resources.default_open;
            this.quickButton.DisabledImage = null;
            this.quickButton.Image = global::Baka_MPlayer.Properties.Resources.default_open;
            this.quickButton.Location = new System.Drawing.Point(12, 10);
            this.quickButton.MouseDownImage = global::Baka_MPlayer.Properties.Resources.down_open;
            this.quickButton.Name = "quickButton";
            this.quickButton.Size = new System.Drawing.Size(25, 25);
            this.quickButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.quickButton.TabIndex = 12;
            this.quickButton.TabStop = false;
            this.xToolTip.SetToolTip(this.quickButton, "Left Click to Open File\r\nMouse Wheel Click to Jump\r\nRight Click to Open URL");
            this.quickButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.quickButton_MouseClick);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.nextButton.BackColor = System.Drawing.Color.Transparent;
            this.nextButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nextButton.DefaultImage = global::Baka_MPlayer.Properties.Resources.default_next;
            this.nextButton.DisabledImage = global::Baka_MPlayer.Properties.Resources.disabled_next;
            this.nextButton.Enabled = false;
            this.nextButton.Image = global::Baka_MPlayer.Properties.Resources.disabled_next;
            this.nextButton.Location = new System.Drawing.Point(318, 10);
            this.nextButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nextButton.MouseDownImage = global::Baka_MPlayer.Properties.Resources.down_next;
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(37, 25);
            this.nextButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.nextButton.TabIndex = 11;
            this.nextButton.TabStop = false;
            this.nextButton.Paint += new System.Windows.Forms.PaintEventHandler(this.nextButton_Paint);
            this.nextButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.nextButton_MouseClick);
            // 
            // previousButton
            // 
            this.previousButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.previousButton.BackColor = System.Drawing.Color.Transparent;
            this.previousButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previousButton.DefaultImage = global::Baka_MPlayer.Properties.Resources.default_previous;
            this.previousButton.DisabledImage = global::Baka_MPlayer.Properties.Resources.disabled_previous;
            this.previousButton.Enabled = false;
            this.previousButton.Image = global::Baka_MPlayer.Properties.Resources.disabled_previous;
            this.previousButton.Location = new System.Drawing.Point(229, 10);
            this.previousButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.previousButton.MouseDownImage = global::Baka_MPlayer.Properties.Resources.down_previous;
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(37, 25);
            this.previousButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.previousButton.TabIndex = 10;
            this.previousButton.TabStop = false;
            this.previousButton.Paint += new System.Windows.Forms.PaintEventHandler(this.previousButton_Paint);
            this.previousButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.previousButton_MouseClick);
            // 
            // rewindButton
            // 
            this.rewindButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.rewindButton.BackColor = System.Drawing.Color.Transparent;
            this.rewindButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rewindButton.DefaultImage = global::Baka_MPlayer.Properties.Resources.default_reverse;
            this.rewindButton.DisabledImage = global::Baka_MPlayer.Properties.Resources.disabled_reverse;
            this.rewindButton.Enabled = false;
            this.rewindButton.Image = global::Baka_MPlayer.Properties.Resources.disabled_reverse;
            this.rewindButton.Location = new System.Drawing.Point(189, 10);
            this.rewindButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rewindButton.MouseDownImage = global::Baka_MPlayer.Properties.Resources.down_reverse;
            this.rewindButton.Name = "rewindButton";
            this.rewindButton.Size = new System.Drawing.Size(32, 25);
            this.rewindButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.rewindButton.TabIndex = 9;
            this.rewindButton.TabStop = false;
            this.rewindButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rewindButton_MouseClick);
            // 
            // playlistButton
            // 
            this.playlistButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistButton.BackColor = System.Drawing.Color.Transparent;
            this.playlistButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.playlistButton.DefaultImage = global::Baka_MPlayer.Properties.Resources.default_playlist;
            this.playlistButton.DisabledImage = global::Baka_MPlayer.Properties.Resources.disabled_playlist;
            this.playlistButton.Enabled = false;
            this.playlistButton.Image = global::Baka_MPlayer.Properties.Resources.disabled_playlist;
            this.playlistButton.Location = new System.Drawing.Point(547, 10);
            this.playlistButton.MouseDownImage = global::Baka_MPlayer.Properties.Resources.down_playlist;
            this.playlistButton.Name = "playlistButton";
            this.playlistButton.Size = new System.Drawing.Size(25, 25);
            this.playlistButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.playlistButton.TabIndex = 8;
            this.playlistButton.TabStop = false;
            this.playlistButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.playlistButton_MouseClick);
            // 
            // speechButton
            // 
            this.speechButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.speechButton.BackColor = System.Drawing.Color.Transparent;
            this.speechButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.speechButton.Image = global::Baka_MPlayer.Properties.Resources.disabled_mic;
            this.speechButton.Location = new System.Drawing.Point(43, 10);
            this.speechButton.Name = "speechButton";
            this.speechButton.Size = new System.Drawing.Size(25, 25);
            this.speechButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.speechButton.TabIndex = 7;
            this.speechButton.TabStop = false;
            this.xToolTip.SetToolTip(this.speechButton, "Toggle voice recognition");
            this.speechButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.speechButton_MouseClick);
            this.speechButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.speechButton_MouseDown);
            this.speechButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.speechButton_MouseUp);
            // 
            // volumeBar
            // 
            this.volumeBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.volumeBar.BackColor = System.Drawing.Color.Transparent;
            this.volumeBar.BarHeight = 1;
            this.volumeBar.BorderRoundRectSize = new System.Drawing.Size(1, 1);
            this.volumeBar.DrawSemitransparentThumb = false;
            this.volumeBar.LargeChange = ((uint)(5u));
            this.volumeBar.Location = new System.Drawing.Point(363, 10);
            this.volumeBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(110, 25);
            this.volumeBar.SmallChange = ((uint)(1u));
            this.volumeBar.TabIndex = 0;
            this.volumeBar.TabStop = false;
            this.volumeBar.ThumbInnerColor = System.Drawing.Color.DarkGray;
            this.volumeBar.ThumbOuterColor = System.Drawing.Color.Silver;
            this.volumeBar.ThumbPenColor = System.Drawing.Color.Gray;
            this.volumeBar.ThumbRoundRectSize = new System.Drawing.Size(14, 16);
            this.volumeBar.ThumbSize = new System.Drawing.Size(16, 14);
            this.volumeBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.volumeBar_Scroll);
            // 
            // playButton
            // 
            this.playButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.playButton.BackColor = System.Drawing.Color.Transparent;
            this.playButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.playButton.Enabled = false;
            this.playButton.Image = global::Baka_MPlayer.Properties.Resources.disabled_play;
            this.playButton.Location = new System.Drawing.Point(274, 0);
            this.playButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(36, 44);
            this.playButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.playButton.TabIndex = 0;
            this.playButton.TabStop = false;
            this.playButton.EnabledChanged += new System.EventHandler(this.playButton_EnabledChanged);
            this.playButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.playButton_MouseDown);
            this.playButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.playButton_MouseUp);
            // 
            // seekPanel
            // 
            this.seekPanel.Controls.Add(this.seekBar);
            this.seekPanel.Controls.Add(this.timeLeftLabel);
            this.seekPanel.Controls.Add(this.durationLabel);
            this.seekPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.seekPanel.Location = new System.Drawing.Point(0, 327);
            this.seekPanel.Name = "seekPanel";
            this.seekPanel.Size = new System.Drawing.Size(584, 14);
            this.seekPanel.TabIndex = 2;
            // 
            // seekBar
            // 
            this.seekBar.BackColor = System.Drawing.Color.Transparent;
            this.seekBar.BorderRoundRectSize = new System.Drawing.Size(1, 1);
            this.seekBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seekBar.Enabled = false;
            this.seekBar.LargeChange = ((uint)(5u));
            this.seekBar.Location = new System.Drawing.Point(65, 0);
            this.seekBar.Maximum = 1000000;
            this.seekBar.Name = "seekBar";
            this.seekBar.Size = new System.Drawing.Size(449, 14);
            this.seekBar.SmallChange = ((uint)(1u));
            this.seekBar.TabIndex = 1;
            this.seekBar.TabStop = false;
            this.seekBar.ThumbInnerColor = System.Drawing.Color.DarkGray;
            this.seekBar.ThumbOuterColor = System.Drawing.Color.Silver;
            this.seekBar.ThumbPenColor = System.Drawing.Color.DarkGray;
            this.seekBar.ThumbRoundRectSize = new System.Drawing.Size(10, 10);
            this.seekBar.ThumbSize = new System.Drawing.Size(20, 10);
            this.seekBar.Value = 0;
            this.seekBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.seekBar_MouseDown);
            this.seekBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.seekBar_MouseMove);
            this.seekBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.seekBar_MouseUp);
            // 
            // timeLeftLabel
            // 
            this.timeLeftLabel.BackColor = System.Drawing.Color.Transparent;
            this.timeLeftLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.timeLeftLabel.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.timeLeftLabel.Location = new System.Drawing.Point(514, 0);
            this.timeLeftLabel.Name = "timeLeftLabel";
            this.timeLeftLabel.Size = new System.Drawing.Size(70, 14);
            this.timeLeftLabel.TabIndex = 2;
            this.timeLeftLabel.Text = "-00:00:00";
            this.timeLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.xToolTip.SetToolTip(this.timeLeftLabel, "Click to toggle between total and remaining time");
            this.timeLeftLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.timeLeftLabel_MouseClick);
            // 
            // durationLabel
            // 
            this.durationLabel.BackColor = System.Drawing.Color.Transparent;
            this.durationLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.durationLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.durationLabel.Location = new System.Drawing.Point(0, 0);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(65, 14);
            this.durationLabel.TabIndex = 0;
            this.durationLabel.Text = "00:00:00";
            this.durationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.durationLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DraggableWindow_MouseDown);
            // 
            // mplayerSplitContainer
            // 
            this.mplayerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mplayerSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mplayerSplitContainer.Location = new System.Drawing.Point(0, 20);
            this.mplayerSplitContainer.Name = "mplayerSplitContainer";
            // 
            // mplayerSplitContainer.Panel1
            // 
            this.mplayerSplitContainer.Panel1.Controls.Add(this.bodySplitContainer);
            // 
            // mplayerSplitContainer.Panel2
            // 
            this.mplayerSplitContainer.Panel2.Controls.Add(this.playlist);
            this.mplayerSplitContainer.Size = new System.Drawing.Size(584, 307);
            this.mplayerSplitContainer.SplitterDistance = 410;
            this.mplayerSplitContainer.TabIndex = 3;
            this.mplayerSplitContainer.TabStop = false;
            this.mplayerSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mplayerSplitContainer_SplitterMoved);
            // 
            // bodySplitContainer
            // 
            this.bodySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodySplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.bodySplitContainer.Location = new System.Drawing.Point(0, 0);
            this.bodySplitContainer.Name = "bodySplitContainer";
            this.bodySplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // bodySplitContainer.Panel1
            // 
            this.bodySplitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.bodySplitContainer.Panel1.Controls.Add(this.statusLabel);
            this.bodySplitContainer.Panel1.Controls.Add(this.albumArtPicbox);
            this.bodySplitContainer.Panel1.Controls.Add(this.mplayerPanel);
            this.bodySplitContainer.Panel1.SizeChanged += new System.EventHandler(this.bodySplitContainer_Panel1_SizeChanged);
            this.bodySplitContainer.Panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bodySplitContainer_Panel1_MouseClick);
            this.bodySplitContainer.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DraggableWindow_MouseDown);
            // 
            // bodySplitContainer.Panel2
            // 
            this.bodySplitContainer.Panel2.Controls.Add(this.outputTextbox);
            this.bodySplitContainer.Panel2.Controls.Add(this.inputTextbox);
            this.bodySplitContainer.Size = new System.Drawing.Size(410, 307);
            this.bodySplitContainer.SplitterDistance = 208;
            this.bodySplitContainer.TabIndex = 0;
            this.bodySplitContainer.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.BackColor = System.Drawing.Color.Black;
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.statusLabel.Location = new System.Drawing.Point(3, 4);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(248, 28);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Open a file to begin playing";
            this.statusLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.statusLabel_MouseClick);
            // 
            // albumArtPicbox
            // 
            this.albumArtPicbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.albumArtPicbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.albumArtPicbox.Image = global::Baka_MPlayer.Properties.Resources.Music_128;
            this.albumArtPicbox.Location = new System.Drawing.Point(0, 0);
            this.albumArtPicbox.Name = "albumArtPicbox";
            this.albumArtPicbox.Size = new System.Drawing.Size(410, 208);
            this.albumArtPicbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.albumArtPicbox.TabIndex = 1;
            this.albumArtPicbox.TabStop = false;
            this.albumArtPicbox.Visible = false;
            this.albumArtPicbox.SizeChanged += new System.EventHandler(this.albumArtPicbox_SizeChanged);
            this.albumArtPicbox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.albumArtPicbox_MouseClick);
            this.albumArtPicbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DraggableWindow_MouseDown);
            // 
            // mplayerPanel
            // 
            this.mplayerPanel.BackColor = System.Drawing.Color.Black;
            this.mplayerPanel.Location = new System.Drawing.Point(0, 0);
            this.mplayerPanel.Name = "mplayerPanel";
            this.mplayerPanel.Size = new System.Drawing.Size(410, 204);
            this.mplayerPanel.TabIndex = 0;
            this.mplayerPanel.Visible = false;
            this.mplayerPanel.MouseDoubleClickFixed += new System.Windows.Forms.MouseEventHandler(this.mplayerPanel_MouseDoubleClickFixed);
            this.mplayerPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mplayerPanel_MouseClick);
            this.mplayerPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DraggableWindow_MouseDown);
            // 
            // outputTextbox
            // 
            this.outputTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.outputTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outputTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputTextbox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextbox.ForeColor = System.Drawing.Color.Silver;
            this.outputTextbox.Location = new System.Drawing.Point(0, 0);
            this.outputTextbox.Name = "outputTextbox";
            this.outputTextbox.ReadOnly = true;
            this.outputTextbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.outputTextbox.Size = new System.Drawing.Size(410, 72);
            this.outputTextbox.TabIndex = 0;
            this.outputTextbox.TabStop = false;
            this.outputTextbox.Text = "Baka MPlayer Loaded...";
            // 
            // inputTextbox
            // 
            this.inputTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.inputTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputTextbox.CueText = "> Insert command here";
            this.inputTextbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputTextbox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputTextbox.ForeColor = System.Drawing.Color.White;
            this.inputTextbox.Location = new System.Drawing.Point(0, 72);
            this.inputTextbox.Name = "inputTextbox";
            this.inputTextbox.Size = new System.Drawing.Size(410, 23);
            this.inputTextbox.TabIndex = 1;
            this.inputTextbox.TabStop = false;
            this.inputTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputTextbox_KeyDown);
            // 
            // playlist
            // 
            this.playlist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.playlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlist.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playlist.ForeColor = System.Drawing.Color.White;
            this.playlist.Location = new System.Drawing.Point(0, 0);
            this.playlist.Margin = new System.Windows.Forms.Padding(4);
            this.playlist.Name = "playlist";
            this.playlist.SelectedIndex = -1;
            this.playlist.Size = new System.Drawing.Size(170, 307);
            this.playlist.TabIndex = 0;
            this.playlist.TabStop = false;
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Baka MPlayer";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayContextMenu
            // 
            this.trayContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.showMenuItem,
            this.menuItem2,
            this.nowPlayingMenuItem,
            this.titleMenuItem,
            this.artistMenuItem,
            this.menuItem6,
            this.playMenuItem,
            this.stopMenuItem,
            this.rewindMenuItem,
            this.nextMenuItem,
            this.previousMenuItem,
            this.menuItem12,
            this.exitMenuItem});
            // 
            // showMenuItem
            // 
            this.showMenuItem.Index = 0;
            this.showMenuItem.Text = "Show &Baka MPlayer";
            this.showMenuItem.Click += new System.EventHandler(this.showMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "-";
            // 
            // nowPlayingMenuItem
            // 
            this.nowPlayingMenuItem.Enabled = false;
            this.nowPlayingMenuItem.Index = 2;
            this.nowPlayingMenuItem.Text = "Now Playing (00:00:00)";
            // 
            // titleMenuItem
            // 
            this.titleMenuItem.Enabled = false;
            this.titleMenuItem.Index = 3;
            this.titleMenuItem.Text = "  Title";
            // 
            // artistMenuItem
            // 
            this.artistMenuItem.Enabled = false;
            this.artistMenuItem.Index = 4;
            this.artistMenuItem.Text = "  Artist";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 5;
            this.menuItem6.Text = "-";
            // 
            // playMenuItem
            // 
            this.playMenuItem.Enabled = false;
            this.playMenuItem.Index = 6;
            this.playMenuItem.Text = "&Play";
            this.playMenuItem.Click += new System.EventHandler(this.playMenuItem_Click);
            // 
            // stopMenuItem
            // 
            this.stopMenuItem.Enabled = false;
            this.stopMenuItem.Index = 7;
            this.stopMenuItem.Text = "&Stop";
            this.stopMenuItem.Click += new System.EventHandler(this.stopMenuItem_Click);
            // 
            // rewindMenuItem
            // 
            this.rewindMenuItem.Enabled = false;
            this.rewindMenuItem.Index = 8;
            this.rewindMenuItem.Text = "&Rewind";
            this.rewindMenuItem.Click += new System.EventHandler(this.rewindMenuItem_Click);
            // 
            // nextMenuItem
            // 
            this.nextMenuItem.Enabled = false;
            this.nextMenuItem.Index = 9;
            this.nextMenuItem.Text = "> &Next";
            this.nextMenuItem.Click += new System.EventHandler(this.nextMenuItem_Click);
            // 
            // previousMenuItem
            // 
            this.previousMenuItem.Enabled = false;
            this.previousMenuItem.Index = 10;
            this.previousMenuItem.Text = "< &Previous";
            this.previousMenuItem.Click += new System.EventHandler(this.previousMenuItem_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 11;
            this.menuItem12.Text = "-";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 12;
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // statusTimer
            // 
            this.statusTimer.Enabled = true;
            this.statusTimer.Interval = 6000;
            this.statusTimer.Tick += new System.EventHandler(this.statusTimer_Tick);
            // 
            // cursorTimer
            // 
            this.cursorTimer.Interval = 2500;
            this.cursorTimer.Tick += new System.EventHandler(this.cursorTimer_Tick);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(584, 391);
            this.Controls.Add(this.mplayerSplitContainer);
            this.Controls.Add(this.seekPanel);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Baka MPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.quickButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previousButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rewindButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speechButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playButton)).EndInit();
            this.seekPanel.ResumeLayout(false);
            this.mplayerSplitContainer.Panel1.ResumeLayout(false);
            this.mplayerSplitContainer.Panel2.ResumeLayout(false);
            this.mplayerSplitContainer.ResumeLayout(false);
            this.bodySplitContainer.Panel1.ResumeLayout(false);
            this.bodySplitContainer.Panel1.PerformLayout();
            this.bodySplitContainer.Panel2.ResumeLayout(false);
            this.bodySplitContainer.Panel2.PerformLayout();
            this.bodySplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtitlesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Panel seekPanel;
        private System.Windows.Forms.SplitContainer mplayerSplitContainer;
        private System.Windows.Forms.SplitContainer bodySplitContainer;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.Label timeLeftLabel;
        private System.Windows.Forms.PictureBox playButton;
        private Baka_MPlayer.Controls.ColorSlider volumeBar;
        private Baka_MPlayer.Controls.ColorSlider seekBar;
        private PlaylistControl playlist;
        private System.Windows.Forms.PictureBox albumArtPicbox;
        private System.Windows.Forms.RichTextBox outputTextbox;
        private Baka_MPlayer.Controls.CustomTextBox inputTextbox;
        private System.Windows.Forms.ToolStripMenuItem newPlayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openURLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLocationFromClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLastFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem showInWindowsExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem playNextFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playPreviousFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rewindToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem shuffleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem repeatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem playlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thisFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopAftercurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem jumpToTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSubtitlesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem fontSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCommandLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takeSnapshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sayMediaNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem mediaInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showPlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideAlbumArtToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem onTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whenPlayingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trayIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dimLightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem neverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bakaMPlayerHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutBakaMPlayerToolStripMenuItem;
        private System.Windows.Forms.PictureBox speechButton;
        private System.Windows.Forms.ToolStripMenuItem increaseVolumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decreaseVolumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioTracksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem volumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripTextBox volumeToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem subtitleTrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showIconInTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hidePopupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitToVideoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chaptersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenu trayContextMenu;
        private System.Windows.Forms.MenuItem showMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem nowPlayingMenuItem;
        private System.Windows.Forms.MenuItem titleMenuItem;
        private System.Windows.Forms.MenuItem artistMenuItem;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem playMenuItem;
        private System.Windows.Forms.MenuItem stopMenuItem;
        private System.Windows.Forms.MenuItem rewindMenuItem;
        private System.Windows.Forms.MenuItem nextMenuItem;
        private System.Windows.Forms.MenuItem previousMenuItem;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.ToolTip xToolTip;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Timer statusTimer;
        private System.Windows.Forms.Timer cursorTimer;
        private System.Windows.Forms.ToolStripMenuItem frameStepToolStripMenuItem;
        private MediaButton playlistButton;
        private MediaButton rewindButton;
        private MediaButton previousButton;
        private MediaButton nextButton;
        private MediaButton quickButton;
        private System.Windows.Forms.ToolStripMenuItem aspectRatioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autodetectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem force43ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem force169ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem force2351ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileWithExternalSubsToolStripMenuItem;
        protected internal VO_Panel mplayerPanel;
        private System.Windows.Forms.ToolStripMenuItem frameBackStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

