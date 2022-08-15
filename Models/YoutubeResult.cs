namespace HTTPClientAPI.Models
{
    [Serializable]
    public class YoutubeResult
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }

    }
}
