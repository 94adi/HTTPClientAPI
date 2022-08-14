using HTTPClientAPI.Models;
using HTTPClientAPI.Models.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
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
            IEnumerable<YoutubeResult> result = new List<YoutubeResult>();

            var youtubeRequestURI = BuildRequestURL(youtubeConfig);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, youtubeRequestURI)
            {
                Headers =
                {
                    { "Accept", "application/json" }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();

            var httpResponseMessage = httpClient.Send(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            MemoryStream contentMemoryStream = (MemoryStream)contentStream;

            var content = Encoding.UTF8.GetString((contentMemoryStream).ToArray());

            if(content != null)
                result = ConvertJSONResponseToYoutubeResult(content);

            return result;
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

        private IEnumerable<YoutubeResult> ConvertJSONResponseToYoutubeResult(string JSON)
        {
            dynamic json = JsonConvert.DeserializeObject(JSON);
            var items = json["items"];

            var resultCollection = new List<YoutubeResult>();

            foreach(var item in items)
            {
                var youtubeResult = ConvertItemToYoutubeResult(item);
                resultCollection.Add(youtubeResult);
            }

            return resultCollection;
        }

        private YoutubeResult ConvertItemToYoutubeResult(object item)
        {
            YoutubeResult youtubeResult = new YoutubeResult();
            dynamic currentItem = item;

            youtubeResult.VideoId = currentItem["id"]["videoId"];

            currentItem = currentItem["snippet"];

            youtubeResult.Title = currentItem["title"];

            youtubeResult.Description = currentItem["description"];

            youtubeResult.Thumbnail = new YoutubeThumbnail
            {
                Url = currentItem["thumbnails"]["high"]["url"],
                Width = currentItem["thumbnails"]["high"]["width"],
                Height = currentItem["thumbnails"]["high"]["height"]
            };

            try
            {
                string publishedAt = currentItem["publishedAt"];
                youtubeResult.PublishedAt = DateTime.ParseExact(publishedAt,
                            "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                youtubeResult.PublishedAt = DateTime.MinValue;
            }

            return youtubeResult;
        }

    }
}
