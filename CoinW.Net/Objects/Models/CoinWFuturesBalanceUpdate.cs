using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Balance update
    /// </summary>
    public record CoinWFuturesBalanceUpdate
    {
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("profitUnreal")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// Frozen quantity
        /// </summary>
        [JsonPropertyName("freeze")]
        public decimal Frozen { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Available
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Mega coupon balance
        /// </summary>
        [JsonPropertyName("almightyGold")]
        public decimal MegaCouponBalance { get; set; }
        /// <summary>
        /// Transfer available
        /// </summary>
        [JsonPropertyName("transferAvailable")]
        public decimal TransferAvailable { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// Available margin
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public decimal AvailableMargin { get; set; }
        /// <summary>
        /// In holding quantity
        /// </summary>
        [JsonPropertyName("hold")]
        public decimal Holding { get; set; }
    }


}
