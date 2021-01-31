using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WELib
{
    /// <summary>
    /// Class for open weather map API functions!
    /// </summary>


    public class OpenWeatherMap
    {
        //declare the variables
        static readonly HttpClient clientH = new HttpClient();

        //------------------------------


        /// <summary>
        /// Weather data grab from openweathermap.org!
        /// </summary>
        /// <param name="apiKey">Required API key from openweathermap.org!</param>
        /// <param name="CityName">Provide a city name!</param>
        /// <param name="units">Add metric for Celsius/Add imperial for Fahrenheit</param>
        /// <returns></returns>
        public static string WeatherForecast(string apiKey, string CityName, string units)
        {
            string outs = string.Empty;
            try
            {

                if (apiKey.Length > 0) // we check the lenght of api key and units
                {
                    if (units.Length > 0)
                    {

                        //Open weather map API link with celsius 
                        // TODO: will decide if I put switch for ferenhait
                        string html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=" + units;

                        HttpResponseMessage response = clientH.GetAsync(string.Format(html, CityName, apiKey)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().Result;

                        string l = "";
                        string line = "";
                        //parssing the oudtput
                        responseBody = responseBody.Replace(",", Environment.NewLine);
                        responseBody = responseBody.Replace("\"", "");
                        responseBody = responseBody.Replace("}", "");
                        responseBody = responseBody.Replace("{", "");
                        responseBody = responseBody.Replace("wind:", "");
                        responseBody = responseBody.Replace("main:", "");
                        //---------------------------------
                        using (var sr = new StringReader(responseBody))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {

                                //we check only for what we need, like: temp, feel, humidity, wind speed
                                if (line.Contains("temp") || line.Contains("feel") || line.Contains("humidity") || line.Contains("speed"))
                                {
                                    l += line + Environment.NewLine;
                                }
                            }
                        }
                        outs = l;
                        //renaming output parts
                        outs = outs.Replace("temp:", "T");
                        outs = outs.Replace("feels_like:", "F");
                        outs = outs.Replace("temp_min:", "Mi");
                        outs = outs.Replace("temp_max:", "Ma");
                        outs = outs.Replace("humidity:", "H");
                        outs = outs.Replace("speed:", "W");
                        outs = Regex.Replace(outs, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                        //---------------------------------
                    }
                    else
                    {
                        //we print the issue on the log viewer console
                        outs = "No openweathermap.org API Key saved or unit input inccorect! Please check";

                    }
                }
            }
            catch
            {
                //In case of error we output this in console.
                outs = "Error: Please check city name!";
            }

            //print the final weather forecast
            return outs;

        }
    }
}
