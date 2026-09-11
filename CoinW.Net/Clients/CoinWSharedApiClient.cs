using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;

namespace CoinW.Net.Clients
{
    /// <inheritdoc />
    public class CoinWSharedApiClient : SharedApiClientBase, ICoinWSharedApiClient
    {
        /// <inheritdoc />
        public ICoinWRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public ICoinWRestClientFuturesSharedApi FuturesRest { get; }
        /// <inheritdoc />
        public ICoinWSocketClientSpotSharedApi SpotSocket { get; }
        /// <inheritdoc />
        public ICoinWSocketClientFuturesSharedApi FuturesSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public CoinWSharedApiClient(
            ICoinWRestClient restClient,
            ICoinWSocketClient socketClient,
            IOptions<CoinWOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                  restClient.SpotApi.SharedApi,
                  restClient.FuturesApi.SharedApi,
                  socketClient.SpotApi.SharedApi,
                  socketClient.FuturesApi.SharedApi
                  )
        {
            SpotRest = restClient.SpotApi.SharedApi;
            FuturesRest = restClient.FuturesApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
            FuturesSocket = socketClient.FuturesApi.SharedApi;
        }
    }
}
