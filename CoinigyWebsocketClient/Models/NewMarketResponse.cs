using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class NewMarketResponse
    {
        [DataMember(Name = "data")]
        public NewMarketData NewMarketData { get; set; }

        [DataMember(Name = "cid")]
        public long Cid { get; set; }

        [DataMember(Name = "event")]
        public string Event { get; set; }

        public static NewMarketResponse FromJson(ISerializer serializer, string json) => serializer.Deserialize<NewMarketResponse>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
