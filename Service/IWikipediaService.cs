using HTTPClientAPI.Models;
using HTTPClientAPI.Models.Config;

namespace HTTPClientAPI.Service
{
    public interface IWikipediaService
    {
        Task<WikipediaResult> GetHighlight(Keyword keyword);
    }
}
