namespace Baka_MPlayer.Forms
{
    partial class HelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this.closeButton = new Baka_MPlayer.Controls.SimpleButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.IsDefaultButton = true;
            this.closeButton.Location = new System.Drawing.Point(142, 364);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(85, 30);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(345, 344);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Black;
            this.tabPage1.Controls.Add(this.richTextBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(337, 312);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Keyboard Controls";
            // 
            // richTextBox4
            // 
            this.richTextBox4.BackColor = System.Drawing.Color.Black;
            this.richTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox4.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox4.ForeColor = System.Drawing.Color.White;
            this.richTextBox4.Location = new System.Drawing.Point(0, 0);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ReadOnly = true;
            this.richTextBox4.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox4.Size = new System.Drawing.Size(337, 312);
            this.richTextBox4.TabIndex = 1;
            this.richTextBox4.Text = resources.GetString("richTextBox4.Text");
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Black;
            this.tabPage2.Controls.Add(this.richTextBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(337, 312);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Voice Commands";
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.Color.Black;
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox3.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox3.ForeColor = System.Drawing.Color.White;
            this.richTextBox3.Location = new System.Drawing.Point(0, 0);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox3.Size = new System.Drawing.Size(337, 312);
            this.richTextBox3.TabIndex = 1;
            this.richTextBox3.Text = "(Note: you must call Baka MPlayer\'s name before you say the command, by default t" +
    "he name is \"baka\")\n\nplay\npause\nstop\nmute\nunmute\nnext [file]\nprevious [file]\nhide" +
    "";
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(369, 402);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "HelpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Baka MPlayer Help";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Baka_MPlayer.Controls.SimpleButton closeButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.RichTextBox richTextBox3;
    }
}