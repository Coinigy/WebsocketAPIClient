using System.Runtime.Serialization;

namespace CoinigyWebsocketClient.Models
{
    public class FavoriteData
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "data")]
        public FavoriteDataData FavoritesDataData{ get; set; }
    }
}
