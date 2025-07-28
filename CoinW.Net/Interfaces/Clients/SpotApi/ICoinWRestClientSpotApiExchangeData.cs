using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoinW.Net.Enums;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace CoinW.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// CoinW Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface ICoinWRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get ticker info
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-24h-trade-summary" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWTicker[]>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get asset and network info
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-coin-deposits-withdrawal-limits" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWAsset[]>> GetAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get symbol information
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-trading-pair-information" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWSymbol[]>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order book snapshot
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-order-book" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH_USDT`</param>
        /// <param name="limit">Number of rows, either 5 or 20</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the most recent trades for a symbol
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-recent-trades" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH_USDT`</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWTrade[]>> GetRecentTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline data
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-k-line" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH_USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWKline[]>> GetKlinesAsync( string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

    }
}
