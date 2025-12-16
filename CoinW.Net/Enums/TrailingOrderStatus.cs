using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Trailing order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TrailingOrderStatus>))]
    public enum TrailingOrderStatus
    {
        /// <summary>
        /// Open
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Closed
        /// </summary>
        [Map("closed", "close")]
        Closed,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("cancel")]
        Canceled
    }
}
