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
using System.IO;
using System.Security.Cryptography;

namespace File_Cryptographer_Beta
{
    public partial class MainWindow : MetroForm
    {
        #region File Varirable
        private  string keys;
        List<string> fileList = new List<string>();
        #endregion

        #region Genral settings var
        public string configPath = Application.StartupPath + @"\settings.config";
        public string encryptedDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\File Cryptographer\Encrypted Files\";
        public string decryptedDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\File Cryptographer\Decrypted Files\";
        #endregion

        #region Initialization Of the Program
        public MainWindow()
        {
            InitializeComponent();
            SavingFeatures sf=new SavingFeatures();
            keys = sf.keyGenerator();
            fileLoadSettings();
        }

        #endregion

        #region General Settings


        #region Settings

        #region Directory
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            if (encryptedLocation.ShowDialog() == DialogResult.OK)
            {
                metroTextBox1.Text = encryptedLocation.SelectedPath+@"\";
                saveFileLocation();
            }
          
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            if (decryptedLocation.ShowDialog() == DialogResult.OK)
            {
                metroTextBox2.Text = decryptedLocation.SelectedPath+@"\";
                saveFileLocation();
            }
        }
        #endregion
        void fileLoadSettings()
        {
            if (!File.Exists(configPath))
            {
                metroTextBox1.Text = encryptedDir;
                metroTextBox2.Text = decryptedDir;
            }
            else
            {
                StreamReader sr = new StreamReader(configPath);
                metroTextBox1.Text = sr.ReadLine();
                metroTextBox2.Text = sr.ReadLine();
                metroToggle1.Checked = bool.Parse(sr.ReadLine());
                metroToggle2.Checked = bool.Parse(sr.ReadLine());
                encryptedDir = metroTextBox1.Text;
                decryptedDir = metroTextBox2.Text;
                sr.Close();
            }
            if (!Directory.Exists(decryptedDir))
            {
                Directory.CreateDirectory(decryptedDir);
            }
            if (!Directory.Exists(encryptedDir))
            {
                Directory.CreateDirectory(encryptedDir);
            }
            webBrowser1.Url = new Uri(encryptedDir);
            webBrowser2.Url = new Uri(decryptedDir);
        }

        void saveFileLocation()
        {
            StreamWriter sw = new StreamWriter(configPath);
            sw.WriteLine(metroTextBox1.Text);
            encryptedDir = metroTextBox1.Text;
            sw.WriteLine(metroTextBox2.Text);
            decryptedDir = metroTextBox2.Text;
            sw.WriteLine(metroToggle1.Checked.ToString());
            sw.WriteLine(metroToggle2.Checked.ToString());
            webBrowser1.Url = new Uri(encryptedDir);
            webBrowser2.Url =new Uri(decryptedDir);
            sw.Close();
        }
        #endregion

        #endregion

        #region Progress Bar 
        Processing p = new Processing();
        void ProgressForm(string list,int remaining,int total)
        {
            p.metroLabel2.Text = getOnlyFileName(list);
            p.metroLabel4.Text = (remaining-1).ToString();
            p.metroLabel6.Text = total.ToString();
            p.metroProgressSpinner1.Maximum = total;
            p.metroProgressSpinner1.Value = total - remaining;
            FileInfo fi = new FileInfo(list);          
            p.metroProgressBar2.Maximum = (int)fi.Length;
        }
        string getOnlyFileName(string InputFile)
        {
            int ls = InputFile.LastIndexOf(@"\")+1;
            return InputFile.Substring(ls, InputFile.Length - ls);
        }

        #endregion

        #region Form Will be Closed
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveFileLocation();
            if (metroToggle1.Checked)
            {
                string[] path = Directory.GetFiles(decryptedDir);
                foreach (string file in path)
                {
                    File.Delete(file);
                }
            }
            Application.Exit();
        }
        #endregion

        #region Single File Handlers
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileEncrypt.Title = "Choose a File";
            openFileEncrypt.FileName = "";
            if (openFileEncrypt.ShowDialog() == DialogResult.OK)
            {
                fileList.Clear();
                FileInfo fi = new FileInfo(openFileEncrypt.FileName);
                fileList.Add(openFileEncrypt.FileName);
                p.metroLabel2.Text = openFileEncrypt.FileName;
                p.metroLabel4.Text = "0";
                p.metroLabel6.Text = "1";
                p.metroProgressBar2.Maximum = (int)fi.Length;
                backgroundWorker1.RunWorkerAsync();
                p.ShowDialog();
            }
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDecrypt.Title = "Choose Decrypted File";
            openFileDecrypt.FileName = "";
            openFileDecrypt.InitialDirectory = encryptedDir;
            if (openFileDecrypt.ShowDialog() == DialogResult.OK)
            {
                fileList.Clear();
                p.metroLabel2.Text = openFileDecrypt.FileName;
                p.metroLabel4.Text = "0";
                p.metroLabel6.Text = "1";
                fileList.Add(openFileDecrypt.FileName);
                FileInfo fi = new FileInfo(openFileDecrypt.FileName);
                p.metroProgressBar2.Maximum = (int)fi.Length;
                backgroundWorker2.RunWorkerAsync();
                p.ShowDialog();
            }
        }

        #endregion

        #region Handling Multiple Files
        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileList.Clear();
            if (EncryptBrowser.ShowDialog() == DialogResult.OK)
            {
                string[] path = Directory.GetFiles(EncryptBrowser.SelectedPath);
                fileList.AddRange(path);
                FileInfo fi = new FileInfo(fileList[0]);
                backgroundWorker1.RunWorkerAsync();
                p.ShowDialog();
            }
        }

        private void folderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fileList.Clear();
            string[] path = Directory.GetFiles(encryptedDir);
            fileList.AddRange(path);
            backgroundWorker2.RunWorkerAsync();
            p.ShowDialog();
        }

        #endregion

        #region Back grounderWorker 1 For Encryption
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int temp = 0;
            foreach (string list in fileList)
            {
                Invoke((MethodInvoker)delegate()
                {
                    ProgressForm(list, fileList.Count - temp, fileList.Count);
                });
                #region Encryption Code Goes Here
                int s = list.LastIndexOf(@"\");
                string filename = list.Substring(s + 1);
                if (!File.Exists(encryptedDir + filename))
                {
                    try
                    {
                        UnicodeEncoding ue = new UnicodeEncoding();
                        byte[] key = ue.GetBytes(keys);

                        string output = encryptedDir + filename;

                        FileStream fsCrypt = new FileStream(output, FileMode.Create);
                        RijndaelManaged RMCrypto = new RijndaelManaged();
                        CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);
                        FileStream fsln = new FileStream(list, FileMode.Open);
                        int data, loc = 0;
                        while ((data = fsln.ReadByte()) != -1)
                        {
                            cs.WriteByte((byte)data);
                            loc++;
                            if (loc >= 2048)
                            {
                                FileInfo fi = new FileInfo(output);
                                backgroundWorker1.ReportProgress((int)fi.Length);
                                loc = 0;
                            }
                        }
                        fsln.Close();
                        loc = 0;
                        cs.Close();
                        fsCrypt.Close();
                    }
                    catch
                    {
                        backgroundWorker1.CancelAsync();
                    }

                }
                #endregion
                if (metroToggle2.Checked)
                {
                    File.Delete(list);
                }

                temp++;
            }
            fileList.Clear();

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            p.metroProgressBar2.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            p.Hide();
            p.notifyIcon1.Visible = false;
        }

        #endregion

        #region Background Worker 2 For Decryption
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

            int temp = 0;
            foreach (string list in fileList)
            {
                Invoke((MethodInvoker)delegate()
                {
                    ProgressForm(list, fileList.Count - temp, fileList.Count);
                });
                #region Decrypt Code
                try
                {
                    UnicodeEncoding ue = new UnicodeEncoding();
                    byte[] key = ue.GetBytes(keys);
                    FileStream fsCrypt = new FileStream(list, FileMode.Open);
                    int lastSlash = list.LastIndexOf(@"\");
                    int total = list.Length;
                    int diff = total - lastSlash;
                    string outputFile = decryptedDir + list.Substring(lastSlash, diff);
                    FileInfo fi = new FileInfo(outputFile);

                    if (!File.Exists(outputFile))
                    {
                        RijndaelManaged RMcrypto = new RijndaelManaged();
                        CryptoStream cs = new CryptoStream(fsCrypt, RMcrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);
                        FileStream fsOut = new FileStream(outputFile, FileMode.Create);
                        int data;
                        int locV = 0;
                        try
                        {
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOut.WriteByte((byte)data);
                                if (locV >= 2048)
                                {
                                    FileInfo fim = new FileInfo(outputFile);
                                    backgroundWorker2.ReportProgress((int)fim.Length);
                                    locV = 0;
                                }
                                locV++;
                            }
                            fsOut.Close();
                            cs.Close();
                            fsCrypt.Close();
                        }
                        catch
                        {
                            backgroundWorker2.CancelAsync();
                        }

                    }
                }
                catch
                {
                    backgroundWorker2.CancelAsync();
                }
                #endregion
                    temp++;
             }
            fileList.Clear();
                
        }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            p.metroProgressBar2.Value = e.ProgressPercentage;
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            p.Hide();
            p.notifyIcon1.Visible = false;
        }
        #endregion

        #region options 
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_Us au = new About_Us();
            au.ShowDialog();
        }

        private void contactSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contact_Settings cs = new Contact_Settings();
            cs.ShowDialog();
        }

        private void defaultKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveKey();   
        }
        public void saveKey()
        {
            saveKeyDialog.Title = "Store at a safe place";
            if (saveKeyDialog.ShowDialog() == DialogResult.OK)
            {
                SavingFeatures sff = new SavingFeatures();
                StreamWriter sw = new StreamWriter(saveKeyDialog.FileName);
                sw.WriteLine(sff.fullKey());
                sw.Close();
            }
        }
        #endregion

        #region Total Files
        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] path1 = Directory.GetFiles(encryptedDir);
            int s1=path1.Length;
            if (materialTabControl1.SelectedIndex == 0)
            {
                metroLabel6.Text = s1.ToString();
            }
            string[] path2 = Directory.GetFiles(decryptedDir);
            int s2 = path2.Length;
            if (materialTabControl1.SelectedIndex == 1)
            {
                metroLabel6.Text = s2.ToString();
            }
            if (materialTabControl1.SelectedIndex == 2)
            {
                metroLabel6.Text = (s1+s2).ToString();
            }
        }
        #endregion
    }
}
