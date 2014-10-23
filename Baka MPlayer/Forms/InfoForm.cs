using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MPlayer.Info;

namespace Baka_MPlayer.Forms
{
    public partial class InfoForm : Form
    {
        private IFileInfo fileInfo;
        private double fileTotalLength;

        private Image AlbumArt
        {
            set { albumArtPicbox.Image = value; }
            get { return albumArtPicbox.Image; }
        }

        public InfoForm(int mpvProcessId)
        {
            InitializeComponent();

            mpvProcessLabel.Text = "mpv's process ID: " + mpvProcessId;
        }

        public void UpdateInfo(IFileInfo fileInfo, double fileTotalLength)
        {
            this.fileInfo = fileInfo;
            this.fileTotalLength = fileTotalLength;

            nameLabel.Text = fileInfo.MovieName;

            // set Media Info tabpage
            infoList.BeginUpdate();
            infoList.Items.Clear();

            SetGeneralInfo();
            SetTagsInfo();

            infoList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            infoList.EndUpdate();

            // set ID3 Tags tabpage
            SetId3Tags();
        }

        private void SetGeneralInfo()
        {
            var nameItem = new ListViewItem("File name", infoList.Groups[0]);
            nameItem.SubItems.Add(fileInfo.GetName);

            var typeItem = new ListViewItem("File format", infoList.Groups[0]);
            typeItem.SubItems.Add(fileInfo.FileFormat);

            var sizeItem = new ListViewItem("File size", infoList.Groups[0]);
            if (fileInfo.IsOnline)
                sizeItem.SubItems.Add("n/a");
            else
                sizeItem.SubItems.Add(Functions.IO.GetFileSize(fileInfo.Url, 2));

            var lengthItem = new ListViewItem("Media length", infoList.Groups[0]);
            lengthItem.SubItems.Add(Functions.Time.ConvertSecondsToTime(fileTotalLength));

            var audCodecItem = new ListViewItem("Audio codec", infoList.Groups[0]);
            if (!string.IsNullOrEmpty(fileInfo.AudioCodec))
                audCodecItem.SubItems.Add(fileInfo.AudioCodec);
            else
                audCodecItem.SubItems.Add("n/a");

            var vidCodecItem = new ListViewItem("Video codec", infoList.Groups[0]);
            if (!string.IsNullOrEmpty(fileInfo.VideoCodec))
                vidCodecItem.SubItems.Add(fileInfo.VideoCodec);
            else
                vidCodecItem.SubItems.Add("n/a");

            var dimensionsItem = new ListViewItem("Video dimensions", infoList.Groups[0]);
            dimensionsItem.SubItems.Add(string.Format("{0} x {1}", fileInfo.VideoWidth, fileInfo.VideoHeight));

            var modifiedItem = new ListViewItem("Last modified", infoList.Groups[0]);
            if (fileInfo.IsOnline)
                modifiedItem.SubItems.Add("n/a");
            else
                modifiedItem.SubItems.Add(File.GetLastWriteTime(fileInfo.Url).ToLocalTime().ToString(CultureInfo.InvariantCulture));

            infoList.Items.AddRange(new[]
            {
                nameItem, typeItem, sizeItem, lengthItem, audCodecItem, vidCodecItem, dimensionsItem, modifiedItem
            });
        }

        private void SetTagsInfo()
        {
            foreach (var info in fileInfo.IdInfos)
            {
                var item = new ListViewItem(info.Id, infoList.Groups[1]);
                item.SubItems.Add(info.Value);
                infoList.Items.Add(item);
            }
        }

        private void SetId3Tags()
        {
            tagList.Items.Clear();
            tagList.BeginUpdate();

            AddTagItem("Title", fileInfo.Id3Tags.Title);
            AddTagItem("Artist", fileInfo.Id3Tags.Artist);
            AddTagItem("Album", fileInfo.Id3Tags.Album);
            AddTagItem("Year", fileInfo.Id3Tags.Date);
            AddTagItem("Track", fileInfo.Id3Tags.Track);
            AddTagItem("Genre", fileInfo.Id3Tags.Genre);
            AddTagItem("Description", fileInfo.Id3Tags.Description);
            AddTagItem("Comment", fileInfo.Id3Tags.Comment);
            tagList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // album art
            if (fileInfo.Id3Tags.AlbumArtTag != null)
            {
                saveImgLabel.Enabled = true;
                AlbumArt = fileInfo.Id3Tags.AlbumArtTag.AlbumArt;

                imgTypeTextBox.Text = fileInfo.Id3Tags.AlbumArtTag.Type;
                demTextBox.Text = string.Format("{0} x {1}", AlbumArt.Width, AlbumArt.Height);
            }
            else
            {
                saveImgLabel.Enabled = false;
                AlbumArt = null;
            }
            tagList.EndUpdate();
        }

        private void AddTagItem(string tagName, string tagValue)
        {
            tagList.Items.Add(tagName).SubItems.Add(tagValue);
        }

        #region InfoList Code

        private int InfoListSelectedIndex
        {
            get
            {
                if (infoList.FocusedItem != null)
                    return infoList.FocusedItem.Index;
                return -1;
            }
            set
            {
                infoList.SelectedItems.Clear();

                if (value > -1)
                {
                    infoList.Items[value].Selected = true;
                    infoList.Items[value].Focused = true;
                }
            }
        }
        private void searchTextbox_TextChanged(object sender, EventArgs e)
        {
            if (searchTextbox.TextLength < 1)
            {
                InfoListSelectedIndex = -1;
                return;
            }

            foreach (ListViewItem item in infoList.Items)
            {
                var itemText = item.Text.Replace('_', ' ').ToUpperInvariant();
                if (itemText.Contains(searchTextbox.Text.ToUpperInvariant()))
                {
                    InfoListSelectedIndex = item.Index;
                    //infoList.TopItem = item;
                    infoList.EnsureVisible(item.Index);
                    break;
                }
            }
        }
        private void infoContextMenu_Popup(object sender, EventArgs e)
        {
            if (InfoListSelectedIndex > -1)
            {
                menuItem1.Enabled = true;
                menuItem2.Enabled = true;
            }
            else
            {
                menuItem1.Enabled = false;
                menuItem2.Enabled = false;
            }
        }
        private void menuItem1_Click(object sender, EventArgs e)
        {
            // Copy Info Type
            Clipboard.SetText(infoList.Items[InfoListSelectedIndex].Text, TextDataFormat.UnicodeText);
        }
        private void menuItem2_Click(object sender, EventArgs e)
        {
            // Copy Info Value
            Clipboard.SetText(infoList.Items[InfoListSelectedIndex].SubItems[1].Text, TextDataFormat.UnicodeText);
        }

        #endregion

        #region TagList Code

        public int TagListSelectedIndex
        {
            get
            {
                if (tagList.FocusedItem != null)
                    return tagList.FocusedItem.Index;
                return -1;
            }
            set
            {
                tagList.SelectedItems.Clear();

                if (value > -1)
                {
                    tagList.Items[value].Selected = true;
                    tagList.Items[value].Focused = true;
                }
            }
        }
        private void tagContextMenu_Popup(object sender, EventArgs e)
        {
            menuItem4.Enabled = TagListSelectedIndex > -1;
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            // Copy Tag Value
            Clipboard.SetText(tagList.Items[TagListSelectedIndex].SubItems[1].Text, TextDataFormat.UnicodeText);
        }

        #endregion

        #region Events

        private void InfoForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            infoList.ContextMenu = infoContextMenu;
            tagList.ContextMenu = tagContextMenu;
        }

        private void saveImgLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                SupportMultiDottedExtensions = true,
                FileName = string.Format("{0} (Album Art).{1}", fileInfo.FileName, fileInfo.Id3Tags.AlbumArtTag.GetPictureExt()),
                Filter = string.Format("Image File (*.{0})|*.{0}", fileInfo.Id3Tags.AlbumArtTag.GetPictureExt())
            };

            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
            {
                try
                {
                    AlbumArt.Save(sfd.FileName, AlbumArt.RawFormat);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error in Saving Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void nameLabel_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(nameLabel.Text);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
