using System.Text;
using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Models
{
    public class BlockData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("data")]
        public BlockItem Block { get; set; }
    }
}
