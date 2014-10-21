using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MPlayer;

namespace Baka_MPlayer.Forms
{
    public partial class InfoForm : Form
    {
        private readonly IMPlayer mp;

        #region Accessor

        private Image AlbumArt
        {
            set { albumArtPicbox.Image = value; }
            get { return albumArtPicbox.Image; }
        }

        #endregion

        public InfoForm(IMPlayer mp)
        {
            InitializeComponent();

            this.mp = mp;
        }

        #region Functions

        public void RefreshInfo()
        {
            nameLabel.Text = mp.FileInfo.MovieName;

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
            nameItem.SubItems.Add(mp.FileInfo.GetName);

            var typeItem = new ListViewItem("File format", infoList.Groups[0]);
            typeItem.SubItems.Add(mp.FileInfo.FileFormat);

            var sizeItem = new ListViewItem("File size", infoList.Groups[0]);
            if (mp.FileInfo.IsOnline)
                sizeItem.SubItems.Add("n/a");
            else
                sizeItem.SubItems.Add(Functions.IO.GetFileSize(mp.FileInfo.Url, 2));

            var lengthItem = new ListViewItem("Media length", infoList.Groups[0]);
            lengthItem.SubItems.Add(Functions.Time.ConvertSecondsToTime(mp.CurrentStatus.TotalLength));

            var audCodecItem = new ListViewItem("Audio codec", infoList.Groups[0]);
            if (!string.IsNullOrEmpty(mp.FileInfo.AudioCodec))
                audCodecItem.SubItems.Add(mp.FileInfo.AudioCodec);
            else
                audCodecItem.SubItems.Add("n/a");

            var vidCodecItem = new ListViewItem("Video codec", infoList.Groups[0]);
            if (!string.IsNullOrEmpty(mp.FileInfo.VideoCodec))
                vidCodecItem.SubItems.Add(mp.FileInfo.VideoCodec);
            else
                vidCodecItem.SubItems.Add("n/a");

            var dimensionsItem = new ListViewItem("Video dimensions", infoList.Groups[0]);
            dimensionsItem.SubItems.Add(string.Format("{0} x {1}", mp.FileInfo.VideoWidth, mp.FileInfo.VideoHeight));

            var modifiedItem = new ListViewItem("Last modified", infoList.Groups[0]);
            if (mp.FileInfo.IsOnline)
                modifiedItem.SubItems.Add("n/a");
            else
                modifiedItem.SubItems.Add(File.GetLastWriteTime(mp.FileInfo.Url).ToLocalTime().ToString(CultureInfo.InvariantCulture));

            infoList.Items.AddRange(new[]
            {
                nameItem, typeItem, sizeItem, lengthItem, audCodecItem, vidCodecItem, dimensionsItem, modifiedItem
            });
        }

        private void SetTagsInfo()
        {
            foreach (var info in mp.FileInfo.IdInfos)
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

            AddTagItem("Title", mp.FileInfo.Id3Tags.Title);
            AddTagItem("Artist", mp.FileInfo.Id3Tags.Artist);
            AddTagItem("Album", mp.FileInfo.Id3Tags.Album);
            AddTagItem("Year", mp.FileInfo.Id3Tags.Date);
            AddTagItem("Track", mp.FileInfo.Id3Tags.Track);
            AddTagItem("Genre", mp.FileInfo.Id3Tags.Genre);
            AddTagItem("Description", mp.FileInfo.Id3Tags.Description);
            AddTagItem("Comment", mp.FileInfo.Id3Tags.Comment);
            tagList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // album art
            if (mp.FileInfo.Id3Tags.AlbumArtTag != null)
            {
                saveImgLabel.Enabled = true;
                AlbumArt = mp.FileInfo.Id3Tags.AlbumArtTag.AlbumArt;

                imgTypeTextBox.Text = mp.FileInfo.Id3Tags.AlbumArtTag.Type;
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

        #endregion

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

            mplayerProcessLabel.Text = mp.GetProcessState();

            RefreshInfo();
        }

        private void saveImgLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                SupportMultiDottedExtensions = true,
                FileName = string.Format("{0} (Album Art).{1}", mp.FileInfo.FileName, mp.FileInfo.Id3Tags.AlbumArtTag.GetPictureExt()),
                Filter = string.Format("Image File (*.{0})|*.{0}", mp.FileInfo.Id3Tags.AlbumArtTag.GetPictureExt())
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
