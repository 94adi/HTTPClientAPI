using HTTPClientAPI.Models;
using HTTPClientAPI.Models.Config;

namespace HTTPClientAPI.Service
{
    public interface IYoutubeService
    {
        Task<IEnumerable<YoutubeResult>> GetTop3Videos(YoutubeConfig youtubeConfig);
    }
}
