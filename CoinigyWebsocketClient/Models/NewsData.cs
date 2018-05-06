using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class NewsData
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "data")]
        public NewsDataItem NewsDataItem { get; set; }
    }
}
