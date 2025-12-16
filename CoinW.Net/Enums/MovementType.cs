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
        /// Deposit
        /// </summary>
        [Map("1")]
        Deposit,
        /// <summary>
        /// Withdrawal
        /// </summary>
        [Map("2")]
        Withdrawal
    }
}
