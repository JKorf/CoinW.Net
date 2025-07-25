using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Trailing status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ProcessingStatus>))]
    public enum ProcessingStatus
    {
        /// <summary>
        /// Waiting
        /// </summary>
        [Map("0")]
        Waiting,
        /// <summary>
        /// Processing
        /// </summary>
        [Map("1")]
        Processing,
        /// <summary>
        /// Success
        /// </summary>
        [Map("2")]
        Success,
        /// <summary>
        /// Failure
        /// </summary>
        [Map("3")]
        Failure
    }
}
