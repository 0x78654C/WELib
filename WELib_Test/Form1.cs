using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

            if (inBox.Text.Length > 0)
            {
                //WELib.AES.Encrypt(inBox.Text, secretBox.Text + forecast, 256, 256);
                outBox.Clear();
                 outBox.Text += "weather data: "+forecast;
                outBox.Text += "Encrypted: "+WELib.Rijndael.EncryptData(inBox.Text, secretBox.Text + forecast);
            }
            else
            {
                MessageBox.Show("InBox must be filled");
            }
        }
    }
}
