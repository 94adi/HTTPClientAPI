namespace HTTPClientAPI.Models
{
    public class WeatherResult
    {
        public float CelsiusDegrees { get; set; }
        public float FahrenheitDegrees { get; set; }
        public float FeelsLikeCelsius { get; set; }
        public float FeelsLikeFahrenheit { get; set; }
        public float UV { get; set; }
        public int HumidityIndex { get; set; }
        public WeatherCondition WeatherCondition { get; set; }
        public bool isDay { get; set; }
        public string WindDir { get; set; }
        public string WindKph { get; set; }

    }

    public class WeatherCondition
    {
        public string Text { get; set; }
        public string IconURL { get; set; }
    }
}
