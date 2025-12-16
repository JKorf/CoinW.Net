using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWLoginRequest
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("params")]
        public CoinWLoginRequestParameters Parameters { get; set; } = default!;
    }

    internal record CoinWLoginRequestParameters
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonPropertyName("passphrase")]
        public string Passphrase { get; set; } = string.Empty;
    }
}
