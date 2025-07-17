using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Asset info
    /// </summary>
    public record CoinWAsset
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("symbolId")]
        public int Id { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Withdrawal enabled
        /// </summary>
        [JsonPropertyName("withDraw")]
        public bool WithdrawEnabled { get; set; }
        /// <summary>
        /// Deposits enabled
        /// </summary>
        [JsonPropertyName("recharge")]
        public bool DepositEnabled { get; set; }
        /// <summary>
        /// Max withdrawal quantity
        /// </summary>
        [JsonPropertyName("maxQty")]
        public decimal MaxWithdrawQuantity { get; set; }
        /// <summary>
        /// Min withdrawal quantity
        /// </summary>
        [JsonPropertyName("minQty")]
        public decimal MinWithdrawQuantity { get; set; }
        /// <summary>
        /// Transaction fee
        /// </summary>
        [JsonPropertyName("txFee")]
        public decimal? TransactionFee { get; set; }
        /// <summary>
        /// Network
        /// </summary>
        [JsonPropertyName("chain")]
        public string Network { get; set; } = string.Empty;
    }


}
