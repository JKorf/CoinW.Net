using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Batch order result
    /// </summary>
    public record CoinWBatchResult
    {
        /// <summary>
        /// ["<c>msgCode</c>"] Result code
        /// </summary>
        [JsonPropertyName("msgCode")]
        public int Code { get; set; }
        /// <summary>
        /// ["<c>openId</c>"] Order id
        /// </summary>
        [JsonPropertyName("openId")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>thirdOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("thirdOrderId")]
        public string? ClientOrderId { get; set; }
    }
}
