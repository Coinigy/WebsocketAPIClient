using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class BlockResponse
    {
        [DataMember(Name = "data")]
        public BlockData BlockData { get; set; }

        [DataMember(Name = "event")]
        public string Event { get; set; }

        public static BlockResponse FromJson(ISerializer serializer, string json) => serializer.Deserialize<BlockResponse>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
