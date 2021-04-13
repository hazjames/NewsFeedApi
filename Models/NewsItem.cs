using System;

namespace NewsFeedApi.Models
{
    public class NewsItem
    {
        public string Title { get; }
        public string Link { get; }
        public string Description { get; }
        public NewsSource Source { get; }
        public DateTime PublishedDate { get; }
        public string Thumbnail { get; }

        public NewsItem(string title, string link, string description, NewsSource source, DateTime publishedDate, string thumbnail)
        {
            Title = title;
            Link = link;
            Description = description;
            Source = source;
            PublishedDate = publishedDate;
            Thumbnail = thumbnail;
        }
    }
}
