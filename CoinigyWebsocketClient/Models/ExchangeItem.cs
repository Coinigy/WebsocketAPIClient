using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class ExchangeItem
    {
        [JsonProperty("exch_id")]
        public long ExchId { get; set; }

        [JsonProperty("exch_code")]
        public string ExchCode { get; set; }

        [JsonProperty("exch_balance_enabled")]
        public long ExchBalanceEnabled { get; set; }

        [JsonProperty("exch_fee")]
        public double ExchFee { get; set; }

        [JsonProperty("exch_trade_enabled")]
        public long ExchTradeEnabled { get; set; }

        [JsonProperty("exch_name")]
        public string ExchName { get; set; }

        [JsonProperty("exch_url")]
        public string ExchUrl { get; set; }

        public static ExchangeItem FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ExchangeItem>(json, Settings);
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
