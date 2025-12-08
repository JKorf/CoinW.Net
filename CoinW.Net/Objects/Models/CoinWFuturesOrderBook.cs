using CryptoExchange.Net.Interfaces;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order book
    /// </summary>
    public record CoinWFuturesOrderBook
    {
        /// <summary>
        /// Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public CoinWFuturesOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public CoinWFuturesOrderBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("n")]
        public string Symbol { get; set; } = string.Empty;
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    public record CoinWFuturesOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("m")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal Price { get; set; }
    }
}
