namespace HTTPClientAPI.Models
{
    [Serializable]
    public class SearchResult
    {
        public string Query { get; set; }
        public IEnumerable<YoutubeResult> YoutubeContent { get; set; }
        public string YoutubeContentJSON { get; set; }
    }
}
