using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Balance update
    /// </summary>
    public record CoinWFuturesBalanceUpdate
    {
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// ["<c>profitUnreal</c>"] Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("profitUnreal")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// ["<c>freeze</c>"] Frozen quantity
        /// </summary>
        [JsonPropertyName("freeze")]
        public decimal Frozen { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>available</c>"] Available
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>almightyGold</c>"] Mega coupon balance
        /// </summary>
        [JsonPropertyName("almightyGold")]
        public decimal MegaCouponBalance { get; set; }
        /// <summary>
        /// ["<c>transferAvailable</c>"] Transfer available
        /// </summary>
        [JsonPropertyName("transferAvailable")]
        public decimal TransferAvailable { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// ["<c>availableMargin</c>"] Available margin
        /// </summary>
        [JsonPropertyName("availableMargin")]
        public decimal AvailableMargin { get; set; }
        /// <summary>
        /// ["<c>hold</c>"] In holding quantity
        /// </summary>
        [JsonPropertyName("hold")]
        public decimal Holding { get; set; }
    }


}
