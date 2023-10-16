//using System.Collections.Generic;
//using System.Text.Json;
//using System.Text.Json.Nodes;

//namespace GatewayApiProject.ModelsOld
//{
//    public class Gateway
//    {

//        public LocationCoord GetLocationCoordinations(Location location)
//        {
//            //get lat & long coordinates of location
//            string jsonLocations = $"https://geocode.maps.co/search?q={location.ToString}";

//            List<LocationCoord> locationList = JsonSerializer.Deserialize<List<LocationCoord>>(jsonLocations);

//            if (locationList != null)
//            {
//                return locationList[0];
//            }
//            return null;
//        }

//        public string GetWeather(Location location)
//        {
//            //get jsonLocation using GetLocation
//            LocationCoord coord = GetLocationCoordinations(location);

//            //var locationData = JsonSerializer.Deserialize<LocationCoord>(jsonLocation);
//            int latitude = coord.lat;
//            int longitude = coord.lng;

//            //implement said lat and long from jsonLocation into the url
//            return $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}appid=089101a1dd78a09af3c99d72f7f5eb07&units=imperial";

//        }
//    }
//}
