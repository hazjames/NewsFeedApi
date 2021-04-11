using System;

namespace NewsFeedApi.Models
{
    public class NewsSource
    {
        public string Name { get; }
        public Uri Url { get; }

        public NewsSource(string name, string url)
        {
            Name = name;
            Url = new Uri(url);
        }
    }
}
