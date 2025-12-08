using CoinW.Net.Enums;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Margin info
    /// </summary>
    public record CoinWMarginInfo
    {
        /// <summary>
        /// Position combine type
        /// </summary>
        [JsonPropertyName("layout")]
        public PositionCombineType PositionCombineType { get; set; }
        /// <summary>
        /// MarginType
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
    }


}
