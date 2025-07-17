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
    /// Movement status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MovementStatus>))]
    public enum MovementStatus
    {
        /// <summary>
        /// Waiting
        /// </summary>
        [Map("1")]
        Waiting,
        /// <summary>
        /// Success
        /// </summary>
        [Map("3")]
        Success
    }
}
