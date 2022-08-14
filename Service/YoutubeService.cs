using HTTPClientAPI.Models;
using HTTPClientAPI.Models.Config;
using Microsoft.Extensions.Options;
using System.Text;

namespace HTTPClientAPI.Service
{
    public class YoutubeService : IYoutubeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly YoutubeOptions _options;
        private readonly YoutubeConfig _config;


        public YoutubeService(IHttpClientFactory httpClientFactory, IOptions<YoutubeOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _config = new YoutubeConfig
            {

            };
        }

        public async Task<IEnumerable<YoutubeResult>> GetTop3Videos(YoutubeConfig youtubeConfig)
        {
            var youtubeRequestURI = BuildRequestURL(youtubeConfig);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, youtubeRequestURI)
            {
                Headers =
                {
                    { "Authorization", "Bearer " + _options.API_KEY },
                    { "Accept", "application/json" }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();

            var httpResponseMessage = httpClient.Send(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            return null;
        }

        private string BuildRequestURL(YoutubeConfig youtubeConfig)
        {
            var youtubeRequestURI = new StringBuilder(StaticDetails.YoutubeBaseURI);
            youtubeRequestURI.Append("part=snippet&");
            youtubeRequestURI.Append(youtubeConfig.Location);
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append(youtubeConfig.LocationRadius);
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append("maxResults=" + StaticDetails.YOUTUBE_MAX_RESULTS);
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append("q=" + youtubeConfig.Query.Value);
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append("relevanceLanguage=en");
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append("safeSearch=strict");
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append("type=video");
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append("access_token=" + _options.ACCESS_TOKEN);
            youtubeRequestURI.Append("&");
            youtubeRequestURI.Append("key=" + _options.API_KEY);

            return youtubeRequestURI.ToString();
        }

        private YoutubeConfig GetConfigBasedOnKeyword(string keyword)
        {
            return null;
        }

    }
}
