using System;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class VoiceForm : Form
    {
        public VoiceForm()
        {
            InitializeComponent();
        }

        public string GetName
        {
            get { return nameTextBox.Text.Trim(); }
        }

        private void VoiceForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.CallName.Length != 0)
                nameTextBox.Text = Properties.Settings.Default.CallName;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (GetName.Length == 0)
            {
                MessageBox.Show("You haven't set a custom name, and that's fine.\nThe default call name will be used. (Hint: \"baka\")",
                    "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nameTextBox.Text = "baka";
            }
        }
    }
}
