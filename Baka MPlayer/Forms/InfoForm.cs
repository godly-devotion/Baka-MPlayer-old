using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class InfoForm : Form
    {
        #region GetFileType Code

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        private string getFileType(string path)
        {
            var x = new SHFILEINFO();
            SHGetFileInfo(path, 0, ref x, (uint)Marshal.SizeOf(x), 0x400);
            return x.szTypeName;
        }

        #endregion

        #region Accessor

        private Image AlbumArt
        {
            set { albumArtPicbox.Image = value; }
            get { return albumArtPicbox.Image; }
        }

        #endregion

        #region Constructor

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

        #endregion

        #region Functions

        private void SetGeneralInfo()
        {
            // file name
            var nameItem = new ListViewItem("File name", infoList.Groups[0]);
            nameItem.SubItems.Add(Info.FullFileName);

            // file type
            var type = getFileType(Info.URL);
            if (string.IsNullOrEmpty(type)) {
                type = Path.GetExtension(Info.FullFileName).Substring(1);
            }
            var typeItem = new ListViewItem("File type", infoList.Groups[0]);
            typeItem.SubItems.Add(type);

            // file size
            var sizeItem = new ListViewItem("File size", infoList.Groups[0]);
            if (Info.FileExists)
                sizeItem.SubItems.Add(Functions.IO.GetFileSize(Info.URL, 2));
            else
                sizeItem.SubItems.Add("Not Available");

            // last modified
            var modifiedItem = new ListViewItem("Last modified", infoList.Groups[0]);
            if (Info.FileExists)
                modifiedItem.SubItems.Add(File.GetLastWriteTime(Info.URL).ToLocalTime().ToString());
            else
                modifiedItem.SubItems.Add("Not Available");

            infoList.Items.AddRange(new[]{nameItem, typeItem, sizeItem, modifiedItem});
        }

        private void SetTagsInfo()
        {
            foreach (ID_Info info in Info.MiscInfo.OtherInfo)
            {
                var item = new ListViewItem(info.ID, infoList.Groups[1]);
                item.SubItems.Add(info.Value);
                infoList.Items.Add(item);
            }
        }

        private void SetId3Tags()
        {
            tagList.Items.Clear();
            tagList.BeginUpdate();

            AddTagItem("Title", Info.ID3Tags.Title);
            AddTagItem("Artist", Info.ID3Tags.Artist);
            AddTagItem("Album", Info.ID3Tags.Album);
            AddTagItem("Year", Info.ID3Tags.Date);
            AddTagItem("Track", Info.ID3Tags.Track);
            AddTagItem("Genre", Info.ID3Tags.Genre);
            AddTagItem("Description", Info.ID3Tags.Description);
            AddTagItem("Comment", Info.ID3Tags.Comment);
            tagList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // album art
            if (Info.ID3Tags.AlbumArtTag.AlbumArt == null)
            {
                saveImgLabel.Enabled = false;
                AlbumArt = null;
            }
            else
            {
                saveImgLabel.Enabled = true;
                AlbumArt = Info.ID3Tags.AlbumArtTag.AlbumArt;

                imgTypeTextBox.Text = Info.ID3Tags.AlbumArtTag.Type;
                demTextBox.Text = string.Format("{0} x {1}", AlbumArt.Width, AlbumArt.Height);
            }
            tagList.EndUpdate();
        }

        private void AddTagItem(string tagName, string tagValue)
        {
            tagList.Items.Add(tagName).SubItems.Add(tagValue);
        }

        public void RefreshInfo()
        {
            nameLabel.Text = Path.GetFileNameWithoutExtension(Info.FullFileName);

            // set Media Info tagpage
            infoList.BeginUpdate();
            infoList.Items.Clear();

            SetGeneralInfo();
            SetTagsInfo();

            infoList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            infoList.EndUpdate();

            // set ID3 Tags tabpage
            SetId3Tags();
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
                var itemText = item.Text.Replace('_', ' ').ToLower();
                if (itemText.Contains(searchTextbox.Text.ToLower()))
                {
                    infoList_SelectedIndex = item.Index;
                    //infoList.TopItem = item;
                    infoList.EnsureVisible(item.Index);
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

        #region Events

        private void saveImgLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fileName = Path.GetFileNameWithoutExtension(Info.FullFileName);
            var sfd = new SaveFileDialog
            {
                SupportMultiDottedExtensions = true,
                FileName = string.Format("{0} (Album Art).{1}", fileName, Info.ID3Tags.AlbumArtTag.GetPictureExt()),
                Filter = string.Format("Image File (*.{0})|*.{0}", Info.ID3Tags.AlbumArtTag.GetPictureExt())
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
            if (nameLabel.Text != "Baka MPlayer")
                Clipboard.SetText(nameLabel.Text);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
