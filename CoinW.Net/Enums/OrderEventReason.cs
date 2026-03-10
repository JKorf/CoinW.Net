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
        /// ["<c>CANCELLED</c>"] Canceled
        /// </summary>
        [Map("CANCELLED")]
        Canceled,
        /// <summary>
        /// ["<c>FILLED</c>"] Filled
        /// </summary>
        [Map("FILLED")]
        Filled,
        /// <summary>
        /// ["<c>REJECTED</c>"] Rejected
        /// </summary>
        [Map("REJECTED")]
        Rejected
    }
}
