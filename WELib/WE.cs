using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WELib
{
    public class WE
    {
        /// <summary>
        /// AES weather forecast dynamic key generator
        /// </summary>
        /// <param name="WeatherForecast"> The result from weather forecast generated or any other string</param>
        /// <param name="KeySize">Length ot the output key. Must be 16 or 32 charactes long.</param>
        /// <param name="shuffle">Activate/Deactive key string shuffle</param>
        /// <returns>string</returns>
        public static string WeatherKeyGenrateAES(string WeatherForecast, int KeySize, bool shuffle)
        {
            //declare output variable
            string output = string.Empty;
            //-----------------------------


            //clean the new lines in forecast
            WeatherForecast = WeatherForecast.Replace("\r", "");
            
            //case of key seize is 16 or 32 chars long
            if(KeySize ==16 || KeySize==32)
            {
                //we encrypt with Rijndael the forecast output with the weather forecast output
                string rWE = Rijndael.EncryptData(WeatherForecast, WeatherForecast);

                //we split the encrypted weather forecast in chunks of the leght of the key size
                string[] rWESplit = SplitStringLenght(rWE, KeySize);

                //we check bool condition for activating shuffle option
                if (shuffle)
                {
                    //we shuffle the the first chunk a bit(cose we can :D)
                    rWESplit[0] = Shuffle(rWESplit[0]);
                }
          
                //we store the final output key
                output = rWESplit[0];
            }
            else
            {
                throw new Exception("Key size must have a length of 16 or 32 charactes!");
            }

            //we return the output key
            return output;
        }

        /// <summary>
        /// String Shuffler
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
        /// <summary>
        /// String splitter
        /// </summary>
        /// <param name="s"></param>
        /// <param name="split"></param>
        /// <returns></returns>

        private static string[] SplitStringLenght(string s, int split)
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
