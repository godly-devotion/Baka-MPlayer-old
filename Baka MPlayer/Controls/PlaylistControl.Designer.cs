﻿namespace Baka_MPlayer.Controls
{
    partial class PlaylistControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaylistControl));
            this.playlistList = new System.Windows.Forms.ListView();
            this.playlistHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.playlistStatusStrip = new System.Windows.Forms.StatusStrip();
            this.currentFileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.currentFileLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.optionsDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.showAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileContextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.seperatorBox = new System.Windows.Forms.PictureBox();
            this.searchTextBox = new Baka_MPlayer.Controls.CustomTextBox();
            this.playlistStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seperatorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // playlistList
            // 
            this.playlistList.AllowDrop = true;
            this.playlistList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.playlistList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playlistList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.playlistHeader});
            this.playlistList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistList.ForeColor = System.Drawing.Color.White;
            this.playlistList.FullRowSelect = true;
            this.playlistList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.playlistList.HideSelection = false;
            this.playlistList.Location = new System.Drawing.Point(0, 22);
            this.playlistList.MultiSelect = false;
            this.playlistList.Name = "playlistList";
            this.playlistList.ShowGroups = false;
            this.playlistList.Size = new System.Drawing.Size(170, 256);
            this.playlistList.TabIndex = 2;
            this.playlistList.TabStop = false;
            this.playlistList.UseCompatibleStateImageBehavior = false;
            this.playlistList.View = System.Windows.Forms.View.Details;
            this.playlistList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.playlistList_ItemDrag);
            this.playlistList.SelectedIndexChanged += new System.EventHandler(this.playlistList_SelectedIndexChanged);
            this.playlistList.DragDrop += new System.Windows.Forms.DragEventHandler(this.playlistList_DragDrop);
            this.playlistList.DragEnter += new System.Windows.Forms.DragEventHandler(this.playlistList_DragEnter);
            this.playlistList.DragOver += new System.Windows.Forms.DragEventHandler(this.playlistList_DragOver);
            this.playlistList.DoubleClick += new System.EventHandler(this.playlistList_DoubleClick);
            this.playlistList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.playlistList_KeyDown);
            this.playlistList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.playlistList_KeyPress);
            // 
            // playlistHeader
            // 
            this.playlistHeader.Text = "File Name";
            this.playlistHeader.Width = 145;
            // 
            // playlistStatusStrip
            // 
            this.playlistStatusStrip.BackColor = System.Drawing.Color.Transparent;
            this.playlistStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentFileButton,
            this.currentFileLabel,
            this.optionsDropDownButton});
            this.playlistStatusStrip.Location = new System.Drawing.Point(0, 278);
            this.playlistStatusStrip.Name = "playlistStatusStrip";
            this.playlistStatusStrip.Size = new System.Drawing.Size(170, 22);
            this.playlistStatusStrip.SizingGrip = false;
            this.playlistStatusStrip.TabIndex = 3;
            // 
            // currentFileButton
            // 
            this.currentFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.currentFileButton.Image = ((System.Drawing.Image)(resources.GetObject("currentFileButton.Image")));
            this.currentFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.currentFileButton.Name = "currentFileButton";
            this.currentFileButton.ShowDropDownArrow = false;
            this.currentFileButton.Size = new System.Drawing.Size(19, 20);
            this.currentFileButton.Text = ">";
            this.currentFileButton.ToolTipText = "Select current file";
            this.currentFileButton.Click += new System.EventHandler(this.currentFileButton_Click);
            // 
            // currentFileLabel
            // 
            this.currentFileLabel.Name = "currentFileLabel";
            this.currentFileLabel.Size = new System.Drawing.Size(101, 17);
            this.currentFileLabel.Spring = true;
            this.currentFileLabel.Text = "File 0 of 0";
            this.currentFileLabel.ToolTipText = "Click here to choose the index of the file to play";
            this.currentFileLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.currentFileLabel_MouseDown);
            // 
            // optionsDropDownButton
            // 
            this.optionsDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.optionsDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllFilesToolStripMenuItem,
            this.refreshPlaylistToolStripMenuItem});
            this.optionsDropDownButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.optionsDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.optionsDropDownButton.Name = "optionsDropDownButton";
            this.optionsDropDownButton.Size = new System.Drawing.Size(35, 20);
            this.optionsDropDownButton.Text = "•••";
            this.optionsDropDownButton.ToolTipText = "Playlist options";
            // 
            // showAllFilesToolStripMenuItem
            // 
            this.showAllFilesToolStripMenuItem.CheckOnClick = true;
            this.showAllFilesToolStripMenuItem.Name = "showAllFilesToolStripMenuItem";
            this.showAllFilesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.showAllFilesToolStripMenuItem.Text = "&Show All Files";
            this.showAllFilesToolStripMenuItem.Click += new System.EventHandler(this.showAllFilesToolStripMenuItem_Click);
            // 
            // refreshPlaylistToolStripMenuItem
            // 
            this.refreshPlaylistToolStripMenuItem.Name = "refreshPlaylistToolStripMenuItem";
            this.refreshPlaylistToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.refreshPlaylistToolStripMenuItem.Text = "&Refresh Playlist";
            this.refreshPlaylistToolStripMenuItem.Click += new System.EventHandler(this.refreshPlaylistToolStripMenuItem_Click);
            // 
            // fileContextMenu
            // 
            this.fileContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3,
            this.menuItem4,
            this.menuItem2});
            this.fileContextMenu.Popup += new System.EventHandler(this.fileContextMenu_Popup);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "&Remove from Playlist";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "&Delete from Disk";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "Re&fresh";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // seperatorBox
            // 
            this.seperatorBox.BackColor = System.Drawing.Color.SteelBlue;
            this.seperatorBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.seperatorBox.Location = new System.Drawing.Point(0, 20);
            this.seperatorBox.Name = "seperatorBox";
            this.seperatorBox.Size = new System.Drawing.Size(170, 2);
            this.seperatorBox.TabIndex = 4;
            this.seperatorBox.TabStop = false;
            // 
            // searchTextBox
            // 
            this.searchTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchTextBox.CueText = "Search Playlist";
            this.searchTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchTextBox.ForeColor = System.Drawing.Color.White;
            this.searchTextBox.Location = new System.Drawing.Point(0, 0);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(170, 20);
            this.searchTextBox.TabIndex = 0;
            this.searchTextBox.TabStop = false;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyDown);
            // 
            // PlaylistControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.playlistList);
            this.Controls.Add(this.playlistStatusStrip);
            this.Controls.Add(this.seperatorBox);
            this.Controls.Add(this.searchTextBox);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PlaylistControl";
            this.Size = new System.Drawing.Size(170, 300);
            this.playlistStatusStrip.ResumeLayout(false);
            this.playlistStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seperatorBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader playlistHeader;
        private System.Windows.Forms.StatusStrip playlistStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel currentFileLabel;
        private System.Windows.Forms.ToolStripDropDownButton optionsDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem showAllFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshPlaylistToolStripMenuItem;
        private System.Windows.Forms.ListView playlistList;
        private System.Windows.Forms.ContextMenu fileContextMenu;
        private System.Windows.Forms.ToolStripDropDownButton currentFileButton;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.PictureBox seperatorBox;
        internal CustomTextBox searchTextBox;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem3;
    }
}
