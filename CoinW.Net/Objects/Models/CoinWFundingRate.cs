using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Funding rate
    /// </summary>
    public record CoinWFundingRate
    {
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("r")]
        public decimal FundingRate { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("nt")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("n")]
        public string Symbol { get; set; } = string.Empty;
    }
}
