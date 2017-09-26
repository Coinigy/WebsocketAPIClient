using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class OrderData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public OrderItem[] Orders { get; set; }
    }
}
