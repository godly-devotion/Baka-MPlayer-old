using System;
using System.Windows.Forms;

namespace Baka_MPlayer.Forms
{
    public partial class InputForm : Form
    {
        public InputForm(string msg, string title, string defaultInput)
        {
            InitializeComponent();

            // set interface
            this.Text = title;
            messageLabel.Text = msg;
            inputTextbox.Text = defaultInput;

            // set focus
            inputTextbox.Focus();
            inputTextbox.SelectAll();
        }

        public string GetInputText
        {
            get { return inputTextbox.Text; }
        }

        private void inputTextbox_TextChanged(object sender, EventArgs e)
        {
            okButton.Enabled = inputTextbox.TextLength > 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
