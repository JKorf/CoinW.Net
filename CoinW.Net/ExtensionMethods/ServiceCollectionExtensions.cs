using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using CoinW.Net;
using CoinW.Net.Clients;
using CoinW.Net.Interfaces;
using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Options;
using CoinW.Net.SymbolOrderBooks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the ICoinWRestClient and ICoinWSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddCoinW(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new CoinWOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            configuration.Bind(options);

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? CoinWEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? CoinWEnvironment.Live.Name;
            options.Rest.Environment = CoinWEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = CoinWEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddCoinWCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the ICoinWRestClient and ICoinWSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the CoinW services</param>
        /// <returns></returns>
        public static IServiceCollection AddCoinW(
            this IServiceCollection services,
            Action<CoinWOptions>? optionsDelegate = null)
        {
            var options = new CoinWOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? CoinWEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? CoinWEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddCoinWCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddCoinWCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<ICoinWRestClient, CoinWRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<CoinWRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new CoinWRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<CoinWRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<CoinWRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options.Proxy, options.HttpKeepAliveInterval);
            });
            services.Add(new ServiceDescriptor(typeof(ICoinWSocketClient), x => { return new CoinWSocketClient(x.GetRequiredService<IOptions<CoinWSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddSingleton<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<ICoinWOrderBookFactory, CoinWOrderBookFactory>();
            services.AddTransient<ICoinWTrackerFactory, CoinWTrackerFactory>();
            services.AddTransient<ITrackerFactory, CoinWTrackerFactory>();
            services.AddSingleton<ICoinWUserClientProvider, CoinWUserClientProvider>(x =>
                new CoinWUserClientProvider(
                    x.GetRequiredService<HttpClient>(),
                    x.GetRequiredService<ILoggerFactory>(),
                    x.GetRequiredService<IOptions<CoinWRestOptions>>(),
                    x.GetRequiredService<IOptions<CoinWSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<ICoinWRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<ICoinWSocketClient>().SpotApi.SharedClient);
            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<ICoinWRestClient>().FuturesApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<ICoinWSocketClient>().FuturesApi.SharedClient);

            return services;
        }
    }
}
