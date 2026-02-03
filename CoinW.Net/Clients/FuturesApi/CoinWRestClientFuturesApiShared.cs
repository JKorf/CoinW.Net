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
        public string Exchange => "CoinW";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.PerpetualLinear };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Balance Client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(AccountTypeFilter.Futures);

        async Task<ExchangeWebResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(Exchange, request, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedBalance[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedBalance[]>(Exchange, TradingMode.Spot, [new SharedBalance("USDT", result.Data.AvailableUsdt, result.Data.AvailableUsdt + result.Data.Holding + result.Data.Frozen)]);
        }

        #endregion

        #region Fee Client
        EndpointOptions<GetFeeRequest> IFeeRestClient.GetFeeOptions { get; } = new EndpointOptions<GetFeeRequest>(true);

        async Task<ExchangeWebResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = ((IFeeRestClient)this).GetFeeOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFee>(Exchange, validationError);

            // Get data
            var result = await Account.GetFeesAsync(
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFee>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, SupportedTradingModes, new SharedFee(result.Data.MakerFee * 100, result.Data.TakerFee * 100));
        }
        #endregion

        #region Klines client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 1500, false);

        async Task<ExchangeWebResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.FuturesKlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.FuturesKlineInterval), interval))
                return new ExchangeWebResult<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedKline[]>(Exchange, validationError);

            // Determine pagination
            // Data is normally returned oldest first, so to do newest first pagination we have to do some calc
            DateTime endTime = request.EndTime ?? DateTime.UtcNow;
            DateTime? startTime = request.StartTime;
            if (pageToken is DateTimeToken dateTimeToken)
                endTime = dateTimeToken.LastTime;

            var limit = request.Limit ?? 1500;
            if (startTime == null || startTime < endTime)
            {
                var offset = (int)interval * limit;
                startTime = endTime.AddSeconds(-offset);
            }

            if (startTime < request.StartTime)
                startTime = request.StartTime;

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                startTime,
                endTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedKline[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (result.Data.Length == limit)
            {
                var minOpenTime = result.Data.Min(x => x.OpenTime);
                if (request.StartTime == null || minOpenTime > request.StartTime.Value)
                    nextToken = new DateTimeToken(minOpenTime.AddSeconds(-(int)(interval - 1)));
            }

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, result.Data.AsEnumerable().Reverse().Select(x => 
                new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)).ToArray(), nextToken);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(new[] { 20 }, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(20, false);
        async Task<ExchangeWebResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetRecentTradesAsync(
                symbol,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTrade[]>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, result.Data.Select(x => 
            new SharedTrade(request.Symbol, symbol, x.QuantityBase, x.Price, x.Timestamp)
            {
                Side = x.Direction == Enums.PositionSide.Short ? SharedOrderSide.Sell : SharedOrderSide.Buy
            }).ToArray());
        }

        #endregion

        #region Futures Symbol client

        EndpointOptions<GetSymbolsRequest> IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);
        async Task<ExchangeWebResult<SharedFuturesSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesSymbolRestClient)this).GetFuturesSymbolsOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFuturesSymbol[]>(Exchange, null, default);
            
            var resultData = result.AsExchangeResult(Exchange, request.TradingMode == null ? SupportedTradingModes : new[] { request.TradingMode.Value }, result.Data.Select(s =>
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

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, resultData.Data);
            return resultData;
        }

        async Task<ExchangeResult<SharedSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<SharedSymbol[]>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<SharedSymbol[]>(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, baseAsset));
        }

        async Task<ExchangeResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbol));
        }

        async Task<ExchangeResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbolName));
        }
        #endregion

        #region Ticker client

        GetTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new GetTickerOptions();
        async Task<ExchangeWebResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickerOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedFuturesTicker>(Exchange, null, default);

            return resultTicker.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedFuturesTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, resultTicker.Data.Symbol), resultTicker.Data.Symbol, resultTicker.Data.LastPrice, resultTicker.Data.HighPrice, resultTicker.Data.LowPrice, resultTicker.Data.Volume, resultTicker.Data.PriceChangePercentage * 100)
            {
            });
        }

        GetTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new GetTickersOptions();
        async Task<ExchangeWebResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickersOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTickers = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!resultTickers)
                return resultTickers.AsExchangeResult<SharedFuturesTicker[]>(Exchange, null, default);

            return resultTickers.AsExchangeResult(Exchange, request.TradingMode == null ? SupportedTradingModes : new[] { request.TradingMode.Value }, resultTickers.Data.Select(x =>
            {
                return new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, x.PriceChangePercentage * 100)
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

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).PlaceFuturesOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.TradingMode,
                SupportedTradingModes,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderTypes,
                ((IFuturesOrderRestClient)this).FuturesSupportedTimeInForce,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            if ((request.Side == SharedOrderSide.Buy && request.PositionSide == SharedPositionSide.Long) 
                || (request.Side == SharedOrderSide.Sell && request.PositionSide == SharedPositionSide.Short))
            {
                if (request.Leverage == null)
                    return new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Missing(nameof(PlaceFuturesOrderRequest.Leverage), $"Required optional parameter `{nameof(PlaceFuturesOrderRequest.Leverage)}` for exchange `{Exchange}` is missing"));

                if (request.PositionSide == null)
                    return new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Missing(nameof(PlaceFuturesOrderRequest.PositionSide), $"Required optional parameter `{nameof(PlaceFuturesOrderRequest.PositionSide)}` for exchange `{Exchange}` is missing"));

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

                if (!result)
                    return result.AsExchangeResult<SharedId>(Exchange, null, default);

                return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(result.Data.OrderId.ToString()));
            }
            else
            {
                // Closing position needs a separate endpoint
                var positionId = ExchangeParameters.GetValue<long?>(request.ExchangeParameters, Exchange, "PositionId");
                if (positionId == null)
                    return new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Missing("PositionId", "Required parameter `PositionId` missing for PlaceFuturesOrderAsync. `PositionId` is required for closing positions."));

                var result = await Trading.ClosePositionAsync(
                    positionId.Value,
                    request.OrderType == SharedOrderType.Market ? FuturesOrderType.Market : FuturesOrderType.Plan,
                    request.Quantity?.QuantityInContracts,
                    request.Quantity == null ? 1: null,
                    request.Price,
                    ct).ConfigureAwait(false);

                return result.AsExchangeResult(Exchange, request.TradingMode, new SharedId(result.Data.OrderId.ToString()));
            }
        }

        EndpointOptions<GetOrderRequest> IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true)
        {
            RequestNotes = "Canceled orders without trades are not returned"
        };
        async Task<ExchangeWebResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, ArgumentError.Invalid(nameof(GetOrderRequest.OrderId), "Invalid order id"));

            var orderResult = await Trading.GetOpenOrdersAsync(FuturesOrderType.Market, orderIds: [orderId], ct: ct).ConfigureAwait(false);
            if (!orderResult)
                return orderResult.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);

            if (orderResult.Data.Length == 0)
            {
                var closedOrders = await Trading.GetOrderHistory7DaysAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
                if(!closedOrders)
                    return closedOrders.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);

                if (closedOrders.Data.Rows.Length == 0)
                    return orderResult.AsExchangeError<SharedFuturesOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

                var order = closedOrders.Data.Rows[0];
                return closedOrders.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedFuturesOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, order.Symbol), order.Symbol,
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
                return orderResult.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedFuturesOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, order.Symbol), order.Symbol,
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


        EndpointOptions<GetOpenOrdersRequest> IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true);
        async Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetOpenFuturesOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(FuturesOrderType.Plan, symbol, ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, null, default);

            return orders.AsExchangeResult(Exchange, request.Symbol == null ? SupportedTradingModes : new[] { request.Symbol.TradingMode }, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
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

        PaginatedEndpointOptions<GetClosedOrdersRequest> IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, true, 1000, true);
        async Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetClosedFuturesOrdersOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder[]>(Exchange, validationError);

            // Determine page token
            int page = 1;
            int pageSize = 50;
            if (pageToken is PageToken pToken) {
                page = pToken.Page;
                pageSize = pToken.PageSize;
            }

            // Get data
            var orders = await Trading.GetOrderHistory7DaysAsync(request.Symbol!.GetSymbol(FormatSymbol),
                page: page,
                pageSize: pageSize,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, null, default);

            // Get next token
            PageToken? nextToken = null;
            if (orders.Data.NextId != 0)
                nextToken = new PageToken(page + 1, pageSize);

            return orders.AsExchangeResult(Exchange, request.Symbol == null ? SupportedTradingModes : new[] { request.Symbol.TradingMode }, orders.Data.Rows.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
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
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, ArgumentError.Invalid(nameof(GetOrderTradesRequest.OrderId), "Invalid order id"));

            var orders = await Trading.GetTransactionHistory3DaysAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            var forOrder = orders.Data.Rows.Where(x => x.OrderId == orderId);
            return orders.AsExchangeResult(Exchange, request.TradingMode, forOrder.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
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

        PaginatedEndpointOptions<GetUserTradesRequest> IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, false, 100, true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesUserTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            int page = 1;
            int pageSize = 50;
            if (pageToken is PageToken pToken)
            {
                page = pToken.Page;
                pageSize = pToken.PageSize;
            }

            // Get data
            var orders = await Trading.GetTransactionHistory3DaysAsync(request.Symbol!.GetSymbol(FormatSymbol),
                page: page,
                pageSize: pageSize,
                ct: ct
                ).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            // Get next token
            PageToken? nextToken = null;
            if (orders.Data.NextId != 0)
                nextToken = new PageToken(page + 1, pageSize);

            return orders.AsExchangeResult(Exchange, request.Symbol.TradingMode, orders.Data.Rows.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
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
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).CancelFuturesOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return new ExchangeWebResult<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id"));

            var order = await Trading.CancelOrderAsync( orderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.TradingMode, new SharedId(request.OrderId));
        }

        EndpointOptions<GetPositionsRequest> IFuturesOrderRestClient.GetPositionsOptions { get; } = new EndpointOptions<GetPositionsRequest>(true);
        async Task<ExchangeWebResult<SharedPosition[]>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetPositionsOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedPosition[]>(Exchange, validationError);

            WebCallResult<CoinWPosition[]> result;
            if (request.Symbol != null)
                result = await Trading.GetPositionsAsync(symbol: request.Symbol.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            else
                result = await Trading.GetPositionsAsync(ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedPosition[]>(Exchange, null, default);

            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol.TradingMode } : new[] { request.TradingMode!.Value };
            return result.AsExchangeResult(Exchange, resultTypes, result.Data.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, Math.Abs(x.PositionSize), x.UpdateTime)
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

        EndpointOptions<ClosePositionRequest> IFuturesOrderRestClient.ClosePositionOptions { get; } = new EndpointOptions<ClosePositionRequest>(true)
        {
            RequiredExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription("PositionId", typeof(long), "The id of the position to close", 123L),
            }
        };
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).ClosePositionOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var positionId = ExchangeParameters.GetValue<long>(request.ExchangeParameters, Exchange, "PositionId");
            var result = await Trading.ClosePositionAsync(positionId, quantityToClose: request.Quantity, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.TradingMode, new SharedId(result.Data.OrderId.ToString()));
        }

        private SharedOrderStatus ParseOrderStatus(FuturesOrderStatus status)
        {
            if (status == FuturesOrderStatus.Open || status == FuturesOrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == FuturesOrderStatus.Canceled) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
        }

        private SharedOrderType ParseOrderType(FuturesOrderType type)
        {
            if (type == FuturesOrderType.Market) return SharedOrderType.Market;
            if (type == FuturesOrderType.Plan) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        #endregion

        #region Tp/SL Client
        EndpointOptions<SetTpSlRequest> IFuturesTpSlRestClient.SetFuturesTpSlOptions { get; } = new EndpointOptions<SetTpSlRequest>(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesTpSlRestClient.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTpSlRestClient)this).SetFuturesTpSlOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var positionResult = await Trading.GetPositionsAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
            if (!positionResult)
                return positionResult.AsExchangeError<SharedId>(Exchange, positionResult.Error!);

            if (positionResult.Data.Length == 0)
                return positionResult.AsExchangeError<SharedId>(Exchange, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var position = positionResult.Data[0];

            var result = await Trading.SetTpSlAsync(
                position.Id,
                request.Symbol.GetSymbol(FormatSymbol),
                takeProfitPrice: request.TpSlSide == SharedTpSlSide.TakeProfit ? request.TriggerPrice : position.TakeProfitPrice,
                stopLossPrice: request.TpSlSide == SharedTpSlSide.StopLoss ? request.TriggerPrice: position.StopLossPrice,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, SupportedTradingModes, new SharedId(position.Id.ToString()));
        }

        EndpointOptions<CancelTpSlRequest> IFuturesTpSlRestClient.CancelFuturesTpSlOptions { get; } = new EndpointOptions<CancelTpSlRequest>(true);
        async Task<ExchangeWebResult<bool>> IFuturesTpSlRestClient.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTpSlRestClient)this).CancelFuturesTpSlOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<bool>(Exchange, validationError);

            var positionResult = await Trading.GetPositionsAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
            if (!positionResult)
                return positionResult.AsExchangeError<bool>(Exchange, positionResult.Error!);

            if (positionResult.Data.Length == 0)
                return positionResult.AsExchangeError<bool>(Exchange, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var position = positionResult.Data[0];

            var result = await Trading.SetTpSlAsync(
                position.Id, 
                request.Symbol.GetSymbol(FormatSymbol),
                request.TpSlSide == SharedTpSlSide.TakeProfit ? null : position.TakeProfitPrice,
                request.TpSlSide == SharedTpSlSide.StopLoss ? null : position.StopLossPrice,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<bool>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, true);
        }

        #endregion
    }
}
