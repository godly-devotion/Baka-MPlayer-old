using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Baka_MPlayer.Forms;
using MPlayer;

namespace Baka_MPlayer.Controls
{
    public partial class PlaylistControl : UserControl
    {
        #region Private Fields

        private MainForm mainForm;
        private IMPlayer mp;

        #endregion

        public PlaylistControl()
        {
            InitializeComponent();
        }

        public void Init(MainForm mainForm, IMPlayer mp)
        {
            this.mainForm = mainForm;
            this.mp = mp;

            playlistList.ContextMenu = fileContextMenu;
        }

        #region Accessors

        /// <summary>
        /// Gets the currently playing item
        /// </summary>
        public ListViewItem GetPlayingItem { get; private set; }

        /// <summary>
        /// Gets the selected item (returns null if none)
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
                    playlistList.TopItem = playlistList.Items[value > 0 ? value - 1 : 0];
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

        public bool Shuffle
        {
            set
            {
                if (value)
                {
                    RandomizeItems();
                    UpdateUI(true);
                }
                else
                {
                    RefreshPlaylist(true);
                }
            }
        }

        public void PlayPrevious()
        {
            var i = GetPlayingItem.Index - 1;

            if (i == -1)
                return;

            var file = Path.Combine(mp.FileInfo.GetDirectoryName, playlistList.Items[i].Text);
            if (File.Exists(file))
            {
                mp.OpenFile(file);
                return;
            }

            // file does not exist
            RefreshPlaylist(true);
            PlayFile(GetPlayingItem.Index - 1);
        }
        public void PlayNext()
        {
            var i = GetPlayingItem.Index + 1;

            if (i == GetTotalItems)
                return;

            var file = Path.Combine(mp.FileInfo.GetDirectoryName, playlistList.Items[i].Text);
            if (File.Exists(file))
            {
                mp.OpenFile(file);
                return;
            }

            // file does not exist
            RefreshPlaylist(true);
            PlayFile(GetPlayingItem.Index + 1);
        }

        public void PlayFile(int index)
        {
            var file = Path.Combine(mp.FileInfo.GetDirectoryName, playlistList.Items[index].Text);
            if (index < GetTotalItems && index > -1 && File.Exists(file))
            {
                mp.OpenFile(file);
                return;
            }
            MessageBox.Show("The file cannot be played because it does not exist.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        public void RefreshPlaylist(bool forceAll)
        {
            if (forceAll || playlistList.FindItemWithText(mp.FileInfo.GetName) == null)
            {
                FillPlaylist();
                mainForm.ShowPlaylist = !GetTotalItems.Equals(1);
                mainForm.CheckShuffleToolStripMenuItem = false;
            }
            UpdateUI(true);
        }

        public bool DisableInteraction
        {
            set
            {
                searchTextBox.Enabled = !value;
                playlistList.Enabled = !value;
            }
        }

        #endregion

        #region Functions

        private void OpenFile(string url)
        {
            Invoke((MethodInvoker)(() => mp.OpenFile(url)));
        }

        private void FillPlaylist()
        {
            playlistList.BeginUpdate();
            playlistList.Items.Clear();

            if (!mp.FileInfo.IsOnline)
            {
                var dirInfo = new DirectoryInfo(mp.FileInfo.GetDirectoryName);
                FileInfo[] files;

                if (showAllFilesToolStripMenuItem.Checked)
                    files = dirInfo.GetFiles("*.*");
                else
                    files = dirInfo.GetFiles("*" + Path.GetExtension(mp.FileInfo.FullFileName));

                var filesToSkipRegex = new Regex(@".*\.(ini|txt|db|jpg|png)$", RegexOptions.IgnoreCase);
                for (var i = 0; i <= files.Length - 1; i++)
                {
                    // skip useless files
                    if (!filesToSkipRegex.IsMatch(files[i].Name))
                        playlistList.Items.Add(files[i].Name);
                }
            }
            else
            {
                playlistList.Items.Add(mp.FileInfo.MovieName);
            }

            playlistList.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            playlistList.EndUpdate();
        }

        private void RandomizeItems()
        {
            playlistList.BeginUpdate();

            // make current file's index 0
            playlistList.Items.Insert(0, (ListViewItem)GetPlayingItem.Clone());
            GetPlayingItem.Remove();

            var randomGen = new Random();
            for (int i = 1; i <= GetTotalItems - 1; i++)
            {
                // generate random number
                var index = randomGen.Next(1, GetTotalItems);
                var item = playlistList.Items[i];
                playlistList.Items.Insert(index, (ListViewItem)item.Clone());
                playlistList.Items.Remove(item);
            }
            playlistList.EndUpdate();
        }

        private void RemoveAt(int index)
        {
            if (GetPlayingItem.Index == index)
            {
                MessageBox.Show("You can't remove the currently playing file.", "Playlist",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            playlistList.Items.RemoveAt(index);
            UpdateUI(false);
        }

        /// <summary>
        /// Deletes the playlist file permanently
        /// </summary>
        private void DeleteFile(int index)
        {
            // check for invalid index
            if (!(index > -1 && index < GetTotalItems))
                throw new IndexOutOfRangeException();

            if (index != GetPlayingItem.Index)
            {
                try
                {
                    File.Delete(Path.Combine(mp.FileInfo.GetDirectoryName, playlistList.Items[index].Text));
                    RemoveAt(SelectedIndex);
                }
                catch (IOException)
                {
                    MessageBox.Show(
                        "The file you are trying to delete seems to be in use by another program. Try closing the program that has this file open.",
                        "Couldn't Delete File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The file couldn't be deleted.\nReason: " + ex.Message,
                        "Couldn't delete file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You can't delete the currently playing file.", "Playlist",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateUI(bool selectCurrentFile)
        {
            GetPlayingItem = playlistList.FindItemWithText(mp.FileInfo.GetName);

            if (selectCurrentFile)
                SelectedIndex = GetPlayingItem.Index;

            mainForm.CallSetBackForwardControls();
            playlistList_SelectedIndexChanged(null, null);
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
                    if (item.Text.ToUpperInvariant().Contains(searchTextBox.Text.ToUpperInvariant()))
                    {
                        SelectedIndex = item.Index;
                        return;
                    }
                }
            }
            else
            {
                SelectedIndex = GetPlayingItem.Index;
            }
        }
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || SelectedIndex.Equals(-1)) return;

            var newUrl = Path.Combine(mp.FileInfo.GetDirectoryName, GetSelectedItem.Text);
            if (!newUrl.Equals(mp.FileInfo.Url, StringComparison.OrdinalIgnoreCase))
            {
                if (File.Exists(newUrl))
                {
                    OpenFile(newUrl);
                    searchTextBox.Text = string.Empty;
                    playlistList.Focus();
                }
                else
                {
                    MessageBox.Show(
                        GetSelectedItem.Text + " does not exist in this folder! It was either modified or deleted.",
                        "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    searchTextBox.Focus();
                }
            }
        }

        // playlistList Events

        private void playlistList_DoubleClick(object sender, EventArgs e)
        {
            PlayFile(GetSelectedItem.Index);
        }
        private void playlistList_DragDrop(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                // Retrieve the index of the insertion mark
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

                UpdateUI(false);
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
            {
                if (e.Shift)
                    DeleteFile(SelectedIndex);
                else
                    RemoveAt(SelectedIndex);
            }
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
            var inputForm = new InputForm(
                "Enter the file number you want to play:\nNote: Value must be between 1 - " + GetTotalItems,
                "Enter File Number",
                (GetPlayingItem.Index + 1).ToString(CultureInfo.InvariantCulture));

            if (inputForm.ShowDialog(this) == DialogResult.OK)
            {
                var i = Functions.TryParse.ToInt(inputForm.GetInputText) - 1;

                if (i >= 0 && i < GetTotalItems)
                {
                    SelectedIndex = i;

                    // play selected item
                    var newUrl = Path.Combine(mp.FileInfo.GetDirectoryName, GetSelectedItem.Text);

                    if (File.Exists(newUrl) && SelectedIndex != GetPlayingItem.Index)
                        OpenFile(newUrl);
                }
            }
            inputForm.Dispose();
        }
        private void showAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshPlaylist(true);
        }
        private void refreshPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshPlaylist(true);
        }

        // context menu

        private void fileContextMenu_Popup(object sender, EventArgs e)
        {
            if (SelectedIndex != -1)
            {
                menuItem1.Enabled = true;
                menuItem3.Enabled = true;
                menuItem2.Enabled = true;
            }
            else
            {
                menuItem1.Enabled = false;
                menuItem3.Enabled = false;
                menuItem2.Enabled = false;
            }
        }
        private void menuItem1_Click(object sender, EventArgs e)
        {
            // Remove from Playlist
            RemoveAt(SelectedIndex);
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            // Delete from Disk
            DeleteFile(SelectedIndex);
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            // Refresh
            RefreshPlaylist(true);
        }

        #endregion
    }
}