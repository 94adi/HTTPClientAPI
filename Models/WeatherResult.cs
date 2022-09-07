namespace HTTPClientAPI.Models
{
    public class WeatherResult
    {
        public float CelsiusDegrees { get; set; }
        public float FeelsLikeCelsius { get; set; }
        public float CelsiusMax { get; set; }
        public float CelsiusMin { get; set; }
        public int HumidityIndex { get; set; }
        public bool isDay { get; set; }
        public string WindDeg { get; set; }
        public string WindKph { get; set; }
        public string Status { get; set; }

    }

}
