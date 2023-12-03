using GatewayApiProject.Models;
//using GatewayApiProject.ModelsOld;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace GatewayApiProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;

        public AddressController(ILogger<AddressController> logger)
        {
            _logger = logger;
        }

		public static async Task<string> Translate(string input)
		{
			var apiEndpoint = String.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", "en", "he", Uri.EscapeUriString(input));

			using (var client = new HttpClient())
			{
				var response = await client.GetStringAsync(apiEndpoint);
				var data = JsonSerializer.Deserialize<List<object>>(response);
				return ((JsonElement)data[0]).EnumerateArray().First().EnumerateArray().First().GetString(); ;
			}
		}

		[HttpGet(Name = "GetAddress")]
        public bool Get(string city, string street)
        {
			city = Translate(city).Result;
			street = Translate(street).Result;
			Location location = new() { City = city, Street = street};
            //get address from api call
            var resource_id = "bf185c7f-1a4e-4662-88c5-fa118a244bda";
            var apiEndpoint = $"https://data.gov.il/api/3/action/datastore_search?resource_id={resource_id}&q={location.City}&limit=50000";

            try
            {
                // Download the JSON response as a string
                var responseJson = new WebClient().DownloadString(apiEndpoint);

                // Deserialize the JSON response into a Location object
                RootLocation fullLocation = JsonSerializer.Deserialize<RootLocation>(responseJson);
                
                if (fullLocation.result.records != null) { 
                    foreach(var record in fullLocation.result.records)
                    {
                        if (string.Equals(record.street_name, location.Street+" "))
                            return true;
                    }
                }

            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
    }
}
