using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    public partial class SimpleButton : Button
    {
        private bool isDefault;

        public SimpleButton()
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
                FlatAppearance.BorderColor = Color.DodgerBlue;
                FlatAppearance.MouseDownBackColor = Color.SteelBlue;
                FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
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
