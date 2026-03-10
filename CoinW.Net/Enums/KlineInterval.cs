using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KlineInterval>))]
    public enum KlineInterval
    {
        /// <summary>
        /// ["<c>60</c>"] One minute
        /// </summary>
        [Map("60")]
        OneMinute = 60,
        /// <summary>
        /// ["<c>180</c>"] Three minutes
        /// </summary>
        [Map("180")]
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// ["<c>300</c>"] Five minutes
        /// </summary>
        [Map("300")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// ["<c>900</c>"] Fifteen minutes
        /// </summary>
        [Map("900")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// ["<c>1800</c>"] Thirty minutes
        /// </summary>
        [Map("1800")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// ["<c>3600</c>"] One hour
        /// </summary>
        [Map("3600")]
        OneHour = 60 * 60,
        /// <summary>
        /// ["<c>7200</c>"] Two hours
        /// </summary>
        [Map("7200")]
        TwoHours = 60 * 60 *2,
        /// <summary>
        /// ["<c>14400</c>"] Four hours
        /// </summary>
        [Map("14400")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// ["<c>86400</c>"] Four hours
        /// </summary>
        [Map("86400")]
        OneDay = 60 * 60 * 24,
    }
}
