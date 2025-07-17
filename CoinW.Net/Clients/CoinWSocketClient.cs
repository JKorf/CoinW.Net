using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Options;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Clients.SpotApi;
using CoinW.Net.Clients.FuturesApi;

namespace CoinW.Net.Clients
{
    /// <inheritdoc cref="ICoinWSocketClient" />
    public class CoinWSocketClient : BaseSocketClient, ICoinWSocketClient
    {
        #region fields
        #endregion

        #region Api clients
        
         /// <inheritdoc />
        public ICoinWSocketClientFuturesApi FuturesApi { get; }

         /// <inheritdoc />
        public ICoinWSocketClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of CoinWSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public CoinWSocketClient(Action<CoinWSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of CoinWSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public CoinWSocketClient(IOptions<CoinWSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "CoinW")
        {
            Initialize(options.Value);
            
            FuturesApi = AddApiClient(new CoinWSocketClientFuturesApi(_logger, options.Value));
            SpotApi = AddApiClient(new CoinWSocketClientSpotApi(_logger, options.Value));
        }
        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            FuturesApi.SetOptions(options);
            SpotApi.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<CoinWSocketOptions> optionsDelegate)
        {
            CoinWSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {            
            FuturesApi.SetApiCredentials(credentials);
            SpotApi.SetApiCredentials(credentials);
        }
    }
}
