using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class MediaButton : PictureBox
    {
        private Image _defaultImg, _disabledImg, _mouseDownImg;

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
            get { return _defaultImg; }
            set { _defaultImg = value; Refresh(); }
        }

        [Description("Image used for disabled state.")]
        public Image DisabledImage
        {
            get { return _disabledImg; }
            set { _disabledImg = value; Refresh(); }
        }

        [Description("Image used for mouse down state.")]
        public Image MouseDownImage
        {
            get { return _mouseDownImg; }
            set { _mouseDownImg = value; Refresh(); }
        }

        #endregion

        #region Events

        private void MediaButton_EnabledChanged(object sender, EventArgs e)
        {
            this.Image = this.Enabled ? _defaultImg : _disabledImg;
        }

        private void MediaButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Enabled && e.Button == MouseButtons.Left)
                this.Image = _mouseDownImg;
        }

        private void MediaButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.Image = this.Enabled ? _defaultImg : _disabledImg;
        }

        #endregion
    }
}
