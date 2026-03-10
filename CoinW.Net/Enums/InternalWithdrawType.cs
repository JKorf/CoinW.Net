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
        /// ["<c>1</c>"] User id
        /// </summary>
        [Map("1")]
        UserId,
        /// <summary>
        /// ["<c>2</c>"] Mobile
        /// </summary>
        [Map("2")]
        Mobile,
        /// <summary>
        /// ["<c>3</c>"] Email
        /// </summary>
        [Map("3")]
        Email
    }
}
