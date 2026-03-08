using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Asset info
    /// </summary>
    public record CoinWAsset
    {
        /// <summary>
        /// ["<c>symbolId</c>"] Id
        /// </summary>
        [JsonPropertyName("symbolId")]
        public int Id { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Asset
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>withDraw</c>"] Withdrawal enabled
        /// </summary>
        [JsonPropertyName("withDraw")]
        public bool WithdrawEnabled { get; set; }
        /// <summary>
        /// ["<c>recharge</c>"] Deposits enabled
        /// </summary>
        [JsonPropertyName("recharge")]
        public bool DepositEnabled { get; set; }
        /// <summary>
        /// ["<c>maxQty</c>"] Max withdrawal quantity
        /// </summary>
        [JsonPropertyName("maxQty")]
        public decimal MaxWithdrawQuantity { get; set; }
        /// <summary>
        /// ["<c>minQty</c>"] Min withdrawal quantity
        /// </summary>
        [JsonPropertyName("minQty")]
        public decimal MinWithdrawQuantity { get; set; }
        /// <summary>
        /// ["<c>txFee</c>"] Transaction fee
        /// </summary>
        [JsonPropertyName("txFee")]
        public decimal? TransactionFee { get; set; }
        /// <summary>
        /// ["<c>chain</c>"] Network
        /// </summary>
        [JsonPropertyName("chain")]
        public string Network { get; set; } = string.Empty;
    }


}
