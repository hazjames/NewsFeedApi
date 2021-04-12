using NewsFeedApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsFeedApi.Services
{
    public interface INewsService
    {
        public Task<IEnumerable<NewsItem>> GetNews(IEnumerable<NewsSource> sources);
        IEnumerable<NewsItem> Sort(IEnumerable<NewsItem> newsItems, string dateSort);
    }
}
