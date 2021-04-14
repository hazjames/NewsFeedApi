using Microsoft.Extensions.Logging;
using NewsFeedApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace NewsFeedApi.Services
{
    public class NewsRssParser : INewsProvider
    {
        private ILogger _logger;

        public List<NewsSource> sources = new();

        public NewsRssParser(ILogger<NewsRssParser> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<NewsItem>> GetNews(NewsSource source)
        {
            try
            {
                var rssFeed = await RetrieveRssFeed(source.Url);

                SetNewsSourceImageUrl(source, rssFeed);

                return GetNewsItems(source, rssFeed);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ArgumentNullException:
                        _logger.LogWarning(ex, $"Url not supplied for source {source.Name}.");
                        break;
                    case XmlException:
                    case UriFormatException:
                        _logger.LogWarning(ex, $"Source URL supplied is invalid for source {source.Name}.");
                        break;
                    default: throw;
                }

                return Enumerable.Empty<NewsItem>();
            }
        }

        private IEnumerable<NewsItem> GetNewsItems(NewsSource source, SyndicationFeed rssFeed)
        {
            if (!rssFeed.Items.Any())
                return Enumerable.Empty<NewsItem>();

            return from item in rssFeed.Items
                   select CreateNewsItem(item, source);
        }

        private NewsItem CreateNewsItem(SyndicationItem item, NewsSource source)
        {
            char[] trimChars = { ' ', '\t', '\n' };

            return new NewsItem(
                       item.Title.Text.Trim(trimChars),
                       item.Links[0].Uri.ToString(),
                       item.Summary?.Text.Trim(trimChars),
                       source,
                       item.PublishDate.UtcDateTime,
                       item.Links.SingleOrDefault(l => l.RelationshipType == "enclosure")?.Uri.ToString());
        }

        private void SetNewsSourceImageUrl(NewsSource source, SyndicationFeed rssFeed)
        {
            source.ImageUrl = rssFeed.ImageUrl?.ToString();
        }

        private async Task<SyndicationFeed> RetrieveRssFeed(string newsSourceUrl)
        {
            using var reader = XmlReader.Create(newsSourceUrl);
            return await Task.Run(() => SyndicationFeed.Load(reader));
        }
    }
}
