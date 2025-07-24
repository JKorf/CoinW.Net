using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.RateLimiting.Guards;
using System.Linq;
using CoinW.Net.Enums;

namespace CoinW.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class CoinWRestClientFuturesApiExchangeData : ICoinWRestClientFuturesApiExchangeData
    {
        private readonly CoinWRestClientFuturesApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal CoinWRestClientFuturesApiExchangeData(ILogger logger, CoinWRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("name", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/instruments", CoinWExchange.RateLimiter.CoinW, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesSymbol[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpumPublic/ticker", CoinWExchange.RateLimiter.CoinW, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result.As<CoinWFuturesTicker>(result.Data?.First());
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesTicker[]>> GetTickersAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpumPublic/tickers", CoinWExchange.RateLimiter.CoinW, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesKline[]>> GetKlinesAsync(string symbol, FuturesKlineInterval interval, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currencyCode", symbol);
            parameters.AddEnum("granularity", interval);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpumPublic/klines", CoinWExchange.RateLimiter.CoinW, 1, false, limitGuard: new SingleLimitGuard(20, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Last Funding Rate

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWValue>> GetLastFundingRateAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/fundingRate", CoinWExchange.RateLimiter.CoinW, 1, false);
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("base", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpumPublic/depth", CoinWExchange.RateLimiter.CoinW, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesTrade[]>> GetRecentTradesAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("base", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpumPublic/trades", CoinWExchange.RateLimiter.CoinW, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Margin Requirements

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWMarginRequirement[]>> GetMarginRequirementsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/ladders", CoinWExchange.RateLimiter.CoinW, 1, true, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWMarginRequirementWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<CoinWMarginRequirement[]>(result.Data?.LadderConfig);
        }

        #endregion

        #region Get Trades

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWTradeHistory>> GetTradeHistoryAsync(string symbol, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            parameters.AddOptional("page", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/trades", CoinWExchange.RateLimiter.CoinW, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWTradeHistory>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
