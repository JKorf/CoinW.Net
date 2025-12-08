using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Max position info
    /// </summary>
    public record CoinWMaxPosition
    {
        /// <summary>
        /// Available buy
        /// </summary>
        [JsonPropertyName("availBuy")]
        public decimal AvailableBuy { get; set; }
        /// <summary>
        /// Available sell
        /// </summary>
        [JsonPropertyName("availSell")]
        public decimal AvailableSell { get; set; }
    }


}
