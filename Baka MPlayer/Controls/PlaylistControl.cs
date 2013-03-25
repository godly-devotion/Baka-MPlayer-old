using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Baka_MPlayer.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class PlaylistControl : UserControl
    {
        #region Variables

        private MainForm mainForm;
        private MPlayer mplayer;
        public ListViewItem GetPlayingItem;

        #endregion

        #region Constructor

        public PlaylistControl()
        {
            InitializeComponent();
        }

        public void Init(MainForm mainForm, MPlayer mplayer)
        {
            this.mainForm = mainForm;
            this.mplayer = mplayer;

            playlistList.ContextMenu = fileContextMenu;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the selected item (if none returns null)
        /// </summary>
        public ListViewItem GetSelectedItem
        {
            get { return playlistList.FocusedItem; }
        }

        /// <summary>
        /// Gets or sets the selected index
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                if (GetSelectedItem != null)
                    return GetSelectedItem.Index;
                return -1;
            }
            set
            {
                playlistList.SelectedItems.Clear();

                if (value > -1)
                {
                    playlistList.Items[value].Selected = true;
                    playlistList.Items[value].Focused = true;
                    playlistList.TopItem = playlistList.Items[value];
                    playlistList_SelectedIndexChanged(null, null);
                }
            }
        }

        /// <summary>
        /// Gets the total number of items
        /// </summary>
        public int GetTotalItems
        {
            get { return playlistList.Items.Count; }
        }

        #endregion

        #region API

        public void SetPlaylist(bool forceRefresh)
        {
            playlistList.BeginUpdate();

            if (Info.FileExists)
            {
                if (!forceRefresh)
                    forceRefresh = playlistList.FindItemWithText(Info.FullFileName) == null;

                if (forceRefresh)
                {
                    mainForm.SetShuffleCheckState(false);
                    FillPlaylist();
                }
                
                GetPlayingItem = playlistList.FindItemWithText(Info.FullFileName);
                SelectedIndex = GetPlayingItem.Index;

                mainForm.SetPlaylistButtonEnable(true);
                mainForm.ShowPlaylist = (forceRefresh || mainForm.ShowPlaylist) && GetTotalItems > 1;
            }
            else
            {
                playlistList.Items.Clear();
                playlistList.Items.Add(Info.FullFileName);

                GetPlayingItem = playlistList.FindItemWithText(Info.FullFileName);
                SelectedIndex = GetPlayingItem.Index;

                mainForm.SetPlaylistButtonEnable(false);
                mainForm.ShowPlaylist = false;
            }

            playlistList.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            playlistList.EndUpdate();

            // playlist complete, do other stuff
            mainForm.CallSetBackForwardControls();
        }

        public void PlayNextFile()
        {
            if (GetPlayingItem.Index < GetTotalItems - 1)
                PlayFile(GetPlayingItem.Index + 1);
        }

        public void PlayPreviousFile()
        {
            if (GetPlayingItem.Index > 0)
                PlayFile(GetPlayingItem.Index - 1);
        }

        public void PlayFile(int index)
        {
            mplayer.OpenFile(string.Format("{0}\\{1}", Info.GetDirectoryName, playlistList.Items[index].Text));
        }

        public void PlayFile(string name)
        {
            var fileName = playlistList.FindItemWithText(name).Text;
            mplayer.OpenFile(string.Format("{0}\\{1}", Info.GetDirectoryName, fileName));
        }

        public void RandomizeItems()
        {
            playlistList.BeginUpdate();

            // make current file's index 0
            playlistList.Items.Insert(0, (ListViewItem) GetPlayingItem.Clone());
            GetPlayingItem.Remove();

            var randomGen = new Random();
            for (int i = 1; i <= GetTotalItems - 1; i++)
            {
                // generate random number
                var index = randomGen.Next(1, GetTotalItems);
                var item = playlistList.Items[i];
                playlistList.Items.Insert(index, (ListViewItem) item.Clone());
                playlistList.Items.Remove(item);
            }
            playlistList.EndUpdate();

            // current playing file is now first
            SelectedIndex = 0;
            GetPlayingItem = playlistList.Items[0];
        }

        public void DisableInteraction(bool disable)
        {
            searchTextBox.Enabled = !disable;
            playlistList.Enabled = !disable;
        }

        #endregion

        #region Functions

        private void OpenFile(string url)
        {
            Invoke((MethodInvoker)(() => mplayer.OpenFile(url)));
        }

        private void FillPlaylist()
        {
            playlistList.Items.Clear();
            var dirInfo = new DirectoryInfo(Info.GetDirectoryName);
            FileInfo[] files;

            if (showAllFilesToolStripMenuItem.Checked)
                files = dirInfo.GetFiles("*.*");
            else
                files = dirInfo.GetFiles('*' + Path.GetExtension(Info.FullFileName));

            for (var i = 0; i <= files.Length - 1; i++) {
                // skip .db files (useless files)
                if (!files[i].Name.EndsWith(".db"))
                    playlistList.Items.Add(files[i].Name);
            }
        }

        private void UpdateUI()
        {
            GetPlayingItem = playlistList.FindItemWithText(Info.FullFileName);
            playlistList_SelectedIndexChanged(null, null);
            mainForm.CallSetBackForwardControls();
        }

        private void RemoveAt(int index)
        {
            if (GetPlayingItem.Index.Equals(index))
            {
                if (SelectedIndex + 1 >= GetTotalItems)
                    mplayer.Stop();
                else
                {
                    //PlayNextFile();
                    MessageBox.Show("You can't remove the playing file!", "A no no", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            playlistList.Items.RemoveAt(index);
            UpdateUI();
        }

        #endregion

        #region Events

        // searchTextBox Events

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            // only one item, nothing to search
            if (GetTotalItems < 2) return;

            if (searchTextBox.TextLength > 0)
            {
                foreach (ListViewItem item in playlistList.Items)
                {
                    if (item.Text.ToLower().Contains(searchTextBox.Text.ToLower()))
                    {
                        SelectedIndex = item.Index;
                        return;
                    }
                }
            }
            else
                SelectedIndex = GetPlayingItem.Index;
        }
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || SelectedIndex.Equals(-1)) return;

            var newURL = string.Format("{0}\\{1}", Info.GetDirectoryName, GetSelectedItem.Text);
            if (newURL != Info.URL)
            {
                if (File.Exists(newURL))
                {
                    OpenFile(newURL);
                    searchTextBox.Text = string.Empty;
                    playlistList.Focus();
                }
                else
                {
                    MessageBox.Show(
                        GetSelectedItem.Text +
                        " does not exist in this folder! It was either modified or deleted.",
                        "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    searchTextBox.Focus();
                }
            }
        }

        // playlistList Events

        private void playlistList_DoubleClick(object sender, EventArgs e)
        {
            // play selected item
            var newURL = string.Format("{0}\\{1}", Info.GetDirectoryName, GetSelectedItem.Text);

            if (File.Exists(newURL) && SelectedIndex != GetPlayingItem.Index)
                OpenFile(newURL);
        }
        private void playlistList_DragDrop(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                // Retrieve the index of the insertion mark;
                //int targetIndex = Playlist.InsertionMark.Index;
                var targetPoint = playlistList.PointToClient(new Point(e.X, e.Y));

                var targetItem = playlistList.GetItemAt(targetPoint.X, targetPoint.Y);

                var targetIndex = playlistList.Items.IndexOf(targetItem);

                // If the insertion mark is not visible, exit the method.
                if (targetIndex.Equals(-1))
                    return;

                // Retrieve the dragged item.
                var draggedItem = (ListViewItem) e.Data.GetData(typeof(ListViewItem));
                
                // Insert a copy of the dragged item at the target index.
                // A copy must be inserted before the original item is removed
                // to preserve item index values. 
                playlistList.Items.Insert(targetIndex, (ListViewItem)draggedItem.Clone());

                // Remove the original copy of the dragged item.
                draggedItem.Remove();

                UpdateUI();
            }
        }
        private void playlistList_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        private void playlistList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                var targetPoint = playlistList.PointToClient(new Point(e.X, e.Y));
                var targetItem = playlistList.GetItemAt(targetPoint.X, targetPoint.Y);
                if (targetItem == null)
                    return;
                var targetIndex = targetItem.Index;
                var fromIndex = SelectedIndex;

                if (fromIndex != targetIndex)
                {
                    playlistList.Items[fromIndex].Selected = false;
                    playlistList.Items[targetIndex].Selected = true;
                }
            }
        }
        private void playlistList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            playlistList.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void playlistList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar))
            {
                searchTextBox.AppendText(e.KeyChar.ToString(CultureInfo.InvariantCulture));
                searchTextBox.Focus();
            }
        }
        private void playlistList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                RemoveAt(SelectedIndex);
        }
        private void playlistList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFileLabel.Text = string.Format("File {0} of {1}", SelectedIndex + 1, GetTotalItems);
        }

        // playlist Options

        private void currentFileButton_Click(object sender, EventArgs e)
        {
            SelectedIndex = -1;
            SelectedIndex = GetPlayingItem.Index;
        }
        private void currentFileLabel_MouseDown(object sender, MouseEventArgs e)
        {
            var inputBox = new InputForm(
                "Enter the file number you want to play:\nNote: Value must be between 1 - " + GetTotalItems,
                "Enter File Number",
                (GetPlayingItem.Index + 1).ToString(CultureInfo.InvariantCulture));

            if (inputBox.ShowDialog(this) == DialogResult.OK)
            {
                var i = Functions.TryParse.ParseInt(inputBox.GetInputText) - 1;

                if (i >= 0 && i < GetTotalItems)
                {
                    SelectedIndex = i;

                    // play selected item
                    var newURL = string.Format("{0}\\{1}", Info.GetDirectoryName, GetSelectedItem.Text);

                    if (File.Exists(newURL) && SelectedIndex != GetPlayingItem.Index)
                        OpenFile(newURL);
                }
            }
        }
        private void showAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPlaylist(true);
        }
        private void refreshPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPlaylist(true);
        }

        // context menu

        private void fileContextMenu_Popup(object sender, EventArgs e)
        {
            if (SelectedIndex != -1)
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
            // Remove from Playlist
            RemoveAt(SelectedIndex);
        }
        private void menuItem2_Click(object sender, EventArgs e)
        {
            // Refresh
            SetPlaylist(true);
        }

        #endregion
    }
}