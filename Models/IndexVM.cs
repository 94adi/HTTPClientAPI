using HTTPClientAPI.Models.Config;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HTTPClientAPI.Models
{
    public class IndexVM
    {
        private readonly IEnumerable<Keyword> _keywords;
        private readonly IEnumerable<YoutubeConfig> _youtubeConfigs;

        public IEnumerable<SelectListItem>? KeywordsList { get; private set; }
        public Keyword? Keyword { get; set; }
        public IEnumerable<Keyword> Keywords { get { return _keywords; } }
        public IEnumerable<YoutubeConfig> YoutubeConfigs { get { return _youtubeConfigs; } }

        public IndexVM()
        {
            //in memory data
            _keywords = new List<Keyword>
            {
                new Keyword
                {
                    Id = 1,
                    Value = "los angeles"
                },
                new Keyword
                {
                    Id = 2,
                    Value = "new york"
                },
                new Keyword
                {
                    Id = 3,
                    Value = "miami"
                }
            };

            _youtubeConfigs = new List<YoutubeConfig>
            {
                new YoutubeConfig
                {
                    Id = 1,
                    Location = "34.0522300,-118.2436800",
                    LocationRadius = "10000ft",
                    Query = new Keyword
                    {
                        Id = 1,
                        Value = "los angeles"
                    }           
                } 
            };

            KeywordsList = Keywords?.Select(i => new SelectListItem
                            {
                                Text = i.Value,
                                Value = i.Id.ToString()
                            });

        }

        public Keyword? GetKeywordById(int id)
        {
            var result = _keywords.Where(i => i.Id == id).FirstOrDefault();
            return result;
        }

        public YoutubeConfig? GetYoutubeConfigById(int id)
        {
            var result = _youtubeConfigs.Where(i => i.Id == id).FirstOrDefault();
            return result;
        }
        //youtube section data
        //wikipedia section data,
        //weather section data
    }
}
