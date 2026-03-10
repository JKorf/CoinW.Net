using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Movement status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MovementStatus>))]
    public enum MovementStatus
    {
        /// <summary>
        /// ["<c>1</c>"] Waiting
        /// </summary>
        [Map("1")]
        Waiting,
        /// <summary>
        /// ["<c>3</c>"] Success
        /// </summary>
        [Map("3")]
        Success
    }
}
