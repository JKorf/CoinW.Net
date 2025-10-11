using System;
using System.Threading;
using System.Threading.Tasks;
using CoinW.Net.Enums;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// CoinW Futures exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface ICoinWRestClientFuturesApiExchangeData
    {
        /// <summary>
        /// Get symbols/instruments
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-instrument-information" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol name, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-last-trade-summary-of-an-instrument" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-last-trade-summary-of-all-instruments" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesTicker[]>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get kline info
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-k-line-of-an-instrument" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesKline[]>> GetKlinesAsync(
            string symbol,
            FuturesKlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get last funding rate
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-last-settelment-funding-fee-rate" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetLastFundingRateAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get order book
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-order-book-of-an-instrument" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get recent trades
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-trade-data-of-an-instrument" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesTrade[]>> GetRecentTradesAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get margin requirements
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-margin-requirements-of-all-instruments" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWMarginRequirement[]>> GetMarginRequirementsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trade history
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/get-historical-public-trades" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size, max 500</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWTradeHistory>> GetTradeHistoryAsync(string symbol, int? page = null, int? pageSize = null, CancellationToken ct = default);

    }
}
