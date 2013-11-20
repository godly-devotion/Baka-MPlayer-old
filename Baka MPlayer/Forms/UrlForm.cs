using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class UrlForm : Form
    {
        #region Buttons

        private void okButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void pasteButton_Click(object sender, EventArgs e)
        {
            URL = Clipboard.GetText();
            urlTextbox.Focus();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            if (urlTextbox.SelectionLength.Equals(0))
                urlTextbox.SelectAll();
            urlTextbox.Copy();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            URL = string.Empty;
            urlTextbox.Focus();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        public string URL
        {
            get { return urlTextbox.Text.Replace("\"", ""); }
            set { urlTextbox.Text = value; }
        }

        public UrlForm()
        {
            InitializeComponent();

            this.URL = Info.URL;
        }

        private void WebForm_Load(object sender, EventArgs e)
        {
            urlTextbox.SelectAll();
            urlTextbox.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var gradientBrush = new LinearGradientBrush(
                this.ClientRectangle, Color.FromArgb(255, 60, 60, 60), Color.Black, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(gradientBrush, this.ClientRectangle);
            }
        }

        private void OpenFile()
        {
            if (URL.Equals(Info.URL, StringComparison.OrdinalIgnoreCase))
            {
                if (MessageBox.Show(string.Format("\"{0}\" is already playing.\nDo you still want to open this file?", Path.GetFileName(Info.URL)),
                    "Already Playing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void urlTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(URL))
                {
                    SetControls(false);
                    return;
                }

                if (File.Exists(URL) || Functions.URL.IsValidURL(URL))
                {
                    // local file or web file
                    SetControls(true);
                }
                else
                {
                    fileTypeLabel.Text = "*.*";
                    SetControls(false);
                }
            }
            catch (ArgumentException)
            {
                fileTypeLabel.Text = "*.*";
                SetControls(false);
            }
        }

        private void SetControls(bool isValid)
        {
            if (isValid)
            {
                string fileType = Path.GetExtension(URL).ToUpperInvariant();
                fileTypeLabel.Text = string.IsNullOrEmpty(fileType) ? "?" : fileType;

                checkPicbox.Image = Properties.Resources.exists;
                okButton.Enabled = true;
            }
            else
            {
                fileTypeLabel.Text = "*.*";
                checkPicbox.Image = Properties.Resources.not_exists;
                okButton.Enabled = false;
            }
        }
    }
}
