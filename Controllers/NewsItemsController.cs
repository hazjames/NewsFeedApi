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
        ///     GET api/news?sortBy=source,publishedDate:desc&include=newssource1,newssource2
        ///
        /// </remarks>
        /// <param name="sortBy">Sort list by passing comma seperated list of news story fields with optional sort order (default: ascending)</param>
        /// <param name="include">Comma seperated list to filter which news sources to include. Cannot be used with exclude parameter.</param>
        /// <param name="exclude">Comma seperated list to filter which news sources to exclude. Cannot be used with include parameter.</param>
        /// <returns>A list of news stories</returns>
        /// <response code="500">If a news sources list cannot be found</response>
        [HttpGet]
        [Route("getNews")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<NewsItem>>> GetNewsItems(string sortBy, string include, string exclude)
        {
            IEnumerable<NewsSource> sources = _newsService.getSources(include, exclude, _sources);

            if (!sources.Any())
                return BadRequest();

            var newsItems = await _newsService.GetNews(sources);

            if (!newsItems.Any())
                return BadRequest();

            newsItems = _newsService.Sort(newsItems.AsQueryable(), sortBy);

            return newsItems.ToList();
        }

        /// <summary>
        /// Gets an sorted aggregated list of news stories from a supplied list of news rss feeds
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST api/news?sortBy=source,publishedDate:desc
        ///     [
        ///         {
        ///             Name:"Source Name 1",
        ///             Url: "https://www.newsource1.com/rss.xml"
        ///         },
        ///         {
        ///             Name:"Source Name 2",
        ///             Url: "https://www.newsource2.com/rss.xml"
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <param name="sortBy">Sort list by passing comma seperated list of news story fields with optional sort order (default: ascending)</param>
        /// <param name="sources">A JSON list of news sources.</param>
        /// <returns>A list of news stories</returns>
        /// <response code="500">If a news sources list cannot be found or is empty</response>
        [HttpPost]
        [Route("getNews")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<NewsItem>>> GetNewsItems(string sortBy, IEnumerable<NewsSource> sources)
        {
            if (sources is null || !sources.Any())
                return BadRequest();

            var newsItems = await _newsService.GetNews(sources);

            if (!newsItems.Any())
                return BadRequest();

            newsItems = _newsService.Sort(newsItems.AsQueryable(), sortBy);

            return newsItems.ToList();
        }

    }
}
