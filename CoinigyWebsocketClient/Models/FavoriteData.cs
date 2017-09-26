using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class FavoriteData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public FavoriteDataData FavoritesDataData{ get; set; }
    }
}
