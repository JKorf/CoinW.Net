using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWSubscriptionResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("errorCode")]
        public int? ErrorCode { get; set; }
        [JsonPropertyName("errorMsg")]
        public string? ErrorMessage { get; set; }
    }
}
