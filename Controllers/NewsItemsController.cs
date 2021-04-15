using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        private readonly IEnumerable<NewsSource> _sources;

        public NewsItemsController(INewsService newsService,
                                    IConfiguration config)
        {
            _newsService = newsService;
            _sources = config.GetSection("NewsRssFeeds")
                .GetChildren()
                .Select(source => new NewsSource(source["name"], source["url"]));
        }

        /// <summary>
        /// Gets a sorted list of news stories from a list of well known news rss feeds
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET api/news?sortBy=source,publishedDate:desc
        ///
        /// </remarks>
        /// <param name="sortBy">Sort list by passing comma seperated list of news story fields with optional sort order (default: ascending)</param>
        /// <returns>A list of news stories</returns>
        /// <response code="500">If a news sources list cannot be found</response>
        [HttpGet]
        [Route("getNews")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<NewsItem>>> GetNewsItems(string sortBy)
        {
            var newsItems = await _newsService.GetNews(_sources);

            newsItems = _newsService.Sort(newsItems.AsQueryable(), sortBy);

            return newsItems.ToList();
        }

    }
}
