using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewsDataItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("feed_name")]
        public string FeedName { get; set; }

        [JsonProperty("feed_image")]
        public string FeedImage { get; set; }

        [JsonProperty("feed_url")]
        public string FeedUrl { get; set; }

        [JsonProperty("published_date")]
        public string PublishedDate { get; set; }

        [JsonProperty("news_id")]
        public long NewsId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
