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
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Ledger id
        /// </summary>
        [JsonPropertyName("ledger_id")]
        public long LedgerId { get; set; }
        /// <summary>
        /// Available balance
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// In hold
        /// </summary>
        [JsonPropertyName("hold")]
        public decimal Hold { get; set; }
    }
}
