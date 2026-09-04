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
        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false);
        #region Subscribe To Kline Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.FuturesKlineIntervalStream)request.Interval;

            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToKlineUpdatesAsync(symbol, interval, update => handler(update.ToType(
                new SharedKline(
                    request.Symbol,
                    symbol, 
                    update.Data.OpenTime,
                    update.Data.ClosePrice, 
                    update.Data.HighPrice,
                    update.Data.LowPrice, 
                    update.Data.OpenPrice,
                    new SharedOrderQuantity(update.Data.Volume)))), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
