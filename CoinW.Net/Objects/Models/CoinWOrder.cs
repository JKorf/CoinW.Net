using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    public record CoinWOrder
    {
        /// <summary>
        /// ["<c>orderNumber</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderNumber")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>date</c>"] CreateTime
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>startingAmount</c>"] Order quantity in quote asset
        /// </summary>
        [JsonPropertyName("startingAmount")]
        public decimal QuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>total</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Order side
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>prize</c>"] Order price
        /// </summary>
        [JsonPropertyName("prize")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal? Fee { get; set; }
        /// <summary>
        /// ["<c>success_count</c>"] Quantity filled in base asset
        /// </summary>
        [JsonPropertyName("success_count")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>success_amount</c>"] Quantity filled in quote asset
        /// </summary>
        [JsonPropertyName("success_amount")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
    }


}
