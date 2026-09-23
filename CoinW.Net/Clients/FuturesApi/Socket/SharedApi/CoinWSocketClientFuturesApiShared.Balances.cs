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
        public SubscribeBalanceOptions SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, false);
        #region Subscribe To Balance Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToBalanceUpdatesAsync(
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

    }
}
