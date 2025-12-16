using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public record CoinWBalance
    {
        /// <summary>
        /// On orders
        /// </summary>
        [JsonPropertyName("onOrders")]
        public decimal OnOrders { get; set; }
        /// <summary>
        /// Available
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
    }
}
