using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using CoinW.Net.Interfaces;
using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Options;

namespace CoinW.Net.SymbolOrderBooks
{
    /// <summary>
    /// CoinW order book factory
    /// </summary>
    public class CoinWOrderBookFactory : ICoinWOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public CoinWOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;            
            
            Futures = new OrderBookFactory<CoinWOrderBookOptions>(CreateFutures, Create);
            Spot = new OrderBookFactory<CoinWOrderBookOptions>(CreateSpot, Create);
        }
                
         /// <inheritdoc />
        public IOrderBookFactory<CoinWOrderBookOptions> Futures { get; }

         /// <inheritdoc />
        public IOrderBookFactory<CoinWOrderBookOptions> Spot { get; }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<CoinWOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(CoinWExchange.FormatSymbol);
            if (symbol.TradingMode == TradingMode.Spot)
                return CreateSpot(symbolName, options);

            return CreateFutures(symbolName, options);
        }

        
         /// <inheritdoc />
        public ISymbolOrderBook CreateFutures(string symbol, Action<CoinWOrderBookOptions>? options = null)
            => new CoinWFuturesSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<ICoinWRestClient>(),
                                                          _serviceProvider.GetRequiredService<ICoinWSocketClient>());

         /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<CoinWOrderBookOptions>? options = null)
            => new CoinWSpotSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<ICoinWRestClient>(),
                                                          _serviceProvider.GetRequiredService<ICoinWSocketClient>());


    }
}
