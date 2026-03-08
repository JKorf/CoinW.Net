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
        /// ["<c>alFreeze</c>"] Frozen quantity
        /// </summary>
        [JsonPropertyName("alFreeze")]
        public decimal Frozen { get; set; }
        /// <summary>
        /// ["<c>alMargin</c>"] Holding quantity
        /// </summary>
        [JsonPropertyName("alMargin")]
        public decimal Holding { get; set; }
        /// <summary>
        /// ["<c>almightyGold</c>"] Mega coupon balance
        /// </summary>
        [JsonPropertyName("almightyGold")]
        public decimal MegaCouponBalance { get; set; }
        /// <summary>
        /// ["<c>availableMargin</c>"] Available margin balance
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public decimal AvailableMargin { get; set; }
        /// <summary>
        /// ["<c>availableUsdt</c>"] Available USDT
        /// </summary>
        [JsonPropertyName("availableUsdt")]
        public decimal AvailableUsdt { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
    }


}
