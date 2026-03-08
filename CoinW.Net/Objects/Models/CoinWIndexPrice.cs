using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Index price
    /// </summary>
    public record CoinWPrice
    {
        /// <summary>
        /// ["<c>p</c>"] Price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>n</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("n")]
        public string Symbol { get; set; } = string.Empty;
    }
}
