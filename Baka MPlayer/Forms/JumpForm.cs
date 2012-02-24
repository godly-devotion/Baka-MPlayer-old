using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class JumpForm : Form
    {
        /// <summary>
        /// Gets the new time to set in seconds
        /// </summary>
        public double GetNewTime { get; private set; }

        public JumpForm()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            int hour, min, sec;

            // enable time inputs
            Functions.CalculateTimeFromSeconds(Info.Current.TotalLength, out hour, out min, out sec);
            hourBox.Enabled = hour > 0;
            minBox.Enabled = min > 0;

            // set default input
            Functions.CalculateTimeFromSeconds(Info.Current.Duration, out hour, out min, out sec);
            hourBox.Value = hour;
            minBox.Value = min;
            secBox.Value = sec;
        }

        private void CheckTimes_ValueChanged(object sender, EventArgs e)
        {
            // parse new times
            /*int hour, min, sec;
            int.TryParse(hourBox.Value.ToString(), out hour);
            int.TryParse(minBox.Value.ToString(), out min);
            int.TryParse(secBox.Value.ToString(), out sec);*/
            int hour = Convert.ToInt32(hourBox.Value);
            int min = Convert.ToInt32(minBox.Value);
            int sec = Convert.ToInt32(secBox.Value);
            double calculatedTotal = (hour * 3600) + (min * 60) + sec;

            if (goToRadioButton.Checked)
            {
                statusLabel.Text = string.Format("Total Time: " + Functions.ConvertTimeFromSeconds(Info.Current.TotalLength));

                if (calculatedTotal > -1 && calculatedTotal < Info.Current.TotalLength)
                {
                    GetNewTime = calculatedTotal;
                    validEntry(true);
                }
                else
                    validEntry(false);
            }
            else if (addRadioButton.Checked)
            {
                double newTime = Info.Current.Duration + calculatedTotal;
                statusLabel.Text = string.Format("Jumps To: " + Functions.ConvertTimeFromSeconds(newTime));

                if (calculatedTotal > -1 && newTime < Info.Current.TotalLength)
                {
                    GetNewTime = newTime;
                    validEntry(true);
                }
                else
                    validEntry(false);
            }
            else if (subtractRadioButton.Checked)
            {
                double newTime = Info.Current.Duration - calculatedTotal;
                statusLabel.Text = string.Format("Jumps To: " + Functions.ConvertTimeFromSeconds(newTime));

                if (calculatedTotal > -1 && newTime > -1)
                {
                    GetNewTime = newTime;
                    validEntry(true);
                }
                else
                    validEntry(false);
            }
        }
        private void validEntry(bool allowJump)
        {
            checkPicbox.Image = allowJump ?
                Properties.Resources.exists : Properties.Resources.not_exists;
            jumpButton.Enabled = allowJump;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var formGraphics = e.Graphics;
            var gradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 60, 60, 60), Color.Black, LinearGradientMode.Vertical);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);
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
