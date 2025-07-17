using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        /// <summary>
        /// Open
        /// </summary>
        [Map("1")]
        Open,
        /// <summary>
        /// Partially filled
        /// </summary>
        [Map("2")]
        PartiallyFilled,
        /// <summary>
        /// Filled
        /// </summary>
        [Map("3")]
        Filled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("4")]
        Canceled
    }
}
