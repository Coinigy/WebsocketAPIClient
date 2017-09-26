using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationDataData
    {
        [JsonProperty("Data")]
        public NotificationDataItem NotificationDataItem { get; set; }

        [JsonProperty("MessageType")]
        public string MessageType { get; set; }
    }
}
