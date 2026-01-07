using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWSocketResponse
    {
        [JsonPropertyName("biz")]
        public string Biz { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("pairCode")]
        public string PairCode { get; set; } = string.Empty;
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("interval")]
        public string? Interval { get; set; }
    }

    internal record CoinWSocketResponse<T> : CoinWSocketResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
