namespace Baka_MPlayer.Forms
{
    partial class OpenSubForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenSubForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mediaTextbox = new System.Windows.Forms.TextBox();
            this.subTextbox = new System.Windows.Forms.TextBox();
            this.browseSubButton = new Baka_MPlayer.Controls.SimpleButton();
            this.browseMediaButton = new Baka_MPlayer.Controls.SimpleButton();
            this.openButton = new Baka_MPlayer.Controls.SimpleButton();
            this.cancelButton = new Baka_MPlayer.Controls.SimpleButton();
            this.mediaCheckPicbox = new System.Windows.Forms.PictureBox();
            this.subCheckPicbox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mediaCheckPicbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subCheckPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Media File:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(35, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Subtitle File:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(580, 61);
            this.label3.TabIndex = 8;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // mediaTextbox
            // 
            this.mediaTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mediaTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.mediaTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.mediaTextbox.BackColor = System.Drawing.Color.Black;
            this.mediaTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mediaTextbox.ForeColor = System.Drawing.Color.White;
            this.mediaTextbox.Location = new System.Drawing.Point(227, 84);
            this.mediaTextbox.Name = "mediaTextbox";
            this.mediaTextbox.Size = new System.Drawing.Size(365, 27);
            this.mediaTextbox.TabIndex = 2;
            this.mediaTextbox.TextChanged += new System.EventHandler(this.mediaTextbox_TextChanged);
            // 
            // subTextbox
            // 
            this.subTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.subTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.subTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.subTextbox.BackColor = System.Drawing.Color.Black;
            this.subTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.subTextbox.ForeColor = System.Drawing.Color.White;
            this.subTextbox.Location = new System.Drawing.Point(227, 122);
            this.subTextbox.Name = "subTextbox";
            this.subTextbox.Size = new System.Drawing.Size(365, 27);
            this.subTextbox.TabIndex = 5;
            this.subTextbox.TextChanged += new System.EventHandler(this.subTextbox_TextChanged);
            // 
            // browseSubButton
            // 
            this.browseSubButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.browseSubButton.BackColor = System.Drawing.Color.Transparent;
            this.browseSubButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.browseSubButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.browseSubButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.browseSubButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseSubButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseSubButton.ForeColor = System.Drawing.Color.White;
            this.browseSubButton.IsDefaultButton = false;
            this.browseSubButton.Location = new System.Drawing.Point(135, 120);
            this.browseSubButton.Margin = new System.Windows.Forms.Padding(4);
            this.browseSubButton.Name = "browseSubButton";
            this.browseSubButton.Size = new System.Drawing.Size(85, 30);
            this.browseSubButton.TabIndex = 4;
            this.browseSubButton.Text = "Browse...";
            this.browseSubButton.UseVisualStyleBackColor = false;
            this.browseSubButton.Click += new System.EventHandler(this.browseSubButton_Click);
            // 
            // browseMediaButton
            // 
            this.browseMediaButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.browseMediaButton.BackColor = System.Drawing.Color.Transparent;
            this.browseMediaButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.browseMediaButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.browseMediaButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.browseMediaButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseMediaButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseMediaButton.ForeColor = System.Drawing.Color.White;
            this.browseMediaButton.IsDefaultButton = false;
            this.browseMediaButton.Location = new System.Drawing.Point(135, 82);
            this.browseMediaButton.Margin = new System.Windows.Forms.Padding(4);
            this.browseMediaButton.Name = "browseMediaButton";
            this.browseMediaButton.Size = new System.Drawing.Size(85, 30);
            this.browseMediaButton.TabIndex = 1;
            this.browseMediaButton.Text = "Browse...";
            this.browseMediaButton.UseVisualStyleBackColor = false;
            this.browseMediaButton.Click += new System.EventHandler(this.browseMediaButton_Click);
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openButton.BackColor = System.Drawing.Color.Transparent;
            this.openButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.openButton.Enabled = false;
            this.openButton.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.openButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.openButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.openButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openButton.ForeColor = System.Drawing.Color.White;
            this.openButton.IsDefaultButton = true;
            this.openButton.Location = new System.Drawing.Point(414, 155);
            this.openButton.Margin = new System.Windows.Forms.Padding(4);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(85, 30);
            this.openButton.TabIndex = 6;
            this.openButton.Text = "&Open";
            this.openButton.UseVisualStyleBackColor = false;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.IsDefaultButton = false;
            this.cancelButton.Location = new System.Drawing.Point(507, 155);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 30);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // mediaCheckPicbox
            // 
            this.mediaCheckPicbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mediaCheckPicbox.BackColor = System.Drawing.Color.Transparent;
            this.mediaCheckPicbox.Image = global::Baka_MPlayer.Properties.Resources.not_exists;
            this.mediaCheckPicbox.Location = new System.Drawing.Point(12, 88);
            this.mediaCheckPicbox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.mediaCheckPicbox.Name = "mediaCheckPicbox";
            this.mediaCheckPicbox.Size = new System.Drawing.Size(20, 19);
            this.mediaCheckPicbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mediaCheckPicbox.TabIndex = 9;
            this.mediaCheckPicbox.TabStop = false;
            // 
            // subCheckPicbox
            // 
            this.subCheckPicbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.subCheckPicbox.BackColor = System.Drawing.Color.Transparent;
            this.subCheckPicbox.Location = new System.Drawing.Point(12, 126);
            this.subCheckPicbox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.subCheckPicbox.Name = "subCheckPicbox";
            this.subCheckPicbox.Size = new System.Drawing.Size(20, 19);
            this.subCheckPicbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.subCheckPicbox.TabIndex = 10;
            this.subCheckPicbox.TabStop = false;
            // 
            // OpenSubForm
            // 
            this.AcceptButton = this.openButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(604, 191);
            this.Controls.Add(this.subCheckPicbox);
            this.Controls.Add(this.mediaCheckPicbox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.browseMediaButton);
            this.Controls.Add(this.browseSubButton);
            this.Controls.Add(this.subTextbox);
            this.Controls.Add(this.mediaTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(620, 230);
            this.Name = "OpenSubForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open File";
            ((System.ComponentModel.ISupportInitialize)(this.mediaCheckPicbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subCheckPicbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mediaTextbox;
        private System.Windows.Forms.TextBox subTextbox;
        private Controls.SimpleButton browseSubButton;
        private Controls.SimpleButton browseMediaButton;
        private Controls.SimpleButton openButton;
        private Controls.SimpleButton cancelButton;
        private System.Windows.Forms.PictureBox mediaCheckPicbox;
        private System.Windows.Forms.PictureBox subCheckPicbox;
    }
}