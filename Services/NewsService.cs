﻿using NewsFeedApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeedApi.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsProvider _newsProvider;

        public NewsService(INewsProvider newsProvider)
        {
            _newsProvider = newsProvider;
        }

        public async Task<IEnumerable<NewsItem>> GetNews(IEnumerable<NewsSource> sources)
        {
            List<NewsItem> newsItems = new();

            if (sources is null || !sources.Any())
            {
                throw new ArgumentException("News sources list missing or empty.");
            }

            foreach (NewsSource source in sources)
            {
                var sourceNewsItems = await _newsProvider.GetNews(source);
                newsItems = newsItems.Concat(sourceNewsItems).ToList();
            }

            return newsItems;
        }

        public IEnumerable<NewsItem> Sort(IEnumerable<NewsItem> newsItems, string dateSort)
        {
            if (dateSort == "ascending")
                return newsItems.OrderBy(i => i.PublishedDate);

            return newsItems.OrderByDescending(i => i.PublishedDate);
        }
    }
}
