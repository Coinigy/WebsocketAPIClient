using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewsData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public NewsDataItem NewsDataItem { get; set; }
    }
}
