using CoinW.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWSocketRequest
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("params")]
        public CoinWSocketRequestParameters Parameters { get; set; } = default!;
    }

    internal record CoinWSocketRequestParameters
    {
        [JsonPropertyName("biz")]
        public string Biz { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("pairCode"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? PairCode { get; set; } = string.Empty;
        [JsonPropertyName("interval"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public KlineIntervalStream? Interval { get; set; }
    }
}
