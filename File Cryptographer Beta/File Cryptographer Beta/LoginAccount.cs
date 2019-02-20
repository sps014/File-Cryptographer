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

namespace File_Cryptographer_Beta
{
    public partial class LoginAccount : MetroForm
    {
        public LoginAccount()
        {
            InitializeComponent();
        }

        private void LoginAccount_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
         SavingFeatures sf=new SavingFeatures();
         if (sf.hashData(metroTextBox1.Text) == sf.getStoredUsername() && sf.hashData(metroTextBox2.Text) == sf.getStoredPassword())
         {
             this.Hide();
             MainWindow mw = new MainWindow();
             mw.ShowDialog();
         }
         else if(sf.hashData(metroTextBox1.Text)!=sf.getStoredUsername())
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

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            SavingFeatures sf=new SavingFeatures();
            if (sf.hashData(metroTextBox2.Text) == sf.getStoredPassword())
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
