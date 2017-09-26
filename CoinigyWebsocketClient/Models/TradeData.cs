using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class TradeData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public TradeItem Trade { get; set; }
    }
}
