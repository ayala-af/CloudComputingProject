using GatewayApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace GatewayApiProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HebrewDateController : ControllerBase
    {
        private readonly ILogger<HebrewDateController> _logger;

        public HebrewDateController(ILogger<HebrewDateController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetHebrewDate")]
        //gDate in format YYYY-MM-DD
        public bool Get(string gDate)
        {
            //get from api call
            
            var apiEndpoint = $"";

            try
            {
                // Download the JSON response as a string
                var responseJson = new WebClient().DownloadString(apiEndpoint);

                // Deserialize the JSON response into a Location object
                RootDate fullDate = JsonSerializer.Deserialize<RootDate>(responseJson);

                

            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
    }
}
