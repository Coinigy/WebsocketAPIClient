using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationResponse
    {
        [DataMember(Name = "data")]
        public NotificationData NotificationData { get; set; }

        [DataMember(Name = "event")]
        public string Event { get; set; }

        public static NotificationResponse FromJson(ISerializer serializer, string json) => serializer.Deserialize<NotificationResponse>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
