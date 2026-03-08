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
        /// ["<c>createdDate</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>direction</c>"] Direction
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide Direction { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>piece</c>"] Quantity in contracts
        /// </summary>
        [JsonPropertyName("piece")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Quantity in base asset
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal QuantityBase { get; set; }
    }


}
