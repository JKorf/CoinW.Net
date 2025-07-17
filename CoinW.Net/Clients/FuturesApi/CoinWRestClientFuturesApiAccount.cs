using CryptoExchange.Net.Objects;
using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.FuturesApi;

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
    }
}
