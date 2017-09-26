using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketItem
    {
        [JsonProperty("orders_channel_name")]
        public object OrdersChannelName { get; set; }

        [JsonProperty("history_channel_name")]
        public object HistoryChannelName { get; set; }

        [JsonProperty("exchmkt_id")]
        public long ExchmktId { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("high")]
        public long High { get; set; }

        [JsonProperty("market_id")]
        public long MarketId { get; set; }

        [JsonProperty("low")]
        public long Low { get; set; }

        [JsonProperty("mkt_name")]
        public string MktName { get; set; }

        [JsonProperty("request_pair")]
        public object RequestPair { get; set; }

        [JsonProperty("primary_code")]
        public string PrimaryCode { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("primary_name")]
        public string PrimaryName { get; set; }

        [JsonProperty("secondary_name")]
        public string SecondaryName { get; set; }

        [JsonProperty("secondary_code")]
        public string SecondaryCode { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; }
    }
}
