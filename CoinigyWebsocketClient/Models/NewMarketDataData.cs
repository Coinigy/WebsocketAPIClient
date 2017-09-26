using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketDataItem
    {
        [JsonProperty("Exchange")]
        public NewMarketExchangeItem Exchange { get; set; }

        [JsonProperty("Config")]
        public object Config { get; set; }

        [JsonProperty("Markets")]
        public NewMarketItem[] Markets { get; set; }
    }
}
