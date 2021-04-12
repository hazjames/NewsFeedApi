using Microsoft.AspNetCore.Mvc;
using NewsFeedApi.Models;
using NewsFeedApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsItem>>> GetNewsItems(string sortDate, IEnumerable<NewsSource> sources)
        {
            var newsItems = await _newsService.GetNews(sources);

            newsItems = _newsService.Sort(newsItems, sortDate);

            return newsItems.ToList();
        }
    }
}
