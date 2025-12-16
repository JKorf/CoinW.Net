using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWSubscriptionResponse
    {
        [JsonPropertyName("result")]
        public bool Success { get; set; }
        [JsonPropertyName("errorCode")]
        public int? ErrorCode { get; set; }
        [JsonPropertyName("errorMsg")]
        public string? ErrorMessage { get; set; }
    }
}
