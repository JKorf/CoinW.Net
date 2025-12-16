using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Ticker update
    /// </summary>
    public record CoinWTickerUpdate
    {
        /// <summary>
        /// Price change
        /// </summary>
        [JsonPropertyName("changePrice")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// Percentage change
        /// </summary>
        [JsonPropertyName("changeRate")]
        public decimal PercentageChange { get; set; }
        /// <summary>
        /// Highest price in last 24h
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Open price 24h ago
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Volume in base asset
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Volume in quote asset
        /// </summary>
        [JsonPropertyName("volValue")]
        public decimal QuoteVolume { get; set; }
    }
}
