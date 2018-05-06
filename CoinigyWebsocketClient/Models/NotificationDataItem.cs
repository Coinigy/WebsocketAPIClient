using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationDataItem
    {
        [DataMember(Name = "pinned")]
        public bool Pinned { get; set; }

        [DataMember(Name = "style")]
        public string Style { get; set; }

        [DataMember(Name = "message_vars")]
        public string MessageVars { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "notification_id")]
        public long NotificationId { get; set; }

        [DataMember(Name = "sound_id")]
        public string SoundId { get; set; }

        [DataMember(Name = "sound")]
        public bool Sound { get; set; }

        [DataMember(Name = "sound_override")]
        public string SoundOverride { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "time")]
        public string Time { get; set; }

        [DataMember(Name = "title_vars")]
        public string TitleVars { get; set; }

        [DataMember(Name = "type")]
        public long Type { get; set; }
    }
}
