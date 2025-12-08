using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;

namespace CoinW.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the CoinW websocket API
    /// </summary>
    public interface ICoinWSocketClient : ISocketClient
    {        
        /// <summary>
        /// Futures API endpoints
        /// </summary>
        /// <see cref="ICoinWSocketClientFuturesApi"/>
        public ICoinWSocketClientFuturesApi FuturesApi { get; }

        /// <summary>
        /// Spot API endpoints
        /// </summary>
        /// <see cref="ICoinWSocketClientSpotApi"/>
        public ICoinWSocketClientSpotApi SpotApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
