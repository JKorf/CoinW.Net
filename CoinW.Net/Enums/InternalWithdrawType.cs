using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Internal withdraw type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<InternalWithdrawType>))]
    public enum InternalWithdrawType
    {
        /// <summary>
        /// User id
        /// </summary>
        [Map("1")]
        UserId,
        /// <summary>
        /// Mobile
        /// </summary>
        [Map("2")]
        Mobile,
        /// <summary>
        /// Email
        /// </summary>
        [Map("3")]
        Email
    }
}
