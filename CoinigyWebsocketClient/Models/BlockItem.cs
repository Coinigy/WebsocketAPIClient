using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class BlockItem
    {
        [JsonProperty("block_time")]
        public string BlockTime { get; set; }

        [JsonProperty("diff")]
        public double Diff { get; set; }

        [JsonProperty("block_reward")]
        public long BlockReward { get; set; }

        [JsonProperty("block_id")]
        public long BlockId { get; set; }

        [JsonProperty("block_size")]
        public long BlockSize { get; set; }

        [JsonProperty("btc_market_cap")]
        public double BtcMarketCap { get; set; }

        [JsonProperty("btc_exch_rate")]
        public double BtcExchRate { get; set; }

        [JsonProperty("curr_code")]
        public string CurrCode { get; set; }

        [JsonProperty("outstanding")]
        public double Outstanding { get; set; }

        [JsonProperty("usd_exch_rate")]
        public double UsdExchRate { get; set; }

        [JsonProperty("miner")]
        public string Miner { get; set; }

        [JsonProperty("txc")]
        public long Txc { get; set; }

        [JsonProperty("usd_market_cap")]
        public double UsdMarketCap { get; set; }
    }
}
