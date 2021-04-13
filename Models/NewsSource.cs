using System;

namespace NewsFeedApi.Models
{
    public class NewsSource
    {
        public string Name { get; }
        public string Url { get; }
        public string ImageUrl { get; set; }

        public NewsSource(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
