using CryptoExchange.Net.Objects;
using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.RateLimiting.Guards;

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

    }
}
