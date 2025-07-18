using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesKlineInterval>))]
    public enum FuturesKlineInterval
    {
        /// <summary>
        /// One minute
        /// </summary>
        [Map("0")]
        OneMinute = 60,
        /// <summary>
        /// Three minutes
        /// </summary>
        [Map("7")]
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// Five minutes
        /// </summary>
        [Map("1")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// Fifteen minutes
        /// </summary>
        [Map("2")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// Thirty minutes
        /// </summary>
        [Map("8")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// One hour
        /// </summary>
        [Map("3")]
        OneHour = 60 * 60,
        /// <summary>
        /// Four hours
        /// </summary>
        [Map("4")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// One day
        /// </summary>
        [Map("5")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// One week
        /// </summary>
        [Map("6")]
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// One month
        /// </summary>
        [Map("9")]
        OneMonth = 60 * 60 * 24 * 30
    }
}
