using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order event reason
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderEventReason>))]
    public enum OrderEventReason
    {
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("CANCELLED")]
        Canceled,
        /// <summary>
        /// Filled
        /// </summary>
        [Map("FILLED")]
        Filled,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("REJECTED")]
        Rejected
    }
}
