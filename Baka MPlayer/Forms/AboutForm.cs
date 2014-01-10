using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            versionLabel.Text = "Version: " + Program.GetVersion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var gradientBrush = new LinearGradientBrush(
                this.ClientRectangle, Color.FromArgb(255, 60, 60, 60), Color.Black, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(gradientBrush, ClientRectangle);
            }
        }

        private void webLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(webLinkLabel.Text);
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }
    }
}
