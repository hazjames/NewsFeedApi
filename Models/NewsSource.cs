using System;

namespace NewsFeedApi.Models
{
    public class NewsSource
    {
        public string Name { get; }
        public string SourceUrl { get; }
        public string ImageUrl { get; set; }

        public NewsSource(string name, string sourceUrl)
        {
            Name = name;
            SourceUrl = sourceUrl;
        }
    }
}
