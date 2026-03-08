using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    public record CoinWOrderDetails
    {
        /// <summary>
        /// ["<c>tradeID</c>"] Order id
        /// </summary>
        [JsonPropertyName("tradeID")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>currencyPair</c>"] Symbol
        /// </summary>
        [JsonPropertyName("currencyPair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>date</c>"] CreateTime
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Order quantity in quote asset
        /// </summary>
        [JsonPropertyName("amount")]
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
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>success_total</c>"] Quantity filled in base asset
        /// </summary>
        [JsonPropertyName("success_total")]
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
