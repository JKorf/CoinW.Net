using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using CoinW.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using System.Text.Json;

namespace CoinW.Net
{
    /// <summary>
    /// CoinW exchange information and configuration
    /// </summary>
    public static class CoinWExchange
    {
        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "CoinW",
                "CoinW",
                "https://github.com/JKorf/CoinW.Net/blob/main/CoinW.Net/Icon/icon.png?raw=true",
                "https://www.coinw.com/",
                ["https://www.coinw.com/api-doc/en/common/introduction"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized
                );

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
        public static string ImageUrl { get; } = "https://github.com/JKorf/CoinW.Net/blob/main/CoinW.Net/Icon/icon.png?raw=true";

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

        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<CoinWSourceGenerationContext>());

        /// <summary>
        /// Aliases for CoinW assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("USDT", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

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
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            if (tradingMode == TradingMode.Spot)
                return $"{baseAsset}_{quoteAsset}";

            if (quoteAsset.Equals("USDT", StringComparison.InvariantCultureIgnoreCase))
                return baseAsset;

            return $"{baseAsset}_{quoteAsset}";
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

            Futures = new RateLimitGate("Futures")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new PathStartFilter("/v1/perpumPublic"), 30, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKey, new PathStartFilter("/v1/perpum/"), 100, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            Futures.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Futures.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);

            Spot = new RateLimitGate("Futures")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, [], 100, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            Spot.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Spot.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate CoinW { get; private set; }
        internal IRateLimitGate Futures { get; private set; }
        internal IRateLimitGate Spot { get; private set; }

    }
}
