using CryptoExchange.Net.Objects.Options;
using System;

namespace CoinW.Net.Objects.Options
{
    /// <summary>
    /// Options for the CoinW SymbolOrderBook
    /// </summary>
    public class CoinWOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static CoinWOrderBookOptions Default { get; set; } = new CoinWOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal CoinWOrderBookOptions Copy()
        {
            var result = Copy<CoinWOrderBookOptions>();
            result.Limit = Limit;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
