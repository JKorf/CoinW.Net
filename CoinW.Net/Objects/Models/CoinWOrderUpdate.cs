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
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Quote quantity filled
        /// </summary>
        [JsonPropertyName("dealFunds")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// Event type
        /// </summary>
        [JsonPropertyName("type")]
        public OrderEventType EventType { get; set; }
        /// <summary>
        /// Event reason
        /// </summary>
        [JsonPropertyName("reason")]
        public OrderEventReason? Reason { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("client_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Quantity remaining
        /// </summary>
        [JsonPropertyName("remaining_size")]
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("product_id")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Average fill price
        /// </summary>
        [JsonPropertyName("dealAvgPrice")]
        public decimal? AverageFillPrice { get; set; }
    }


}
