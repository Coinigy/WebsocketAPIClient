using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class FavoriteDataItem
    {
        [JsonProperty("exch_code")]
        public string ExchCode { get; set; }

        [JsonProperty("last_price")]
        public double LastPrice { get; set; }

        [JsonProperty("btc_volume_24")]
        public double BtcVolume24 { get; set; }

        [JsonProperty("base_curr")]
        public string BaseCurr { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("exch_name")]
        public string ExchName { get; set; }

        [JsonProperty("exch_id")]
        public long ExchId { get; set; }

        [JsonProperty("exchmkt_id")]
        public long ExchmktId { get; set; }

        [JsonProperty("mkt_name")]
        public string MktName { get; set; }

        [JsonProperty("mkt_id")]
        public long MktId { get; set; }

        [JsonProperty("primary_curr")]
        public string PrimaryCurr { get; set; }

        [JsonProperty("volume_24")]
        public double Volume24 { get; set; }
    }
}
