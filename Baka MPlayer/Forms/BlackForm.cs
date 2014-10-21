using System;
using System.Drawing;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class BlackForm : Form
    {
        private readonly Form _parentForm;
        private readonly string _title;

        public BlackForm(Form parentForm, string title)
        {
            InitializeComponent();

            _parentForm = parentForm;
            _title = title;

            // set location & size to primary screen to reduce flicker while starting
            this.Location = new Point(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y);
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        public void RefreshTitle()
        {
            titleLabel.Text = _title;
        }

        #region Events

        private void BlackForm_GotFocus(object sender, EventArgs e)
        {
            _parentForm.Focus();
        }

        private void BlackForm_VisibleChanged(object sender, EventArgs e)
        {
            // set up Black background
            var scrn = Screen.FromControl(_parentForm);
            this.Location = new Point(scrn.Bounds.X, scrn.Bounds.Y);
            this.Size = new Size(scrn.Bounds.Width, scrn.Bounds.Height);
        }

        #endregion
    }
}
