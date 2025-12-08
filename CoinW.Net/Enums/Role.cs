using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Role
    /// </summary>
    [JsonConverter(typeof(EnumConverter<Role>))]
    public enum Role
    {
        /// <summary>
        /// Taker
        /// </summary>
        [Map("1")]
        Taker,
        /// <summary>
        /// Maker
        /// </summary>
        [Map("2")]
        Maker
    }
}
