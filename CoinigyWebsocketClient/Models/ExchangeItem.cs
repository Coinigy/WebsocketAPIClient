using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class ExchangeItem
    {
        [DataMember(Name = "exch_id")]
        public long ExchId { get; set; }

        [DataMember(Name = "exch_code")]
        public string ExchCode { get; set; }

        [DataMember(Name = "exch_balance_enabled")]
        public bool ExchBalanceEnabled { get; set; }

        [DataMember(Name = "exch_fee")]
        public decimal ExchFee { get; set; }

        [DataMember(Name = "exch_trade_enabled")]
        public bool ExchTradeEnabled { get; set; }

        [DataMember(Name = "exch_name")]
        public string ExchName { get; set; }

        [DataMember(Name = "exch_url")]
        public string ExchUrl { get; set; }

        public static ExchangeItem FromJson(ISerializer serializer, string json) => serializer.Deserialize<ExchangeItem>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
