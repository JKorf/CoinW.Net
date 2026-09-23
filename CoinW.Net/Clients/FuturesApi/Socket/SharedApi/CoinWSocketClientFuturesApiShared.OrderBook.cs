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
        public SubscribeOrderBookOptions SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(_exchangeName, false, new[] { 100 });
        #region Subscribe To Order Book Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SubscribeOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToOrderBookUpdatesAsync(symbol, update => handler(
                update.ToType(
                    new SharedOrderBook(SharedQuantityType.Contracts, update.SequenceNumber, update.Data.Asks, update.Data.Bids))), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
