using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Movement type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MovementType>))]
    public enum MovementType
    {
        /// <summary>
        /// ["<c>1</c>"] Deposit
        /// </summary>
        [Map("1")]
        Deposit,
        /// <summary>
        /// ["<c>2</c>"] Withdrawal
        /// </summary>
        [Map("2")]
        Withdrawal
    }
}
