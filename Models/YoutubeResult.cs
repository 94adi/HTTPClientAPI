namespace HTTPClientAPI.Models
{
    public class YoutubeResult
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public YoutubeThumbnail Thumbnail { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }
    }

    public class YoutubeThumbnail
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
