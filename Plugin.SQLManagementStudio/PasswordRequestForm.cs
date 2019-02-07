using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.SQLManagementStudio
{
    internal class PasswordRequestForm : Form
    {
        private TextBox passwordBox;

        internal string Password { get { return this.passwordBox.Text; } }

        internal PasswordRequestForm()
        {
            this.Text = "RDCMan Plugin credential request";
            this.ClientSize = new Size(400, 200);

            Label question = new Label() {
                Text      = "Please enter your password.",
                AutoSize  = true,
                BackColor = Color.Transparent,
                Location  = new Point(10, 40)
            };
            this.Controls.Add(question);

            passwordBox = new TextBox() {
                PasswordChar = '\u25CF',
                TabStop      = true,
                TabIndex     = 0,
                Width        = 380,
                Location     = new Point(10, 90)
            };
            passwordBox.KeyDown += new KeyEventHandler(passwordBox_Keydown);

            this.Controls.Add(passwordBox);

            Button ok = new Button() {
                Text         = "&OK",
                DialogResult = DialogResult.OK,
                Size         = new Size(75, 26),
                TabStop      = true,
                TabIndex     = 1,
                Width        = 80,
                Location     = new Point(220, 160)
            };
            this.Controls.Add(ok);

            Button cancel = new Button() {
                Text         = "&Cancel",
                DialogResult = DialogResult.Cancel,
                Size         = new Size(75, 26),
                TabStop      = true,
                TabIndex     = 2,
                Width        = 80,
                Location     = new Point(310, 160)
            };
            this.Controls.Add(cancel);
        }

        private void passwordBox_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}