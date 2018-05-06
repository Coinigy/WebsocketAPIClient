using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class OrderItem
    {
        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "ordertype")]
        public string Ordertype { get; set; }

        [DataMember(Name = "quantity")]
        public decimal Quantity { get; set; }

        [DataMember(Name = "total")]
        public decimal Total { get; set; }
    }
}
