using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class OrderData
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "data")]
        public OrderItem[] Orders { get; set; }
    }
}
