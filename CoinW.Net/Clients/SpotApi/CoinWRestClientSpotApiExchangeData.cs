using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.RateLimiting.Guards;
using CoinW.Net.Enums;
using System.Linq;

namespace CoinW.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class CoinWRestClientSpotApiExchangeData : ICoinWRestClientSpotApiExchangeData
    {
        private readonly CoinWRestClientSpotApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal CoinWRestClientSpotApiExchangeData(ILogger logger, CoinWRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWTicker[]>> GetTickersAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection() { { "command", "returnTicker" } };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false
                , limitGuard: new SingleLimitGuard(80, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnTicker"));
            var result = await _baseClient.SendAsync<Dictionary<string, CoinWTicker>>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<CoinWTicker[]>(default);

            foreach (var kvp in result.Data)
                kvp.Value.Symbol = kvp.Key;

            return result.As(result.Data.Values.ToArray());
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWAsset[]>> GetAssetsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnCurrencies");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false
                , limitGuard: new SingleLimitGuard(80, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnCurrencies"));
            var result = await _baseClient.SendAsync<Dictionary<string, CoinWAsset>>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<CoinWAsset[]>(default);

            return result.As(result.Data.Values.ToArray());
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWSymbol[]>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnSymbol");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(80, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnSymbol"));
            var result = await _baseClient.SendAsync<CoinWSymbol[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnOrderBook");
            parameters.Add("symbol", symbol);
            parameters.AddOptional("size", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnOrderBook"));
            var result = await _baseClient.SendAsync<CoinWOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWTrade[]>> GetRecentTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnTradeHistory");
            parameters.Add("symbol", symbol);
            parameters.AddOptionalMillisecondsString("start", startTime);
            parameters.AddOptionalMillisecondsString("end", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnTradeHistory"));
            var result = await _baseClient.SendAsync<CoinWTrade[]>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                // Time is return in UTC+8, so convert to UTC
                foreach (var item in result.Data)
                    item.Time = new DateTime(item.Time.Year, item.Time.Month, item.Time.Day, item.Time.Hour - 8, item.Time.Minute, item.Time.Second, DateTimeKind.Utc);
            }    
            
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnChartData");
            parameters.Add("currencyPair", symbol);
            parameters.AddEnum("period", interval);
            parameters.AddOptionalMillisecondsString("start", startTime);
            parameters.AddOptionalMillisecondsString("end", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnChartData"));
            var result = await _baseClient.SendAsync<CoinWKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
