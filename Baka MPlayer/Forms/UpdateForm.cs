using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class UpdateForm : Form
    {
        private const string downloadURL = "http://bakamplayer.netii.net/Baka%20MPlayer.7z";

        public UpdateForm(UpdateInfo info)
        {
            InitializeComponent();

            Display(info);
        }
        private void UpdateForm_Load(object sender, System.EventArgs e)
        {
            this.MinimumSize = this.Size;
        }

        private void Display(UpdateInfo info)
        {
            if (info.UpdateAvailable)
            {
                statusLabel.Text = "Update Available!";
                versionLabel.Text = string.Format("Latest Version: {0} (Released {1})\nYour Version: {2}",
                    info.LatestVer, info.Date, Application.ProductVersion);
                newLabel.Text = info.BugFixes;
                downloadButton.Enabled = true;
            }
            else
            {
                statusLabel.Text = "No Updates Available";
                versionLabel.Text = "Your version is the latest one as of now.";
                downloadButton.Enabled = false;
            }
        }

        private void downloadButton_Click(object sender, System.EventArgs e)
        {
            Process.Start(downloadURL);
            this.Dispose();
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var formGraphics = e.Graphics;
            var gradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 60, 60, 60), Color.Black, LinearGradientMode.Vertical);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);
        }
    }
}
