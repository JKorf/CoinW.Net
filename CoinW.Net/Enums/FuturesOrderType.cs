using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace CoinW.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesOrderType>))]
    public enum FuturesOrderType
    {
        /// <summary>
        /// ["<c>execute</c>"] Market
        /// </summary>
        [Map("execute", "2")]
        Market,
        /// <summary>
        /// ["<c>plan</c>"] Plan order (also for limit orders)
        /// </summary>
        [Map("plan", "1")]
        Plan,
        /// <summary>
        /// ["<c>planTrigger</c>"] Planned trigger order
        /// </summary>
        [Map("planTrigger", "3")]
        PlanTrigger,
        /// <summary>
        /// ["<c>moveStopProfitLoss</c>"] Trailing stop order
        /// </summary>
        [Map("moveStopProfitLoss")]
        TrailingStop
    }
}
