using NewsFeedApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeedApi.Services
{
    public interface INewsService
    {
        public Task<IEnumerable<NewsItem>> GetNews(IEnumerable<NewsSource> sources);
        IEnumerable<NewsItem> Sort(IQueryable<NewsItem> newsItems, string sortBy);
        IEnumerable<NewsSource> getSources(string include, string exclude, IEnumerable<NewsSource> sources);
    }
}
