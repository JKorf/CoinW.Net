using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Trailing status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TrailingStatus>))]
    public enum TrailingStatus
    {
        /// <summary>
        /// Limit order
        /// </summary>
        [Map("1")]
        Waiting,
        /// <summary>
        /// Market order
        /// </summary>
        [Map("2")]
        Processing,
        /// <summary>
        /// Success
        /// </summary>
        [Map("2")]
        Success,
        /// <summary>
        /// Market order
        /// </summary>
        [Map("3")]
        Failure
    }
}
