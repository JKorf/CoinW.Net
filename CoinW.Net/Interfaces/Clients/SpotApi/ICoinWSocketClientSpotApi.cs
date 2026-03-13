using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using CoinW.Net.Objects.Models;
using CoinW.Net.Enums;
using CryptoExchange.Net.Interfaces.Clients;

namespace CoinW.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// CoinW Spot streams
    /// </summary>
    public interface ICoinWSocketClientSpotApi : ISocketApiClient<CoinWCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to ticker updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-24h-trade-summary" /><br />
        /// Endpoint:<br />
        /// ticker
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<CoinWTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-24h-trades-summary-for-all-instruments" /><br />
        /// Endpoint:<br />
        /// ticker_all
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<CoinWSymbolUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to incremental order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-incremental-order-book" /><br />
        /// Endpoint:<br />
        /// depth
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to snapshot order book updates for the top rows
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-incremental-order-book" /><br />
        /// Endpoint:<br />
        /// depth_snapshot
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to Kline updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-k-line" /><br />
        /// Endpoint:<br />
        /// candles
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineIntervalStream interval, Action<DataEvent<CoinWKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-trades" /><br />
        /// Endpoint:<br />
        /// fills
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<CoinWTradeUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balance updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/account/subscribe-assets" /><br />
        /// Endpoint:<br />
        /// assets
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<CoinWBalanceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/check/subscribe-current-orders" /><br />
        /// Endpoint:<br />
        /// order
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<CoinWOrderUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ICoinWSocketClientSpotApiShared SharedClient { get; }
    }
}
