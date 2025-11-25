using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWResponse
    {
        [JsonPropertyName("code")]
        public int? Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }

    internal record CoinWResponse<T> : CoinWResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
