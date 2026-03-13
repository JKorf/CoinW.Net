using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;

namespace CoinW.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the CoinW Rest API. 
    /// </summary>
    public interface ICoinWRestClient : IRestClient<CoinWCredentials>
    {
        /// <summary>
        /// Futures API endpoints
        /// </summary>
        /// <see cref="ICoinWRestClientFuturesApi"/>
        public ICoinWRestClientFuturesApi FuturesApi { get; }

        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="ICoinWRestClientSpotApi"/>
        public ICoinWRestClientSpotApi SpotApi { get; }
    }
}
