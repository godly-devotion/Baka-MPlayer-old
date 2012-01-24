namespace Baka_MPlayer.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.customTabControl1 = new System.Windows.Forms.CustomTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.durationLabel = new System.Windows.Forms.Label();
            this.infoList = new System.Windows.Forms.ListView();
            this.infoColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.valueColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.searchTextbox = new Baka_MPlayer.Controls.CustomTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imgTypeTextBox = new System.Windows.Forms.TextBox();
            this.imgLabel = new System.Windows.Forms.Label();
            this.demTextBox = new System.Windows.Forms.TextBox();
            this.demLabel = new System.Windows.Forms.Label();
            this.saveImgLabel = new System.Windows.Forms.LinkLabel();
            this.musicComment = new System.Windows.Forms.TextBox();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.musicGenre = new System.Windows.Forms.TextBox();
            this.GenreLabel = new System.Windows.Forms.Label();
            this.musicTrack = new System.Windows.Forms.TextBox();
            this.TrackLabel = new System.Windows.Forms.Label();
            this.musicYear = new System.Windows.Forms.TextBox();
            this.YearLabel = new System.Windows.Forms.Label();
            this.musicAlbum = new System.Windows.Forms.TextBox();
            this.musicArtist = new System.Windows.Forms.TextBox();
            this.musicTitle = new System.Windows.Forms.TextBox();
            this.AlbumLabel = new System.Windows.Forms.Label();
            this.ArtistLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.albumArtPicbox = new System.Windows.Forms.PictureBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.closeButton = new Baka_MPlayer.Controls.ButtonControl();
            this.customTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // customTabControl1
            // 
            this.customTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customTabControl1.Controls.Add(this.tabPage1);
            this.customTabControl1.Controls.Add(this.tabPage2);
            // 
            // 
            // 
            this.customTabControl1.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.customTabControl1.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.customTabControl1.DisplayStyleProvider.HotTrack = true;
            this.customTabControl1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customTabControl1.DisplayStyleProvider.Opacity = 1F;
            this.customTabControl1.DisplayStyleProvider.Overlap = 0;
            this.customTabControl1.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 3);
            this.customTabControl1.DisplayStyleProvider.Radius = 2;
            this.customTabControl1.DisplayStyleProvider.ShowTabCloser = false;
            this.customTabControl1.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
            this.customTabControl1.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.customTabControl1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabControl1.HotTrack = true;
            this.customTabControl1.Location = new System.Drawing.Point(12, 28);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(410, 337);
            this.customTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.customTabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Black;
            this.tabPage1.Controls.Add(this.durationLabel);
            this.tabPage1.Controls.Add(this.infoList);
            this.tabPage1.Controls.Add(this.searchTextbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 30);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(402, 303);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Media Info";
            this.tabPage1.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPages_Paint);
            // 
            // durationLabel
            // 
            this.durationLabel.BackColor = System.Drawing.Color.Transparent;
            this.durationLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.durationLabel.ForeColor = System.Drawing.Color.White;
            this.durationLabel.Location = new System.Drawing.Point(3, 280);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(396, 20);
            this.durationLabel.TabIndex = 2;
            this.durationLabel.Text = "Duration: 0:00:00 / 0:00:00 (00.0%)";
            this.durationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // infoList
            // 
            this.infoList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoList.BackColor = System.Drawing.Color.Black;
            this.infoList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.infoColumnHeader,
            this.valueColumnHeader});
            this.infoList.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoList.ForeColor = System.Drawing.Color.White;
            this.infoList.FullRowSelect = true;
            this.infoList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.infoList.Location = new System.Drawing.Point(6, 39);
            this.infoList.MultiSelect = false;
            this.infoList.Name = "infoList";
            this.infoList.Size = new System.Drawing.Size(390, 238);
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
            // searchTextbox
            // 
            this.searchTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextbox.BackColor = System.Drawing.Color.Black;
            this.searchTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchTextbox.CueText = "Search for Property";
            this.searchTextbox.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextbox.ForeColor = System.Drawing.Color.White;
            this.searchTextbox.Location = new System.Drawing.Point(6, 6);
            this.searchTextbox.Name = "searchTextbox";
            this.searchTextbox.Size = new System.Drawing.Size(390, 27);
            this.searchTextbox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Black;
            this.tabPage2.Controls.Add(this.imgTypeTextBox);
            this.tabPage2.Controls.Add(this.imgLabel);
            this.tabPage2.Controls.Add(this.demTextBox);
            this.tabPage2.Controls.Add(this.demLabel);
            this.tabPage2.Controls.Add(this.saveImgLabel);
            this.tabPage2.Controls.Add(this.musicComment);
            this.tabPage2.Controls.Add(this.CommentLabel);
            this.tabPage2.Controls.Add(this.musicGenre);
            this.tabPage2.Controls.Add(this.GenreLabel);
            this.tabPage2.Controls.Add(this.musicTrack);
            this.tabPage2.Controls.Add(this.TrackLabel);
            this.tabPage2.Controls.Add(this.musicYear);
            this.tabPage2.Controls.Add(this.YearLabel);
            this.tabPage2.Controls.Add(this.musicAlbum);
            this.tabPage2.Controls.Add(this.musicArtist);
            this.tabPage2.Controls.Add(this.musicTitle);
            this.tabPage2.Controls.Add(this.AlbumLabel);
            this.tabPage2.Controls.Add(this.ArtistLabel);
            this.tabPage2.Controls.Add(this.TitleLabel);
            this.tabPage2.Controls.Add(this.albumArtPicbox);
            this.tabPage2.Location = new System.Drawing.Point(4, 30);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(402, 303);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ID3 Tags";
            this.tabPage2.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPages_Paint);
            // 
            // imgTypeTextBox
            // 
            this.imgTypeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.imgTypeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgTypeTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imgTypeTextBox.ForeColor = System.Drawing.Color.White;
            this.imgTypeTextBox.Location = new System.Drawing.Point(30, 212);
            this.imgTypeTextBox.Name = "imgTypeTextBox";
            this.imgTypeTextBox.ReadOnly = true;
            this.imgTypeTextBox.Size = new System.Drawing.Size(100, 22);
            this.imgTypeTextBox.TabIndex = 16;
            // 
            // imgLabel
            // 
            this.imgLabel.AutoEllipsis = true;
            this.imgLabel.BackColor = System.Drawing.Color.Transparent;
            this.imgLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imgLabel.ForeColor = System.Drawing.Color.White;
            this.imgLabel.Location = new System.Drawing.Point(27, 190);
            this.imgLabel.Name = "imgLabel";
            this.imgLabel.Size = new System.Drawing.Size(103, 19);
            this.imgLabel.TabIndex = 15;
            this.imgLabel.Text = "Image Type:";
            this.imgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // demTextBox
            // 
            this.demTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.demTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.demTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.demTextBox.ForeColor = System.Drawing.Color.White;
            this.demTextBox.Location = new System.Drawing.Point(30, 266);
            this.demTextBox.Name = "demTextBox";
            this.demTextBox.ReadOnly = true;
            this.demTextBox.Size = new System.Drawing.Size(100, 22);
            this.demTextBox.TabIndex = 18;
            // 
            // demLabel
            // 
            this.demLabel.AutoEllipsis = true;
            this.demLabel.BackColor = System.Drawing.Color.Transparent;
            this.demLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.demLabel.ForeColor = System.Drawing.Color.White;
            this.demLabel.Location = new System.Drawing.Point(30, 244);
            this.demLabel.Name = "demLabel";
            this.demLabel.Size = new System.Drawing.Size(100, 19);
            this.demLabel.TabIndex = 17;
            this.demLabel.Text = "Demensions:";
            this.demLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveImgLabel
            // 
            this.saveImgLabel.ActiveLinkColor = System.Drawing.Color.DodgerBlue;
            this.saveImgLabel.BackColor = System.Drawing.Color.Transparent;
            this.saveImgLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveImgLabel.DisabledLinkColor = System.Drawing.Color.Silver;
            this.saveImgLabel.ForeColor = System.Drawing.Color.White;
            this.saveImgLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.saveImgLabel.LinkColor = System.Drawing.Color.DeepSkyBlue;
            this.saveImgLabel.Location = new System.Drawing.Point(6, 159);
            this.saveImgLabel.Name = "saveImgLabel";
            this.saveImgLabel.Size = new System.Drawing.Size(150, 22);
            this.saveImgLabel.TabIndex = 14;
            this.saveImgLabel.TabStop = true;
            this.saveImgLabel.Text = "&Save Image";
            this.saveImgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveImgLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.saveImgLabel_LinkClicked);
            // 
            // musicComment
            // 
            this.musicComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.musicComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.musicComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.musicComment.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.musicComment.ForeColor = System.Drawing.Color.White;
            this.musicComment.Location = new System.Drawing.Point(166, 199);
            this.musicComment.Multiline = true;
            this.musicComment.Name = "musicComment";
            this.musicComment.ReadOnly = true;
            this.musicComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.musicComment.Size = new System.Drawing.Size(230, 99);
            this.musicComment.TabIndex = 13;
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.BackColor = System.Drawing.Color.Transparent;
            this.CommentLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommentLabel.ForeColor = System.Drawing.Color.White;
            this.CommentLabel.Location = new System.Drawing.Point(162, 177);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(75, 19);
            this.CommentLabel.TabIndex = 12;
            this.CommentLabel.Text = "Comment:";
            // 
            // musicGenre
            // 
            this.musicGenre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.musicGenre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.musicGenre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.musicGenre.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.musicGenre.ForeColor = System.Drawing.Color.White;
            this.musicGenre.Location = new System.Drawing.Point(222, 151);
            this.musicGenre.Name = "musicGenre";
            this.musicGenre.ReadOnly = true;
            this.musicGenre.Size = new System.Drawing.Size(174, 23);
            this.musicGenre.TabIndex = 11;
            // 
            // GenreLabel
            // 
            this.GenreLabel.AutoSize = true;
            this.GenreLabel.BackColor = System.Drawing.Color.Transparent;
            this.GenreLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenreLabel.ForeColor = System.Drawing.Color.White;
            this.GenreLabel.Location = new System.Drawing.Point(162, 150);
            this.GenreLabel.Name = "GenreLabel";
            this.GenreLabel.Size = new System.Drawing.Size(52, 19);
            this.GenreLabel.TabIndex = 10;
            this.GenreLabel.Text = "Genre:";
            // 
            // musicTrack
            // 
            this.musicTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.musicTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.musicTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.musicTrack.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.musicTrack.ForeColor = System.Drawing.Color.White;
            this.musicTrack.Location = new System.Drawing.Point(222, 122);
            this.musicTrack.Name = "musicTrack";
            this.musicTrack.ReadOnly = true;
            this.musicTrack.Size = new System.Drawing.Size(174, 23);
            this.musicTrack.TabIndex = 9;
            this.musicTrack.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TrackLabel
            // 
            this.TrackLabel.AutoSize = true;
            this.TrackLabel.BackColor = System.Drawing.Color.Transparent;
            this.TrackLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrackLabel.ForeColor = System.Drawing.Color.White;
            this.TrackLabel.Location = new System.Drawing.Point(162, 121);
            this.TrackLabel.Name = "TrackLabel";
            this.TrackLabel.Size = new System.Drawing.Size(47, 19);
            this.TrackLabel.TabIndex = 8;
            this.TrackLabel.Text = "Track:";
            // 
            // musicYear
            // 
            this.musicYear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.musicYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.musicYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.musicYear.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.musicYear.ForeColor = System.Drawing.Color.White;
            this.musicYear.Location = new System.Drawing.Point(222, 93);
            this.musicYear.Name = "musicYear";
            this.musicYear.ReadOnly = true;
            this.musicYear.Size = new System.Drawing.Size(174, 23);
            this.musicYear.TabIndex = 7;
            // 
            // YearLabel
            // 
            this.YearLabel.AutoSize = true;
            this.YearLabel.BackColor = System.Drawing.Color.Transparent;
            this.YearLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YearLabel.ForeColor = System.Drawing.Color.White;
            this.YearLabel.Location = new System.Drawing.Point(162, 92);
            this.YearLabel.Name = "YearLabel";
            this.YearLabel.Size = new System.Drawing.Size(41, 19);
            this.YearLabel.TabIndex = 6;
            this.YearLabel.Text = "Year:";
            // 
            // musicAlbum
            // 
            this.musicAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.musicAlbum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.musicAlbum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.musicAlbum.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.musicAlbum.ForeColor = System.Drawing.Color.White;
            this.musicAlbum.Location = new System.Drawing.Point(222, 64);
            this.musicAlbum.Name = "musicAlbum";
            this.musicAlbum.ReadOnly = true;
            this.musicAlbum.Size = new System.Drawing.Size(174, 23);
            this.musicAlbum.TabIndex = 5;
            // 
            // musicArtist
            // 
            this.musicArtist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.musicArtist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.musicArtist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.musicArtist.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.musicArtist.ForeColor = System.Drawing.Color.White;
            this.musicArtist.Location = new System.Drawing.Point(222, 35);
            this.musicArtist.Name = "musicArtist";
            this.musicArtist.ReadOnly = true;
            this.musicArtist.Size = new System.Drawing.Size(174, 23);
            this.musicArtist.TabIndex = 3;
            // 
            // musicTitle
            // 
            this.musicTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.musicTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.musicTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.musicTitle.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.musicTitle.ForeColor = System.Drawing.Color.White;
            this.musicTitle.Location = new System.Drawing.Point(222, 6);
            this.musicTitle.Name = "musicTitle";
            this.musicTitle.ReadOnly = true;
            this.musicTitle.Size = new System.Drawing.Size(174, 23);
            this.musicTitle.TabIndex = 1;
            // 
            // AlbumLabel
            // 
            this.AlbumLabel.AutoSize = true;
            this.AlbumLabel.BackColor = System.Drawing.Color.Transparent;
            this.AlbumLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlbumLabel.ForeColor = System.Drawing.Color.White;
            this.AlbumLabel.Location = new System.Drawing.Point(162, 63);
            this.AlbumLabel.Name = "AlbumLabel";
            this.AlbumLabel.Size = new System.Drawing.Size(54, 19);
            this.AlbumLabel.TabIndex = 4;
            this.AlbumLabel.Text = "Album:";
            // 
            // ArtistLabel
            // 
            this.ArtistLabel.AutoSize = true;
            this.ArtistLabel.BackColor = System.Drawing.Color.Transparent;
            this.ArtistLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArtistLabel.ForeColor = System.Drawing.Color.White;
            this.ArtistLabel.Location = new System.Drawing.Point(162, 34);
            this.ArtistLabel.Name = "ArtistLabel";
            this.ArtistLabel.Size = new System.Drawing.Size(48, 19);
            this.ArtistLabel.TabIndex = 2;
            this.ArtistLabel.Text = "Artist:";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.TitleLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.Location = new System.Drawing.Point(162, 5);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(42, 19);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title:";
            // 
            // albumArtPicbox
            // 
            this.albumArtPicbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.albumArtPicbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.albumArtPicbox.Location = new System.Drawing.Point(6, 6);
            this.albumArtPicbox.Name = "albumArtPicbox";
            this.albumArtPicbox.Size = new System.Drawing.Size(150, 150);
            this.albumArtPicbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.albumArtPicbox.TabIndex = 42;
            this.albumArtPicbox.TabStop = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoEllipsis = true;
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameLabel.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.nameLabel.Location = new System.Drawing.Point(0, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(434, 25);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Baka MPlayer";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.IsDefaultButton = true;
            this.closeButton.Location = new System.Drawing.Point(175, 372);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(85, 30);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.customTabControl1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Info";
            this.Load += new System.EventHandler(this.InfoForm_Load);
            this.customTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CustomTabControl customTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Baka_MPlayer.Controls.CustomTextBox searchTextbox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.ListView infoList;
        private System.Windows.Forms.ColumnHeader infoColumnHeader;
        private System.Windows.Forms.ColumnHeader valueColumnHeader;
        private System.Windows.Forms.Label durationLabel;
        private Baka_MPlayer.Controls.ButtonControl closeButton;
        private System.Windows.Forms.TextBox imgTypeTextBox;
        private System.Windows.Forms.Label imgLabel;
        private System.Windows.Forms.TextBox demTextBox;
        private System.Windows.Forms.Label demLabel;
        private System.Windows.Forms.LinkLabel saveImgLabel;
        private System.Windows.Forms.TextBox musicComment;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.TextBox musicGenre;
        private System.Windows.Forms.Label GenreLabel;
        private System.Windows.Forms.TextBox musicTrack;
        private System.Windows.Forms.Label TrackLabel;
        private System.Windows.Forms.TextBox musicYear;
        private System.Windows.Forms.Label YearLabel;
        private System.Windows.Forms.TextBox musicAlbum;
        private System.Windows.Forms.TextBox musicArtist;
        private System.Windows.Forms.TextBox musicTitle;
        private System.Windows.Forms.Label AlbumLabel;
        private System.Windows.Forms.Label ArtistLabel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.PictureBox albumArtPicbox;

    }
}