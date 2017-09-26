using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewsResponse
    {
        [JsonProperty("data")]
        public NewsData NewsData { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        public static NewsResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<NewsResponse>(json, Settings);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Settings);
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
