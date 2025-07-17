using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Objects.Models;

namespace CoinW.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class CoinWRestClientFuturesApiExchangeData : ICoinWRestClientFuturesApiExchangeData
    {
        private readonly CoinWRestClientFuturesApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal CoinWRestClientFuturesApiExchangeData(ILogger logger, CoinWRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "XXX", CoinWExchange.RateLimiter.CoinW, 1, false);
            var result = await _baseClient.SendAsync<CoinWModel>(request, null, ct).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        #endregion
    }
}
