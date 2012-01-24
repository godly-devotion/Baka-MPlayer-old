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
        public int GetNewTime { get; private set; }

        public JumpForm()
        {
            InitializeComponent();
        }

        private void jumpButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckTimes_ValueChanged(object sender, EventArgs e)
        {
            // parse new times
            int hour, min, sec;
            int.TryParse(hourBox.Value.ToString(), out hour);
            int.TryParse(minBox.Value.ToString(), out min);
            int.TryParse(secBox.Value.ToString(), out sec);
            int calculatedTotal = (hour * 3600) + (min * 60) + sec;

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
                int newTime = Info.Current.Duration + calculatedTotal;
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
                int newTime = Info.Current.Duration - calculatedTotal;
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
