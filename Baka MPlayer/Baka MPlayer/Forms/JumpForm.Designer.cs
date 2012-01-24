namespace Baka_MPlayer.Forms
{
    partial class JumpForm
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
            this.subtractRadioButton = new System.Windows.Forms.RadioButton();
            this.addRadioButton = new System.Windows.Forms.RadioButton();
            this.goToRadioButton = new System.Windows.Forms.RadioButton();
            this.timePanel = new System.Windows.Forms.Panel();
            this.secBox = new System.Windows.Forms.NumericUpDown();
            this.minBox = new System.Windows.Forms.NumericUpDown();
            this.hourBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.jumpButton = new Baka_MPlayer.Controls.ButtonControl();
            this.checkPicbox = new System.Windows.Forms.PictureBox();
            this.timePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hourBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkPicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // subtractRadioButton
            // 
            this.subtractRadioButton.AutoSize = true;
            this.subtractRadioButton.Location = new System.Drawing.Point(231, 44);
            this.subtractRadioButton.Name = "subtractRadioButton";
            this.subtractRadioButton.Size = new System.Drawing.Size(150, 23);
            this.subtractRadioButton.TabIndex = 3;
            this.subtractRadioButton.Text = "Subtract from Time";
            this.subtractRadioButton.UseVisualStyleBackColor = true;
            this.subtractRadioButton.Click += new System.EventHandler(this.CheckTimes_ValueChanged);
            // 
            // addRadioButton
            // 
            this.addRadioButton.AutoSize = true;
            this.addRadioButton.Location = new System.Drawing.Point(120, 44);
            this.addRadioButton.Name = "addRadioButton";
            this.addRadioButton.Size = new System.Drawing.Size(105, 23);
            this.addRadioButton.TabIndex = 2;
            this.addRadioButton.Text = "Add to Time";
            this.addRadioButton.UseVisualStyleBackColor = true;
            this.addRadioButton.Click += new System.EventHandler(this.CheckTimes_ValueChanged);
            // 
            // goToRadioButton
            // 
            this.goToRadioButton.AutoSize = true;
            this.goToRadioButton.Checked = true;
            this.goToRadioButton.Location = new System.Drawing.Point(14, 44);
            this.goToRadioButton.Name = "goToRadioButton";
            this.goToRadioButton.Size = new System.Drawing.Size(100, 23);
            this.goToRadioButton.TabIndex = 1;
            this.goToRadioButton.TabStop = true;
            this.goToRadioButton.Text = "Go To Time";
            this.goToRadioButton.UseVisualStyleBackColor = true;
            this.goToRadioButton.Click += new System.EventHandler(this.CheckTimes_ValueChanged);
            // 
            // timePanel
            // 
            this.timePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.timePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timePanel.Controls.Add(this.secBox);
            this.timePanel.Controls.Add(this.minBox);
            this.timePanel.Controls.Add(this.hourBox);
            this.timePanel.Controls.Add(this.label2);
            this.timePanel.Controls.Add(this.label3);
            this.timePanel.Location = new System.Drawing.Point(71, 73);
            this.timePanel.Name = "timePanel";
            this.timePanel.Size = new System.Drawing.Size(193, 30);
            this.timePanel.TabIndex = 4;
            // 
            // secBox
            // 
            this.secBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.secBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.secBox.ForeColor = System.Drawing.Color.White;
            this.secBox.Location = new System.Drawing.Point(143, 3);
            this.secBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.secBox.Name = "secBox";
            this.secBox.Size = new System.Drawing.Size(45, 23);
            this.secBox.TabIndex = 4;
            this.secBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.secBox.ValueChanged += new System.EventHandler(this.CheckTimes_ValueChanged);
            // 
            // minBox
            // 
            this.minBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.minBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.minBox.Enabled = false;
            this.minBox.ForeColor = System.Drawing.Color.White;
            this.minBox.Location = new System.Drawing.Point(73, 3);
            this.minBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.minBox.Name = "minBox";
            this.minBox.Size = new System.Drawing.Size(45, 23);
            this.minBox.TabIndex = 2;
            this.minBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.minBox.ValueChanged += new System.EventHandler(this.CheckTimes_ValueChanged);
            // 
            // hourBox
            // 
            this.hourBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.hourBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hourBox.Enabled = false;
            this.hourBox.ForeColor = System.Drawing.Color.White;
            this.hourBox.Location = new System.Drawing.Point(3, 3);
            this.hourBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.hourBox.Name = "hourBox";
            this.hourBox.Size = new System.Drawing.Size(45, 23);
            this.hourBox.TabIndex = 0;
            this.hourBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hourBox.ValueChanged += new System.EventHandler(this.CheckTimes_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(124, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = ":";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gainsboro;
            this.label3.Location = new System.Drawing.Point(54, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = ":";
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(0, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(394, 30);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Total Time: 00:00:00";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jumpButton
            // 
            this.jumpButton.BackColor = System.Drawing.Color.Transparent;
            this.jumpButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.jumpButton.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.jumpButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.jumpButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.jumpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jumpButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jumpButton.ForeColor = System.Drawing.Color.White;
            this.jumpButton.IsDefaultButton = true;
            this.jumpButton.Location = new System.Drawing.Point(271, 73);
            this.jumpButton.Margin = new System.Windows.Forms.Padding(4);
            this.jumpButton.Name = "jumpButton";
            this.jumpButton.Size = new System.Drawing.Size(85, 30);
            this.jumpButton.TabIndex = 5;
            this.jumpButton.Text = "&Jump";
            this.jumpButton.UseVisualStyleBackColor = true;
            this.jumpButton.Click += new System.EventHandler(this.jumpButton_Click);
            // 
            // checkPicbox
            // 
            this.checkPicbox.Image = global::Baka_MPlayer.Properties.Resources.not_exists;
            this.checkPicbox.Location = new System.Drawing.Point(38, 75);
            this.checkPicbox.Name = "checkPicbox";
            this.checkPicbox.Size = new System.Drawing.Size(27, 27);
            this.checkPicbox.TabIndex = 14;
            this.checkPicbox.TabStop = false;
            // 
            // JumpForm
            // 
            this.AcceptButton = this.jumpButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(394, 122);
            this.Controls.Add(this.checkPicbox);
            this.Controls.Add(this.jumpButton);
            this.Controls.Add(this.subtractRadioButton);
            this.Controls.Add(this.addRadioButton);
            this.Controls.Add(this.goToRadioButton);
            this.Controls.Add(this.timePanel);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JumpForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jump to Time";
            this.timePanel.ResumeLayout(false);
            this.timePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hourBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkPicbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton subtractRadioButton;
        private System.Windows.Forms.RadioButton addRadioButton;
        private System.Windows.Forms.RadioButton goToRadioButton;
        private System.Windows.Forms.Panel timePanel;
        private System.Windows.Forms.NumericUpDown secBox;
        private System.Windows.Forms.NumericUpDown minBox;
        private System.Windows.Forms.NumericUpDown hourBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label statusLabel;
        private Baka_MPlayer.Controls.ButtonControl jumpButton;
        private System.Windows.Forms.PictureBox checkPicbox;
    }
}