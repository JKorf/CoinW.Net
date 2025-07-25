using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Position combine type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionCombineType>))]
    public enum PositionCombineType
    {
        /// <summary>
        /// Positions in same direction will be merged
        /// </summary>
        [Map("0")]
        Merged,
        /// <summary>
        /// Positions in same direction will remain separate
        /// </summary>
        [Map("1")]
        Split
    }
}
