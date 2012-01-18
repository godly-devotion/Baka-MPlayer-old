using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class ButtonControl : Button
    {
        private bool isDefault;

        public ButtonControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        //[Category("Appearance")]
        [Description("Indicates the default button.")]
        public bool IsDefaultButton
        {
            get { return isDefault; }
            set { isDefault = value; SetButtonColor(); }
        }

        private void SetButtonColor()
        {
            if (isDefault)
            {
                // flat appearance
                FlatAppearance.BorderColor = Color.DeepSkyBlue;
                FlatAppearance.MouseDownBackColor = Color.DimGray;
                FlatAppearance.MouseOverBackColor = Color.DeepSkyBlue;
            }
            else
            {
                // flat appearance
                FlatAppearance.BorderColor = Color.Gray;
                FlatAppearance.MouseDownBackColor = Color.DimGray;
                FlatAppearance.MouseOverBackColor = Color.Gray;
            }
        }
    }
}
