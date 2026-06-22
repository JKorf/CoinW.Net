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
    internal partial class CoinWRestClientFuturesApi : ICoinWRestClientFuturesApiShared
    {
        private const string _topicId = "CoinWFutures";
        private const string _exchangeName = "CoinW";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.PerpetualLinear };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(CoinWExchange.Metadata, this);

        #region Balance Client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Futures);

        async Task<HttpResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);

            return HttpResult.Ok<SharedBalance[]>(result, [
                new SharedBalance(
                    SupportedTradingModes,
                    "USDT", 
                    result.Data.AvailableUsdt, 
                    result.Data.AvailableUsdt + result.Data.Holding + result.Data.Frozen)]);
        }

        #endregion

        #region Fee Client
        GetFeeOptions IFeeRestClient.GetFeeOptions { get; } = new GetFeeOptions(_exchangeName, true);

        async Task<HttpResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFeeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFee>(Exchange, validationError);

            // Get data
            var result = await Account.GetFeesAsync(
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFee>(result);

            // Return
            return HttpResult.Ok(result, new SharedFee(result.Data.MakerFee * 100, result.Data.TakerFee * 100));
        }
        #endregion

        #region Klines client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, true, false, true, 1500, false);

        async Task<HttpResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.FuturesKlineInterval)request.Interval;

            var validationError = SharedClient.GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            var direction = DataDirection.Ascending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 1500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                pageParams.StartTime,
                pageParams.EndTime,
                pageParams.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data.Max(x => x.OpenTime)),
                    result.Data.Length,
                    result.Data.Select(x => x.OpenTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                   .Select(x =>
                        new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume))
                   .ToArray(), nextPageRequest);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, new[] { 20 }, false);
        async Task<HttpResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            return HttpResult.Ok(result, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 20, false);
        async Task<HttpResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetRecentTradesAsync(
                symbol,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x => 
            new SharedTrade(request.Symbol, symbol, x.QuantityBase, x.Price, x.Timestamp)
            {
                Side = x.Direction == Enums.PositionSide.Short ? SharedOrderSide.Sell : SharedOrderSide.Buy
            }).ToArray());
        }

        #endregion

        #region Futures Symbol client

        GetFuturesSymbolsOptions IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new GetFuturesSymbolsOptions(_exchangeName, false);
        async Task<HttpResult<SharedFuturesSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(result);
            
            var resultData = HttpResult.Ok(result, result.Data.Select(s =>
            new SharedFuturesSymbol(TradingMode.PerpetualLinear,
            s.BaseAsset,
            s.QuoteAsset,
            s.Name,
            s.Status == Enums.FuturesSymbolStatus.Online)
            {
                MinTradeQuantity = s.MinPositionQuantity,
                ContractSize = s.LotSize,
                MaxLongLeverage = s.MaxLeverage,
                MaxShortLeverage = s.MaxLeverage,
                MaxTradeQuantity = s.MaxPositionQuantity,
                PriceDecimals = s.PriceDecimals
            }).ToArray());

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, EnvironmentName, null, resultData.Data!);
            return resultData;
        }

        async Task<ExchangeCallResult<SharedSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, EnvironmentName, null, baseAsset));
        }

        async Task<ExchangeCallResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbol));
        }

        async Task<ExchangeCallResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbolName));
        }
        #endregion

        #region Ticker client

        GetFuturesTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new GetFuturesTickerOptions(_exchangeName);
        async Task<HttpResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker);

            return HttpResult.Ok(resultTicker, new SharedFuturesTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, resultTicker.Data.Symbol), resultTicker.Data.Symbol, resultTicker.Data.LastPrice, resultTicker.Data.HighPrice, resultTicker.Data.LowPrice, resultTicker.Data.Volume, resultTicker.Data.PriceChangePercentage * 100)
            {
            });
        }

        GetFuturesTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new GetFuturesTickersOptions(_exchangeName);
        async Task<HttpResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTickers = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!resultTickers.Success)
                return HttpResult.Fail<SharedFuturesTicker[]>(resultTickers);

            return HttpResult.Ok(resultTickers, resultTickers.Data.Select(x =>
            {
                return new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, x.PriceChangePercentage * 100)
                {
                };
            }).ToArray());
        }

        #endregion

        #region Futures Order Client

        SharedFeeDeductionType IFuturesOrderRestClient.FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType IFuturesOrderRestClient.FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        SharedOrderType[] IFuturesOrderRestClient.FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        SharedTimeInForce[] IFuturesOrderRestClient.FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled };
        SharedQuantitySupport IFuturesOrderRestClient.FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAndQuoteAssetAndContracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.BaseAndQuoteAssetAndContracts,
                SharedQuantityType.Contracts);

        string IFuturesOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, true)
        {
            OptionalExchangeParameters = [            
                new ParameterDescription(["PositionId", "id"], typeof(long),  "Id of the position to close", "Required for closing positions"),
            ]
        };
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if ((request.Side == SharedOrderSide.Buy && request.PositionSide == SharedPositionSide.Long) 
                || (request.Side == SharedOrderSide.Sell && request.PositionSide == SharedPositionSide.Short))
            {
                if (request.Leverage == null)
                    return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Missing(nameof(PlaceFuturesOrderRequest.Leverage), $"Required optional parameter `{nameof(PlaceFuturesOrderRequest.Leverage)}` for exchange `{Exchange}` is missing"));

                if (request.PositionSide == null)
                    return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Missing(nameof(PlaceFuturesOrderRequest.PositionSide), $"Required optional parameter `{nameof(PlaceFuturesOrderRequest.PositionSide)}` for exchange `{Exchange}` is missing"));

                var result = await Trading.PlaceOrderAsync(
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

                var result = await Trading.ClosePositionAsync(
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

        GetFuturesOrderOptions IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new GetFuturesOrderOptions(_exchangeName, true)
        {
            RequestNotes = "Canceled orders without trades are not returned"
        };
        async Task<HttpResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, ArgumentError.Invalid(nameof(GetOrderRequest.OrderId), "Invalid order id"));

            var orderResult = await Trading.GetOpenOrdersAsync(FuturesOrderType.Market, orderIds: [orderId], ct: ct).ConfigureAwait(false);
            if (!orderResult.Success)
                return HttpResult.Fail<SharedFuturesOrder>(orderResult);

            if (orderResult.Data.Length == 0)
            {
                var closedOrders = await Trading.GetOrderHistory7DaysAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
                if(!closedOrders.Success)
                    return HttpResult.Fail<SharedFuturesOrder>(closedOrders);

                if (closedOrders.Data.Rows.Length == 0)
                    return HttpResult.Fail<SharedFuturesOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

                var order = closedOrders.Data.Rows[0];
                return HttpResult.Ok(closedOrders, new SharedFuturesOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, order.Symbol), order.Symbol,
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
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, order.Symbol), order.Symbol,
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
                    Fee = order.Fee,
                    Leverage = order.Leverage,
                    StopLossPrice = order.StopLossPrice,
                    TakeProfitPrice = order.TakeProfitPrice
                });
            }
        }


        GetOpenFuturesOrdersOptions IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new GetOpenFuturesOrdersOptions(_exchangeName, true);
        async Task<HttpResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(FuturesOrderType.Plan, symbol, ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(orders);

            return HttpResult.Ok(orders, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol,
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
                Fee = x.Fee,
                Leverage = x.Leverage,
                StopLossPrice = x.StopLossPrice,
                TakeProfitPrice = x.TakeProfitPrice
            }).ToArray());
        }

        GetFuturesClosedOrdersOptions IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, false, true, true, 1000)
        {
            MaxAge = TimeSpan.FromDays(7)
        };
        async Task<HttpResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetClosedFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            // Determine page token
            var direction = DataDirection.Descending;
            var limit = request.Limit ?? 500;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetOrderHistory7DaysAsync(
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
                        ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol,
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

        GetFuturesOrderTradesOptions IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new GetFuturesOrderTradesOptions(_exchangeName, true);
        async Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, ArgumentError.Invalid(nameof(GetOrderTradesRequest.OrderId), "Invalid order id"));

            var orders = await Trading.GetTransactionHistory3DaysAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedUserTrade[]>(orders);

            var forOrder = orders.Data.Rows.Where(x => x.OrderId == orderId);
            return HttpResult.Ok(orders, forOrder.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol,
                x.OrderId.ToString(),
                x.Id.ToString(),
                (x.PositionSide == PositionSide.Long && x.Status == TrailingOrderStatus.Open || x.PositionSide == PositionSide.Short && x.Status == TrailingOrderStatus.Closed) ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.QuantityOpen,
                x.ClosePrice ?? x.OpenPrice ?? 0,
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                Role = x.Role == Role.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        GetFuturesUserTradesOptions IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new GetFuturesUserTradesOptions(_exchangeName, false, true, false, 100)
        {
            MaxAge = TimeSpan.FromDays(3)
        };
        async Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesUserTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetTransactionHistory3DaysAsync(request.Symbol!.GetSymbol(FormatSymbol),
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
                            ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol,
                            x.OrderId.ToString(),
                            x.Id.ToString(),
                            (x.PositionSide == PositionSide.Long && x.Status == TrailingOrderStatus.Open || x.PositionSide == PositionSide.Short && x.Status == TrailingOrderStatus.Closed) ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.QuantityOpen,
                            x.ClosePrice ?? x.OpenPrice ?? 0,
                            x.CreateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            Fee = x.Fee,                
                            Role = x.Role == Role.Maker ? SharedRole.Maker : SharedRole.Taker
                        })
                    .ToArray(), nextPageRequest);
        }

        CancelFuturesOrderOptions IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id"));

            var order = await Trading.CancelOrderAsync( orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));
        }

        GetPositionsOptions IFuturesOrderRestClient.GetPositionsOptions { get; } = new GetPositionsOptions(_exchangeName, true);
        async Task<HttpResult<SharedPosition[]>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetPositionsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPosition[]>(Exchange, validationError);

            HttpResult<CoinWPosition[]> result;
            if (request.Symbol != null)
                result = await Trading.GetPositionsAsync(symbol: request.Symbol.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            else
                result = await Trading.GetPositionsAsync(ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedPosition[]>(result);

            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol.TradingMode } : new[] { request.TradingMode!.Value };
            return HttpResult.Ok(result, result.Data.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol, Math.Abs(x.PositionSize), x.UpdateTime)
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

        ClosePositionOptions IFuturesOrderRestClient.ClosePositionOptions { get; } = new ClosePositionOptions(_exchangeName, true)
        {
            RequiredExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription(["PositionId", "id"], typeof(long), "The id of the position to close", 123L),
            }
        };
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.ClosePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var positionId = request.GetParamValue<long>(Exchange, "PositionId", "id");
            var result = await Trading.ClosePositionAsync(positionId, quantityToClose: request.Quantity, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

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

        #endregion

        #region Tp/SL Client
        SetFuturesTpSlOptions IFuturesTpSlRestClient.SetFuturesTpSlOptions { get; } = new SetFuturesTpSlOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesTpSlRestClient.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.SetFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var positionResult = await Trading.GetPositionsAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
            if (!positionResult.Success)
                return HttpResult.Fail<SharedId>(Exchange, positionResult.Error!);

            if (positionResult.Data.Length == 0)
                return HttpResult.Fail<SharedId>(Exchange, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var position = positionResult.Data[0];

            var result = await Trading.SetTpSlAsync(
                position.Id,
                request.Symbol.GetSymbol(FormatSymbol),
                takeProfitPrice: request.TpSlSide == SharedTpSlSide.TakeProfit ? request.TriggerPrice : position.TakeProfitPrice,
                stopLossPrice: request.TpSlSide == SharedTpSlSide.StopLoss ? request.TriggerPrice: position.StopLossPrice,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(position.Id.ToString()));
        }

        CancelFuturesTpSlOptions IFuturesTpSlRestClient.CancelFuturesTpSlOptions { get; } = new CancelFuturesTpSlOptions(_exchangeName, true);
        async Task<HttpResult<bool>> IFuturesTpSlRestClient.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<bool>(Exchange, validationError);

            var positionResult = await Trading.GetPositionsAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
            if (!positionResult.Success)
                return HttpResult.Fail<bool>(Exchange, positionResult.Error!);

            if (positionResult.Data.Length == 0)
                return HttpResult.Fail<bool>(Exchange, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var position = positionResult.Data[0];

            var result = await Trading.SetTpSlAsync(
                position.Id, 
                request.Symbol.GetSymbol(FormatSymbol),
                request.TpSlSide == SharedTpSlSide.TakeProfit ? null : position.TakeProfitPrice,
                request.TpSlSide == SharedTpSlSide.StopLoss ? null : position.StopLossPrice,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<bool>(result);

            // Return
            return HttpResult.Ok(result, true);
        }

        #endregion
    }
}
