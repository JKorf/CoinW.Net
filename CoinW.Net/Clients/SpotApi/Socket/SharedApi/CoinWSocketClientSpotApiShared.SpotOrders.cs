using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.SpotApi
{
    internal partial class CoinWSocketClientSpotSharedApi
    {

        #region Subscribe To Spot Order Updates

        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
            => await SubscribeToSpotOrderUpdatesAsync(request, x => handler(x.ToType<SharedSpotOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeSpotOrderOptions SubscribeSpotOrderOptions { get; } = new SubscribeSpotOrderOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToOrderUpdatesAsync(
                update =>
                {
                    if (update.Data.Reason == Enums.OrderEventReason.Rejected)
                        return;

                    handler(update.ToType<SharedSpotOrderUpdate[]>(new[] {
                    new SharedSpotOrderUpdate(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        update.Data.OrderType == Enums.OrderType.Limit ? SharedOrderType.Limit : update.Data.OrderType == Enums.OrderType.Market ? SharedOrderType.Market : SharedOrderType.Other,
                        update.Data.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        ParseOrderStatus(update.Data),
                        update.Data.Timestamp)
                    {
                        AveragePrice = update.Data.AverageFillPrice,
#pragma warning disable CS0618 // Type or member is obsolete
                        Fee = update.Data.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                        ClientOrderId = update.Data.ClientOrderId?.ToString(),
                        OrderQuantity = new SharedOrderQuantity(update.Data.Quantity == 0 ? null : update.Data.Quantity),
                        QuantityFilled = new SharedOrderQuantity(update.Data.Quantity - update.Data.QuantityRemaining, update.Data.QuoteQuantityFilled),
                        OrderPrice = update.Data.Price == 0 ? null : update.Data.Price
                    }
                }));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        private SharedOrderStatus ParseOrderStatus(CoinWOrderUpdate data)
        {
            if (data.Reason == Enums.OrderEventReason.Canceled
                || data.Reason == Enums.OrderEventReason.Rejected)
            {
                return SharedOrderStatus.Canceled;
            }

            if (data.EventType == Enums.OrderEventType.Done)
                return SharedOrderStatus.Filled;

            if (data.EventType == Enums.OrderEventType.Received)
                return SharedOrderStatus.Open;

            if (data.Reason == Enums.OrderEventReason.Filled)
                return SharedOrderStatus.Open; // Filled but not done?

            return SharedOrderStatus.Unknown;
        }
    }
}
