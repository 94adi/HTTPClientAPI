using HTTPClientAPI.Models;

namespace HTTPClientAPI.Service
{
    public interface IWeatherService
    {
        Task<WeatherResult> GetWeather(Keyword keyword);
    }
}
