using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using CoinW.Net.Objects.Models;
using CoinW.Net.Enums;

namespace CoinW.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// CoinW Spot streams
    /// </summary>
    public interface ICoinWSocketClientSpotApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to ticker updates for a symbol
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-24h-trade-summary" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<CoinWTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates for all symbols
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-24h-trades-summary-for-all-instruments" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<CoinWSymbolUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to incremental order book updates
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-incremental-order-book" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to snapshot order book updates for the top rows
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-incremental-order-book" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to Kline updates
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-k-line" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineIntervalStream interval, Action<DataEvent<CoinWKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/market/subscribe-trades" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<CoinWTradeUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balance updates
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/subscribe-assets" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<CoinWBalanceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order updates
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/check/subscribe-current-orders" /></para>
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
