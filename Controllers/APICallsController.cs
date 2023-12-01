using CloudComputingProject.Models;
using System.Net;
using System.Text.Json;

public static class APICallsController
{
	public static async Task<bool> IsHebrewHolidayAsync()
	{
		using (var client = new HttpClient())
		{
			var apiEndpoint = "http://www.apigateway.somee.com/HebrewDate/IsHebrewHoliday";
			var response = await client.GetStringAsync(apiEndpoint);
			return bool.Parse(response);
		}
	}
	
	public static async Task<bool> IsActualLocationAsync(string city, string street)
	{
		var apiEndpoint = $"http://www.apigateway.somee.com/Address?city={city}&{street}";

		using (var client = new HttpClient())
		{
			var response = await client.GetStringAsync(apiEndpoint);
			return bool.Parse(response);
		}
	}
}

