using GatewayApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace GatewayApiProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
    //    private static readonly string[] Summaries = new[]
    //    {
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public Weather Get(string location)
        {
            //return $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid=089101a1dd78a09af3c99d72f7f5eb07&units=metric";
            
                var apiKey = "089101a1dd78a09af3c99d72f7f5eb07"; 
                var apiEndpoint = $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={apiKey}&units=metric";
                Weather weather = new();
                try
                {
                    // Download the JSON response as a string
                    var responseJson = new WebClient().DownloadString(apiEndpoint);

                    // Deserialize the JSON response into a Weather object
                    RootWeather fullWeather = JsonSerializer.Deserialize<RootWeather>(responseJson);
                    weather.temp = fullWeather.main.temp;
                    weather.humidity = fullWeather.main.humidity;
                    weather.feels_like = fullWeather.main.feels_like;
                
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return weather;
            
        }
    }
}