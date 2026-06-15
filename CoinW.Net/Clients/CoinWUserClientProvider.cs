using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace CoinW.Net.Clients
{
    /// <inheritdoc />
    public class CoinWUserClientProvider : UserClientProvider<
        ICoinWRestClient,
        ICoinWSocketClient,
        CoinWRestOptions,
        CoinWSocketOptions,
        CoinWCredentials,
        CoinWEnvironment
        >, ICoinWUserClientProvider
    {
        
        /// <inheritdoc />
        public override string ExchangeName => CoinWExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public CoinWUserClientProvider(Action<CoinWOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }
        
        /// <summary>
        /// ctor
        /// </summary>
        public CoinWUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<CoinWRestOptions> restOptions,
            IOptions<CoinWSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override ICoinWRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<CoinWRestOptions> options) 
            => new CoinWRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override ICoinWSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<CoinWSocketOptions> options)
            => new CoinWSocketClient(options, loggerFactory);
    }
}
