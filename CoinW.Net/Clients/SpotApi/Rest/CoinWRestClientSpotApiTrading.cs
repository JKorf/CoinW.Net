using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using CoinW.Net.Interfaces.Clients.SpotApi;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CoinW.Net.Enums;
using CryptoExchange.Net.RateLimiting.Guards;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net;

namespace CoinW.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class CoinWRestClientSpotApiTrading : ICoinWRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly CoinWRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal CoinWRestClientSpotApiTrading(ILogger logger, CoinWRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }


        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<CoinWOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity, decimal? quoteQuantity, decimal? price, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "doTrade");
            parameters.Add("symbol", symbol);
            parameters.Add("type", side);
            parameters.Add("isMarket", (type == OrderType.Market ? true : false).ToString().ToLower());
            parameters.Add("amount", quantity);
            parameters.Add("rate", price);
            parameters.Add("funds", quoteQuantity);
            parameters.Add("out_trade_no", clientOrderId ?? ExchangeHelpers.RandomString(32));
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "doTrade" + key));
            var result = await _baseClient.SendAsync<CoinWOrderResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult> CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "cancelOrder");
            parameters.Add("orderNumber", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "cancelOrder" + key));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "cancelAllOrder");
            parameters.Add("currencyPair", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "cancelAllOrder" + key));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<CoinWOrder[]>> GetOpenOrdersAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnOpenOrders");
            parameters.Add("currencyPair", symbol);
            parameters.Add("startAt", startTime);
            parameters.Add("endAT", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnOpenOrders" + key));
            var result = await _baseClient.SendAsync<CoinWOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<CoinWOrderDetails>> GetOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnOrderTrades");
            parameters.Add("orderNumber", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnOrderTrades" + key));
            var result = await _baseClient.SendAsync<CoinWOrderDetails>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            var time = result.Data.Timestamp;
            result.Data.Timestamp = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc).AddHours(-8);
            return result;
        }

        #endregion

        #region Get Order Transaction History

        /// <inheritdoc />
        public async Task<HttpResult<CoinWOrderTransaction[]>> GetOrderTransactionHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnUTradeHistory");
            parameters.Add("currencyPair", symbol);
            parameters.Add("startAt", startTime);
            parameters.Add("endAt", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnUTradeHistory" + key));
            var result = await _baseClient.SendAsync<CoinWOrderTransaction[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<CoinWUserTrade[]>> GetUserTradesAsync(string? symbol = null, long? fromId = null, long? toId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "getUserTrades");
            parameters.Add("symbol", symbol);
            parameters.Add("after", fromId);
            parameters.Add("before", toId);
            parameters.Add("startAt", startTime);
            parameters.Add("endAt", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "getUserTrades" + key));
            var result = await _baseClient.SendAsync<CoinWUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<CoinWUserTrade[]>(result);

            return HttpResult.Ok(result, result.Data.List);
        }

        #endregion

    }
}
