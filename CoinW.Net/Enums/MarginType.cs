using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Margin type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MarginType>))]
    public enum MarginType
    {
        /// <summary>
        /// Isolated margin
        /// </summary>
        [Map("0")]
        IsolatedMargin,
        /// <summary>
        /// Cross margin
        /// </summary>
        [Map("1")]
        CrossMargin
    }
}
