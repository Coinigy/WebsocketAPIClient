using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class BlockItem
    {
        [DataMember(Name = "block_time")]
        public string BlockTime { get; set; }

        [DataMember(Name = "diff")]
        public double Diff { get; set; }

        [DataMember(Name = "block_reward")]
        public long BlockReward { get; set; }

        [DataMember(Name = "block_id")]
        public long BlockId { get; set; }

        [DataMember(Name = "block_size")]
        public long BlockSize { get; set; }

        [DataMember(Name = "btc_market_cap")]
        public double BtcMarketCap { get; set; }

        [DataMember(Name = "btc_exch_rate")]
        public double BtcExchRate { get; set; }

        [DataMember(Name = "curr_code")]
        public string CurrCode { get; set; }

        [DataMember(Name = "outstanding")]
        public double Outstanding { get; set; }

        [DataMember(Name = "usd_exch_rate")]
        public double UsdExchRate { get; set; }

        [DataMember(Name = "miner")]
        public string Miner { get; set; }

        [DataMember(Name = "txc")]
        public long Txc { get; set; }

        [DataMember(Name = "usd_market_cap")]
        public double UsdMarketCap { get; set; }
    }
}
