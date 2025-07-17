using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order event type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderEventType>))]
    public enum OrderEventType
    {
        /// <summary>
        /// Received
        /// </summary>
        [Map("RECEIVED")]
        Received,
        /// <summary>
        /// Done
        /// </summary>
        [Map("DONE")]
        Done
    }
}
