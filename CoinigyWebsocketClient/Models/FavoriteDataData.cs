using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class FavoriteDataData
    {
        [JsonProperty("Data")]
        public FavoriteDataItem[] Favorites { get; set; }

        [JsonProperty("MessageType")]
        public string MessageType { get; set; }
    }
}
