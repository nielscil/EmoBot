using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alice.StandardContent
{
    internal static class WeatherAPI
    {
        private const string Request = "?q={0}&units=metric&appid=b4cf58a66894a114580988cc65d8bdcb";

        public static Uri BaseAddress
        {
            get
            {
                return new Uri("http://api.openweathermap.org/data/2.5/weather");
            }
        }

        public static float GetTempature(string place)
        {
            Task<float> temp = GetTempatureAsync(place);
            temp.Wait();
            return temp.Result;
        }

        public static async Task<float> GetTempatureAsync(string place)
        {
            float tempature = float.MinValue;
            string responseString;
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var response = await client.GetAsync(string.Format(Request, place));
                responseString = await response.Content.ReadAsStringAsync();
            }

            if(!string.IsNullOrEmpty(responseString))
            {
                var weather = JsonConvert.DeserializeObject<Weather>(responseString);
                tempature = weather.Main.Tempature;
            }

            return tempature;
        }

        public class Weather
        {
            [JsonProperty("main")]
            public Main Main { get; set; }
        }

        public class Main
        {
            [JsonProperty("temp")]
            public float Tempature { get; set; }
        }
    }
}
