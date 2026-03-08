using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Deposit address info
    /// </summary>
    public record CoinWDepositAddress
    {
        /// <summary>
        /// ["<c>chainName</c>"] Network name
        /// </summary>
        [JsonPropertyName("chainName")]
        public string NetworkName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>address</c>"] Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>memo</c>"] Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
        /// <summary>
        /// ["<c>minRechargeAmount</c>"] Min deposit quantity
        /// </summary>
        [JsonPropertyName("minRechargeAmount")]
        public decimal MinDepositQuantity { get; set; }
    }


}
