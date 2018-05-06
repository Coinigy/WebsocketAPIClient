using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationDataData
    {
        [DataMember(Name = "Data")]
        public NotificationDataItem NotificationDataItem { get; set; }

        [DataMember(Name = "MessageType")]
        public string MessageType { get; set; }
    }
}
