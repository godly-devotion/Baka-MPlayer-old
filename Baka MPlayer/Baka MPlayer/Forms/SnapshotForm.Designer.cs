namespace Baka_MPlayer.Forms
{
    partial class SnapshotForm
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
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.saveButton = new Baka_MPlayer.Controls.ButtonControl();
            this.cancelButton = new Baka_MPlayer.Controls.ButtonControl();
            this.cleanNameCheckbox = new System.Windows.Forms.CheckBox();
            this.demensionsLabel = new System.Windows.Forms.Label();
            this.snapshotPicbox = new System.Windows.Forms.PictureBox();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.snapshotPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.Transparent;
            this.bottomPanel.Controls.Add(this.saveButton);
            this.bottomPanel.Controls.Add(this.cancelButton);
            this.bottomPanel.Controls.Add(this.cleanNameCheckbox);
            this.bottomPanel.Controls.Add(this.demensionsLabel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 372);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(584, 40);
            this.bottomPanel.TabIndex = 0;
            this.bottomPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bottomPanel_Paint);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.BackColor = System.Drawing.Color.Transparent;
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.saveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.saveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.IsDefaultButton = true;
            this.saveButton.Location = new System.Drawing.Point(486, 6);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(85, 30);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "&Save As";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
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
            this.cancelButton.Location = new System.Drawing.Point(393, 6);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(85, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // cleanNameCheckbox
            // 
            this.cleanNameCheckbox.AutoSize = true;
            this.cleanNameCheckbox.Location = new System.Drawing.Point(194, 10);
            this.cleanNameCheckbox.Name = "cleanNameCheckbox";
            this.cleanNameCheckbox.Size = new System.Drawing.Size(163, 23);
            this.cleanNameCheckbox.TabIndex = 1;
            this.cleanNameCheckbox.Text = "&Use neater file name";
            this.cleanNameCheckbox.UseVisualStyleBackColor = true;
            // 
            // demensionsLabel
            // 
            this.demensionsLabel.AutoSize = true;
            this.demensionsLabel.Location = new System.Drawing.Point(12, 11);
            this.demensionsLabel.Name = "demensionsLabel";
            this.demensionsLabel.Size = new System.Drawing.Size(176, 19);
            this.demensionsLabel.TabIndex = 0;
            this.demensionsLabel.Text = "Demensions: 9000 x 9000";
            // 
            // snapshotPicbox
            // 
            this.snapshotPicbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.snapshotPicbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.snapshotPicbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snapshotPicbox.Location = new System.Drawing.Point(0, 0);
            this.snapshotPicbox.Name = "snapshotPicbox";
            this.snapshotPicbox.Size = new System.Drawing.Size(584, 372);
            this.snapshotPicbox.TabIndex = 1;
            this.snapshotPicbox.TabStop = false;
            this.snapshotPicbox.SizeChanged += new System.EventHandler(this.snapshotPicbox_SizeChanged);
            // 
            // SnapshotForm
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.snapshotPicbox);
            this.Controls.Add(this.bottomPanel);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(550, 300);
            this.Name = "SnapshotForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview Snapshot";
            this.Load += new System.EventHandler(this.SnapshotForm_Load);
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.snapshotPicbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.PictureBox snapshotPicbox;
        private Baka_MPlayer.Controls.ButtonControl saveButton;
        private Baka_MPlayer.Controls.ButtonControl cancelButton;
        private System.Windows.Forms.CheckBox cleanNameCheckbox;
        private System.Windows.Forms.Label demensionsLabel;
    }
}