using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Cryptographer_Recovery
{
    public partial class Form1 : MetroForm
    {
        string keys,outDir=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\File Cryptographer Recovered\";
        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            metroTextBox2.Text = outDir;
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Choose Key File.";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                getKey(openFileDialog1.FileName);
                metroTextBox1.Text = openFileDialog1.FileName;
            }
        }
        void getKey(string ip)
        {
            StreamReader sr = new StreamReader(ip);
            string mn=sr.ReadLine();
            sr.Close();
            keys = mn.Substring(10, 8);
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                metroTextBox2.Text = folderBrowserDialog1.SelectedPath;
                outDir = folderBrowserDialog1.SelectedPath + @"\";
            }
        }
        List<string> fileList = new List<string>();
        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            if (keys != null)
            {
                openFileDialog2.FileName = "";
                openFileDialog2.Title = "Choose fle to decrypt";
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    fileList.Clear();
                    fileList.Add(openFileDialog2.FileName);
                    backgroundWorker1.RunWorkerAsync();
                    p.ShowDialog();
                }
            }
            else
            {
                MetroMessageBox.Show(this, "No Key File Added","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            if (keys != null)
            {
                if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                {
                    fileList.Clear();
                    string[] mn = Directory.GetFiles(folderBrowserDialog2.SelectedPath);
                    fileList.AddRange(mn);
                    backgroundWorker1.RunWorkerAsync();
                    p.ShowDialog();
                }
            }
            else
            {
                MetroMessageBox.Show(this, "No Key File Added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
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
                    string outputFile = outDir + list.Substring(lastSlash, diff);
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
                                    backgroundWorker1.ReportProgress((int)fim.Length);
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
                            backgroundWorker1.CancelAsync();
                        }

                    }
                }
                catch
                {
                    backgroundWorker1.CancelAsync();
                }
                #endregion
                temp++;
            }
            fileList.Clear();
                

        }
        string getOnlyFileName(string InputFile)
        {
            int ls = InputFile.LastIndexOf(@"\") + 1;
            return InputFile.Substring(ls, InputFile.Length - ls);
        }
        Processing p = new Processing();
        void ProgressForm(string list, int remaining, int total)
        {
            p.metroLabel1.Text = getOnlyFileName(list);
            p.metroLabel4.Text = (remaining - 1).ToString();
            p.metroLabel6.Text = total.ToString();
            p.metroProgressSpinner1.Maximum = total;
            p.metroProgressSpinner1.Value = total - remaining;
            FileInfo fi = new FileInfo(list);
            p.metroProgressBar2.Maximum = (int)fi.Length;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            p.metroProgressBar2.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            p.Hide();
            fileList.Clear();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MetroMessageBox.Show(this, "This is an integral part of File Cryptographer..only File encrypted with File Cryptographer will be decrypted.","About it.",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

    }
}
