using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Price stats
    /// </summary>
    public record CoinWTicker
    {
        /// <summary>
        /// Symbol id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonIgnore]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Is frozen
        /// </summary>
        [JsonPropertyName("isFrozen")]
        public bool IsFrozen { get; set; }

        /// <summary>
        /// Percentage change
        /// </summary>
        [JsonPropertyName("percentChange")]
        public decimal PercentageChange { get; set; }
        /// <summary>
        /// Highest price in last 24h
        /// </summary>
        [JsonPropertyName("high24hr")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("low24hr")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonPropertyName("highestBid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonPropertyName("lowestAsk")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Volume in quote asset
        /// </summary>
        [JsonPropertyName("baseVolume")]
        public decimal Volume { get; set; }
    }
}
