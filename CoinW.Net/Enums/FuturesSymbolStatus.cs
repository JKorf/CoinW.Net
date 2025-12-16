using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesSymbolStatus>))]
    public enum FuturesSymbolStatus
    {
        /// <summary>
        /// Online
        /// </summary>
        [Map("online")]
        Online,
        /// <summary>
        /// Offline
        /// </summary>
        [Map("offline")]
        Offline,
        /// <summary>
        /// Pre-test
        /// </summary>
        [Map("pretest")]
        PreTest,
        /// <summary>
        /// Settling
        /// </summary>
        [Map("settlement")]
        Settling,
        /// <summary>
        /// Pre-offline
        /// </summary>
        [Map("preOffline")]
        PreOffline
    }
}
