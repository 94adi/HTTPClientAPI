using HTTPClientAPI.Models;
using HTTPClientAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HTTPClientAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IYoutubeService _youtubeService;

        public HomeController(ILogger<HomeController> logger,
                              IYoutubeService youtubeService)
        {
            _logger = logger;
            _youtubeService = youtubeService;
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

            return RedirectToAction("Result", searchResult);
        }

        [HttpGet]
        public IActionResult Result(SearchResult searchResult)
        {
            if(searchResult.YoutubeContentJSON != null)
                searchResult.YoutubeContent = JsonConvert.DeserializeObject<List<YoutubeResult>>(searchResult.YoutubeContentJSON);
           
            return View(searchResult);
        }
    }
}