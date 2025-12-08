using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// User trade
    /// </summary>
    public record CoinWOrderTransaction
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("tradeID")]
        public decimal OrderId { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order quantity in quote asset
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal QuoteOrderQuantity { get; set; }
        /// <summary>
        /// Order quantity in base asset
        /// </summary>
        [JsonPropertyName("total")]
        public decimal OrderQuantity { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("prize")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity filled in base asset
        /// </summary>
        [JsonPropertyName("success_count")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Quantity filled in quote asset
        /// </summary>
        [JsonPropertyName("success_amount")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("out_trade_no")]
        public string? ClientOrderId { get; set; }
    }


}
