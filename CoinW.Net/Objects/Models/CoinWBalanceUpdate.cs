using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Balance update
    /// </summary>
    public record CoinWBalanceUpdate
    {
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>ledger_id</c>"] Ledger id
        /// </summary>
        [JsonPropertyName("ledger_id")]
        public long LedgerId { get; set; }
        /// <summary>
        /// ["<c>available</c>"] Available balance
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// ["<c>hold</c>"] In hold
        /// </summary>
        [JsonPropertyName("hold")]
        public decimal Hold { get; set; }
    }
}
