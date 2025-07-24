using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Trigger order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerOrderType>))]
    public enum TriggerOrderType
    {
        /// <summary>
        /// Limit order
        /// </summary>
        [Map("0")]
        Limit,
        /// <summary>
        /// Market order
        /// </summary>
        [Map("1")]
        Market
    }
}
