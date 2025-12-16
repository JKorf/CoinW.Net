using CoinW.Net.Enums;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWSpotSocketRequest
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("params"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CoinWSpotSocketRequestParameters Parameters { get; set; } = default!;
    }

    internal record CoinWFuturesSocketRequest
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("params"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CoinWFuturesSocketRequestParameters Parameters { get; set; } = default!;
    }

    internal record CoinWSocketRequestParameters
    {
        [JsonPropertyName("biz")]
        public string Biz { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("pairCode"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? PairCode { get; set; } = string.Empty;
    }

    internal record CoinWSpotSocketRequestParameters : CoinWSocketRequestParameters
    {
        [JsonPropertyName("interval"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public KlineIntervalStream? Interval { get; set; }
    }

    internal record CoinWFuturesSocketRequestParameters : CoinWSocketRequestParameters
    {
        [JsonPropertyName("interval"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FuturesKlineIntervalStream? Interval { get; set; }
    }
}
