using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MPlayer.Info;

namespace Baka_MPlayer.Forms
{
    public partial class SnapshotForm : Form
    {
        private Image SnapshotImage
        {
            get { return snapshotPicbox.Image; }
            set { snapshotPicbox.Image = value; }
        }

        private readonly IFileInfo fileInfo;

        public SnapshotForm(Bitmap image, IFileInfo fileInfo)
        {
            InitializeComponent();
            
            this.SnapshotImage = image;
            this.fileInfo = fileInfo;
            snapshotPicbox_SizeChanged(null, null);
        }

        private void SnapshotForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(this.Width, 50);
            dimensionsLabel.Text = string.Format("Demensions: {0} x {1}", SnapshotImage.Width, SnapshotImage.Height);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // set file name
            var fileName = cleanNameCheckbox.Checked ?
                CleanName(fileInfo.MovieName) : fileInfo.MovieName;

            var sfd = new SaveFileDialog
            {
                FileName = fileName + "_snapshot[1].png",
                Filter = "PNG Images|*.png|" +
                         "Bitmap Images|*.bmp|" +
                         "GIF Images|*.gif|" +
                         "JPEG Images|*.jpg|" +
                         "TIFF Images|*.tiff"
            };

            int total = 1;
            while (File.Exists(string.Format("{0}\\{1}_snapshot[{2}].png", fileInfo.GetDirectoryName, fileName, total)))
            {
                total++;
                sfd.FileName = string.Format("{0}_snapshot[{1}].png", fileName, total);
            }

            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
            {
                switch (sfd.FilterIndex)
                {
                    case 1:
                        SnapshotImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 2:
                        SnapshotImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        SnapshotImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 4:
                        SnapshotImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 5:
                        SnapshotImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                }
            }
        }

        private void copyLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetImage(SnapshotImage);
        }

        private void snapshotPicbox_SizeChanged(object sender, EventArgs e)
        {
            if (snapshotPicbox.Width <= snapshotPicbox.Image.Width ||
                snapshotPicbox.Height <= snapshotPicbox.Image.Height)
                snapshotPicbox.SizeMode = PictureBoxSizeMode.Zoom;
            else
                snapshotPicbox.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private static string CleanName(string input)
        {
            var ext = Path.GetExtension(input);
            const StringComparison ord = StringComparison.Ordinal;
            input = input.Remove(input.LastIndexOf(ext, StringComparison.OrdinalIgnoreCase), ext.Length);

            // Remove all [ and ending ]
            while (input.IndexOf("[", ord) != -1 && input.IndexOf("]", ord) != -1)
                input = input.Replace(input.Substring(input.IndexOf("[", ord), (input.IndexOf("]", ord) - input.IndexOf("[", ord) + 1)), "");

            //Remove all { and ending }
            while (input.IndexOf("{", ord) != -1 && input.IndexOf("}", ord) != -1)
                input = input.Replace(input.Substring(input.IndexOf("{", ord), (input.IndexOf("}", ord) - input.IndexOf("{", ord) + 1)), "");

            input = input.Replace("_", " ");
            return input.Trim() + ext;
        }
    }
}
