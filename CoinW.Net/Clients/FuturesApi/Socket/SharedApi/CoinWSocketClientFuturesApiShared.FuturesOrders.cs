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
    internal partial class CoinWSocketClientFuturesSharedApi
    {

        #region Subscribe To Futures Order Updates

        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
            => await SubscribeToFuturesOrderUpdatesAsync(request, x => handler(x.ToType<SharedFuturesOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeFuturesOrderOptions SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToOrderUpdatesAsync(
                update => handler(update.ToType(update.Data.Select(x => 
                    new SharedFuturesOrderUpdate(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
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
#pragma warning disable CS0618 // Type or member is obsolete
                        Fee = x.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                        Leverage = x.Leverage,
                        PositionSide = x.PositionSide == Enums.PositionSide.Long ? SharedPositionSide.Long : x.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : null,
                        TakeProfitPrice = x.TakeProfitPrice,
                        StopLossPrice = x.StopLossPrice
                    }
                ).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
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
            if (type == FuturesOrderType.Market)
                return SharedOrderType.Market;

            if (type == FuturesOrderType.Plan)
                return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }
    }
}
