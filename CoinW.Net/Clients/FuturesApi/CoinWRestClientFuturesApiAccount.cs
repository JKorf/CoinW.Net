using CryptoExchange.Net.Objects;
using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.RateLimiting.Guards;
using CoinW.Net.Enums;

namespace CoinW.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class CoinWRestClientFuturesApiAccount : ICoinWRestClientFuturesApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly CoinWRestClientFuturesApi _baseClient;

        internal CoinWRestClientFuturesApiAccount(CoinWRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWValue>> GetLeverageAsync(long? positionId = null, long? orderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("positionId", positionId);
            parameters.AddOptional("orderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/positions/leverage", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Margin Rate

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWValue>> GetMarginRateAsync(long positionId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("positionId", positionId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/positions/marginRate", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Max Trade Size

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWMaxTrade>> GetMaxTradeSizeAsync(string symbol, int leverage, MarginType marginType, decimal orderPrice, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            parameters.Add("leverage", leverage);
            parameters.AddEnum("positionModel", marginType);
            parameters.Add("orderPRice", orderPrice);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/maxSize", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(8, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWMaxTrade>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Max Transferable

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWValue>> GetMaxTransferableAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/account/available", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFuturesBalance>> GetBalancesAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/account/getUserAssets", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesBalance>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Fees

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWFees>> GetFeesAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/account/fees", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFees>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Margin Mode

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWMarginInfo>> GetMarginModeAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/positions/type", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWMarginInfo>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Margin Mode

        /// <inheritdoc />
        public async Task<WebCallResult> SetMarginModeAsync(MarginType marginType, PositionCombineType positionCombineType, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("positionModel", marginType);
            parameters.AddEnum("layout", positionCombineType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/positions/type", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Toggle Mega Coupon

        /// <inheritdoc />
        public async Task<WebCallResult> ToggleMegaCouponAsync(bool enabled, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("status", enabled ? 1 : 0);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/v1/perpum/account/almightyGoldInfo", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Max Position Size

        /// <inheritdoc />
        public async Task<WebCallResult<CoinWMaxPosition>> GetMaxPositionSizeAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrument", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/perpum/orders/availSize", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWMaxPosition>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data == null)
                return result.AsError<CoinWMaxPosition>(new ServerError("Only available after opening a position"));
;
            return result;
        }

        #endregion

    }
}
