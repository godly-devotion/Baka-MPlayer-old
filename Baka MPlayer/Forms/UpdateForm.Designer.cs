﻿namespace Baka_MPlayer.Forms
{
    partial class UpdateForm
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
            this.closeButton = new Baka_MPlayer.Controls.SimpleButton();
            this.downloadButton = new Baka_MPlayer.Controls.SimpleButton();
            this.statusLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.newLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.changelogLink = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.IsDefaultButton = false;
            this.closeButton.Location = new System.Drawing.Point(385, 288);
            this.closeButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(85, 30);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadButton.BackColor = System.Drawing.Color.Transparent;
            this.downloadButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.downloadButton.Enabled = false;
            this.downloadButton.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.downloadButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.downloadButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.downloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadButton.ForeColor = System.Drawing.Color.White;
            this.downloadButton.IsDefaultButton = true;
            this.downloadButton.Location = new System.Drawing.Point(280, 288);
            this.downloadButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(95, 30);
            this.downloadButton.TabIndex = 0;
            this.downloadButton.Text = "&Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusLabel.Font = new System.Drawing.Font("Calibri", 18F);
            this.statusLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.statusLabel.Location = new System.Drawing.Point(0, 0);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(484, 30);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Checking for updates...";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.newLabel);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 204);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "What\'s New?";
            // 
            // newLabel
            // 
            this.newLabel.AutoEllipsis = true;
            this.newLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newLabel.Location = new System.Drawing.Point(3, 23);
            this.newLabel.Name = "newLabel";
            this.newLabel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.newLabel.Size = new System.Drawing.Size(454, 178);
            this.newLabel.TabIndex = 0;
            this.newLabel.Text = "Checking for updates...";
            // 
            // versionLabel
            // 
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.versionLabel.ForeColor = System.Drawing.Color.LightGray;
            this.versionLabel.Location = new System.Drawing.Point(0, 30);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Padding = new System.Windows.Forms.Padding(20, 2, 20, 0);
            this.versionLabel.Size = new System.Drawing.Size(484, 40);
            this.versionLabel.TabIndex = 4;
            this.versionLabel.Text = "Check for updates...";
            // 
            // changelogLink
            // 
            this.changelogLink.ActiveLinkColor = System.Drawing.Color.DodgerBlue;
            this.changelogLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.changelogLink.AutoSize = true;
            this.changelogLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.changelogLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.changelogLink.LinkColor = System.Drawing.Color.DeepSkyBlue;
            this.changelogLink.Location = new System.Drawing.Point(15, 294);
            this.changelogLink.Name = "changelogLink";
            this.changelogLink.Size = new System.Drawing.Size(245, 19);
            this.changelogLink.TabIndex = 2;
            this.changelogLink.TabStop = true;
            this.changelogLink.Text = "Click here to view the full changelog";
            this.changelogLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.changelogLink_LinkClicked);
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(484, 331);
            this.ControlBox = false;
            this.Controls.Add(this.changelogLink);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Check For Updates";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Baka_MPlayer.Controls.SimpleButton closeButton;
        private Baka_MPlayer.Controls.SimpleButton downloadButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label newLabel;
        private System.Windows.Forms.LinkLabel changelogLink;
    }
}