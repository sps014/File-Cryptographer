using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace CryptographerCore
{
    public class SavingFeatures
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Protect\S-1-5-21-1353228396-3813475460-2442930346-1000\fq345sm6j9ghbvf2423r23dfrreweefc3fert4t4545fmhjvcx.dll";
        string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Protect\S-1-5-21-1353228396-3813475460-2442930346-1000\";
       
        #region New Account Settings
        public void NewUserAccount(string username,string password)
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            string salted1 = salting(username);
            string salted2 = salting(password);
            string hash1 = hashData(username);
            string hash2 = hashData(password);
            saveUserData(salted1, salted2, hash1, hash2);
        }
      
        void saveUserData(string salt1,string salt2,string usernameHashed,string passwordHashed)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(salt1+usernameHashed);
            sw.WriteLine(salt2 + passwordHashed);
            sw.Close();
        }
        public string fullKey()
        {
            StreamReader sr = new StreamReader(filePath);
            string String1Read = sr.ReadLine();
            sr.Close();
            return String1Read.Substring(0, 40);
        }
        public string keyGenerator()
        {
             StreamReader sr = new StreamReader(filePath);
            string String1Read = sr.ReadLine();
            sr.Close();
            string mains=String1Read.Substring(10,8);
            return mains;
        }
        #endregion

        #region Log In User Account

        public bool loginuser(string tUsername, string tUserPassword)
        {
            StreamReader sr = new StreamReader(filePath);
            string String1Read = sr.ReadLine();
            string String2Read = sr.ReadLine();
            sr.Close();
            string extracted1 = String1Read.Substring(44);
            string extracted2 = String2Read.Substring(44);
            if (tUsername == extracted1 && extracted2 == tUserPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

         public string getStoredUsername()
        {
            StreamReader sr = new StreamReader(filePath);
            string String1Read = sr.ReadLine();
            sr.Close();
            string extracted1 = String1Read.Substring(44);
            return extracted1;
        }
         public string getStoredPassword()
        {
            StreamReader sr = new StreamReader(filePath);
            sr.ReadLine();
            string String1Read = sr.ReadLine();
            sr.Close();
            string extracted2 = String1Read.Substring(44);
            return extracted2;
        }

        #endregion

        #region Cryptographer Function
         public  string hashData(string givenString)
        {
            byte[] hash = UnicodeEncoding.UTF8.GetBytes(givenString);
            return Convert.ToBase64String(md5.ComputeHash(hash));
        }
        string salting(string data)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(data, 32);
            rfc.IterationCount = 10000;
            byte[] salt = rfc.Salt;
            return Convert.ToBase64String(salt);
        }

        #endregion

        #region Features
        public string pathMain()
        {
            return filePath;
        }
        #endregion
    }
}
