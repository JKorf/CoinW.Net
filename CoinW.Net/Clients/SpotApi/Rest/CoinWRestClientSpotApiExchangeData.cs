using System;
using System.Collections.Generic;
using System.Net.Http;
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
        public async Task<HttpResult<CoinWTicker[]>> GetTickersAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings) { { "command", "returnTicker" } };

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false
                , limitGuard: new SingleLimitGuard(80, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnTicker"));
            var result = await _baseClient.SendAsync<Dictionary<string, CoinWTicker>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<CoinWTicker[]>(result);

            foreach (var kvp in result.Data)
                kvp.Value.Symbol = kvp.Key;

            return HttpResult.Ok(result, result.Data.Values.ToArray());
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<HttpResult<CoinWAsset[]>> GetAssetsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnCurrencies");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false
                , limitGuard: new SingleLimitGuard(80, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnCurrencies"));
            var result = await _baseClient.SendAsync<Dictionary<string, CoinWAsset>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<CoinWAsset[]>(result);

            return HttpResult.Ok(result, result.Data.Values.ToArray());
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<CoinWSymbol[]>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnSymbol");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(80, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnSymbol"));
            var result = await _baseClient.SendAsync<CoinWSymbol[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<CoinWOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnOrderBook");
            parameters.Add("symbol", symbol);
            parameters.Add("size", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnOrderBook"));
            var result = await _baseClient.SendAsync<CoinWOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<CoinWTrade[]>> GetRecentTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnTradeHistory");
            parameters.Add("symbol", symbol);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnTradeHistory"));
            var result = await _baseClient.SendAsync<CoinWTrade[]>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                // Time is return in UTC+8, so convert to UTC
                foreach (var item in result.Data)
                {
                    var time = item.Time.AddHours(-8);
                    item.Time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc);
                }
            }    
            
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<CoinWKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnChartData");
            parameters.Add("currencyPair", symbol);
            parameters.Add("period", interval);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/public", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnChartData"));
            var result = await _baseClient.SendAsync<CoinWKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
