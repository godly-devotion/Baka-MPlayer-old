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
        public int GetTotalItems { get { return playlistList.Items.Count; } }

        /// <summary>
        /// Gets the current file's name
        /// </summary>
        public string GetCurrentFile { get; private set; }

        /// <summary>
        /// Gets or sets the currently selected index
        /// </summary>
        public int SelectedIndex
        {//playlistList.SelectedIndices[0];
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
                    playlistList.FocusedItem = playlistList.Items[value];               
            }
        }

#endregion
#region Methods

        private void OpenFile(string dirString)
        {
            Invoke((MethodInvoker)(() => mplayer.OpenFile(dirString)));
        }

        public void SetPlaylist()
        {
            playlistList.BeginUpdate();

            if (File.Exists(Info.URL))
            {
                if (playlistList.Items.IndexOfKey(Path.GetFileName(Info.URL)).Equals(-1))
                    refreshRequired = true;

                if (refreshRequired)
                    FillPlaylist();

                SelectedIndex = playlistList.FindItemWithText(Path.GetFileName(Info.URL)).Index;
                mainForm.SetPlaylistButtonEnable(true);
                mainForm.SetPlaylistVisibility(GetTotalItems > 1);
            }
            else
            {
                mainForm.SetPlaylistButtonEnable(false);
                mainForm.SetPlaylistVisibility(false);
                playlistList.Items.Clear();
            }

			playlistList.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            playlistList.EndUpdate();

            if (refreshRequired)
                mainForm.SetShuffleCheckState(false);

            // set playlist index
            GetPlaylistIndex = SelectedIndex;
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

        public void Shuffle()
        {
            var randomGen = new Random();
            var maxVal = playlistList.Items.Count;

            playlistList.BeginUpdate();
            // make current file's index 0
            playlistList.Items.RemoveAt(SelectedIndex);
            playlistList.Items.Insert(0, GetCurrentFile);

            for (int i = 1; i <= maxVal - 1; i++)
            {
                // generate two random numbers
                var index1 = randomGen.Next(1, maxVal);
                var index2 = randomGen.Next(1, maxVal);
                // swap the two items
                var temp = playlistList.Items[index1];
                playlistList.Items[index1] = playlistList.Items[index2];
                playlistList.Items[index2] = temp;
            }
            playlistList.EndUpdate();
            refreshRequired = false;
            // current playing file is now first
            playlistList.FocusedItem = playlistList.Items[0];
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
                GetPlaylistIndex + 1.ToString());

            if (inputBox.ShowDialog(this) == DialogResult.OK)
            {
                int i; int.TryParse(inputBox.GetInputText, out i); --i;
                if (i >= 0 && i < GetTotalItems)
                {
                    SelectedIndex = i;
                    PlaySelectedItem();
                }
            }
        }

        private void showAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshRequired = true;
            SetPlaylist();
        }

        private void refreshPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshRequired = true;
            SetPlaylist();
        }

#endregion

#region searchTextBox Events

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            // only one item, nothing to search
            if (playlistList.Items.Count < 2) return;

            if (searchTextBox.TextLength > 0)
            {
                var item = playlistList.FindItemWithText(searchTextBox.Text);
                if (item != null)
                    playlistList.FocusedItem = item;
            }
            else
            {
                playlistList.FocusedItem = playlistList.Items[GetPlaylistIndex];
            }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || playlistList.Items.Count < 2) return;

            var newURL = Path.GetDirectoryName(Info.URL) + '\\' + GetCurrentFile;
            if (SelectedIndex != -1 && newURL != Info.URL)
            {
                if (File.Exists(newURL))
                {
                    OpenFile(newURL);
                    searchTextBox.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show(playlistList.FocusedItem.Text + " does not exist in this folder! It was either modified or deleted.",
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
                if (targetIndex == -1)
                {
                    return;
                }

                // Retrieve the dragged item.
                var draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                // Insert a copy of the dragged item at the target index.
                // A copy must be inserted before the original item is removed
                // to preserve item index values. 
                playlistList.Items.Insert(targetIndex, (ListViewItem)draggedItem.Clone());

                // Remove the original copy of the dragged item.
                playlistList.Items.Remove(draggedItem);
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
                var targetIndex = targetItem.Index;
                var fromIndex = SelectedIndex; //playlistList.SelectedIndices[0];

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

        private void playlistList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                playlistList.Items.RemoveAt(SelectedIndex);
        }

        private void playlistList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFileLabel.Text = string.Format("File {0} of {1}", SelectedIndex + 1, GetTotalItems);
        }

#endregion

    }
}