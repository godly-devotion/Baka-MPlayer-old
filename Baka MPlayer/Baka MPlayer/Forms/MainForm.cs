using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class MainForm : Form
    {
        #region Display Output

        public void SetOutput(string output)
        {

        }

        public void ClearOutput()
        {
        }

        #endregion



        public void SetShuffleCheckState(bool check)
        {
            Invoke((MethodInvoker)(() => shuffleToolStripMenuItem.Checked = check));
        }

        public void SetPlaylistButtonEnable(bool enable)
        {
            Invoke((MethodInvoker)(() => playlistButton.Enabled = enable));
        }

        public void SetPlaylistVisibility(bool show)
        {
            //Invoke((MethodInvoker)(() => ShowPlaylist = show));
        }



        public MainForm()
        {
            InitializeComponent();
        }

        public void CallMediaOpened()
        {
            Invoke((MethodInvoker)MediaOpened);
        }
        private void MediaOpened()
        {

        }

        public void CallMediaEnded()
        {
            Invoke((MethodInvoker)MediaEnded);
        }
        private void MediaEnded()
        {

        }

        public void CallPlayStateChanged()
        {
            Invoke((MethodInvoker)PlayStateChanged);
        }
        private void PlayStateChanged()
        {

        }

        public void CallDurationChanged()
        {
            Invoke((MethodInvoker)DurationChanged);
        }
        private void DurationChanged()
        {

        }

        #region Help

        private void mPlayersCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutBakaMPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        #endregion
    }
}
