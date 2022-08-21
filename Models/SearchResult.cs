namespace HTTPClientAPI.Models
{
    [Serializable]
    public class SearchResult
    {
        public string Query { get; set; }

        public IEnumerable<YoutubeResult> YoutubeContent { get; set; }

        public string YoutubeContentJSON { get; set; }

        public WikipediaResult WikipediaContent { get; set; }

        public string WikipediaContentJSON { get; set; }

        public WeatherResult WeatherContent { get; set; }

        public string WeatherContentJSON { get; set; }
    }
}
