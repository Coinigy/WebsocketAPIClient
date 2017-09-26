using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public NewMarketDataItem NewMarketDataData { get; set; }
    }
}
