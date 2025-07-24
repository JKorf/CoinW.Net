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
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesOrderType>))]
    public enum FuturesOrderType
    {
        /// <summary>
        /// Market
        /// </summary>
        [Map("execute", "2")]
        Market,
        /// <summary>
        /// Plan order (also for limit orders)
        /// </summary>
        [Map("plan", "1")]
        Plan,
        /// <summary>
        /// Planned trigger order
        /// </summary>
        [Map("planTrigger", "3")]
        PlanTrigger,
        /// <summary>
        /// Trailing stop order
        /// </summary>
        [Map("moveStopProfitLoss")]
        TrailingStop
    }
}
