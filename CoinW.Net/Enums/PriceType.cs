using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PriceType>))]
    public enum PriceType
    {
        /// <summary>
        /// Index price
        /// </summary>
        [Map("1")]
        IndexPrice,
        /// <summary>
        /// Last trade price
        /// </summary>
        [Map("2")]
        LastPrice,
        /// <summary>
        /// Mark price
        /// </summary>
        [Map("3")]
        MarkPrice
    }
}
