using GatewayApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Globalization;
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

		/// <summary>
		/// Check if there's a Hebrew holiday for the current week
		/// </summary>
		/// <returns>True if there's a holiday; otherwise, false</returns>
        [HttpGet("IsHebrewHoliday", Name = "GetIsHebrewHoliday")]
        //dates are in format of YYYY-MM-DD
        public bool GetIsHebrewHoliday()
        {
			// Get today's date
			DateTime today = DateTime.Today;

			// Calculate the end of the week
			DateTime endOfWeek = today.AddDays(DayOfWeek.Saturday - today.DayOfWeek);

			// Format dates for API
			string startDate = today.ToString("yyyy-MM-dd");
			string endDate = endOfWeek.ToString("yyyy-MM-dd");

            //easily modifiable to accept location queries, as it may differ slightly being in IL or not
            //if (location == "") 
            string location = "Jerusalem";

			//get from api call
			string apiEndpoint = $"https://www.hebcal.com/hebcal?v=1&cfg=json&maj=on&min=on&mod=on&nx=on&year=now&month=x&ss=on&mf=on&c=on&geo=city&city={location}&M=on&s=on&start={startDate}&end={endDate}";
			


            try
            {
                // Download the JSON response as a string
                var responseJson = new WebClient().DownloadString(apiEndpoint);

                // Deserialize the JSON response into a Location object
                RootDate fullDate = JsonSerializer.Deserialize<RootDate>(responseJson);

				JObject data = JObject.Parse(responseJson);

				// Check if there is a holiday in the week
				bool isHoliday = data["items"].Any(item =>
				(string)item["category"] == "holiday" &&
				!((string)item["title"]).StartsWith("Parashat") &&
				!((string)item["title"]).StartsWith("Shabbat") &&
				!((string)item["title"]).StartsWith("Yom HaAliyah") &&
				!((string)item["title"]).StartsWith("Rosh Chodesh") &&
				!((string)item["title"]).EndsWith("Memorial Day"));

				////possibility of registering the holiday name
				//var holidayItem = data["items"].FirstOrDefault(item =>
				//(string)item["category"] == "holiday" &&
				//!((string)item["title"]).StartsWith("Parashat") &&
				//!((string)item["title"]).StartsWith("Shabbat") &&
				//!((string)item["title"]).StartsWith("Yom HaAliyah") &&
				//!((string)item["title"]).StartsWith("Rosh Chodesh") &&
				//!((string)item["title"]).EndsWith("Memorial Day"));
				//if (holidayItem != null)
				//{
				//	var j = $"Holiday found: {holidayItem["title"]}";
				//	return j;
				//}

				return isHoliday;

			}
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }

		/// <summary>
		/// Convert a Gregorian date to a Hebrew date
		/// </summary>
		/// <param name="gDate">The Gregorian date in the format "YYYY-MM-DD"</param>
		/// <returns>The Hebrew date parts corresponding to the Gregorian date given</returns>
		[HttpGet("HebrewDate", Name = "GetHebrewDate")]
		
		public HeDateParts GetHebrewDate(string gDate)
		{
			DateTime date = DateTime.Today;
			// parse the date string into a DateTime object
			// note: gDates are in format of "YYYY-MM-DD"
			if (gDate != "")
				date = DateTime.ParseExact(gDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

			// get Hebrew date from api call
			string apiEndpoint = $"https://www.hebcal.com/converter?cfg=json&gy={date.Year}&gm={date.Month}&gd={date.Day}&g2h=1";

			HeDateParts hebDate = new();

			try
			{
				// download the JSON response as a string
				var responseJson = new WebClient().DownloadString(apiEndpoint);

				// deserialize the JSON response into a Location object
				RootDate fullDate = JsonSerializer.Deserialize<RootDate>(responseJson);
				
				// extract Hebrew date parts
				hebDate = new() { y = fullDate.heDateParts.y, m = fullDate.heDateParts.m, d = fullDate.heDateParts.d };

				return hebDate;

			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
			}

			return null;

		}
	}
}
