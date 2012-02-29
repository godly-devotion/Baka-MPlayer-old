using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            infoList.ContextMenu = infoContextMenu;

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
            this.Dispose();
        }

        #region InfoList Code

        /// <summary>
        /// Gets or sets the currently selected index
        /// </summary>
        public int SelectedIndex
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
                SelectedIndex = -1;
                return;
            }

            foreach (ListViewItem item in infoList.Items)
            {
                if (item.Name.Contains(searchTextbox.Text) || item.Name.Contains(searchTextbox.Text))
                {
                    SelectedIndex = item.Index;
                    break;
                }
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {

        }

        #endregion

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

        #region Set Data

        private void setInfoList()
        {

        }

        private void setID3Tags()
        {
            musicTitle.Text = Info.ID3Tags.Title;
            musicArtist.Text = Info.ID3Tags.Artist;
            musicAlbum.Text = Info.ID3Tags.Album;
            musicYear.Text = Info.ID3Tags.Date;
            musicTrack.Text = Info.ID3Tags.Track.ToString();
            musicGenre.Text = Info.ID3Tags.Genre;
            musicComment.Text = Info.ID3Tags.Comment;

            // album art

        }

        private Image AlbumArt
        {
            set { albumArtPicbox.Image = value; }
            get { return albumArtPicbox.Image; }
        }

        #endregion
    }
}
