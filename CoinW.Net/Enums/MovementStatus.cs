using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Movement status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MovementStatus>))]
    public enum MovementStatus
    {
        /// <summary>
        /// Waiting
        /// </summary>
        [Map("1")]
        Waiting,
        /// <summary>
        /// Success
        /// </summary>
        [Map("3")]
        Success
    }
}
