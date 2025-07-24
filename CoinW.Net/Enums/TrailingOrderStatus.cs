using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Trailing order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TrailingOrderStatus>))]
    public enum TrailingOrderStatus
    {
        /// <summary>
        /// Open
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Closed
        /// </summary>
        [Map("closed")]
        Closed,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("cancel")]
        Canceled
    }
}
