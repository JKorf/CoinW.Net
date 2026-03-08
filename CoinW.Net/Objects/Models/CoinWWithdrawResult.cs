using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Withdraw result
    /// </summary>
    public record CoinWWithdrawResult
    {
        /// <summary>
        /// ["<c>depositNumber</c>"] Id
        /// </summary>
        [JsonPropertyName("depositNumber")]
        public long Id { get; set; }
    }


}
