using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Account type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountType>))]
    public enum AccountType
    {
        /// <summary>
        /// ["<c>WEALTH</c>"] Funding account
        /// </summary>
        [Map("WEALTH")]
        Funding,
        /// <summary>
        /// ["<c>SPOT</c>"] Spot account
        /// </summary>
        [Map("SPOT")]
        Spot
    }
}
