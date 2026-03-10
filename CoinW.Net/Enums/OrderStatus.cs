using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        /// <summary>
        /// ["<c>1</c>"] Open
        /// </summary>
        [Map("1")]
        Open,
        /// <summary>
        /// ["<c>2</c>"] Partially filled
        /// </summary>
        [Map("2")]
        PartiallyFilled,
        /// <summary>
        /// ["<c>3</c>"] Filled
        /// </summary>
        [Map("3")]
        Filled,
        /// <summary>
        /// ["<c>4</c>"] Canceled
        /// </summary>
        [Map("4")]
        Canceled
    }
}
