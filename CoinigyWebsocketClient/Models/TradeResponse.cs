using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class TradeResponse
    {
        [DataMember(Name = "data")]
        public TradeData TradeData { get; set; }

        [DataMember(Name = "event")]
        public string Event { get; set; }

        public static TradeResponse FromJson(ISerializer serializer, string json) => serializer.Deserialize<TradeResponse>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
