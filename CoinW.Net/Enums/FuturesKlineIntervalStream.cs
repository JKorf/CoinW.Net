using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesKlineIntervalStream>))]
    public enum FuturesKlineIntervalStream
    {
        /// <summary>
        /// ["<c>1m</c>"] One minute
        /// </summary>
        [Map("1m")]
        OneMinute = 60,
        /// <summary>
        /// ["<c>3</c>"] Three minutes
        /// </summary>
        [Map("3")]
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// ["<c>5</c>"] Five minutes
        /// </summary>
        [Map("5")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// ["<c>15</c>"] Fifteen minutes
        /// </summary>
        [Map("15")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// ["<c>30</c>"] Thirty minutes
        /// </summary>
        [Map("30")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// ["<c>1H</c>"] One hour
        /// </summary>
        [Map("1H")]
        OneHour = 60 * 60,
        /// <summary>
        /// ["<c>4H</c>"] Four hours
        /// </summary>
        [Map("4H")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// ["<c>1D</c>"] One day
        /// </summary>
        [Map("1D")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// ["<c>1W</c>"] One week
        /// </summary>
        [Map("1W")]
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// ["<c>1M</c>"] One month
        /// </summary>
        [Map("1M")]
        OneMonth = 60 * 60 * 24 * 30
    }
}
