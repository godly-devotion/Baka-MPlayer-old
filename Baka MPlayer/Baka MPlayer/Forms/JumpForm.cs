using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class JumpForm : Form
    {
        private readonly int currentTime;
        private readonly int totalTime;

        /// <summary>
        /// Gets the new time to set in seconds
        /// </summary>
        public int GetNewTime { get; private set; }

        public JumpForm(int currentTime, int totalTime)
        {
            InitializeComponent();

            this.currentTime = currentTime;
            this.totalTime = totalTime;
        }

        private void jumpButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckTimes_ValueChanged(object sender, EventArgs e)
        {
            // calculated times
            int tHour, tMin, tSec;
            // parse new times
            int hour, min, sec;
            int.TryParse(hourBox.Value.ToString(), out hour);
            int.TryParse(minBox.Value.ToString(), out min);
            int.TryParse(secBox.Value.ToString(), out sec);
            int calculatedTotal = (hour * 3600) + (min * 60) + sec;

            if (goToRadioButton.Checked)
            {
                Functions.CalculateTimeFromSeconds(totalTime, out tHour, out tMin, out tSec);
                statusLabel.Text = string.Format("Total Time: {0}:{1}:{2}",
                    tHour.ToString("#0"), tMin.ToString("00"), tSec.ToString("00"));

                if (calculatedTotal > -1 && calculatedTotal < totalTime)
                {
                    GetNewTime = calculatedTotal;
                    validEntry(true);
                }
                else
                    validEntry(false);
            }
            else if (addRadioButton.Checked)
            {
                int newTime = currentTime + calculatedTotal;
                Functions.CalculateTimeFromSeconds(newTime, out tHour, out tMin, out tSec);
                statusLabel.Text = string.Format("Jumps To: {0}:{1}:{2}",
                    tHour.ToString("#0"), tMin.ToString("00"), tSec.ToString("00"));

                if (calculatedTotal > -1 && newTime < totalTime)
                {
                    GetNewTime = newTime;
                    validEntry(true);
                }
                else
                    validEntry(false);
            }
            else if (subtractRadioButton.Checked)
            {
                int newTime = currentTime - calculatedTotal;
                Functions.CalculateTimeFromSeconds(newTime, out tHour, out tMin, out tSec);
                statusLabel.Text = string.Format("Jumps To: {0}:{1}:{2}",
                    tHour.ToString("#0"), tMin.ToString("00"), tSec.ToString("00"));

                if (calculatedTotal > -1 && newTime > -1)
                {
                    GetNewTime = newTime;
                    validEntry(true);
                }
                else
                    validEntry(false);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var formGraphics = e.Graphics;
            var gradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 60, 60, 60), Color.Black, LinearGradientMode.Vertical);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);
        }

        private void validEntry(bool allowJump)
        {
            checkPicbox.Image = allowJump ?
                Properties.Resources.exists : Properties.Resources.not_exists;
            jumpButton.Enabled = allowJump;
        }
    }
}
