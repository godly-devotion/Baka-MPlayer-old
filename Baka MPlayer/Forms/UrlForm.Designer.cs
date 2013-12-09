namespace Baka_MPlayer.Forms
{
    partial class UrlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UrlForm));
            this.checkPicbox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.urlTextbox = new System.Windows.Forms.TextBox();
            this.fileTypeLabel = new System.Windows.Forms.Label();
            this.okButton = new Baka_MPlayer.Controls.SimpleButton();
            this.closeButton = new Baka_MPlayer.Controls.SimpleButton();
            this.pasteButton = new Baka_MPlayer.Controls.SimpleButton();
            this.copyButton = new Baka_MPlayer.Controls.SimpleButton();
            this.clearButton = new Baka_MPlayer.Controls.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.checkPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // checkPicbox
            // 
            this.checkPicbox.BackColor = System.Drawing.Color.Transparent;
            this.checkPicbox.Image = global::Baka_MPlayer.Properties.Resources.not_exists;
            this.checkPicbox.Location = new System.Drawing.Point(12, 12);
            this.checkPicbox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkPicbox.Name = "checkPicbox";
            this.checkPicbox.Size = new System.Drawing.Size(20, 27);
            this.checkPicbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.checkPicbox.TabIndex = 0;
            this.checkPicbox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "&URL:";
            // 
            // urlTextbox
            // 
            this.urlTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.urlTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.urlTextbox.BackColor = System.Drawing.Color.Black;
            this.urlTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.urlTextbox.ForeColor = System.Drawing.Color.White;
            this.urlTextbox.Location = new System.Drawing.Point(80, 12);
            this.urlTextbox.Name = "urlTextbox";
            this.urlTextbox.Size = new System.Drawing.Size(481, 27);
            this.urlTextbox.TabIndex = 1;
            this.urlTextbox.TextChanged += new System.EventHandler(this.urlTextbox_TextChanged);
            // 
            // fileTypeLabel
            // 
            this.fileTypeLabel.AutoEllipsis = true;
            this.fileTypeLabel.BackColor = System.Drawing.Color.Transparent;
            this.fileTypeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileTypeLabel.Font = new System.Drawing.Font("Candara", 12F);
            this.fileTypeLabel.Location = new System.Drawing.Point(567, 12);
            this.fileTypeLabel.Name = "fileTypeLabel";
            this.fileTypeLabel.Size = new System.Drawing.Size(55, 27);
            this.fileTypeLabel.TabIndex = 8;
            this.fileTypeLabel.Text = "*.*";
            this.fileTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.Transparent;
            this.okButton.Enabled = false;
            this.okButton.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.okButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.okButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.ForeColor = System.Drawing.Color.White;
            this.okButton.IsDefaultButton = true;
            this.okButton.Location = new System.Drawing.Point(12, 49);
            this.okButton.Margin = new System.Windows.Forms.Padding(4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(85, 30);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.IsDefaultButton = false;
            this.closeButton.Location = new System.Drawing.Point(537, 49);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(85, 30);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Clos&e";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // pasteButton
            // 
            this.pasteButton.BackColor = System.Drawing.Color.Transparent;
            this.pasteButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.pasteButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.pasteButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.pasteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pasteButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasteButton.ForeColor = System.Drawing.Color.White;
            this.pasteButton.IsDefaultButton = false;
            this.pasteButton.Location = new System.Drawing.Point(182, 49);
            this.pasteButton.Margin = new System.Windows.Forms.Padding(4);
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Size = new System.Drawing.Size(85, 30);
            this.pasteButton.TabIndex = 3;
            this.pasteButton.Text = "&Paste";
            this.pasteButton.UseVisualStyleBackColor = false;
            this.pasteButton.Click += new System.EventHandler(this.pasteButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.BackColor = System.Drawing.Color.Transparent;
            this.copyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.copyButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.copyButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.copyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.copyButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.ForeColor = System.Drawing.Color.White;
            this.copyButton.IsDefaultButton = false;
            this.copyButton.Location = new System.Drawing.Point(275, 49);
            this.copyButton.Margin = new System.Windows.Forms.Padding(4);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(85, 30);
            this.copyButton.TabIndex = 4;
            this.copyButton.Text = "&Copy";
            this.copyButton.UseVisualStyleBackColor = false;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.BackColor = System.Drawing.Color.Transparent;
            this.clearButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.clearButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.clearButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearButton.ForeColor = System.Drawing.Color.White;
            this.clearButton.IsDefaultButton = false;
            this.clearButton.Location = new System.Drawing.Point(368, 49);
            this.clearButton.Margin = new System.Windows.Forms.Padding(4);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(85, 30);
            this.clearButton.TabIndex = 5;
            this.clearButton.Text = "C&lear";
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // UrlForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(634, 92);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.pasteButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fileTypeLabel);
            this.Controls.Add(this.urlTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkPicbox);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UrlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open URL Location";
            this.Load += new System.EventHandler(this.WebForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkPicbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox checkPicbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox urlTextbox;
        private System.Windows.Forms.Label fileTypeLabel;
        private Baka_MPlayer.Controls.SimpleButton okButton;
        private Baka_MPlayer.Controls.SimpleButton closeButton;
        private Baka_MPlayer.Controls.SimpleButton pasteButton;
        private Baka_MPlayer.Controls.SimpleButton copyButton;
        private Baka_MPlayer.Controls.SimpleButton clearButton;
    }
}