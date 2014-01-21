using System;
using System.IO;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class OpenSubForm : Form
    {
        private bool _isValidMediaFile;
        private bool _isValidSubFile;

        public string MediaFile {
            get { return mediaTextbox.Text.Trim(); }
        }
        public string SubFile {
            get { return subTextbox.Text.Trim(); }
        }

        public OpenSubForm()
        {
            InitializeComponent();
        }

        private void browseMediaButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = string.Format("Multimedia|{0}|Video Files|{1}|Audio Files|{2}|All Files (*.*)|*.*",
                    Properties.Resources.VideoFiles + "; " + Properties.Resources.AudioFiles,
                    Properties.Resources.VideoFiles, Properties.Resources.AudioFiles)
            };

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
                mediaTextbox.Text = ofd.FileName;
        }

        private void browseSubButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Subtitles|*.sub;*.srt;*.ass;*.ssa|All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
                subTextbox.Text = ofd.FileName;
        }

        private void mediaTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(MediaFile) || Functions.Url.IsValidUrl(MediaFile))
                {
                    mediaCheckPicbox.Image = Properties.Resources.exists;
                    _isValidMediaFile = true;
                }
                else
                {
                    mediaCheckPicbox.Image = Properties.Resources.not_exists;
                    _isValidMediaFile = false;
                }
            }
            catch (ArgumentException)
            {
                mediaCheckPicbox.Image = Properties.Resources.not_exists;
                _isValidMediaFile = false;
            }
            finally
            {
                SetOpenButton();
            }
        }

        private void subTextbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (SubFile.Length.Equals(0))
                {
                    subCheckPicbox.Image = null;
                    return;
                }

                if (File.Exists(SubFile))
                {
                    subCheckPicbox.Image = Properties.Resources.exists;
                    _isValidSubFile = true;
                }
                else
                {
                    subCheckPicbox.Image = Properties.Resources.not_exists;
                    _isValidSubFile = false;
                }
            }
            catch (ArgumentException)
            {
                subCheckPicbox.Image = Properties.Resources.not_exists;
                _isValidSubFile = false;
            }
            finally
            {
                SetOpenButton();
            }
        }

        private void SetOpenButton()
        {
            if (SubFile.Length > 0 && _isValidSubFile && _isValidMediaFile)
            {
                openButton.Enabled = true;
                return;
            }

            if (SubFile.Length.Equals(0) && _isValidMediaFile)
            {
                openButton.Enabled = true;
                return;
            }

            openButton.Enabled = false;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
