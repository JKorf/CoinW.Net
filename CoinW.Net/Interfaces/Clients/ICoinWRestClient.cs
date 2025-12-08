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
    public interface ICoinWRestClient : IRestClient
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

        /// <summary>
        /// Update specific options
        /// </summary>
        /// <param name="options">Options to update. Only specific options are changeable after the client has been created</param>
        void SetOptions(UpdateOptions options);

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
