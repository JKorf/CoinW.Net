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
        /// ["<c>1</c>"] Taker
        /// </summary>
        [Map("1")]
        Taker,
        /// <summary>
        /// ["<c>2</c>"] Maker
        /// </summary>
        [Map("2")]
        Maker
    }
}
