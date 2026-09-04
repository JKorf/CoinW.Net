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

        #region Get Futures Ticker

        async Task<ICallResult<SharedFuturesTicker>> IGetFuturesTicker.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await GetFuturesTickerAsync(request, ct).ConfigureAwait(false);

        public GetFuturesTickerOptions GetFuturesTickerOptions { get; } = new GetFuturesTickerOptions(_exchangeName);
        public async Task<HttpResult<SharedFuturesTicker>> GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await _api.ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker);

            return HttpResult.Ok(resultTicker, new SharedFuturesTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, resultTicker.Data.Symbol), 
                resultTicker.Data.Symbol,
                resultTicker.Data.LastPrice,
                resultTicker.Data.HighPrice,
                resultTicker.Data.LowPrice,
                new SharedOrderQuantity(), // Volume is of the last trade, not the 24h volume
                resultTicker.Data.PriceChangePercentage * 100)
            {
            });
        }

        #endregion

        #region Get All Futures Tickers

        async Task<ICallResult<SharedFuturesTicker[]>> IGetAllFuturesTickers.GetAllFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await GetAllFuturesTickersAsync(request, ct).ConfigureAwait(false);

        Task<HttpResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllFuturesTickersAsync(request, ct);
        GetAllFuturesTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions => GetAllFuturesTickersOptions;

        public GetAllFuturesTickersOptions GetAllFuturesTickersOptions { get; } = new GetAllFuturesTickersOptions(_exchangeName);
        public async Task<HttpResult<SharedFuturesTicker[]>> GetAllFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllFuturesTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTickers = await _api.ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!resultTickers.Success)
                return HttpResult.Fail<SharedFuturesTicker[]>(resultTickers);

            return HttpResult.Ok(resultTickers, resultTickers.Data.Select(x =>
            {
                return new SharedFuturesTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol, 
                    x.LastPrice, 
                    x.HighPrice, 
                    x.LowPrice,
                    new SharedOrderQuantity(), // Volume is of the last trade, not the 24h volume
                    x.PriceChangePercentage * 100)
                {
                };
            }).ToArray());
        }

        #endregion

    }
}
