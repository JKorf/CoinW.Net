using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Ticker update
    /// </summary>
    public record CoinWTickerUpdate
    {
        /// <summary>
        /// ["<c>changePrice</c>"] Price change
        /// </summary>
        [JsonPropertyName("changePrice")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// ["<c>changeRate</c>"] Percentage change
        /// </summary>
        [JsonPropertyName("changeRate")]
        public decimal PercentageChange { get; set; }
        /// <summary>
        /// ["<c>high</c>"] Highest price in last 24h
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>low</c>"] Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>open</c>"] Open price 24h ago
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>vol</c>"] Volume in base asset
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>volValue</c>"] Volume in quote asset
        /// </summary>
        [JsonPropertyName("volValue")]
        public decimal QuoteVolume { get; set; }
    }
}
