using CoinW.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
