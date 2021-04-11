using System;

namespace NewsFeedApi.Models
{
    public class NewsStory
    {
        public string Title { get; }
        public Uri Link { get; }
        public string Description { get; }
        public string Source { get; }
        public DateTime PublishedDate { get; }
        public Uri Thumbnail { get; }

        public NewsStory(string title, string link, string description, string source, DateTime publishedDate, string thumbnail)
        {
            Title = title;
            Link = new Uri(link);
            Description = description;
            Source = source;
            PublishedDate = publishedDate;
            Thumbnail = new Uri(thumbnail);
        }
    }
}
