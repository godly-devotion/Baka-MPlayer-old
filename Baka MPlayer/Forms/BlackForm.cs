using System;
using System.Drawing;
using System.Windows.Forms;
using MPlayer.Info;

namespace Baka_MPlayer.Forms
{
    public partial class BlackForm : Form
    {
        private readonly MainForm mainForm;
        private readonly IFileInfo fileInfo;

        public BlackForm(MainForm mainForm, IFileInfo fileInfo)
        {
            InitializeComponent();

            this.mainForm = mainForm;
            this.fileInfo = fileInfo;

            // set location & size to primary screen to reduce flicker while starting
            this.Location = new Point(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y);
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        public void RefreshTitle()
        {
            titleLabel.Text = Functions.URL.DecodeURL(fileInfo.MovieName);
        }

        #region Events

        private void BlackForm_GotFocus(object sender, EventArgs e)
        {
            mainForm.Focus();
        }

        private void BlackForm_VisibleChanged(object sender, EventArgs e)
        {
            // set up Black background
            var scrn = Screen.FromControl(mainForm);
            this.Location = new Point(scrn.Bounds.X, scrn.Bounds.Y);
            this.Size = new Size(scrn.Bounds.Width, scrn.Bounds.Height);
        }

        #endregion
    }
}
