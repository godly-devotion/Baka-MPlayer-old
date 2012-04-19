using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Baka_MPlayer.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class PlaylistControl : UserControl
    {
        private MainForm mainForm;
        private MPlayer mplayer;

        // specifies if the playlist needs to refresh
        // (directory change, file deleted/added, etc)
        public bool refreshRequired;

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
        /// Gets current file's index in the playlist
        /// </summary>
        public int GetPlaylistIndex { get; private set; }

        /// <summary>
        /// Gets the total number of files on the playlist
        /// </summary>
        public int GetTotalItems
        {
            get { return playlistList.Items.Count; }
        }

        /// <summary>
        /// Gets the current file's name
        /// </summary>
        public string GetCurrentFile { get; private set; }

        /// <summary>
        /// Gets or sets the currently selected index
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                if (playlistList.FocusedItem != null)
                    return playlistList.FocusedItem.Index;
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

        public bool DisableInteraction
        {
            set
            {
                searchTextBox.Enabled = !value;
                playlistList.Enabled = !value;
            }
        }

        #endregion

        #region Methods

        private void OpenFile(string dirString)
        {
            Invoke((MethodInvoker) (() => mplayer.OpenFile(dirString)));
        }

        public void SetPlaylist()
        {
            // Set GetCurrentFile
            GetCurrentFile = Path.GetFileName(Info.URL);

            playlistList.BeginUpdate();

            if (File.Exists(Info.URL))
            {
                if (!refreshRequired)
                    refreshRequired = playlistList.FindItemWithText(GetCurrentFile) == null;

                if (refreshRequired)
                    FillPlaylist();

                SelectedIndex = playlistList.FindItemWithText(GetCurrentFile).Index;
                mainForm.SetPlaylistButtonEnable(true);
                mainForm.ShowPlaylist = (refreshRequired || mainForm.ShowPlaylist) && GetTotalItems > 1;
            }
            else
            {
                mainForm.SetPlaylistButtonEnable(false);
                mainForm.ShowPlaylist = false;
                playlistList.Items.Clear();
            }

            playlistList.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            playlistList.EndUpdate();

            if (refreshRequired)
            {
                mainForm.SetShuffleCheckState(false);
                refreshRequired = false;
            }

            // set playlist index
            GetPlaylistIndex = SelectedIndex;
            mainForm.CallSetBackForwardControls();
        }

        private void FillPlaylist()
        {
            playlistList.Items.Clear();
            var dirInfo = new DirectoryInfo(Path.GetDirectoryName(Info.URL));
            FileInfo[] files;

            if (showAllFilesToolStripMenuItem.Checked)
                files = dirInfo.GetFiles("*.*");
            else
                files = dirInfo.GetFiles('*' + Path.GetExtension(Info.URL));

            for (var i = 0; i <= files.Length - 1; i++)
            {
                // skip .db files (useless files)
                if (files[i].Name.EndsWith(".db") == false)
                    playlistList.Items.Add(files[i].Name);
            }
        }

        public void Shuffle()
        {
            var randomGen = new Random();
            var maxVal = playlistList.Items.Count;

            playlistList.BeginUpdate();
            // make current file's index 0
            var curItem = playlistList.Items[GetPlaylistIndex];
            playlistList.Items.Insert(0, (ListViewItem) curItem.Clone());
            playlistList.Items.Remove(curItem);

            for (int i = 1; i <= maxVal - 1; i++)
            {
                // generate random number
                var index = randomGen.Next(1, maxVal);
                var item = playlistList.Items[i];
                playlistList.Items.Insert(index, (ListViewItem) item.Clone());
                playlistList.Items.Remove(item);
            }
            playlistList.EndUpdate();
            refreshRequired = false;
            // current playing file is now first
            SelectedIndex = 0;
            GetPlaylistIndex = 0;
        }

        private void UpdateUI()
        {
            GetPlaylistIndex = playlistList.FindItemWithText(GetCurrentFile).Index;
            playlistList_SelectedIndexChanged(null, null);
            mainForm.CallSetBackForwardControls();
        }

        private void PlaySelectedItem()
        {
            var newUrl = Path.GetDirectoryName(Info.URL) + "\\" + playlistList.Items[SelectedIndex].Text;
            if (SelectedIndex != -1 && File.Exists(newUrl) && SelectedIndex != GetPlaylistIndex)
                OpenFile(newUrl);
        }

        public void PlayFile(int index)
        {
            var currentDir = Path.GetDirectoryName(Info.URL);
            mplayer.OpenFile(string.Format("{0}\\{1}", currentDir, playlistList.Items[index].Text));
        }

        public void PlayFile(string name)
        {
            var currentDir = Path.GetDirectoryName(Info.URL);
            var fileName = playlistList.FindItemWithText(name).Text;
            mplayer.OpenFile(string.Format("{0}\\{1}", currentDir, fileName));
        }

        public void PlayPreviousFile()
        {
            if (GetPlaylistIndex > 0)
                PlayFile(GetPlaylistIndex - 1);
        }

        public void PlayNextFile()
        {
            if (GetPlaylistIndex < GetTotalItems - 1)
                PlayFile(GetPlaylistIndex + 1);
        }

        public void RemoveAt(int index)
        {
            if (GetPlaylistIndex == index)
            {
                if (SelectedIndex + 1 >= GetTotalItems)
                    mplayer.Stop();
                else
                    PlayNextFile();
            }
            playlistList.Items.RemoveAt(index);
            UpdateUI();
        }

        public void RefreshPlaylist()
        {
            refreshRequired = true;
            SetPlaylist();
        }

        #endregion

        #region Playlist Options

        private void currentFileButton_Click(object sender, EventArgs e)
        {
            SelectedIndex = -1;
            SelectedIndex = GetPlaylistIndex;
        }

        private void currentFileLabel_MouseDown(object sender, MouseEventArgs e)
        {
            var inputBox = new InputForm(
                "Enter the file number you want to play:\nNote: Value must be between 1 - " + GetTotalItems,
                "Enter File Number",
                (GetPlaylistIndex + 1).ToString());

            if (inputBox.ShowDialog(this) == DialogResult.OK)
            {
                int i;
                int.TryParse(inputBox.GetInputText, out i);
                --i;
                if (i >= 0 && i < GetTotalItems)
                {
                    SelectedIndex = i;
                    PlaySelectedItem();
                }
            }
        }

        private void showAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshPlaylist();
        }

        private void refreshPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshPlaylist();
        }

        #endregion

        #region Context Menu

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
            RefreshPlaylist();
        }

        #endregion

        #region searchTextBox Events

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
                SelectedIndex = GetPlaylistIndex;
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || SelectedIndex.Equals(-1)) return;

            var newURL = Path.GetDirectoryName(Info.URL) + '\\' + playlistList.Items[SelectedIndex].Text;
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
                        playlistList.FocusedItem.Text +
                        " does not exist in this folder! It was either modified or deleted.",
                        "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    searchTextBox.Focus();
                }
            }
        }

        #endregion

        #region PlaylistList Events

        private void playlistList_DoubleClick(object sender, EventArgs e)
        {
            PlaySelectedItem();
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
                var draggedItem = (ListViewItem) e.Data.GetData(typeof (ListViewItem));

                // Insert a copy of the dragged item at the target index.
                // A copy must be inserted before the original item is removed
                // to preserve item index values. 
                playlistList.Items.Insert(targetIndex, (ListViewItem) draggedItem.Clone());

                // Remove the original copy of the dragged item.
                playlistList.Items.Remove(draggedItem);

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
                searchTextBox.AppendText(e.KeyChar.ToString());
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

        #endregion
    }
}