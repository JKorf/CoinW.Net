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
        /// ["<c>open</c>"] Open
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// ["<c>closed</c>"] Closed
        /// </summary>
        [Map("closed", "close")]
        Closed,
        /// <summary>
        /// ["<c>cancel</c>"] Canceled
        /// </summary>
        [Map("cancel")]
        Canceled
    }
}
