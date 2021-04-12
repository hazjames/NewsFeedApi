using NewsFeedApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace NewsFeedApi.Services
{
    public class NewsRssParser : INewsProvider
    {
        public List<NewsSource> sources = new();

        public async Task<IEnumerable<NewsItem>> GetNews(NewsSource source)
        {
            var rssFeed = await RetrieveRssFeed(source.SourceUrl);

            SetNewsSourceImageUrl(source, rssFeed);

            return GetNewsItems(source, rssFeed);
        }

        private IEnumerable<NewsItem> GetNewsItems(NewsSource source, SyndicationFeed rssFeed)
        {
            char[] trimChars = { ' ', '\t', '\n' };

            return from item in rssFeed.Items
                   select new NewsItem(
                       item.Title.Text.Trim(trimChars),
                       item.Links[0].Uri.ToString(),
                       item.Summary.Text.Trim(trimChars),
                       source,
                       item.PublishDate.UtcDateTime,
                       item.Links.SingleOrDefault(l => l.RelationshipType == "enclosure").Uri.ToString());
        }

        private void SetNewsSourceImageUrl(NewsSource source, SyndicationFeed rssFeed)
        {
            source.ImageUrl = rssFeed.ImageUrl.AbsoluteUri;
        }

        private async Task<SyndicationFeed> RetrieveRssFeed(string newsSourceUrl)
        {
            using (var reader = XmlReader.Create(newsSourceUrl))
            {
                return await Task.Run(() => SyndicationFeed.Load(reader));
            }
        }
    }
}
