using CryptoExchange.Net.Interfaces;
using System;

namespace CoinW.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// CoinW Spot API endpoints
    /// </summary>
    public interface ICoinWRestClientSpotApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="ICoinWRestClientSpotApiAccount" />
        public ICoinWRestClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="ICoinWRestClientSpotApiExchangeData" />
        public ICoinWRestClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="ICoinWRestClientSpotApiTrading" />
        public ICoinWRestClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ICoinWRestClientSpotApiShared SharedClient { get; }
    }
}
