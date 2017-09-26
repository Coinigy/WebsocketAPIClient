using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class TradeItem
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("exchId")]
        public long ExchId { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("marketid")]
        public long Marketid { get; set; }

        [JsonProperty("market_history_id")]
        public long MarketHistoryId { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("time_local")]
        public string TimeLocal { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("tradeid")]
        public string Tradeid { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
