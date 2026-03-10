using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderType>))]
    public enum OrderType
    {
        /// <summary>
        /// ["<c>LIMIT</c>"] Limit
        /// </summary>
        [Map("LIMIT", "0")]
        Limit,
        /// <summary>
        /// ["<c>MARKET</c>"] Market
        /// </summary>
        [Map("MARKET", "1")]
        Market
    }
}
