using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    public record CoinWFuturesTrade
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Direction
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide Direction { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Quantity in contracts
        /// </summary>
        [JsonPropertyName("piece")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal QuantityBase { get; set; }
    }


}
