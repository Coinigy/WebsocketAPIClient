using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class OrderItem
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("ordertype")]
        public string Ordertype { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}
