using HTTPClientAPI.Models;
using HTTPClientAPI.Models.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace HTTPClientAPI.Service
{

    //API: https://openweathermap.org/

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
            var URL = $"{baseURL}q={query}&appid={_options.API_KEY}&units=metric";
            return URL;
        }

        private WeatherResult ConvertJSONResponseToWeatherResult(string JSONdata)
        {
            var result = new WeatherResult();

            float floatValueStore;
            int intValueStore;
            long longValueStore;
            try
            {
                dynamic json = JsonConvert.DeserializeObject(JSONdata);
                dynamic main = json["main"];
                dynamic wind = json["wind"];
                dynamic sys = json["sys"];
                dynamic status = json["weather"][0]["main"];

                result.CelsiusDegrees = (float.TryParse((string)main["temp"], out floatValueStore)) == true ? floatValueStore : result.CelsiusDegrees;
                result.FeelsLikeCelsius = (float.TryParse((string)main["feels_like"], out floatValueStore)) == true ? floatValueStore : result.FeelsLikeCelsius;
                result.CelsiusMax = (float.TryParse((string)main["temp_max"], out floatValueStore)) == true ? floatValueStore : result.CelsiusMax;
                result.CelsiusMin = (float.TryParse((string)main["temp_min"], out floatValueStore)) == true ? floatValueStore : result.CelsiusMin;
                result.HumidityIndex = (int.TryParse((string)main["humidity"], out intValueStore)) == true ? intValueStore : result.HumidityIndex;
                result.WindDeg = (string)wind["deg"];
                result.WindKph = (string)wind["speed"];
                result.Status = (string)status;
            }
            catch(Exception e)
            {
                //log exception
            }

            return result;

        }
    }
}
