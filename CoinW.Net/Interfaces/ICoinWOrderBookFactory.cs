using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using CoinW.Net.Objects.Options;

namespace CoinW.Net.Interfaces
{
    /// <summary>
    /// CoinW local order book factory
    /// </summary>
    public interface ICoinWOrderBookFactory : IExchangeService
    {
        
        /// <summary>
        /// Futures order book factory methods
        /// </summary>
        IOrderBookFactory<CoinWOrderBookOptions> Futures { get; }

        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        IOrderBookFactory<CoinWOrderBookOptions> Spot { get; }


        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<CoinWOrderBookOptions>? options = null);

        
        /// <summary>
        /// Create a new Futures local order book instance
        /// </summary>
        ISymbolOrderBook CreateFutures(string symbol, Action<CoinWOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new Spot local order book instance
        /// </summary>
        ISymbolOrderBook CreateSpot(string symbol, Action<CoinWOrderBookOptions>? options = null);

    }
}