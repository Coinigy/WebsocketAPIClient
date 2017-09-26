using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class TradeResponse
    {
        [JsonProperty("data")]
        public TradeData TradeData { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        public static TradeResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TradeResponse>(json, Settings);
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
