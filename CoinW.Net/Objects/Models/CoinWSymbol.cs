using CoinW.Net.Enums;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public record CoinWSymbol
    {
        /// <summary>
        /// ["<c>currencyBase</c>"] Base asset
        /// </summary>
        [JsonPropertyName("currencyBase")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>maxBuyCount</c>"] Max order quantity
        /// </summary>
        [JsonPropertyName("maxBuyCount")]
        public decimal MaxOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>pricePrecision</c>"] Price precision
        /// </summary>
        [JsonPropertyName("pricePrecision")]
        public int PriceDecimalPlaces { get; set; }
        /// <summary>
        /// ["<c>minBuyPrice</c>"] Min order price
        /// </summary>
        [JsonPropertyName("minBuyPrice")]
        public decimal MinOrderPrice { get; set; }
        /// <summary>
        /// ["<c>currencyPair</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("currencyPair")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>minBuyAmount</c>"] Min order value
        /// </summary>
        [JsonPropertyName("minBuyAmount")]
        public decimal MinOrderValue { get; set; }
        /// <summary>
        /// ["<c>maxBuyPrice</c>"] Max order price
        /// </summary>
        [JsonPropertyName("maxBuyPrice")]
        public decimal MaxOrderPrice { get; set; }
        /// <summary>
        /// ["<c>currencyQuote</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("currencyQuote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>countPrecision</c>"] Quantity precision
        /// </summary>
        [JsonPropertyName("countPrecision")]
        public int QuantityDecimalPlaces { get; set; }
        /// <summary>
        /// ["<c>minBuyCount</c>"] Min order quantity
        /// </summary>
        [JsonPropertyName("minBuyCount")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Symbol status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>maxBuyAmount</c>"] Max order value
        /// </summary>
        [JsonPropertyName("maxBuyAmount")]
        public decimal MaxOrderValue { get; set; }
    }


}
