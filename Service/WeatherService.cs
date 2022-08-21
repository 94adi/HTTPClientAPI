using HTTPClientAPI.Models;
using HTTPClientAPI.Models.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace HTTPClientAPI.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherOptions _options;

        public WeatherService(IHttpClientFactory httpClientFactory, IOptions<WeatherOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public async Task<WeatherResult> GetWeather(Keyword keyword)
        {
            var query = keyword.Value;
            query = query.Replace(" ", "_");
            var requestURL = BuildRequestURL(query);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestURL);

            var httpClient = _httpClientFactory.CreateClient();

            var httpResponseMessage = httpClient.Send(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            MemoryStream contentMemoryStream = (MemoryStream)contentStream;

            var content = Encoding.UTF8.GetString((contentMemoryStream).ToArray());

            var result = ConvertJSONResponseToWeatherResult(content);

            return result;
        }

        private string BuildRequestURL(string query)
        {
            var baseURL = StaticDetails.WeatherBaseURI;
            var URL = $"{baseURL}key={_options.API_KEY}&q={query}&api=no";
            return URL;
        }

        private WeatherResult ConvertJSONResponseToWeatherResult(string JSONdata)
        {
            var result = new WeatherResult();
            dynamic json = JsonConvert.DeserializeObject(JSONdata);
            json = json["current"];
            result.CelsiusDegrees = (float)json["temp_c"];
            result.FahrenheitDegrees = (float)json["temp_f"];
            result.isDay = (bool)json["is_day"];
            result.HumidityIndex = (int)json["humidity"];
            result.UV = (float)json["uv"];
            result.WindDir = json["wind_dir"];
            result.WindKph = json["wind_kph"];
            result.FeelsLikeCelsius = (float)json["feelslike_c"];
            result.FeelsLikeFahrenheit = (float)json["feelslike_f"];
            result.WeatherCondition = new WeatherCondition
                                        {
                                            Text = json["condition"]["text"],
                                            IconURL = json["condition"]["icon"]
                                        };
            return result;

        }
    }
}
