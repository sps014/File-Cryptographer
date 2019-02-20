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
using MetroFramework;
using System.IO;

namespace File_Cryptographer_Beta
{
    public partial class Contact_Settings : MetroForm
    {
        private string username, password,key;
        public Contact_Settings()
        {
            InitializeComponent();
            username = sf.getStoredUsername();
            key = sf.fullKey();
            password=sf.getStoredPassword();
        }

        SavingFeatures sf = new SavingFeatures();
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            if (sf.hashData(metroTextBox1.Text) == sf.getStoredUsername() && sf.hashData(metroTextBox2.Text) == sf.getStoredPassword())
            {
              nowCreateNewUserAccount();

            }
            else if (sf.hashData(metroTextBox1.Text) != sf.getStoredUsername())
            {
                MetroMessageBox.Show(this, "Check Username ..you have entered wrong.", "Wrong entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (sf.hashData(metroTextBox2.Text) != sf.getStoredPassword())
            {
                MetroMessageBox.Show(this, "Check Password ..you have entered wrong.", "Wrong entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MetroMessageBox.Show(this, "Check Username or Password..you have entered wrong.", "Wrong entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nowCreateNewUserAccount()
        {
            if (metroTextBox5.Text.Length < 3)
            {
                MetroMessageBox.Show(this, "Name must be minimum 3 character long..", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (metroTextBox4.Text.Length < 8)
            {
                MetroMessageBox.Show(this, "password must be minimum 8 character long..", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (metroTextBox3.Text != metroTextBox3.Text)
            {
                MetroMessageBox.Show(this, "password does not matched..", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                saveFileDialog1.Title = "Store at a safe place";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    sw.WriteLine(key);
                    sw.Close();
                }
                SavingFeatures sf = new SavingFeatures();
                sf.NewUserAccount(metroTextBox5.Text, metroTextBox4.Text);
                Application.Restart();
            }
        }

        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBox4.Text == metroTextBox3.Text)
            {
                metroCheckBox2.Checked = true;
            }
            else
            {
                metroCheckBox2.Checked = false;
            }
        }

        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBox4.Text == metroTextBox3.Text)
            {
                metroCheckBox2.Checked = true;
            }
            else
            {
                metroCheckBox2.Checked = false;
            }
        }
    }
}
