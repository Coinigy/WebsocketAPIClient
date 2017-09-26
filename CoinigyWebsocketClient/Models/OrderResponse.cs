using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class OrderResponse
    {
        [JsonProperty("data")]
        public OrderData OrderData { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        public static OrderResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<OrderResponse>(json, Settings);
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
