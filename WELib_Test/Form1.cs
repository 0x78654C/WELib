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
            forecast = forecast.Replace("\r", "");
            Stopwatch sw = new Stopwatch();
            if (inBox.Text.Length > 0)
            {
                string com = WELib.WE.WeatherKeyGenrateAES(forecast, 32,false);

        
                outBox.Clear();
                outBox.Text += "Forecast info: " + forecast + Environment.NewLine;
                outBox.Text += "Forecast generated AES key: " + com + Environment.NewLine;
                outBox.Text += "Forecast generated AES key Length: " + com.Length.ToString() + Environment.NewLine;
                outBox.Text += "_______________________________________________________________________________"+Environment.NewLine;
                sw.Start();
                outBox.Text += Environment.NewLine+"AES Encrypted Output: " +WELib.AES.Encrypt(inBox.Text, com, Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text)) + Environment.NewLine;
                sw.Stop();


                // string ee = WELib.AES.Encrypt(inBox.Text, secretBox.Text, Int32.Parse(textBox1.Text),Int32.Parse( textBox2.Text));
                // string ee= WELib.Rijndael.EncryptData(inBox.Text, secretBox.Text + forecast);

                label8.Text = sw.ElapsedMilliseconds + @" milliseconds \ " + sw.ElapsedTicks + " tiks";
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
                outBox.Text += "decrypted: " + WELib.AES.Decrypt(inBox.Text, secretBox.Text, Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text));
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


        public string[] SplitStringLenght(string s, int split)
        {


            List<string> list = new List<string>();

            // Integer Division
            int TimesThroughTheLoop = s.Length / split;


            for (int i = 0; i < TimesThroughTheLoop; i++)
            {
                list.Add(s.Substring(i * split, split));

            }

            // Pickup the end of the string
            if (TimesThroughTheLoop * split != s.Length)
            {
                list.Add(s.Substring(TimesThroughTheLoop * split));
            }

            return list.ToArray();


        }

    }
    
}
