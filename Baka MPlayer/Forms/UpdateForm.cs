using System.Diagnostics;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class UpdateForm : Form
    {
        private const string url_x86_64  = "http://bakamplayer.u8sand.net/Baka MPlayer.7z"; // x86_64
        private const string url_x86 = "http://bakamplayer.u8sand.net/Baka MPlayer x86.7z"; // x86

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

        private void changelogLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/godly-devotion/Baka-MPlayer/blob/master/CHANGELOG.rst");
        }

        private void downloadButton_Click(object sender, System.EventArgs e)
        {
            Process.Start(Functions.OS.IsRunning64Bit() ? url_x86_64 : url_x86);
            this.Dispose();
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }
    }
}
