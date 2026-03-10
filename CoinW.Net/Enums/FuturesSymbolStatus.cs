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
        /// ["<c>online</c>"] Online
        /// </summary>
        [Map("online")]
        Online,
        /// <summary>
        /// ["<c>offline</c>"] Offline
        /// </summary>
        [Map("offline")]
        Offline,
        /// <summary>
        /// ["<c>pretest</c>"] Pre-test
        /// </summary>
        [Map("pretest")]
        PreTest,
        /// <summary>
        /// ["<c>settlement</c>"] Settling
        /// </summary>
        [Map("settlement")]
        Settling,
        /// <summary>
        /// ["<c>preOffline</c>"] Pre-offline
        /// </summary>
        [Map("preOffline")]
        PreOffline
    }
}
