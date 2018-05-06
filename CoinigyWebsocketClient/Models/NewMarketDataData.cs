using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketDataItem
    {
        [DataMember(Name = "Exchange")]
        public NewMarketExchangeItem Exchange { get; set; }

        [DataMember(Name = "Config")]
        public object Config { get; set; }

        [DataMember(Name = "Markets")]
        public NewMarketItem[] Markets { get; set; }
    }
}
