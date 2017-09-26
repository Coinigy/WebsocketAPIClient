using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public NotificationDataData NotificationDataData { get; set; }
    }
}
