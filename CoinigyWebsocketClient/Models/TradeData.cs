using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class TradeData
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "data")]
        public TradeItem Trade { get; set; }
    }
}
