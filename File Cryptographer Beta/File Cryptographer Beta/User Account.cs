using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptographerCore;

namespace File_Cryptographer_Beta
{
    public partial class User_Account : MetroForm
    {
        public User_Account()
        {
            InitializeComponent();
        }

        private void User_Account_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text.Length < 3)
            {
                MetroMessageBox.Show(this, "Name must be minimum 3 character long..", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (metroTextBox2.Text.Length < 8)
            {
                MetroMessageBox.Show(this, "password must be minimum 8 character long..", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (metroTextBox2.Text != metroTextBox3.Text)
            {
                MetroMessageBox.Show(this, "password does not matched..", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SavingFeatures sf = new SavingFeatures();
                sf.NewUserAccount(metroTextBox1.Text, metroTextBox2.Text);
                this.Hide();
                MainWindow mw = new MainWindow();
                mw.ShowDialog();
            }
        }

        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBox2.Text == metroTextBox3.Text)
            {
                metroCheckBox1.Checked = true;
            }
            else
            {
                metroCheckBox1.Checked = false;
            }
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBox2.Text == metroTextBox3.Text)
            {
                metroCheckBox1.Checked = true;
            }
            else
            {
                metroCheckBox1.Checked = false;
            }
        }
    }
}
