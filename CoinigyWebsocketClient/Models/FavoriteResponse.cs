using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class FavoriteResponse
    {
        [JsonProperty("data")]
        public FavoriteData Data { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        public static FavoriteResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FavoriteResponse>(json, Settings);
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
