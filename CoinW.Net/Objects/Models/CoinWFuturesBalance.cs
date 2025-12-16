using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Balance
    /// </summary>
    public record CoinWFuturesBalance
    {
        /// <summary>
        /// Frozen quantity
        /// </summary>
        [JsonPropertyName("alFreeze")]
        public decimal Frozen { get; set; }
        /// <summary>
        /// Holding quantity
        /// </summary>
        [JsonPropertyName("alMargin")]
        public decimal Holding { get; set; }
        /// <summary>
        /// Mega coupon balance
        /// </summary>
        [JsonPropertyName("almightyGold")]
        public decimal MegaCouponBalance { get; set; }
        /// <summary>
        /// Available margin balance
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public decimal AvailableMargin { get; set; }
        /// <summary>
        /// Available USDT
        /// </summary>
        [JsonPropertyName("availableUsdt")]
        public decimal AvailableUsdt { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
    }


}
