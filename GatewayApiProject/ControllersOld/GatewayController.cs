//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using GatewayApiProject.ModelsOld;
//using Microsoft.AspNetCore.Mvc;

//namespace GatewayApiProject.ControllersOld
//{
//    //[ApiController]
//    //[Route("[controller]")]
//    //public class GatewayController : ControllerBase
//    //{
//    //    private static readonly string[] Summaries = new[]
//    //    {
//    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//    //};

//    //    private readonly ILogger<WeatherForecastController> _logger;

//    //    public WeatherForecastController(ILogger<WeatherForecastController> logger)
//    //    {
//    //        _logger = logger;
//    //    }

//    //    [HttpGet(Name = "GetWeatherForecast")]
//    //    public IEnumerable<WeatherForecast> Get()
//    //    {
//    //        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//    //        {
//    //            Date = DateTime.Now.AddDays(index),
//    //            TemperatureC = Random.Shared.Next(-20, 55),
//    //            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//    //        })
//    //        .ToArray();
//    //    }
//}
//[Route("api/[controller]")]
//[ApiController]
//public class GatewayController : ControllerBase
//{
//    private readonly Gateway _gateway;

//    public GatewayController(Gateway gateway)
//    {
//        _gateway = gateway;
//    }

//    // GET api/gateway/location/{location}
//    [HttpGet("location/{location}")]
//    public async Task<ActionResult<LocationCoord>> GetLocationCoordinationsAsync(Location location)
//    {
//        string locationCoord;// = await _gateway.GetLocationCoordinationsAsync(location);
//        if ("" == "")
//        {
//            return NotFound();
//        }

//        return Ok(locationCoord);
//    }

//    // GET api/gateway/weather/{location}
//    [HttpGet("weather/{location}")]
//    public async Task<ActionResult<string>> GetWeatherAsync(Location location)
//    {
//        string weatherUrl;// = await _gateway.GetWeatherAsync(location);
//        if ("" == "")
//        {
//            return NotFound();
//        }

//        return Ok(weatherUrl);
//    }
//}

