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
        /// ["<c>asks</c>"] Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public CoinWFuturesOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>bids</c>"] Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public CoinWFuturesOrderBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// ["<c>n</c>"] Symbol
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
        /// ["<c>m</c>"] Quantity
        /// </summary>
        [JsonPropertyName("m")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>p</c>"] Price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal Price { get; set; }
    }
}
