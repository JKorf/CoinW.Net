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
        /// ["<c>0</c>"] Not triggered
        /// </summary>
        [Map("0")]
        NotTriggered,
        /// <summary>
        /// ["<c>1</c>"] Triggered
        /// </summary>
        [Map("1")]
        Triggered,
        /// <summary>
        /// ["<c>2</c>"] Canceled
        /// </summary>
        [Map("2")]
        Canceled
    }
}
