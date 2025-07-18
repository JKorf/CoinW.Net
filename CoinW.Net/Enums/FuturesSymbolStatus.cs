using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesSymbolStatus>))]
    public enum FuturesSymbolStatus
    {
        /// <summary>
        /// ONline
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
