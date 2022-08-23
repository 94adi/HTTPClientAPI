using HTTPClientAPI.Models;
using HTTPClientAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HTTPClientAPI.Controllers
{
    public class HomeController : Controller
    {
        //TO DO:
        //1. Handle possible exception such the application doesn't break if any of the APIs don't work
        //2. Add additional in memory data and consider refactor where necessary with keyword/youtube config
        //3. Add data in a view (Result.cshtml)

        private readonly ILogger<HomeController> _logger;
        private readonly IYoutubeService _youtubeService;
        private readonly IWikipediaService _wikipediaService;
        private readonly IWeatherService _weatherService;

        public HomeController(ILogger<HomeController> logger,
                              IYoutubeService youtubeService,
                              IWikipediaService wikipediaService,
                              IWeatherService weatherService)
        {
            _logger = logger;
            _youtubeService = youtubeService;
            _wikipediaService = wikipediaService;
            _weatherService = weatherService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IndexVM model = new IndexVM();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexVM model)
        {
            if(model == null || model.Keyword == null)
            {
                return View(model);
            }

            SearchResult searchResult = new SearchResult();

            var keyword = model.GetKeywordById(model.Keyword.Id);

            searchResult.Query = keyword?.Value == null ? "" : keyword.Value;

            var youtubeConfig = model.GetYoutubeConfigById(model.Keyword.Id);

            searchResult.YoutubeContent = await _youtubeService.GetTop3Videos(youtubeConfig);
            searchResult.YoutubeContentJSON = JsonConvert.SerializeObject(searchResult.YoutubeContent);

            searchResult.WikipediaContent = await _wikipediaService.GetHighlight(keyword);
            searchResult.WikipediaContentJSON = JsonConvert.SerializeObject(searchResult.WikipediaContent);

            searchResult.WeatherContent = await _weatherService.GetWeather(keyword);
            searchResult.WeatherContentJSON = JsonConvert.SerializeObject(searchResult.WeatherContent);

            return RedirectToAction("Result", searchResult);
        }

        [HttpGet]
        public IActionResult Result(SearchResult searchResult)
        {
            if(searchResult.YoutubeContentJSON != null)
                searchResult.YoutubeContent = JsonConvert.DeserializeObject<List<YoutubeResult>>(searchResult.YoutubeContentJSON);

            if (searchResult.WikipediaContentJSON != null)
                searchResult.WikipediaContent = JsonConvert.DeserializeObject<WikipediaResult>(searchResult.WikipediaContentJSON);

            if (searchResult.WeatherContentJSON != null)
                searchResult.WeatherContent = JsonConvert.DeserializeObject<WeatherResult>(searchResult.WeatherContentJSON);
           
            return View(searchResult);
        }
    }
}