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
        /// ["<c>ordinary_withdraw</c>"] On chain withdraw
        /// </summary>
        [Map("ordinary_withdraw")]
        OnChain,
        /// <summary>
        /// ["<c>internal_transfer</c>"] Internal transfer
        /// </summary>
        [Map("internal_transfer")]
        Internal
    }
}
