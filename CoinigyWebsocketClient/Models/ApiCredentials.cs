using CoinigyWebsocketClient.Interfaces;

namespace CoinigyWebsocketClient.Models
{
    public class ApiCredentials : IApiCredentials
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public ApiCredentials(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }
    }
}
