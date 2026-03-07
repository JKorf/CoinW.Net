using System;
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-24h-trade-summary" /><br />
        /// Endpoint:<br />
        /// GET api/v1/public
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWTicker[]>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get asset and network info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-coin-deposits-withdrawal-limits" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/public
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWAsset[]>> GetAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get symbol information
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-trading-pair-information" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/public
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWSymbol[]>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order book snapshot
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-order-book" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/public
        /// </para>
        /// </summary>
        /// <param name="symbol">["symbol"] The symbol, for example `ETH_USDT`</param>
        /// <param name="limit">["size"] Number of rows, either 5 or 20</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the most recent trades for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-recent-trades" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/public
        /// </para>
        /// </summary>
        /// <param name="symbol">["symbol"] The symbol, for example `ETH_USDT`</param>
        /// <param name="startTime">["start"] Filter by start time</param>
        /// <param name="endTime">["end"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWTrade[]>> GetRecentTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline data
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/get-k-line" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/public
        /// </para>
        /// </summary>
        /// <param name="symbol">["currencyPair"] The symbol, for example `ETH_USDT`</param>
        /// <param name="interval">["period"] Kline interval</param>
        /// <param name="startTime">["start"] Filter by start time</param>
        /// <param name="endTime">["end"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWKline[]>> GetKlinesAsync( string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

    }
}
