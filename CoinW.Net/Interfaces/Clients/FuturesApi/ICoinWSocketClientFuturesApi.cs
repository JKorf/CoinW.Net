using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using CoinW.Net.Objects.Models;
using CoinW.Net.Enums;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// CoinW Futures streams
    /// </summary>
    public interface ICoinWSocketClientFuturesApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to ticker updates
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-24h-trade-summary" /></para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-order-book" /></para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-trade-data" /></para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-k-line-utc+0" /></para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, FuturesKlineIntervalStream interval, Action<DataEvent<CoinWFuturesStreamKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to index price updates
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-index-price" /></para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(string symbol, Action<DataEvent<CoinWPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to mark price updates
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-mark-price" /></para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<CoinWPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to funding rate updates
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-funding-fee-rate" /></para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToFundingRateUpdatesAsync(string symbol, Action<DataEvent<CoinWFundingRate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<CoinWFuturesOrder[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ICoinWSocketClientFuturesApiShared SharedClient { get; }
    }
}
