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
        /// ["<c>0</c>"] Waiting
        /// </summary>
        [Map("0")]
        Waiting,
        /// <summary>
        /// ["<c>1</c>"] Processing
        /// </summary>
        [Map("1")]
        Processing,
        /// <summary>
        /// ["<c>2</c>"] Success
        /// </summary>
        [Map("2")]
        Success,
        /// <summary>
        /// ["<c>3</c>"] Failure
        /// </summary>
        [Map("3")]
        Failure
    }
}
