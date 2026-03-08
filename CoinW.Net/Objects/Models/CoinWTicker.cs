using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Price stats
    /// </summary>
    public record CoinWTicker
    {
        /// <summary>
        /// ["<c>id</c>"] Symbol id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonIgnore]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>isFrozen</c>"] Is frozen
        /// </summary>
        [JsonPropertyName("isFrozen")]
        public bool IsFrozen { get; set; }

        /// <summary>
        /// ["<c>percentChange</c>"] Percentage change
        /// </summary>
        [JsonPropertyName("percentChange")]
        public decimal PercentageChange { get; set; }
        /// <summary>
        /// ["<c>high24hr</c>"] Highest price in last 24h
        /// </summary>
        [JsonPropertyName("high24hr")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>low24hr</c>"] Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("low24hr")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>highestBid</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("highestBid")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>lowestAsk</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("lowestAsk")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>baseVolume</c>"] Volume in quote asset
        /// </summary>
        [JsonPropertyName("baseVolume")]
        public decimal Volume { get; set; }
    }
}
