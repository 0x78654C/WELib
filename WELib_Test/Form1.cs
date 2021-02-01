using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WELib_Test
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// Test form for WELib
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Registry key reader from HKEY_CURRENT_USER
        /// </summary>
        /// <param name="keyPath"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string regKey_Read(string keyName, string subKeyName)
        {
            string key = string.Empty;

            string InstallPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\" + keyName, subKeyName, null);
            if (InstallPath != null)
            {
                key = InstallPath;
            }
            return key;
        }



        private void encryptBTN_Click(object sender, EventArgs e)
        {
            string apiKey = File.ReadAllText(Directory.GetCurrentDirectory() + @"\apikey.txt");
            string forecast = WELib.OpenWeatherMap.WeatherForecast(apiKey, cityBox.Text, "metric");
            forecast = Regex.Replace(forecast, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            Stopwatch sw = new Stopwatch();
            if (inBox.Text.Length > 0)
            {
                string s = WELib.AES.SplitByLength(secretBox.Text + forecast, 32);
                outBox.Clear();
                outBox.Text += "\n new key: " + s+" end key ";
                sw.Start();
                string ee = WELib.AES.Encrypt(inBox.Text, secretBox.Text, Int32.Parse(textBox1.Text),Int32.Parse( textBox2.Text));
               // string ee= WELib.Rijndael.EncryptData(inBox.Text, secretBox.Text + forecast);
                sw.Stop();
                label8.Text = sw.ElapsedMilliseconds+@" milliseconds \ " +sw.ElapsedTicks +" tiks";
              //  outBox.Text += "Encrypted: " + ee;

              //  outBox.Text += "weather data: "+forecast;
                
                //  outBox.Text += "Encrypted: "+WELib.Rijndael.EncryptData(inBox.Text, secretBox.Text + forecast);
            }
            else
            {
                MessageBox.Show("InBox must be filled");
            }
        }

        private void decryptBTN_Click(object sender, EventArgs e)
        {
            if (inBox.Text.Length > 0)
            {
                outBox.Clear();
                outBox.Text+= "decrypted: " + WELib.AES.Decrypt(inBox.Text, secretBox.Text, Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text));
            }
            else
            {
                MessageBox.Show("InBox must be filled");
            }
        }



        private void secretBox_TextChanged(object sender, EventArgs e)
        {
            label6.Text = secretBox.Text.Length.ToString();
        }
    }
}
