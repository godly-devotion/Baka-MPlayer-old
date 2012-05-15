﻿namespace Baka_MPlayer.Forms
{
    partial class InfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.infoList = new System.Windows.Forms.ListView();
            this.infoColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.valueColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.seperatorBox = new System.Windows.Forms.PictureBox();
            this.mplayerProcessLabel = new System.Windows.Forms.Label();
            this.searchTextbox = new Baka_MPlayer.Controls.CustomTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tagList = new System.Windows.Forms.ListView();
            this.tagColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.valueColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.imgTypeTextBox = new System.Windows.Forms.TextBox();
            this.imgLabel = new System.Windows.Forms.Label();
            this.demTextBox = new System.Windows.Forms.TextBox();
            this.demLabel = new System.Windows.Forms.Label();
            this.saveImgLabel = new System.Windows.Forms.LinkLabel();
            this.albumArtPicbox = new System.Windows.Forms.PictureBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.closeButton = new Baka_MPlayer.Controls.ButtonControl();
            this.infoContextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tagContextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seperatorBox)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(12, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(410, 334);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Black;
            this.tabPage1.Controls.Add(this.infoList);
            this.tabPage1.Controls.Add(this.seperatorBox);
            this.tabPage1.Controls.Add(this.mplayerProcessLabel);
            this.tabPage1.Controls.Add(this.searchTextbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(402, 301);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Media Info";
            // 
            // infoList
            // 
            this.infoList.BackColor = System.Drawing.Color.Black;
            this.infoList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.infoList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.infoColumnHeader,
            this.valueColumnHeader});
            this.infoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoList.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoList.ForeColor = System.Drawing.Color.White;
            this.infoList.FullRowSelect = true;
            this.infoList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.infoList.HideSelection = false;
            this.infoList.Location = new System.Drawing.Point(0, 29);
            this.infoList.MultiSelect = false;
            this.infoList.Name = "infoList";
            this.infoList.Size = new System.Drawing.Size(402, 250);
            this.infoList.TabIndex = 1;
            this.infoList.UseCompatibleStateImageBehavior = false;
            this.infoList.View = System.Windows.Forms.View.Details;
            // 
            // infoColumnHeader
            // 
            this.infoColumnHeader.Text = "Info Type";
            this.infoColumnHeader.Width = 160;
            // 
            // valueColumnHeader
            // 
            this.valueColumnHeader.Text = "Value";
            this.valueColumnHeader.Width = 220;
            // 
            // seperatorBox
            // 
            this.seperatorBox.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.seperatorBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.seperatorBox.Location = new System.Drawing.Point(0, 27);
            this.seperatorBox.Name = "seperatorBox";
            this.seperatorBox.Size = new System.Drawing.Size(402, 2);
            this.seperatorBox.TabIndex = 5;
            this.seperatorBox.TabStop = false;
            // 
            // mplayerProcessLabel
            // 
            this.mplayerProcessLabel.BackColor = System.Drawing.Color.Transparent;
            this.mplayerProcessLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mplayerProcessLabel.ForeColor = System.Drawing.Color.White;
            this.mplayerProcessLabel.Location = new System.Drawing.Point(0, 279);
            this.mplayerProcessLabel.Name = "mplayerProcessLabel";
            this.mplayerProcessLabel.Size = new System.Drawing.Size(402, 22);
            this.mplayerProcessLabel.TabIndex = 2;
            this.mplayerProcessLabel.Text = "Baka MPlayer";
            this.mplayerProcessLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchTextbox
            // 
            this.searchTextbox.BackColor = System.Drawing.Color.Black;
            this.searchTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchTextbox.CueText = "Search for Property";
            this.searchTextbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchTextbox.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextbox.ForeColor = System.Drawing.Color.White;
            this.searchTextbox.Location = new System.Drawing.Point(0, 0);
            this.searchTextbox.Name = "searchTextbox";
            this.searchTextbox.Size = new System.Drawing.Size(402, 27);
            this.searchTextbox.TabIndex = 0;
            this.searchTextbox.TextChanged += new System.EventHandler(this.searchTextbox_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Black;
            this.tabPage2.Controls.Add(this.tagList);
            this.tabPage2.Controls.Add(this.imgTypeTextBox);
            this.tabPage2.Controls.Add(this.imgLabel);
            this.tabPage2.Controls.Add(this.demTextBox);
            this.tabPage2.Controls.Add(this.demLabel);
            this.tabPage2.Controls.Add(this.saveImgLabel);
            this.tabPage2.Controls.Add(this.albumArtPicbox);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(402, 301);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ID3 Tags";
            this.tabPage2.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPages_Paint);
            // 
            // tagList
            // 
            this.tagList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagList.BackColor = System.Drawing.Color.Black;
            this.tagList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tagList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tagColumnHeader,
            this.valueColumnHeader1});
            this.tagList.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagList.ForeColor = System.Drawing.Color.White;
            this.tagList.FullRowSelect = true;
            this.tagList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tagList.HideSelection = false;
            this.tagList.Location = new System.Drawing.Point(6, 6);
            this.tagList.MultiSelect = false;
            this.tagList.Name = "tagList";
            this.tagList.Size = new System.Drawing.Size(264, 289);
            this.tagList.TabIndex = 45;
            this.tagList.UseCompatibleStateImageBehavior = false;
            this.tagList.View = System.Windows.Forms.View.Details;
            // 
            // tagColumnHeader
            // 
            this.tagColumnHeader.Text = "Tag";
            this.tagColumnHeader.Width = 100;
            // 
            // valueColumnHeader1
            // 
            this.valueColumnHeader1.Text = "Value";
            this.valueColumnHeader1.Width = 155;
            // 
            // imgTypeTextBox
            // 
            this.imgTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgTypeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.imgTypeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgTypeTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imgTypeTextBox.ForeColor = System.Drawing.Color.White;
            this.imgTypeTextBox.Location = new System.Drawing.Point(283, 195);
            this.imgTypeTextBox.Name = "imgTypeTextBox";
            this.imgTypeTextBox.ReadOnly = true;
            this.imgTypeTextBox.Size = new System.Drawing.Size(100, 22);
            this.imgTypeTextBox.TabIndex = 16;
            this.imgTypeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // imgLabel
            // 
            this.imgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgLabel.AutoEllipsis = true;
            this.imgLabel.BackColor = System.Drawing.Color.Transparent;
            this.imgLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imgLabel.ForeColor = System.Drawing.Color.White;
            this.imgLabel.Location = new System.Drawing.Point(283, 173);
            this.imgLabel.Name = "imgLabel";
            this.imgLabel.Size = new System.Drawing.Size(100, 19);
            this.imgLabel.TabIndex = 15;
            this.imgLabel.Text = "Image Type:";
            this.imgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // demTextBox
            // 
            this.demTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.demTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.demTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.demTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.demTextBox.ForeColor = System.Drawing.Color.White;
            this.demTextBox.Location = new System.Drawing.Point(283, 249);
            this.demTextBox.Name = "demTextBox";
            this.demTextBox.ReadOnly = true;
            this.demTextBox.Size = new System.Drawing.Size(100, 22);
            this.demTextBox.TabIndex = 18;
            this.demTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // demLabel
            // 
            this.demLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.demLabel.AutoEllipsis = true;
            this.demLabel.BackColor = System.Drawing.Color.Transparent;
            this.demLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.demLabel.ForeColor = System.Drawing.Color.White;
            this.demLabel.Location = new System.Drawing.Point(283, 227);
            this.demLabel.Name = "demLabel";
            this.demLabel.Size = new System.Drawing.Size(100, 19);
            this.demLabel.TabIndex = 17;
            this.demLabel.Text = "Demensions:";
            this.demLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveImgLabel
            // 
            this.saveImgLabel.ActiveLinkColor = System.Drawing.Color.DodgerBlue;
            this.saveImgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveImgLabel.BackColor = System.Drawing.Color.Transparent;
            this.saveImgLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveImgLabel.DisabledLinkColor = System.Drawing.Color.Silver;
            this.saveImgLabel.ForeColor = System.Drawing.Color.White;
            this.saveImgLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.saveImgLabel.LinkColor = System.Drawing.Color.DeepSkyBlue;
            this.saveImgLabel.Location = new System.Drawing.Point(276, 129);
            this.saveImgLabel.Name = "saveImgLabel";
            this.saveImgLabel.Size = new System.Drawing.Size(120, 22);
            this.saveImgLabel.TabIndex = 14;
            this.saveImgLabel.TabStop = true;
            this.saveImgLabel.Text = "&Save Image";
            this.saveImgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveImgLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.saveImgLabel_LinkClicked);
            // 
            // albumArtPicbox
            // 
            this.albumArtPicbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.albumArtPicbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.albumArtPicbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.albumArtPicbox.Location = new System.Drawing.Point(276, 6);
            this.albumArtPicbox.Name = "albumArtPicbox";
            this.albumArtPicbox.Size = new System.Drawing.Size(120, 120);
            this.albumArtPicbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.albumArtPicbox.TabIndex = 42;
            this.albumArtPicbox.TabStop = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoEllipsis = true;
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameLabel.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.nameLabel.Location = new System.Drawing.Point(0, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.nameLabel.Size = new System.Drawing.Size(434, 28);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Baka MPlayer";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myToolTip.SetToolTip(this.nameLabel, "Click here to copy text");
            this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.IsDefaultButton = true;
            this.closeButton.Location = new System.Drawing.Point(175, 374);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(85, 30);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // infoContextMenu
            // 
            this.infoContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            this.infoContextMenu.Popup += new System.EventHandler(this.infoContextMenu_Popup);
            // 
            // menuItem1
            // 
            this.menuItem1.Enabled = false;
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Copy &Info Type";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Enabled = false;
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Copy &Value";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Info Type";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 220;
            // 
            // tagContextMenu
            // 
            this.tagContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem4});
            this.tagContextMenu.Popup += new System.EventHandler(this.tagContextMenu_Popup);
            // 
            // menuItem3
            // 
            this.menuItem3.Enabled = false;
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "Copy &Tag";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Enabled = false;
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "Copy Tag &Value";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(434, 412);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Info";
            this.Load += new System.EventHandler(this.InfoForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seperatorBox)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Baka_MPlayer.Controls.CustomTextBox searchTextbox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.ListView infoList;
        private System.Windows.Forms.ColumnHeader infoColumnHeader;
        private System.Windows.Forms.ColumnHeader valueColumnHeader;
        private System.Windows.Forms.Label mplayerProcessLabel;
        private Baka_MPlayer.Controls.ButtonControl closeButton;
        private System.Windows.Forms.TextBox imgTypeTextBox;
        private System.Windows.Forms.Label imgLabel;
        private System.Windows.Forms.TextBox demTextBox;
        private System.Windows.Forms.Label demLabel;
        private System.Windows.Forms.LinkLabel saveImgLabel;
        private System.Windows.Forms.PictureBox albumArtPicbox;
        private System.Windows.Forms.ContextMenu infoContextMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.ListView tagList;
        private System.Windows.Forms.ColumnHeader tagColumnHeader;
        private System.Windows.Forms.ColumnHeader valueColumnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenu tagContextMenu;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.PictureBox seperatorBox;
        private System.Windows.Forms.ToolTip myToolTip;

    }
}