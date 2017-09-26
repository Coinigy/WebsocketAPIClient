using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class NotificationDataItem
    {
        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("message_vars")]
        public string MessageVars { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("notification_id")]
        public long NotificationId { get; set; }

        [JsonProperty("sound_id")]
        public string SoundId { get; set; }

        [JsonProperty("sound")]
        public bool Sound { get; set; }

        [JsonProperty("sound_override")]
        public string SoundOverride { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("title_vars")]
        public string TitleVars { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }
    }
}
