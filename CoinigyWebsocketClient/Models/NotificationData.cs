using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationData
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "data")]
        public NotificationDataData NotificationDataData { get; set; }
    }
}
