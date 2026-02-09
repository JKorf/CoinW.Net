using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace CoinW.Net.Clients
{
    /// <inheritdoc />
    public class CoinWUserClientProvider : ICoinWUserClientProvider
    {
        private ConcurrentDictionary<string, ICoinWRestClient> _restClients = new ConcurrentDictionary<string, ICoinWRestClient>();
        private ConcurrentDictionary<string, ICoinWSocketClient> _socketClients = new ConcurrentDictionary<string, ICoinWSocketClient>();
        
        private readonly IOptions<CoinWRestOptions> _restOptions;
        private readonly IOptions<CoinWSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <inheritdoc />
        public string ExchangeName => CoinWExchange.ExchangeName;

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
        {
            _httpClient = httpClient ?? new HttpClient();
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, ApiCredentials credentials, CoinWEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public ICoinWRestClient GetRestClient(string userIdentifier, ApiCredentials? credentials = null, CoinWEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public ICoinWSocketClient GetSocketClient(string userIdentifier, ApiCredentials? credentials = null, CoinWEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private ICoinWRestClient CreateRestClient(string userIdentifier, ApiCredentials? credentials, CoinWEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new CoinWRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private ICoinWSocketClient CreateSocketClient(string userIdentifier, ApiCredentials? credentials, CoinWEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new CoinWSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IOptions<CoinWRestOptions> SetRestEnvironment(CoinWEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new CoinWRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<CoinWSocketOptions> SetSocketEnvironment(CoinWEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new CoinWSocketOptions();
            var restOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
