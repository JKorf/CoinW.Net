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
    /// Role
    /// </summary>
    [JsonConverter(typeof(EnumConverter<Role>))]
    public enum Role
    {
        /// <summary>
        /// Taker
        /// </summary>
        [Map("1")]
        Taker,
        /// <summary>
        /// Maker
        /// </summary>
        [Map("2")]
        Maker
    }
}
