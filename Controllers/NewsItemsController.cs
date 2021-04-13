using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsFeedApi.Models;
using NewsFeedApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeedApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class NewsItemsController : ControllerBase
    {
        private readonly INewsService _newsService;

        private readonly ILogger _logger;

        private readonly IEnumerable<NewsSource> _sources;

        public NewsItemsController(ILogger<NewsItemsController> logger,
                                    INewsService newsService,
                                    IConfiguration config)
        {
            _logger = logger;
            _newsService = newsService;
            _sources = config.GetSection("NewsRssFeeds")
                .GetChildren()
                .Select(source => new NewsSource(source["name"], source["url"]));
        }

        // GET: api/news
        [HttpGet]
        [Route("getNews")]
        public async Task<ActionResult<IEnumerable<NewsItem>>> GetNewsItems(string sortDate)
        {
            if (_sources is null || !_sources.Any())
            {
                _logger.LogError("News sources list not found.");
                throw new ArgumentException("No news sources were found. Please contact your administrator.");
            }

            var newsItems = await _newsService.GetNews(_sources);

            newsItems = _newsService.Sort(newsItems, sortDate);

            return newsItems.ToList();
        }

    }
}
