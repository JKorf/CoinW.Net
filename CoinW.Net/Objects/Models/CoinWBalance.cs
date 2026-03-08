using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public record CoinWBalance
    {
        /// <summary>
        /// ["<c>onOrders</c>"] On orders
        /// </summary>
        [JsonPropertyName("onOrders")]
        public decimal OnOrders { get; set; }
        /// <summary>
        /// ["<c>available</c>"] Available
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
    }
}
