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
        /// ["<c>0</c>"] Quantity in quote asset
        /// </summary>
        [Map("0")]
        QuoteAsset,
        /// <summary>
        /// ["<c>1</c>"] Quantity in number of contracts
        /// </summary>
        [Map("1")]
        Contracts,
        /// <summary>
        /// ["<c>2</c>"] Quantity in base asset
        /// </summary>
        [Map("2")]
        BaseAsset
    }
}
