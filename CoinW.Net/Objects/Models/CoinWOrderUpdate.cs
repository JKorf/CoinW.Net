using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order update
    /// </summary>
    public record CoinWOrderUpdate
    {
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>dealFunds</c>"] Quote quantity filled
        /// </summary>
        [JsonPropertyName("dealFunds")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Event type
        /// </summary>
        [JsonPropertyName("type")]
        public OrderEventType EventType { get; set; }
        /// <summary>
        /// ["<c>reason</c>"] Event reason
        /// </summary>
        [JsonPropertyName("reason")]
        public OrderEventReason? Reason { get; set; }
        /// <summary>
        /// ["<c>client_id</c>"] Client order id
        /// </summary>
        [JsonPropertyName("client_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>remaining_size</c>"] Quantity remaining
        /// </summary>
        [JsonPropertyName("remaining_size")]
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>product_id</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("product_id")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>order_type</c>"] Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>dealAvgPrice</c>"] Average fill price
        /// </summary>
        [JsonPropertyName("dealAvgPrice")]
        public decimal? AverageFillPrice { get; set; }
    }


}
