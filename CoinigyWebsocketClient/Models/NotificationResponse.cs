using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationResponse
    {
        [JsonProperty("data")]
        public NotificationData NotificationData { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        public static NotificationResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<NotificationResponse>(json, Settings);
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
