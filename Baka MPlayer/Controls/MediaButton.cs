using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class MediaButton : PictureBox
    {
        private Image _DefaultImg;
        private Image _DisabledImg;
        private Image _MouseDownImg;

        public MediaButton()
        {
            InitializeComponent();

            // set properties
            this.BackColor = Color.Transparent;
            this.Cursor = Cursors.Hand;
            this.SizeMode = PictureBoxSizeMode.CenterImage;

            // set events
            this.EnabledChanged += MediaButton_EnabledChanged;
            this.MouseDown += MediaButton_MouseDown;
            this.MouseUp += MediaButton_MouseUp;
        }

        #region Properties

        [Description("Image used for default state.")]
        public Image DefaultImage
        {
            get { return _DefaultImg; }
            set { _DefaultImg = value; Refresh(); }
        }

        [Description("Image used for disabled state.")]
        public Image DisabledImage
        {
            get { return _DisabledImg; }
            set { _DisabledImg = value; Refresh(); }
        }

        [Description("Image used for mouse down state.")]
        public Image MouseDownImage
        {
            get { return _MouseDownImg; }
            set { _MouseDownImg = value; Refresh(); }
        }

        #endregion

        #region Events

        private void MediaButton_EnabledChanged(object sender, EventArgs e)
        {
            this.Image = this.Enabled ? _DefaultImg : _DisabledImg;
        }

        private void MediaButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Enabled && e.Button == MouseButtons.Left)
                this.Image = _MouseDownImg;
        }

        private void MediaButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.Image = this.Enabled ? _DefaultImg : _DisabledImg;
        }

        #endregion
    }
}
