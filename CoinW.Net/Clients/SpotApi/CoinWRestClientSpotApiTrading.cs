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
        public async Task<WebCallResult<CoinWOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity, decimal? quoteQuantity, decimal? price, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "doTrade");
            parameters.Add("symbol", symbol);
            parameters.AddEnum("type", side);
            parameters.Add("isMarket", (type == OrderType.Market ? true : false).ToString().ToLower());
            parameters.AddOptionalString("amount", quantity);
            parameters.AddOptionalString("rate", price);
            parameters.AddOptionalString("funds", quoteQuantity);
            parameters.AddOptional("out_trade_no", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "doTrade" + key));
            var result = await _baseClient.SendAsync<CoinWOrderResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult> CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "cancelOrder");
            parameters.AddString("orderNumber", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "cancelOrder" + key));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<WebCallResult> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "cancelAllOrder");
            parameters.AddOptional("currencyPair", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "cancelAllOrder" + key));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWOrder[]>> GetOpenOrdersAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnOpenOrders");
            parameters.Add("currencyPair", symbol);
            parameters.AddOptionalMilliseconds("startAt", startTime);
            parameters.AddOptionalMilliseconds("endAT", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnOpenOrders" + key));
            var result = await _baseClient.SendAsync<CoinWOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWOrderDetails>> GetOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnOrderTrades");
            parameters.Add("orderNumber", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnOrderTrades" + key));
            var result = await _baseClient.SendAsync<CoinWOrderDetails>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            var time = result.Data.Timestamp;
            result.Data.Timestamp = new DateTime(time.Year, time.Month, time.Day, time.Hour - 8, time.Minute, time.Second, DateTimeKind.Utc);
            return result;
        }

        #endregion

        #region Get Order Transaction History

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWOrderTransaction[]>> GetOrderTransactionHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "returnUTradeHistory");
            parameters.Add("currencyPair", symbol);
            parameters.AddOptionalMillisecondsString("startAt", startTime);
            parameters.AddOptionalMillisecondsString("endAt", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "returnUTradeHistory" + key));
            var result = await _baseClient.SendAsync<CoinWOrderTransaction[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWUserTrade[]>> GetUserTradesAsync(string? symbol = null, long? fromId = null, long? toId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("command", "getUserTrades");
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptional("after", fromId);
            parameters.AddOptional("before", toId);
            parameters.AddOptionalMillisecondsString("startAt", startTime);
            parameters.AddOptionalMillisecondsString("endAt", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, host, key) => "getUserTrades" + key));
            var result = await _baseClient.SendAsync<CoinWUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result.As<CoinWUserTrade[]>(result.Data?.List);
        }

        #endregion

    }
}
