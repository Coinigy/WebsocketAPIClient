using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class OrderResponse
    {
        [DataMember(Name = "data")]
        public OrderData OrderData { get; set; }

        [DataMember(Name = "event")]
        public string Event { get; set; }

        public static OrderResponse FromJson(ISerializer serializer, string json) => serializer.Deserialize<OrderResponse>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
