using NewsFeedApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsFeedApi.Services
{
    public interface INewsProvider
    {
        Task<IEnumerable<NewsItem>> GetNews(NewsSource source);
    }
}
