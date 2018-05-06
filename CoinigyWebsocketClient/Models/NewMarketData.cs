using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketData
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "data")]
        public NewMarketDataItem NewMarketDataData { get; set; }
    }
}
