using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NewsDataItem
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "feed_name")]
        public string FeedName { get; set; }

        [DataMember(Name = "feed_image")]
        public string FeedImage { get; set; }

        [DataMember(Name = "feed_url")]
        public string FeedUrl { get; set; }

        [DataMember(Name = "published_date")]
        public string PublishedDate { get; set; }

        [DataMember(Name = "news_id")]
        public long NewsId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
