using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class SnapshotForm : Form
    {
        private Image image
        {
            get { return snapshotPicbox.Image; }
            set { snapshotPicbox.Image = value; }
        }

        public SnapshotForm(Bitmap image)
        {
            InitializeComponent();

            this.image = image;
            snapshotPicbox_SizeChanged(null, null);
        }

        private void SnapshotForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(this.Width, 50);
            demensionsLabel.Text = string.Format("Demensions: {0} x {1}", image.Width, image.Height);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // set file name
            var fileName = cleanNameCheckbox.Checked ?
                cleanName(Info.FileName) : Info.FileName;

            var sfd = new SaveFileDialog();
            sfd.FileName = fileName + "_snapshot[1].png";

            int total = 1;
            while (File.Exists(string.Format("{0}\\{1}_snapshot[{2}].png", Info.GetDirectoryName, fileName, total)))
            {
                total++;
                sfd.FileName = string.Format("{0}_snapshot[{1}].png", fileName, total);
            }

            sfd.Filter = "PNG Images|*.png|" +
                         "Bitmap Images|*.bmp|" +
                         "GIF Images|*.gif|" +
                         "JPEG Images|*.jpg|" +
                         "TIFF Images|*.tiff";
            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
            {
                if (sfd.FilterIndex.Equals(0))
                    image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                if (sfd.FilterIndex.Equals(1))
                    image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                if (sfd.FilterIndex.Equals(2))
                    image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                if (sfd.FilterIndex.Equals(3))
                    image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                if (sfd.FilterIndex.Equals(4))
                    image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
            }
        }

        private void copyLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetImage(image);
        }

        private void snapshotPicbox_SizeChanged(object sender, EventArgs e)
        {
            if (snapshotPicbox.Width <= snapshotPicbox.Image.Width ||
                snapshotPicbox.Height <= snapshotPicbox.Image.Height)
                snapshotPicbox.SizeMode = PictureBoxSizeMode.Zoom;
            else
                snapshotPicbox.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private static string cleanName(string input)
        {
            var extension = Path.GetExtension(input);
            input = input.Remove(input.IndexOf(extension), extension.Length);

            // Remove all [ and ending ]
            while (input.IndexOf("[") != -1 && input.IndexOf("]") != -1)
                input = input.Replace(input.Substring(input.IndexOf("["), (input.IndexOf("]") - input.IndexOf("[") + 1)), "");

            //Remove all { and ending }
            while (input.IndexOf("{") != -1 && input.IndexOf("}") != -1)
                input = input.Replace(input.Substring(input.IndexOf("{"), (input.IndexOf("}") - input.IndexOf("{") + 1)), "");

            // Replace "_" & "." with " "
            input = input.Replace("_", " ");

            // Remove all trailing spaces & add ext. and return
            return input.Trim() + extension;
        }
    }
}
