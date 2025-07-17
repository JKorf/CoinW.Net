using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Objects.Options;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Clients.SpotApi;

namespace CoinW.Net.Clients
{
    /// <inheritdoc cref="ICoinWRestClient" />
    public class CoinWRestClient : BaseRestClient, ICoinWRestClient
    {
        #region Api clients
                
         /// <inheritdoc />
        public ICoinWRestClientFuturesApi FuturesApi { get; }

         /// <inheritdoc />
        public ICoinWRestClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the CoinWRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public CoinWRestClient(Action<CoinWRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the CoinWRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public CoinWRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<CoinWRestOptions> options) : base(loggerFactory, "CoinW")
        {
            Initialize(options.Value);
                        
            FuturesApi = AddApiClient(new CoinWRestClientFuturesApi(_logger, httpClient, options.Value));
            SpotApi = AddApiClient(new CoinWRestClientSpotApi(this, _logger, httpClient, options.Value));
        }

        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            SpotApi.SetOptions(options);
            FuturesApi.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<CoinWRestOptions> optionsDelegate)
        {
            CoinWRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            FuturesApi.SetApiCredentials(credentials);
            SpotApi.SetApiCredentials(credentials);
        }
    }
}
