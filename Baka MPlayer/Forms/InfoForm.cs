using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class InfoForm : Form
    {
        public InfoForm(string resp)
        {
            InitializeComponent();

            mplayerProcessLabel.Text = resp;
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            infoList.ContextMenu = infoContextMenu;
            tagList.ContextMenu = tagContextMenu;

            RefreshInfo();
        }

        public void RefreshInfo()
        {
            nameLabel.Text = Path.GetFileNameWithoutExtension(Info.URL);
            setInfoList();
            setID3Tags();
        }

        private void saveImgLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fileName = Path.GetFileNameWithoutExtension(Info.FileName);
            var sfd = new SaveFileDialog
            {
                SupportMultiDottedExtensions = true,
                FileName = string.Format("{0} (Album Art).{1}", fileName, AlbumArt.RawFormat),
                Filter = string.Format("Image File (*.{0})|*.{0}", fileName)
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

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            var formGraphics = e.Graphics;
            var gradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 30, 30, 30), Color.Black, LinearGradientMode.Vertical);
            formGraphics.FillRectangle(gradientBrush, this.ClientRectangle);
        }

        private void tabPages_Paint(object sender, PaintEventArgs e)
        {
            var tab = (TabPage)sender;
            var formGraphics = e.Graphics;
            var gradientBrush = new LinearGradientBrush(tab.ClientRectangle, Color.FromArgb(255, 60, 60, 60), Color.Black, LinearGradientMode.Vertical);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);
        }

        #endregion

        #region InfoList Code

        public int infoList_SelectedIndex
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
                infoList_SelectedIndex = -1;
                return;
            }

            foreach (ListViewItem item in infoList.Items)
            {
                if (item.Text.Contains(searchTextbox.Text.ToUpper()))
                {
                    infoList_SelectedIndex = item.Index;
                    infoList.TopItem = item;
                    break;
                }
            }
        }
        private void infoContextMenu_Popup(object sender, EventArgs e)
        {
            if (infoList_SelectedIndex > -1)
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
            Clipboard.SetText(infoList.Items[infoList_SelectedIndex].Text, TextDataFormat.UnicodeText);
        }
        private void menuItem2_Click(object sender, EventArgs e)
        {
            // Copy Info Value
            Clipboard.SetText(infoList.Items[infoList_SelectedIndex].SubItems[1].Text, TextDataFormat.UnicodeText);
        }

        #endregion

        #region TagList Code

        public int tagList_SelectedIndex
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
            if (tagList_SelectedIndex > -1)
            {
                menuItem3.Enabled = true;
                menuItem4.Enabled = true;
            }
            else
            {
                menuItem3.Enabled = false;
                menuItem4.Enabled = false;
            }
        }
        private void menuItem3_Click(object sender, EventArgs e)
        {
            // Copy Tag
            Clipboard.SetText(tagList.Items[tagList_SelectedIndex].Text, TextDataFormat.UnicodeText);
        }
        private void menuItem4_Click(object sender, EventArgs e)
        {
            // Copy Tag Value
            Clipboard.SetText(tagList.Items[tagList_SelectedIndex].SubItems[1].Text, TextDataFormat.UnicodeText);
        }

        #endregion

        #region Set Data

        private void setInfoList()
        {
            infoList.Items.Clear();

            foreach (ID_Info info in Info.MiscInfo.OtherInfo)
            {
                infoList.Items.Add(info.ID).SubItems.Add(info.Value);
            }
            infoList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void setID3Tags()
        {
            tagList.Items.Clear();

            addTagItem("Title", Info.ID3Tags.Title);
            addTagItem("Artist", Info.ID3Tags.Artist);
            addTagItem("Album", Info.ID3Tags.Album);
            addTagItem("Year", Info.ID3Tags.Date);
            addTagItem("Track", Info.ID3Tags.Track > 0 ?
                Info.ID3Tags.Track.ToString() : string.Empty);
            addTagItem("Genre", Info.ID3Tags.Genre);
            addTagItem("Description", Info.ID3Tags.Description);
            addTagItem("Comment", Info.ID3Tags.Comment);
            tagList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // album art
            if (Info.ID3Tags.AlbumArt == null)
            {
                saveImgLabel.Enabled = false;
                AlbumArt = null;
            }
            else
            {
                saveImgLabel.Enabled = true;
                AlbumArt = Info.ID3Tags.AlbumArt;
            }
        }

        private void addTagItem(string tagName, string tagValue)
        {
            tagList.Items.Add(tagName).SubItems.Add(tagValue);
        }

        private Image AlbumArt
        {
            set { albumArtPicbox.Image = value; }
            get { return albumArtPicbox.Image; }
        }

        #endregion

    }
}
