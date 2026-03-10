using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesKlineInterval>))]
    public enum FuturesKlineInterval
    {
        /// <summary>
        /// ["<c>0</c>"] One minute
        /// </summary>
        [Map("0")]
        OneMinute = 60,
        /// <summary>
        /// ["<c>7</c>"] Three minutes
        /// </summary>
        [Map("7")]
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// ["<c>1</c>"] Five minutes
        /// </summary>
        [Map("1")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// ["<c>2</c>"] Fifteen minutes
        /// </summary>
        [Map("2")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// ["<c>8</c>"] Thirty minutes
        /// </summary>
        [Map("8")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// ["<c>3</c>"] One hour
        /// </summary>
        [Map("3")]
        OneHour = 60 * 60,
        /// <summary>
        /// ["<c>4</c>"] Four hours
        /// </summary>
        [Map("4")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// ["<c>5</c>"] One day
        /// </summary>
        [Map("5")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// ["<c>6</c>"] One week
        /// </summary>
        [Map("6")]
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// ["<c>9</c>"] One month
        /// </summary>
        [Map("9")]
        OneMonth = 60 * 60 * 24 * 30
    }
}
