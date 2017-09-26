using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketResponse
    {
        [JsonProperty("data")]
        public NewMarketData NewMarketData { get; set; }

        [JsonProperty("cid")]
        public long Cid { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        public static NewMarketResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<NewMarketResponse>(json, Settings);
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
