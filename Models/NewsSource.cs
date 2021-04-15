using System;

namespace NewsFeedApi.Models
{
    public class NewsSource : IComparable
    {
        public string Name { get; }
        public string Url { get; }
        public string ImageUrl { get; set; }

        public NewsSource(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            NewsSource otherNewsSource = obj as NewsSource;
            if(otherNewsSource == null)
            {
                throw new ArgumentException("Object is now a NewsSource");
            }

            return this.Name.CompareTo(otherNewsSource.Name);
        }
    }
}
