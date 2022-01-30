using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OA.Base.Helpers.StringCiphers
{
    public class StringCipher: IStringCipher
    {
        //private const int Keysize = 256;
        //private const int DerivationIterations = 1000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public string Encrypt(string inputText)
        {
            string encryptionkey = "SAUW193BX628TD57";
            byte[] keybytes = Encoding.ASCII.GetBytes(encryptionkey.Length.ToString());
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes pwdbytes = new PasswordDeriveBytes(encryptionkey, keybytes);
            using ICryptoTransform encryptrans = rijndaelCipher.CreateEncryptor(pwdbytes.GetBytes(32), pwdbytes.GetBytes(16));
            using MemoryStream mstrm = new MemoryStream();
            using CryptoStream cryptstm = new CryptoStream(mstrm, encryptrans, CryptoStreamMode.Write);
            cryptstm.Write(plainText, 0, plainText.Length);
            cryptstm.Close();
            return Convert.ToBase64String(mstrm.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns></returns>
        public string Decrypt(string encryptText)
        {
            string encryptionkey = "SAUW193BX628TD57";
            byte[] keybytes = Encoding.ASCII.GetBytes(encryptionkey.Length.ToString());
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            //byte[] encryptedData = Convert.FromBase64String(encryptText.Replace("-", " "));
            byte[] encryptedData = Convert.FromBase64String(encryptText);
            PasswordDeriveBytes pwdbytes = new PasswordDeriveBytes(encryptionkey, keybytes);
            using ICryptoTransform decryptrans = rijndaelCipher.CreateDecryptor(pwdbytes.GetBytes(32), pwdbytes.GetBytes(16));
            using MemoryStream mstrm = new MemoryStream(encryptedData);
            using CryptoStream cryptstm = new CryptoStream(mstrm, decryptrans, CryptoStreamMode.Read);
            byte[] plainText = new byte[encryptedData.Length];
            int decryptedCount = cryptstm.Read(plainText, 0, plainText.Length);
            return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
        }
    }
}
