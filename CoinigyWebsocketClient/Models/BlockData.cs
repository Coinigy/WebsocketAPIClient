using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class BlockData
    {
	    [DataMember(Name = "channel")]
        public string Channel { get; set; }

	    [DataMember(Name = "data")]
        public BlockItem Block { get; set; }
    }
}
