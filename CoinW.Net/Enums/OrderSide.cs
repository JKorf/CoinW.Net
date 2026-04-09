using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderSide>))]
    public enum OrderSide
    {
        /// <summary>
        /// ["<c>0</c>"] Buy
        /// </summary>
        [Map("0", "BUY", "buy")]
        Buy,
        /// <summary>
        /// ["<c>1</c>"] Sell
        /// </summary>
        [Map("1", "SELL", "sell")]
        Sell
    }
}
