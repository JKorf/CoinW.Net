using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;

namespace CoinW.Net.Clients
{
    /// <inheritdoc />
    public class CoinWSharedApiClient : ICoinWSharedApiClient
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
            ICoinWSocketClient socketClient)
        {
            SpotRest = restClient.SpotApi.SharedApi;
            FuturesRest = restClient.FuturesApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
            FuturesSocket = socketClient.FuturesApi.SharedApi;
        }
    }
}
