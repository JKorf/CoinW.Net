using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CoinW.Net.Enums;
using CryptoExchange.Net.RateLimiting.Guards;
using CoinW.Net.Objects.Models;
using System.Collections.Generic;
using System.Linq;
using CryptoExchange.Net.Objects.Errors;

namespace CoinW.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class CoinWRestClientFuturesApiTrading : ICoinWRestClientFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly CoinWRestClientFuturesApi _baseClient;
        private readonly ILogger _logger;

        internal CoinWRestClientFuturesApiTrading(ILogger logger, CoinWRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWOrderId>> PlaceOrderAsync(string symbol, PositionSide side, FuturesOrderType orderType, decimal quantity, int leverage, decimal? price = null, QuantityUnit? quantityUnit = null, MarginType? marginType = null, decimal? stopLossPrice = null, decimal? takeProfitPrice = null, decimal? triggerPrice = null, TriggerOrderType? triggerOrderType = null, int? goldenId = null, string? clientOrderId = null, bool? useMegaCoupon = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            parameters.AddEnum("direction", side);
            parameters.AddEnum("positionType", orderType);
            parameters.AddString("quantity", quantity);
            parameters.Add("leverage", leverage);
            parameters.AddOptionalString("openPrice", price);
            parameters.AddEnumAsInt("quantityUnit", quantityUnit ?? QuantityUnit.Contracts);
            parameters.AddEnumAsInt("positionModel", marginType ?? MarginType.IsolatedMargin);
            parameters.AddOptionalString("stopLossPrice", stopLossPrice);
            parameters.AddOptionalString("stopProfitPrice", takeProfitPrice);
            parameters.AddOptionalString("triggerPrice", triggerPrice);
            parameters.AddOptionalEnumAsInt("triggerType", triggerOrderType);
            parameters.AddOptional("goldId", goldenId);
            parameters.AddOptional("thirdOrderId", clientOrderId);
            parameters.AddOptional("useAlmightyGold", useMegaCoupon);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/order", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<CoinWOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<CallResult<CoinWBatchResult>[]>> PlaceMultipleOrdersAsync(IEnumerable<CoinWFuturesOrderRequest> requests, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(requests.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/batchOrders", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWBatchResult[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<CallResult<CoinWBatchResult>[]>(default);

            var ordersResult = new List<CallResult<CoinWBatchResult>>();
            foreach (var item in result.Data)
            {
                if (item.Code > 0)
                    ordersResult.Add(new CallResult<CoinWBatchResult>(item, null, new ServerError(item.Code, _baseClient.GetErrorInfo(item.Code, null))));
                else
                    ordersResult.Add(new CallResult<CoinWBatchResult>(item));
            }

            if (ordersResult.All(x => !x.Success))
                return result.AsErrorWithData(new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), ordersResult.ToArray());

            return result.As(ordersResult.ToArray());
        }

        #endregion

        #region Close Position

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWOrderId>> ClosePositionAsync(long positionId, FuturesOrderType? orderType = null, decimal? quantityToClose = null, decimal? factorToClose = null, decimal? price = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("id", positionId);
            parameters.AddOptionalEnum("positionType", orderType);
            parameters.AddOptional("closeNum", quantityToClose);
            parameters.AddOptional("closeRate", factorToClose);
            parameters.AddOptional("orderPrice", price);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/v1/perpum/positions", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<CoinWOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Close Positions By Client Order Id

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWBatchResult[]>> ClosePositionsByClientOrderIdAsync(IEnumerable<string> clientOrderIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(clientOrderIds.Select(x => new Dictionary<string, object> { { "thirdOrderId", x } }).ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/v1/perpum/batchClose", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWBatchResult[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Close All Positions

        /// <inheritdoc />
        public async Task<WebCallResult> CloseAllPositionsAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/v1/perpum/allpositions", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Reverse Position

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWOrderId>> ReversePositionAsync(long positionId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("id", positionId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/positions/reverse", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Adjust Margin

        /// <inheritdoc />
        public async Task<WebCallResult> AdjustMarginAsync(long positionId, decimal addMargin, decimal reduceMargin, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("id", positionId);
            parameters.Add("addMargin", addMargin);
            parameters.Add("reduceMargin", reduceMargin);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/positions/margin", CoinWExchange.RateLimiter.CoinW, 1, false,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Tp Sl

        /// <inheritdoc />
        public async Task<WebCallResult> SetTpSlAsync(long orderOrPositionId, string symbol, decimal? takeProfitPrice = null, decimal? takeProfitOrderPrice = null, decimal? takeProfitRate = null, decimal? stopLossPrice = null, decimal? stopLossOrderPrice = null, decimal? stopLossRate = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("id", orderOrPositionId);
            parameters.Add("instrument", symbol);
            parameters.AddOptional("stopProfitPrice", takeProfitPrice);
            parameters.AddOptional("stopProfitOrderPrice", takeProfitOrderPrice);
            parameters.AddOptional("stopProfitRate", takeProfitRate);
            parameters.AddOptional("stopLossPrice", stopLossPrice);
            parameters.AddOptional("stopLossOrderPrice", stopLossOrderPrice);
            parameters.AddOptional("stopLossRate", stopLossRate);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/TPSL", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Trailing Tp Sl

        /// <inheritdoc />
        public async Task<WebCallResult> SetTrailingTpSlAsync(long positionId, decimal callbackRate, decimal? triggerPrice = null, decimal? quantity = null, QuantityUnit? quantityType = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("openId", positionId);
            parameters.Add("callbackRate", callbackRate);
            parameters.AddOptional("triggerPrice", triggerPrice);
            parameters.AddOptional("quantity", quantity);
            parameters.AddOptionalEnum("quantityUnit", quantityType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/moveTPSL", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion


        #region Edit Order

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWEditResult>> EditOrderAsync(long orderId, string symbol, PositionSide side, FuturesOrderType orderType, decimal quantity, int leverage, decimal? price = null, QuantityUnit? quantityUnit = null, MarginType? marginType = null, decimal? stopLossPrice = null, decimal? takeProfitPrice = null, decimal? triggerPrice = null, TriggerOrderType? triggerOrderType = null, int? goldenId = null, string? clientOrderId = null, bool? useMegaCoupon = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            parameters.AddEnum("direction", side);
            parameters.AddEnum("positionType", orderType);
            parameters.AddString("quantity", quantity);
            parameters.Add("leverage", leverage);
            parameters.AddOptionalString("openPrice", price);
            parameters.AddEnumAsInt("quantityUnit", quantityUnit ?? QuantityUnit.Contracts);
            parameters.AddEnumAsInt("positionModel", marginType ?? MarginType.IsolatedMargin);
            parameters.AddOptionalString("stopLossPrice", stopLossPrice);
            parameters.AddOptionalString("stopProfitPrice", takeProfitPrice);
            parameters.AddOptionalString("triggerPrice", triggerPrice);
            parameters.AddOptionalEnumAsInt("triggerType", triggerOrderType);
            parameters.AddOptional("goldId", goldenId);
            parameters.AddOptional("thirdOrderId", clientOrderId);
            parameters.AddOptional("useAlmightyGold", useMegaCoupon);
            var request = _definitions.GetOrCreate(HttpMethod.Put, "/v1/perpum/order", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWEditResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult> CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("id", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/v1/perpum/order", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<WebCallResult> CancelOrdersAsync(IEnumerable<long> orderIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("sourceIds", orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Delete, "/v1/perpum/batchOrders", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesOrderPage>> GetOpenOrdersAsync(string symbol, FuturesOrderType orderType, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            parameters.AddEnum("positionType", orderType);
            parameters.AddOptional("page", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/open", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesOrderPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesOrder[]>> GetOpenOrdersAsync(FuturesOrderType orderType, string? symbol = null, IEnumerable<long>? orderIds = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instrument", symbol);
            parameters.AddEnum("positionType", orderType);
            parameters.AddOptional("sourceIds", orderIds == null ? null : string.Join(",", orderIds));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/order", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Order Quantity

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWValue>> GetOpenOrderCountAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/openQuantity", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tp Sl

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWTpSl[]>> GetTpSlAsync(long? orderId = null, long? positionId = null, long? planOrderId = null, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("openId", positionId);
            parameters.AddOptional("planOrderId", planOrderId);
            parameters.AddOptional("stopFrom", orderId != null ? 1 : positionId != null ? 2 : 3);
            parameters.AddOptional("instrument", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/TPSL", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWTpSl[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Trailing Tp Sl

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWTrailingTpSl[]>> GetTrailingTpSlAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/moveTPSL", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWTrailingTpSl[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order History 7D

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWHistOrderPage>> GetOrderHistory7DaysAsync(string? symbol = null, FuturesOrderType? orderType = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instrument", symbol);
            parameters.AddOptionalEnum("originType", orderType);
            parameters.AddOptional("page", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/history", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWHistOrderPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order History 3 Months

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWHistOrderPage>> GetOrderHistory3MonthsAsync(string? symbol = null, FuturesOrderType? orderType = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instrument", symbol);
            parameters.AddOptionalEnum("originType", orderType);
            parameters.AddOptional("page", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/archive", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWHistOrderPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Positions

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWPosition[]>> GetPositionsAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/positions", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWPosition[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Position History

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWPositionHistoryPage>> GetPositionHistoryAsync(string? symbol = null, MarginType? marginType = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instrument", symbol);
            parameters.AddOptional("positionModel", marginType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/positions/history", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(15, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<CoinWPositionHistoryPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Positions

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWPosition[]>> GetPositionsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "v1/perpum/positions/all", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(30, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            var result = await _baseClient.SendAsync<CoinWPosition[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Transaction History 3 Days

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesTransactionPage>> GetTransactionHistory3DaysAsync(string symbol, OrderType? orderType = null, MarginType? marginType = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            parameters.AddOptionalEnum("originType", orderType);
            parameters.AddOptionalEnum("positionModel", marginType);
            parameters.AddOptional("page", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/deals", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesTransactionPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Transaction History 3 Months

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesTransactionPage>> GetTransactionHistory3MonthsAsync(string symbol, OrderType? orderType = null, MarginType? marginType = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            parameters.AddOptionalEnum("originType", orderType);
            parameters.AddOptionalEnum("positionModel", marginType);
            parameters.AddOptional("page", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/deals/history", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesTransactionPage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
