using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.SharedApis;
using System.Text.Json.Serialization;
using CoinW.Net.Converters;

namespace CoinW.Net
{
    /// <summary>
    /// CoinW exchange information and configuration
    /// </summary>
    public static class CoinWExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "CoinW";

        /// <summary>
        /// Display name
        /// </summary>
        public static string DisplayName => "CoinW";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        #warning todo
        public static string ImageUrl { get; } = "TODO";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.coinw.com/";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://www.coinw.com/api-doc/en/common/introduction"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        internal static JsonSerializerContext _serializerContext = new CoinWSourceGenerationContext();

        /// <summary>
        /// Format a base and quote asset to an CoinW recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            if (tradingMode == TradingMode.Spot)
                return $"{baseAsset}_{quoteAsset}";

            return baseAsset;
        }

        /// <summary>
        /// Rate limiter configuration for the CoinW API
        /// </summary>
        public static CoinWRateLimiters RateLimiter { get; } = new CoinWRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the CoinW API
    /// </summary>
    public class CoinWRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal CoinWRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            CoinW = new RateLimitGate("CoinW");
            CoinW.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            CoinW.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate CoinW { get; private set; }

    }
}
