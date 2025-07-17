using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Withdraw type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WithdrawType>))]
    public enum WithdrawType
    {
        /// <summary>
        /// On chain withdraw
        /// </summary>
        [Map("ordinary_withdraw")]
        OnChain,
        /// <summary>
        /// Internal transfer
        /// </summary>
        [Map("internal_transfer")]
        Internal
    }
}
