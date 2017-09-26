using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class BlockResponse
    {
        [JsonProperty("data")]
        public BlockData BlockData { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        public static BlockResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<BlockResponse>(json, Settings);
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
