namespace GatewayApiProject.ModelsOld
{
    public class WeatherForecastOriginal
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public class LocationCoord
    {
        public int lat = 0;
        public int lng = 0;
    }



}