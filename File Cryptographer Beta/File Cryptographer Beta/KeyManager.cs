using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptographerCore;
using System.Windows.Forms;

namespace File_Cryptographer_Beta
{
    public partial class KeyManager : MetroForm
    {
        public string key;
        public KeyManager()
        {
            InitializeComponent();
            SavingFeatures sf = new SavingFeatures();
            key = sf.keyGenerator();
            metroTextBox1.Text = key;
        }

        private void KeyManager_Load(object sender, EventArgs e)
        {

        }
    }
}
