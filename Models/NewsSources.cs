namespace NewsFeedApi.Models
{
    public static class NewsSources
    {
        public static readonly NewsSource bbcnews = new("BBC News", "http://feeds.bbci.co.uk/news/video_and_audio/news_front_page/rss.xml");
        public static readonly NewsSource dailymail = new("Daily Mail", "https://www.dailymail.co.uk/articles.rss");
        public static readonly NewsSource dailymirror = new("Daily Mirror", "https://www.mirror.co.uk/?service=rss");
        public static readonly NewsSource guardian = new("The Guardian", "https://www.theguardian.com/uk/rss");
        public static readonly NewsSource independent = new("The Independent", "https://www.independent.co.uk/rss");
        public static readonly NewsSource skynews = new("Sky News", "http://feeds.skynews.com/feeds/rss/home.xml");
        public static readonly NewsSource telegraph = new("The Telegraph", "https://www.telegraph.co.uk/rss.xml");
    }
}
