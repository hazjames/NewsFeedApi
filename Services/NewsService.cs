using NewsFeedApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
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

        public IEnumerable<NewsSource> getSources(string include, string exclude, IEnumerable<NewsSource> sources)
        {
            if (!string.IsNullOrEmpty(include) && !string.IsNullOrEmpty(exclude))
                return Enumerable.Empty<NewsSource>();

            if (!string.IsNullOrEmpty(include))
            {
                var selectedSources = include.Split(',');

                return from source in sources
                       where selectedSources.Contains(source.Name.Replace(" ", string.Empty).ToLower())
                       select source;
            }

            if (!string.IsNullOrEmpty(exclude))
            {
                var excludedSources = exclude.Split(',');
                return from source in sources
                       where !excludedSources.Contains(source.Name.Replace(" ", string.Empty).ToLower())
                       select source;
            }

            return sources;
        }

        public IEnumerable<NewsItem> Sort(IQueryable<NewsItem> newsItems, string sortBy)
        {
            if (!newsItems.Any())
                return newsItems.AsEnumerable<NewsItem>();

            if (string.IsNullOrWhiteSpace(sortBy))
                return newsItems.OrderByDescending(i => i.PublishedDate)
                    .AsEnumerable<NewsItem>();

            var sortParams = sortBy.Trim().Split(',');
            var newsItemProperties = typeof(NewsItem).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (string param in sortParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQuery = param.Split(':')[0];
                var newsItemProperty = newsItemProperties.FirstOrDefault(p => p.Name.Equals(propertyFromQuery, StringComparison.InvariantCultureIgnoreCase));

                if (newsItemProperty == null)
                    continue;

                var sortingOrder = param.EndsWith("desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{newsItemProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return newsItems.OrderByDescending(i => i.PublishedDate)
                    .AsEnumerable<NewsItem>();
            }

            return newsItems.OrderBy(orderQuery);
        }
    }
}
