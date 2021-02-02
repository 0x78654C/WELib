using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WELib
{
    /// <summary>
    /// Rijndael Encyption class
    /// </summary>
    public class Rijndael
    {

        /// <summary>
        /// Rijndael decryption function
        /// </summary>
        /// <param name="input">Enter string for decryption</param>
        /// <param name="secret">Enter your secret key</param>
        /// <returns>pass</returns>
        public static string DecryptData(string input, string secret)
        {
            string DecryptedData = string.Empty;
            try
            {
                string InputText = input;
                string Password = secret;
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = System.Text.Encoding.ASCII.GetBytes(Password.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(EncryptedData);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                byte[] PlainText = new byte[EncryptedData.Length];
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                DecryptedData = System.Text.Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            }
            catch 
            {
                throw new Exception("Error: Something went wrong on decryption!");

            }
            return DecryptedData;

        }

        /// <summary>
        /// RijndaelEncryption function
        /// </summary>
        /// <param name="input">Enter string for encryption</param>
        /// <param name="secret">Enter your secret key</param>
        /// <returns>string</returns>
        public static string EncryptData(string input, string secret)
        {
            string EncryptedData = string.Empty;
            try
            {
                string Password = secret;
                string InputText = input;
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
                byte[] Salt = System.Text.Encoding.ASCII.GetBytes(Password.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(PlainText, 0, PlainText.Length);
                cryptoStream.FlushFinalBlock();
                byte[] CipherBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                EncryptedData = Convert.ToBase64String(CipherBytes);

            }
            catch 
            {
                throw new Exception("Error: Something went wrong on encryption!");
            }
            return EncryptedData;

        }


    }
}
