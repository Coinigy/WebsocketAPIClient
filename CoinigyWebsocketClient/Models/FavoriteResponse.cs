using System.Runtime.Serialization;
using PureSocketCluster;

namespace CoinigyWebsocketClient.Models
{
    public class FavoriteResponse
    {
        [DataMember(Name = "data")]
        public FavoriteData Data { get; set; }

        [DataMember(Name = "event")]
        public string Event { get; set; }

        public static FavoriteResponse FromJson(ISerializer serializer, string json) => serializer.Deserialize<FavoriteResponse>(json);

	    public byte[] ToJson(ISerializer serializer) => serializer.Serialize(this);
    }
}
