using CoinW.Net.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    public record CoinWOrder
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderNumber")]
        public long OrderId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order quantity in quote asset
        /// </summary>
        [JsonPropertyName("startingAmount")]
        public decimal QuoteQuantity { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("prize")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal? Fee { get; set; }
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
    }


}
