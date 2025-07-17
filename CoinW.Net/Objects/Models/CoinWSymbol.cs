using CoinW.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public record CoinWSymbol
    {
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("currencyBase")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Max order quantity
        /// </summary>
        [JsonPropertyName("maxBuyCount")]
        public decimal MaxOrderQuantity { get; set; }
        /// <summary>
        /// Price precision
        /// </summary>
        [JsonPropertyName("pricePrecision")]
        public int PriceDecimalPlaces { get; set; }
        /// <summary>
        /// Min order price
        /// </summary>
        [JsonPropertyName("minBuyPrice")]
        public decimal MinOrderPrice { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("currencyPair")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Min order value
        /// </summary>
        [JsonPropertyName("minBuyAmount")]
        public decimal MinOrderValue { get; set; }
        /// <summary>
        /// Max order price
        /// </summary>
        [JsonPropertyName("maxBuyPrice")]
        public decimal MaxOrderPrice { get; set; }
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("currencyQuote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Quantity precision
        /// </summary>
        [JsonPropertyName("countPrecision")]
        public int QuantityDecimalPlaces { get; set; }
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonPropertyName("minBuyCount")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// Symbol status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Max order value
        /// </summary>
        [JsonPropertyName("maxBuyAmount")]
        public decimal MaxOrderValue { get; set; }
    }


}
