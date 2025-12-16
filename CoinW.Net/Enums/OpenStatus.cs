using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Open status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OpenStatus>))]
    public enum OpenStatus
    {
        /// <summary>
        /// Open
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Close
        /// </summary>
        [Map("closed", "close")]
        Close
    }
}
