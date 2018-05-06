using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class FavoriteDataData
    {
        [DataMember(Name = "Data")]
        public FavoriteDataItem[] Favorites { get; set; }

        [DataMember(Name = "MessageType")]
        public string MessageType { get; set; }
    }
}
