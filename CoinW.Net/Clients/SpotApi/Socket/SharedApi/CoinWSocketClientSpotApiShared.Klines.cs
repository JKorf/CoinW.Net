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
        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false);
        #region Subscribe To Kline Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.KlineIntervalStream)request.Interval;

            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToKlineUpdatesAsync(symbol, interval, update => {
                handler(update.ToType(
                    new SharedKline(
                        request.Symbol,
                        symbol,
                        update.Data.OpenTime,
                        update.Data.ClosePrice,
                        update.Data.HighPrice,
                        update.Data.LowPrice, 
                        update.Data.OpenPrice,
                        new SharedOrderQuantity(update.Data.Volume, update.Data.QuoteVolume))));
            }, ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
