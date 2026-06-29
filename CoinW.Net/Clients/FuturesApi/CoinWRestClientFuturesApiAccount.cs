using CryptoExchange.Net.Objects;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.RateLimiting.Guards;
using CoinW.Net.Enums;
using CryptoExchange.Net.Objects.Errors;

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
        public async Task<HttpResult<CoinWValue>> GetLeverageAsync(long? positionId = null, long? orderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("positionId", positionId);
            parameters.Add("orderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/positions/leverage", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Margin Rate

        /// <inheritdoc />
        public async Task<HttpResult<CoinWValue>> GetMarginRateAsync(long positionId, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("positionId", positionId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/positions/marginRate", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Max Trade Size

        /// <inheritdoc />
        public async Task<HttpResult<CoinWMaxTrade>> GetMaxTradeSizeAsync(string symbol, int leverage, MarginType marginType, decimal orderPrice, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("instrument", symbol);
            parameters.Add("leverage", leverage);
            parameters.Add("positionModel", marginType);
            parameters.Add("orderPrice", orderPrice);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/orders/maxSize", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(8, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWMaxTrade>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Max Transferable

        /// <inheritdoc />
        public async Task<HttpResult<CoinWValue>> GetMaxTransferableAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/account/available", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWValue>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<CoinWFuturesBalance>> GetBalancesAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/account/getUserAssets", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFuturesBalance>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Fees

        /// <inheritdoc />
        public async Task<HttpResult<CoinWFees>> GetFeesAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/account/fees", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWFees>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Margin Mode

        /// <inheritdoc />
        public async Task<HttpResult<CoinWMarginInfo>> GetMarginModeAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/positions/type", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWMarginInfo>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Margin Mode

        /// <inheritdoc />
        public async Task<HttpResult> SetMarginModeAsync(MarginType marginType, PositionCombineType positionCombineType, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("positionModel", marginType);
            parameters.Add("layout", positionCombineType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/perpum/positions/type", CoinWExchange.RateLimiter.CoinW, 1, true, 
                limitGuard: new SingleLimitGuard(2, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Toggle Mega Coupon

        /// <inheritdoc />
        public async Task<HttpResult> ToggleMegaCouponAsync(bool enabled, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("status", enabled ? 1 : 0);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v1/perpum/account/almightyGoldInfo", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Max Position Size

        /// <inheritdoc />
        public async Task<HttpResult<CoinWMaxPosition>> GetMaxPositionSizeAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("instrument", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v1/perpum/orders/availSize", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<CoinWMaxPosition>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data == null)
                return HttpResult.Fail<CoinWMaxPosition>(result, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Only available after opening a position")));
;
            return result;
        }

        #endregion

    }
}
