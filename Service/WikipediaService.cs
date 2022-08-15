using HTTPClientAPI.Models;
using HTTPClientAPI.Models.Config;
using Newtonsoft.Json;
using System.Text;

namespace HTTPClientAPI.Service
{
    public class WikipediaService : IWikipediaService
    {

        //Wikipedia API swagger page: https://en.wikipedia.org/api/rest_v1/#/Page%20content/get_page_summary__title_

        private readonly IHttpClientFactory _httpClientFactory;

        public WikipediaService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WikipediaResult> GetHighlight(Keyword keyword)
        {
            var query = keyword.Value;
            query = query.Replace(" ", "_");

            var wikipediaRequestURI = StaticDetails.WikipediaBaseURI + query; //+keyword here
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, wikipediaRequestURI)
            {
                Headers =
                {
                    { "Accept", "application/json" },
                    { "charset", "utf-8" },
                    { "profile", "\"https://www.mediawiki.org/wiki/Specs/Summary/1.4.2\"" }
                }
            };

            
            var httpClient = _httpClientFactory.CreateClient();

            var httpResponseMessage = httpClient.Send(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            MemoryStream contentMemoryStream = (MemoryStream)contentStream;

            var content = Encoding.UTF8.GetString((contentMemoryStream).ToArray());

            var result = ConvertJSONResponseToWikipediaResult(content);

            return result;
        }

        private WikipediaResult ConvertJSONResponseToWikipediaResult(string JSONdata)
        {
            var result = new WikipediaResult();
            dynamic json = JsonConvert.DeserializeObject(JSONdata);

            result.ShortDescription = json["description"];
            result.Description = json["extract"];

            dynamic thumbnail = json["thumbnail"];

            result.Thumbnail = new Thumbnail
            {
                Url = (string)thumbnail.source,
                Width = (int)thumbnail.width,
                Height = (int)thumbnail.height
            };

            result.PageUrl = json["content_urls"]["desktop"]["page"];

            return result;
        }
    }
}
