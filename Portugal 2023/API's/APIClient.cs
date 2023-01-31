using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portugal_2023
{
    public class APIClient
    {
        HttpClient _httpClient;
        ReqVars vars = new ReqVars();
        Weather intense = new Weather();
        WeatherLON intenseLON = new WeatherLON();
        public string varsyr { get; set; }
        public double intcarb { get; set; }
        public double intcarbLON { get; set; }

        public APIClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ReqVars> GetGBP()
        {

            Uri uri = new Uri("https://v6.exchangerate-api.com/v6/13190f03d445d8a2357ad591/pair/EUR/GBP");
            try
            {
                HttpResponseMessage rs = await _httpClient.GetAsync(uri);
                string rsStr = await rs.Content.ReadAsStringAsync();
                vars = JsonConvert.DeserializeObject<ReqVars>(rsStr);
                varsyr = Convert.ToString(vars.conversion_rate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);            
            }

            return vars;
        }

        public async Task<Weather> GetWeather()
        {

            Uri uri = new Uri("https://api.open-meteo.com/v1/forecast?latitude=38.72&longitude=-9.13&current_weather=true");
            try
            {
                HttpResponseMessage rs = await _httpClient.GetAsync(uri);
                string rsStr = await rs.Content.ReadAsStringAsync();
                intense = JsonConvert.DeserializeObject<Weather>(rsStr);
                intcarb = intense.current_weather.temperature;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return intense;
        }
        public async Task<WeatherLON> GetWeatherLON()
        {

            Uri uri = new Uri("https://api.open-meteo.com/v1/forecast?latitude=51.51&longitude=-0.13&current_weather=true");
            try
            {
                HttpResponseMessage rs = await _httpClient.GetAsync(uri);
                string rsStr = await rs.Content.ReadAsStringAsync();
                intenseLON = JsonConvert.DeserializeObject<WeatherLON>(rsStr);
                intcarbLON = intense.current_weather.temperature;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return intenseLON;
        }
    }
}
