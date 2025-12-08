using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// CoinW Futures API endpoints
    /// </summary>
    public interface ICoinWRestClientFuturesApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="ICoinWRestClientFuturesApiAccount" />
        public ICoinWRestClientFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="ICoinWRestClientFuturesApiExchangeData" />
        public ICoinWRestClientFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="ICoinWRestClientFuturesApiTrading" />
        public ICoinWRestClientFuturesApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ICoinWRestClientFuturesApiShared SharedClient { get; }
    }
}
