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
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesOrderStatus>))]
    public enum FuturesOrderStatus
    {
        /// <summary>
        /// Unfilled
        /// </summary>
        [Map("unFinish")]
        Open,
        /// <summary>
        /// Partially filled
        /// </summary>
        [Map("part")]
        PartiallyFilled,
        /// <summary>
        /// Filled
        /// </summary>
        [Map("Finish")]
        Filled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("Cancel", "cancelAll")]
        Canceled,
        /// <summary>
        /// Marker change
        /// </summary>
        [Map("markerChange")]
        MarkerChange
    }
}
