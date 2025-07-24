using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Type of quantity
    /// </summary>
    [JsonConverter(typeof(EnumConverter<QuantityUnit>))]
    public enum QuantityUnit
    {
        /// <summary>
        /// Quantity in quote asset
        /// </summary>
        [Map("0")]
        QuoteAsset,
        /// <summary>
        /// Quantity in number of contracts
        /// </summary>
        [Map("1")]
        Contracts,
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [Map("2")]
        BaseAsset
    }
}
