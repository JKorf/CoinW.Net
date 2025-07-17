using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CoinW.Net.Enums;
using CryptoExchange.Net.RateLimiting.Guards;

namespace CoinW.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class CoinWRestClientFuturesApiTrading : ICoinWRestClientFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly CoinWRestClientFuturesApi _baseClient;
        private readonly ILogger _logger;

        internal CoinWRestClientFuturesApiTrading(ILogger logger, CoinWRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

    }
}
