// Based on: http://support.microsoft.com/kb/323116

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class AudioLevelBar : Control
    {
        public AudioLevelBar()
        {
            InitializeComponent();
        }

        #region Properties

        private int min = 0;
        /// <summary>
        /// Gets or sets the minimum value
        /// </summary>
        public int Minimum
        {
            get { return min; }
            set
            {
                if (value < 0)
                    min = 0;

                if (value > max)
                    min = value;

                if (val < min)
                    val = min;

                this.Invalidate();
            }
        }

        private int max = 100;
        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
        public int Maximum
        {
            get { return max; }
            set
            {
                if (value < min)
                    min = value;

                max = value;

                if (val > max)
                    val = max;

                this.Invalidate();
            }
        }

        private int val = 0;
        /// <summary>
        /// Gets or sets the audio level value
        /// </summary>
        public int Value
        {
            get { return val; }
            set
            {
                val = value;
                this.Invalidate();
            }
        }

        private Color barColor = Color.DodgerBlue;
        /// <summary>
        /// Gets or sets the color of the progress bar
        /// </summary>
        public Color ProgressBarColor
        {
            get { return barColor; }
            set
            {
                barColor = value;
                this.Invalidate();
            }
        }

        #endregion

        #region Overrided Events

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = this.ClientRectangle;
            float percent = (val - min) / (float)(max - min);

            // calculate area for drawing the progress
            int t = (int)(rect.Height * percent);
            rect.Height = t;
            rect.Y = this.ClientRectangle.Height - t;

            // draw the progress meter
            using (var brush = new SolidBrush(barColor))
            {
                g.FillRectangle(brush, rect);
            }

            // draw red bar
            if (val == 100)
            {
                using (var redBrush = new SolidBrush(Color.DeepPink))
                {
                    g.FillRectangle(redBrush, 0, 0, ClientRectangle.Width, 2);
                }
            }

            g.Dispose();
        }

        #endregion
    }
}
