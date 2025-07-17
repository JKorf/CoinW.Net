﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Internal
{
    internal record CoinWSocketResponse<T>
    {
        [JsonPropertyName("biz")]
        public string Biz { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("pairCode")]
        public string PairCode { get; set; } = string.Empty;
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
