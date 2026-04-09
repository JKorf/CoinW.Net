using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesOrderStatus>))]
    public enum FuturesOrderStatus
    {
        /// <summary>
        /// ["<c>unFinish</c>"] Unfilled
        /// </summary>
        [Map("unFinish")]
        Open,
        /// <summary>
        /// ["<c>part</c>"] Partially filled
        /// </summary>
        [Map("part")]
        PartiallyFilled,
        /// <summary>
        /// ["<c>Finish</c>"] Filled
        /// </summary>
        [Map("Finish", "finish")]
        Filled,
        /// <summary>
        /// ["<c>Cancel</c>"] Canceled
        /// </summary>
        [Map("Cancel", "cancelAll", "cancel")]
        Canceled,
        /// <summary>
        /// ["<c>markerChange</c>"] Marker change
        /// </summary>
        [Map("markerChange")]
        MarkerChange
    }
}
