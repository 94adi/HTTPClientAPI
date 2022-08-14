using HTTPClientAPI.Models;
using HTTPClientAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
        public IActionResult Index(IndexVM model)
        {
            if(model.Keyword.Id == null)
            {
                return View(model);
            }

            SearchResult searchResult = new SearchResult();
            searchResult.Query = model.GetKeywordById(model.Keyword.Id).Value;
            var youtubeConfig = model.GetYoutubeConfigById(model.Keyword.Id);

            var youtubeResult = _youtubeService.GetTop3Videos(youtubeConfig);
            //use api


            searchResult.YoutubeContent = "content";
            //generate model /w data
            return RedirectToAction("Result", searchResult);
        }

        [HttpGet]
        public IActionResult Result(SearchResult model)
        {
            return View(model);
        }


    }
}