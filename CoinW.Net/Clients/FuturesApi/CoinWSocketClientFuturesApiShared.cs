using CryptoExchange.Net.SharedApis;
using System;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects.Sockets;
using System.Linq;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net;
using CoinW.Net.Enums;

namespace CoinW.Net.Clients.FuturesApi
{
    internal partial class CoinWSocketClientFuturesApi : ICoinWSocketClientFuturesApiShared
    {
        private const string _topicId = "CoinWFutures";
        private const string _exchangeName = "CoinW";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.PerpetualLinear };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(CoinWExchange.Metadata, this);

        #region Balance client
        SubscribeBalanceOptions IBalanceSocketClient.SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToBalanceUpdatesAsync(
                update => handler(update.ToType(update.Data.Select(x =>
                    new SharedBalance(
                        SupportedTradingModes, 
                        x.Asset.ToUpperInvariant(), 
                        x.Available,
                        x.Available + x.Frozen + x.Holding)).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Kline client
        SubscribeKlineOptions IKlineSocketClient.SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> IKlineSocketClient.SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.FuturesKlineIntervalStream)request.Interval;

            var validationError = SharedClient.SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToKlineUpdatesAsync(symbol, interval, update => handler(update.ToType(
                new SharedKline(request.Symbol, symbol, update.Data.OpenTime, update.Data.ClosePrice, update.Data.HighPrice, update.Data.LowPrice, update.Data.OpenPrice, update.Data.Volume))), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Order Book client
        SubscribeOrderBookOptions IOrderBookSocketClient.SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(_exchangeName, false, new[] { 100 });
        async Task<WebSocketResult<UpdateSubscription>> IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToOrderBookUpdatesAsync(symbol, update => handler(update.ToType(new SharedOrderBook(update.Data.Asks, update.Data.Bids))), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Ticker client

        SubscribeTickerOptions ITickerSocketClient.SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName);
        async Task<WebSocketResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToTickerUpdatesAsync(symbol, update => handler(update.ToType(new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, update.Data.Symbol), symbol, update.Data.LastPrice, update.Data.HighPrice, update.Data.LowPrice, update.Data.Volume, update.Data.PercentageChange * 100)
            {
                QuoteVolume = update.Data.VolumeUsdt
            })), ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Trade client

        SubscribeTradeOptions ITradeSocketClient.SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await SubscribeToTradeUpdatesAsync(symbol, update => handler(update.ToType(update.Data.Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.Direction == Enums.PositionSide.Long ? SharedOrderSide.Buy : SharedOrderSide.Sell,
            } ).ToArray())), ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Futures Order client

        SubscribeFuturesOrderOptions IFuturesOrderSocketClient.SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToOrderUpdatesAsync(
                update => handler(update.ToType(update.Data.Select(x => 
                    new SharedFuturesOrder(
                        ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.Id.ToString(),
                        ParseOrderType(x.OrderType),
                        (x.PositionSide == PositionSide.Long && x.OpenStatus == OpenStatus.Open || x.PositionSide == PositionSide.Short && x.OpenStatus == OpenStatus.Close) ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        ParseOrderStatus(x.OrderStatus),
                        x.CreateTime)
                    {
                        AveragePrice = x.AveragePrice,
                        ClientOrderId = x.ClientOrderId,
                        OrderPrice = x.OrderPrice == 0 ? null : x.OrderPrice,
                        OrderQuantity = new SharedOrderQuantity(x.QuantityUnit == Enums.QuantityUnit.BaseAsset ? x.OrderQuantity : null, x.QuantityUnit == Enums.QuantityUnit.QuoteAsset ? x.OrderQuantity : null, contractQuantity: x.QuantityUnit == Enums.QuantityUnit.Contracts ? x.OrderQuantity : null),
                        QuantityFilled = new SharedOrderQuantity(contractQuantity : x.QuantityFilled),
                        UpdateTime = x.UpdateTime,
                        Fee = x.Fee,
                        Leverage = x.Leverage,
                        PositionSide = x.PositionSide == Enums.PositionSide.Long ? SharedPositionSide.Long : x.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : null,
                        TakeProfitPrice = x.TakeProfitPrice,
                        StopLossPrice = x.StopLossPrice
                    }
                ).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
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
            if (type == FuturesOrderType.Market)
                return SharedOrderType.Market;

            if (type == FuturesOrderType.Plan)
                return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }
        #endregion

        #region Position client
        SubscribePositionOptions IPositionSocketClient.SubscribePositionOptions { get; } = new SubscribePositionOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> IPositionSocketClient.SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<DataEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToPositionDetailUpdatesAsync(
                update => {
                    handler(update.ToType(update.Data
                        .Where(x => x.OrderStatus != FuturesOrderStatus.MarkerChange)
                        .Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol, x.PositionSize, x.CreateTime)
                        {
                            Id = x.PositionId.ToString(),
                            AverageOpenPrice = x.OpenPrice,
                            PositionMode = SharedPositionMode.HedgeMode,
                            PositionSide = x.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                            Leverage = x.Leverage,
                            UpdateTime = x.UpdateTime
                    }).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
