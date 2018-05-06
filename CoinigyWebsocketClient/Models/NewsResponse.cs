using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class NewsResponse
    {
        [DataMember(Name = "data")]
        public NewsData NewsData { get; set; }

        [DataMember(Name = "event")]
        public string Event { get; set; }

        public static NewsResponse FromJson(ISerializer serializer, string json) => serializer.Deserialize<NewsResponse>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
