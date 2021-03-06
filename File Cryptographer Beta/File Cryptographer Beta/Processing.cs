﻿using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Cryptographer_Beta
{
    public partial class Processing : MetroForm
    {
        public Processing()
        {
            InitializeComponent();
        }


        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000);
        }

        private void showProgressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowDialog();
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowDialog();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowDialog();
        }
    }
}
