using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using MPlayer.Info;

namespace Baka_MPlayer.Forms
{
    public partial class JumpForm : Form
    {
        /// <summary>
        /// Gets the new time to set in seconds
        /// </summary>
        public double GetNewTime { get; private set; }

        private readonly Status currentStatus;

        public JumpForm(Status currentStatus)
        {
            InitializeComponent();

            this.currentStatus = currentStatus;
            Init();
        }
        private void Init()
        {
            int hour, min, sec;

            // enable time inputs
            Functions.Time.CalculateTimeFromSeconds(currentStatus.TotalLength, out hour, out min, out sec);
            hourBox.Enabled = hour > 0;
            minBox.Enabled = min > 0;

            // set default input
            Functions.Time.CalculateTimeFromSeconds(currentStatus.Duration, out hour, out min, out sec);
            hourBox.Value = hour;
            minBox.Value = min;
            secBox.Value = sec;
        }

        private void CheckTimes_ValueChanged(object sender, EventArgs e)
        {
            var hour = Convert.ToInt32(hourBox.Value, CultureInfo.InvariantCulture);
            var min = Convert.ToInt32(minBox.Value, CultureInfo.InvariantCulture);
            var sec = Convert.ToInt32(secBox.Value, CultureInfo.InvariantCulture);
            double calculatedTotal = (hour * 3600) + (min * 60) + sec;

            if (goToRadioButton.Checked)
            {
                statusLabel.Text = string.Format("Total Time: " + Functions.Time.ConvertSecondsToTime(currentStatus.TotalLength));

                if (calculatedTotal > -1 && calculatedTotal < currentStatus.TotalLength)
                {
                    GetNewTime = calculatedTotal;
                    ValidEntry(true);
                }
                else ValidEntry(false);
            }
            else if (addRadioButton.Checked)
            {
                double newTime = currentStatus.Duration + calculatedTotal;
                statusLabel.Text = string.Format("Jumps To: " + Functions.Time.ConvertSecondsToTime(newTime));

                if (calculatedTotal > -1 && newTime < currentStatus.TotalLength)
                {
                    GetNewTime = newTime;
                    ValidEntry(true);
                }
                else ValidEntry(false);
            }
            else if (subtractRadioButton.Checked)
            {
                double newTime = currentStatus.Duration - calculatedTotal;
                statusLabel.Text = string.Format("Jumps To: " + Functions.Time.ConvertSecondsToTime(newTime));

                if (calculatedTotal > -1 && newTime > -1)
                {
                    GetNewTime = newTime;
                    ValidEntry(true);
                }
                else ValidEntry(false);
            }
        }
        private void ValidEntry(bool allowJump)
        {
            checkPicbox.Image = allowJump ?
                Properties.Resources.exists : Properties.Resources.not_exists;
            jumpButton.Enabled = allowJump;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var gradientBrush = new LinearGradientBrush(
                this.ClientRectangle, Color.FromArgb(255, 60, 60, 60), Color.Black, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(gradientBrush, ClientRectangle);
            }
        }

        private void JumpForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Dispose();
                    break;
            }
        }
    }
}
