using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketItem
    {
        [DataMember(Name = "orders_channel_name")]
        public object OrdersChannelName { get; set; }

        [DataMember(Name = "history_channel_name")]
        public object HistoryChannelName { get; set; }

        [DataMember(Name = "exchmkt_id")]
        public long ExchmktId { get; set; }

        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        [DataMember(Name = "high")]
        public long High { get; set; }

        [DataMember(Name = "market_id")]
        public long MarketId { get; set; }

        [DataMember(Name = "low")]
        public long Low { get; set; }

        [DataMember(Name = "mkt_name")]
        public string MktName { get; set; }

        [DataMember(Name = "request_pair")]
        public object RequestPair { get; set; }

        [DataMember(Name = "primary_code")]
        public string PrimaryCode { get; set; }

        [DataMember(Name = "price")]
        public long Price { get; set; }

        [DataMember(Name = "primary_name")]
        public string PrimaryName { get; set; }

        [DataMember(Name = "secondary_name")]
        public string SecondaryName { get; set; }

        [DataMember(Name = "secondary_code")]
        public string SecondaryCode { get; set; }

        [DataMember(Name = "volume")]
        public long Volume { get; set; }
    }
}
