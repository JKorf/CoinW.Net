using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Max trade info
    /// </summary>
    public record CoinWMaxTrade
    {
        /// <summary>
        /// Max buy quantity in number of contracts
        /// </summary>
        [JsonPropertyName("maxBuy")]
        public int MaxBuy { get; set; }
        /// <summary>
        /// Max sell quantity in number of contracts
        /// </summary>
        [JsonPropertyName("maxSell")]
        public int MaxSell { get; set; }
    }


}
