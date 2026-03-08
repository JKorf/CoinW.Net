using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Funding rate
    /// </summary>
    public record CoinWFundingRate
    {
        /// <summary>
        /// ["<c>r</c>"] Funding rate
        /// </summary>
        [JsonPropertyName("r")]
        public decimal FundingRate { get; set; }
        /// <summary>
        /// ["<c>nt</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("nt")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>n</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("n")]
        public string Symbol { get; set; } = string.Empty;
    }
}
