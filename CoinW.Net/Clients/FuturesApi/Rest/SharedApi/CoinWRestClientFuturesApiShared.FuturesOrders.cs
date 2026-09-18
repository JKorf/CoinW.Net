using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using System.Linq;
using CryptoExchange.Net;
using CoinW.Net.Enums;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects.Errors;

namespace CoinW.Net.Clients.FuturesApi
{
    internal partial class CoinWRestClientFuturesSharedApi
    {

        public SharedFeeDeductionType FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        public SharedFeeAssetType FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        public SharedOrderType[] FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        public SharedTimeInForce[] FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled };
        public SharedQuantitySupport FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAndQuoteAssetAndContracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.BaseAndQuoteAssetAndContracts,
                SharedQuantityType.Contracts);

        public string GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        #region Place Futures Order

        async Task<IExchangeCallResult<SharedId>> IPlaceFuturesOrder.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
            => await PlaceFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public PlaceFuturesOrderOptions PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [            
                ExchangeParameterRule.Optional("PositionId", "Id of the position to close", "Required for closing positions", aliases: ["id"])
            ]
        };
        public async Task<HttpResult<SharedId>> PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if ((request.Side == SharedOrderSide.Buy && request.PositionSide == SharedPositionSide.Long) 
                || (request.Side == SharedOrderSide.Sell && request.PositionSide == SharedPositionSide.Short))
            {
                if (request.Leverage == null)
                    return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Missing(nameof(PlaceFuturesOrderRequest.Leverage), $"Required optional parameter `{nameof(PlaceFuturesOrderRequest.Leverage)}` for exchange `{Exchange}` is missing"));

                if (request.PositionSide == null)
                    return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Missing(nameof(PlaceFuturesOrderRequest.PositionSide), $"Required optional parameter `{nameof(PlaceFuturesOrderRequest.PositionSide)}` for exchange `{Exchange}` is missing"));

                var result = await _api.Trading.PlaceOrderAsync(
                    request.Symbol!.GetSymbol(FormatSymbol),
                    request.PositionSide == SharedPositionSide.Long ? Enums.PositionSide.Long : Enums.PositionSide.Short,
                    request.OrderType == SharedOrderType.Limit ? Enums.FuturesOrderType.Plan : Enums.FuturesOrderType.Market,
                    quantity: request.Quantity?.QuantityInBaseAsset ?? request.Quantity?.QuantityInContracts ?? request.Quantity?.QuantityInQuoteAsset ?? 0,
                    quantityUnit: request.Quantity?.QuantityInBaseAsset != null ? QuantityUnit.BaseAsset : request.Quantity?.QuantityInContracts != null ? QuantityUnit.Contracts : QuantityUnit.QuoteAsset,
                    leverage: (int)request.Leverage!,
                    price: request.Price,
                    marginType: request.MarginMode == null ? null : request.MarginMode == SharedMarginMode.Cross ? MarginType.CrossMargin : MarginType.IsolatedMargin,
                    stopLossPrice: request.StopLossPrice,
                    takeProfitPrice: request.TakeProfitPrice,
                    clientOrderId: request.ClientOrderId,
                    ct: ct).ConfigureAwait(false);

                if (!result.Success)
                    return HttpResult.Fail<SharedId>(result);

                return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
            }
            else
            {
                // Closing position needs a separate endpoint
                var positionId = request.GetParamValue<long?>(Exchange, "PositionId", "id");
                if (positionId == null)
                    return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Missing("PositionId", "Required parameter `PositionId` missing for PlaceFuturesOrderAsync. `PositionId` is required for closing positions."));

                var result = await _api.Trading.ClosePositionAsync(
                    positionId.Value,
                    request.OrderType == SharedOrderType.Market ? FuturesOrderType.Market : FuturesOrderType.Plan,
                    request.Quantity?.QuantityInContracts,
                    request.Quantity == null ? 1: null,
                    request.Price,
                    ct).ConfigureAwait(false);

                if (!result.Success)
                    return HttpResult.Fail<SharedId>(result);

                return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
            }
        }

        #endregion

        #region Get Futures Order

        async Task<IExchangeCallResult<SharedFuturesOrder>> IGetFuturesOrder.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public GetFuturesOrderOptions GetFuturesOrderOptions { get; } = new GetFuturesOrderOptions(_exchangeName, true)
        {
            RequestNotes = "Canceled orders without trades are not returned"
        };
        public async Task<HttpResult<SharedFuturesOrder>> GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, ArgumentError.Invalid(nameof(GetOrderRequest.OrderId), "Invalid order id"));

            var orderResult = await _api.Trading.GetOpenOrdersAsync(FuturesOrderType.Market, orderIds: [orderId], ct: ct).ConfigureAwait(false);
            if (!orderResult.Success)
                return HttpResult.Fail<SharedFuturesOrder>(orderResult);

            if (orderResult.Data.Length == 0)
            {
                var closedOrders = await _api.Trading.GetOrderHistory7DaysAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
                if(!closedOrders.Success)
                    return HttpResult.Fail<SharedFuturesOrder>(closedOrders);

                if (closedOrders.Data.Rows.Length == 0)
                    return HttpResult.Fail<SharedFuturesOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

                var order = closedOrders.Data.Rows[0];
                return HttpResult.Ok(closedOrders, new SharedFuturesOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, order.Symbol), order.Symbol,
                    order.Id.ToString(),
                    ParseOrderType(order.OrderType),
                    (order.PositionSide == PositionSide.Long && order.Status == OpenStatus.Open || order.PositionSide == PositionSide.Short && order.Status == OpenStatus.Close )? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(order.OrderStatus),
                    order.CreateTime)
                {
                    AveragePrice = order.AveragePrice,
                    ClientOrderId = string.IsNullOrEmpty(order.ClientOrderId) ? null : order.ClientOrderId,
                    OrderPrice = order.OrderPrice == 0 ? null : order.OrderPrice,
                    OrderQuantity = new SharedOrderQuantity(order.QuantityUnit == Enums.QuantityUnit.BaseAsset ? order.OrderQuantity : null, order.QuantityUnit == Enums.QuantityUnit.QuoteAsset ? order.OrderQuantity : null, contractQuantity: order.QuantityUnit == Enums.QuantityUnit.Contracts ? order.OrderQuantity : null),
                    QuantityFilled = new SharedOrderQuantity(contractQuantity: order.QuantityFilled),
                    UpdateTime = order.UpdateTime,
                    PositionSide = order.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                    Leverage = order.Leverage
                });
            }
            else
            {
                var order = orderResult.Data[0];
                return HttpResult.Ok(orderResult, new SharedFuturesOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, order.Symbol), order.Symbol,
                    order.Id.ToString(),
                    ParseOrderType(order.OrderType),
                    (order.PositionSide == PositionSide.Long && order.OpenStatus == OpenStatus.Open || order.PositionSide == PositionSide.Short && order.OpenStatus == OpenStatus.Close )? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(order.OrderStatus),
                    order.CreateTime)
                {
                    ClientOrderId = string.IsNullOrEmpty(order.ClientOrderId) ? null : order.ClientOrderId,
                    OrderPrice = order.OrderPrice == 0 ? null : order.OrderPrice,
                    OrderQuantity = new SharedOrderQuantity(order.QuantityUnit == Enums.QuantityUnit.BaseAsset ? order.OrderQuantity : null, order.QuantityUnit == Enums.QuantityUnit.QuoteAsset ? order.OrderQuantity : null, contractQuantity: order.QuantityUnit == Enums.QuantityUnit.Contracts ? order.OrderQuantity : null),
                    QuantityFilled = new SharedOrderQuantity(contractQuantity : order.QuantityFilled),
                    UpdateTime = order.UpdateTime,
                    PositionSide = order.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
#pragma warning disable CS0618 // Type or member is obsolete
                    Fee = order.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                    Leverage = order.Leverage,
                    StopLossPrice = order.StopLossPrice,
                    TakeProfitPrice = order.TakeProfitPrice
                });
            }
        }

        #endregion

        #region Get Open Futures Orders

        async Task<IExchangeCallResult<SharedFuturesOrder[]>> IGetOpenFuturesOrders.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
            => await GetOpenFuturesOrdersAsync(request, ct).ConfigureAwait(false);

        public GetOpenFuturesOrdersOptions GetOpenFuturesOrdersOptions { get; } = new GetOpenFuturesOrdersOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesOrder[]>> GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await _api.Trading.GetOpenOrdersAsync(FuturesOrderType.Plan, symbol, ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(orders);

            return HttpResult.Ok(orders, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), x.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderType),
                (x.PositionSide == PositionSide.Long && x.OpenStatus == OpenStatus.Open || x.PositionSide == PositionSide.Short && x.OpenStatus == OpenStatus.Close) ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.OrderStatus),
                x.CreateTime)
            {
                ClientOrderId = string.IsNullOrEmpty(x.ClientOrderId) ? null : x.ClientOrderId,
                OrderPrice = x.OrderPrice == 0 ? null : x.OrderPrice,
                OrderQuantity = new SharedOrderQuantity(x.QuantityUnit == Enums.QuantityUnit.BaseAsset ? x.OrderQuantity : null, x.QuantityUnit == Enums.QuantityUnit.QuoteAsset ? x.OrderQuantity : null, contractQuantity: x.QuantityUnit == Enums.QuantityUnit.Contracts ? x.OrderQuantity : null),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                UpdateTime = x.UpdateTime,
                PositionSide = x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
#pragma warning disable CS0618 // Type or member is obsolete
                Fee = x.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                Leverage = x.Leverage,
                StopLossPrice = x.StopLossPrice,
                TakeProfitPrice = x.TakeProfitPrice
            }).ToArray());
        }

        #endregion

        #region Get Closed Futures Orders

        async Task<IExchangeCallResult<SharedFuturesOrder[]>> IGetClosedFuturesOrders.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetClosedFuturesOrdersAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetFuturesClosedOrdersOptions GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, false, true, true, 1000)
        {
            MaxAge = TimeSpan.FromDays(7)
        };
        public async Task<HttpResult<SharedFuturesOrder[]>> GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetClosedFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            // Determine page token
            var direction = DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetOrderHistory7DaysAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                page: pageParams.Page,
                pageSize: pageParams.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromPage(pageParams),
                    result.Data.Rows.Length,
                    result.Data.Rows.Select(x => x.CreateTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams,
                    maxAge: TimeSpan.FromDays(7));

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Rows, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Select(x => new SharedFuturesOrder(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), x.Symbol,
                        x.Id.ToString(),
                        ParseOrderType(x.OrderType),
                        (x.PositionSide == PositionSide.Long && x.Status == OpenStatus.Open || x.PositionSide == PositionSide.Short && x.Status == OpenStatus.Close) ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        ParseOrderStatus(x.OrderStatus),
                        x.CreateTime)
                    {
                        AveragePrice = x.AveragePrice,
                        ClientOrderId = string.IsNullOrEmpty(x.ClientOrderId) ? null : x.ClientOrderId,
                        OrderPrice = x.OrderPrice == 0 ? null : x.OrderPrice,
                        OrderQuantity = new SharedOrderQuantity(x.QuantityUnit == Enums.QuantityUnit.BaseAsset ? x.OrderQuantity : null, x.QuantityUnit == Enums.QuantityUnit.QuoteAsset ? x.OrderQuantity : null, contractQuantity: x.QuantityUnit == Enums.QuantityUnit.Contracts ? x.OrderQuantity : null),
                        QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                        UpdateTime = x.UpdateTime,
                        PositionSide = x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                        Leverage = x.Leverage
                    }).ToArray(), nextPageRequest);
        }

        #endregion

        #region Get Futures Order Trades

        async Task<IExchangeCallResult<SharedUserTrade[]>> IGetFuturesOrderTrades.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
            => await GetFuturesOrderTradesAsync(request, ct).ConfigureAwait(false);

        public GetFuturesOrderTradesOptions GetFuturesOrderTradesOptions { get; } = new GetFuturesOrderTradesOptions(_exchangeName, true);
        public async Task<HttpResult<SharedUserTrade[]>> GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, ArgumentError.Invalid(nameof(GetOrderTradesRequest.OrderId), "Invalid order id"));

            var orders = await _api.Trading.GetTransactionHistory3DaysAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedUserTrade[]>(orders);

            var forOrder = orders.Data.Rows.Where(x => x.OrderId == orderId);
            return HttpResult.Ok(orders, forOrder.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), x.Symbol,
                x.OrderId.ToString(),
                x.Id.ToString(),
                (x.PositionSide == PositionSide.Long && x.Status == TrailingOrderStatus.Open || x.PositionSide == PositionSide.Short && x.Status == TrailingOrderStatus.Closed) ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                new SharedOrderQuantity(contractQuantity: x.QuantityOpen),
                x.ClosePrice ?? x.OpenPrice ?? 0,
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                Role = x.Role == Role.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        #endregion

        #region Get Futures User Trade History

        async Task<IExchangeCallResult<SharedUserTrade[]>> IGetFuturesUserTradeHistory.GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetFuturesUserTradeHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => GetFuturesUserTradeHistoryAsync(request, pageRequest, ct);
        GetFuturesUserTradeHistoryOptions IFuturesOrderRestClient.GetFuturesUserTradesOptions => GetFuturesUserTradeHistoryOptions;


        public GetFuturesUserTradeHistoryOptions GetFuturesUserTradeHistoryOptions { get; } = new GetFuturesUserTradeHistoryOptions(_exchangeName, false, true, false, 100)
        {
            MaxAge = TimeSpan.FromDays(3),
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<GetUserTradesRequest>.NotSupported(x => x.StartTime),
                RequestParameterRuleOverride<GetUserTradesRequest>.NotSupported(x => x.EndTime),
                ]
        };
        public async Task<HttpResult<SharedUserTrade[]>> GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetFuturesUserTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetTransactionHistory3DaysAsync(request.Symbol!.GetSymbol(FormatSymbol),
                page: pageParams.Page,
                pageSize: pageParams.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            // Get next token
            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromPage(pageParams),
                    result.Data.Rows.Length,
                    result.Data.Rows.Select(x => x.CreateTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams,
                    maxAge: TimeSpan.FromDays(7));

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Rows, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), x.Symbol,
                            x.OrderId.ToString(),
                            x.Id.ToString(),
                            (x.PositionSide == PositionSide.Long && x.Status == TrailingOrderStatus.Open || x.PositionSide == PositionSide.Short && x.Status == TrailingOrderStatus.Closed) ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            new SharedOrderQuantity(contractQuantity: x.QuantityOpen),
                            x.ClosePrice ?? x.OpenPrice ?? 0,
                            x.CreateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            Fee = x.Fee,                
                            Role = x.Role == Role.Maker ? SharedRole.Maker : SharedRole.Taker
                        })
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Cancel Futures Order

        async Task<IExchangeCallResult<SharedId>> ICancelFuturesOrder.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesOrderOptions CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id"));

            var order = await _api.Trading.CancelOrderAsync( orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        #endregion

        #region Get Positions

        async Task<IExchangeCallResult<SharedPosition[]>> IGetPositions.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
            => await GetPositionsAsync(request, ct).ConfigureAwait(false);

        public GetPositionsOptions GetPositionsOptions { get; } = new GetPositionsOptions(_exchangeName, true);
        public async Task<HttpResult<SharedPosition[]>> GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = GetPositionsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPosition[]>(Exchange, validationError);

            HttpResult<CoinWPosition[]> result;
            if (request.Symbol != null)
                result = await _api.Trading.GetPositionsAsync(symbol: request.Symbol.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            else
                result = await _api.Trading.GetPositionsAsync(ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedPosition[]>(result);

            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol.TradingMode } : new[] { request.TradingMode!.Value };
            return HttpResult.Ok(result, result.Data.Select(x => 
                new SharedPosition(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    new SharedOrderQuantity(contractQuantity: Math.Abs(x.PositionSize)),
                    x.UpdateTime)
                {
                    Id = x.Id.ToString(),
                    UnrealizedPnl = x.UnrealizedPnl,
                    LiquidationPrice = x.LiquidationPrice == 0 ? null : x.LiquidationPrice,
                    Leverage = x.Leverage,
                    AverageOpenPrice = x.OpenPrice,
                    PositionMode = SharedPositionMode.HedgeMode,
                    PositionSide = x.PositionSide == PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                    UpdateTime = x.UpdateTime,
                    StopLossPrice = x.StopLossPrice,
                    TakeProfitPrice = x.TakeProfitPrice
                }).ToArray());
        }

        #endregion

        #region Close Position

        public ClosePositionOptions ClosePositionOptions { get; } = new ClosePositionOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [
                ExchangeParameterRule.Required("PositionId", "The id of the position to close", 123L)
            ]
        };
        public async Task<HttpResult<SharedId>> ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = ClosePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var positionId = request.GetParamValue<long>(Exchange, "PositionId", "id");
            var result = await _api.Trading.ClosePositionAsync(positionId, quantityToClose: request.Quantity, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }


        async Task<IExchangeCallResult<SharedId>> ICloseFullPosition.CloseFullPositionAsync(CloseFullPositionRequest request, CancellationToken ct)
            => await CloseFullPositionAsync(request, ct).ConfigureAwait(false);

        public CloseFullPositionOptions CloseFullPositionOptions { get; } = new CloseFullPositionOptions(_exchangeName, true)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<CloseFullPositionRequest>.Required(x => x.PositionId)
            ]
        };
        public async Task<HttpResult<SharedId>> CloseFullPositionAsync(CloseFullPositionRequest request, CancellationToken ct)
        {
            var validationError = CloseFullPositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.PositionId, out var positionId))
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid(nameof(CloseFullPositionRequest.PositionId), "Invalid position id"));

            var result = await _api.Trading.ClosePositionAsync(positionId, factorToClose: 1, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        #endregion

        private SharedOrderStatus ParseOrderStatus(FuturesOrderStatus orderStatus)
        {
            if (orderStatus == FuturesOrderStatus.Open || orderStatus == FuturesOrderStatus.PartiallyFilled)
                return SharedOrderStatus.Open;

            if (orderStatus == FuturesOrderStatus.Canceled)
                return SharedOrderStatus.Canceled;

            if (orderStatus == FuturesOrderStatus.Filled)
                return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(FuturesOrderType type)
        {
            if (type == FuturesOrderType.Market) return SharedOrderType.Market;
            if (type == FuturesOrderType.Plan) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

    }
}
