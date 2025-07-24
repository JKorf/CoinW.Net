using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Trigger status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerStatus>))]
    public enum TriggerStatus
    {
        /// <summary>
        /// Not triggered
        /// </summary>
        [Map("0")]
        NotTriggered,
        /// <summary>
        /// Triggered
        /// </summary>
        [Map("1")]
        Triggered,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("2")]
        Canceled
    }
}
