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
        /// ["<c>tradeID</c>"] Order id
        /// </summary>
        [JsonPropertyName("tradeID")]
        public decimal OrderId { get; set; }
        /// <summary>
        /// ["<c>date</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Order quantity in quote asset
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal QuoteOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>total</c>"] Order quantity in base asset
        /// </summary>
        [JsonPropertyName("total")]
        public decimal OrderQuantity { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Order side
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>prize</c>"] Price
        /// </summary>
        [JsonPropertyName("prize")]
        public decimal Price { get; set; }
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
        /// <summary>
        /// ["<c>out_trade_no</c>"] Client order id
        /// </summary>
        [JsonPropertyName("out_trade_no")]
        public string? ClientOrderId { get; set; }
    }


}
