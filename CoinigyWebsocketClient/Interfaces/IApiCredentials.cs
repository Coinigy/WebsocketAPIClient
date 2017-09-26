using Newtonsoft.Json;

namespace CoinigyWebsocketClient.Interfaces
{
    public interface IApiCredentials
    {
        [JsonProperty(PropertyName = "apiKey")]
        string ApiKey { get; set; }

        [JsonProperty(PropertyName = "apiSecret")]
        string ApiSecret { get; set; }
    }
}
