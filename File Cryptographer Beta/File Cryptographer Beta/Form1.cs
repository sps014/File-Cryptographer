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
using System.IO;

namespace File_Cryptographer_Beta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int splashtime = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            splashtime++;
            
            if (splashtime >= 2)
            {
                launcher();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        void launcher()
        {
            SavingFeatures sf=new SavingFeatures();
            timer1.Stop();
            this.Hide();

            if (File.Exists(sf.pathMain()))
            {
                LoginAccount au = new LoginAccount();
                au.ShowDialog();
            }
            else
            {
                User_Account ua = new User_Account();
                ua.ShowDialog();
            }

        }
    }
}
