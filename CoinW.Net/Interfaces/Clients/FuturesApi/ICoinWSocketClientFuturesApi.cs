using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using CoinW.Net.Objects.Models;
using CoinW.Net.Enums;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// CoinW Futures streams
    /// </summary>
    public interface ICoinWSocketClientFuturesApi : ISocketApiClient<CoinWCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-24h-trade-summary" /><br />
        /// Endpoint:<br />
        /// ticker_swap
        /// </para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesTickerUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-order-book" /><br />
        /// Endpoint:<br />
        /// depth
        /// </para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-trade-data" /><br />
        /// Endpoint:<br />
        /// fills
        /// </para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-k-line-utc+0" /><br />
        /// Endpoint:<br />
        /// candles_swap_utc
        /// </para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, FuturesKlineIntervalStream interval, Action<DataEvent<CoinWFuturesStreamKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to index price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-index-price" /><br />
        /// Endpoint:<br />
        /// index_price
        /// </para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(string symbol, Action<DataEvent<CoinWPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to mark price updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-mark-price" /><br />
        /// Endpoint:<br />
        /// mark_price
        /// </para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<CoinWPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to funding rate updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/market/subscribe-funding-fee-rate" /><br />
        /// Endpoint:<br />
        /// funding_rate
        /// </para>
        /// </summary>
        /// <param name="symbol">The futures symbol, for example `ETH`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToFundingRateUpdatesAsync(string symbol, Action<DataEvent<CoinWFundingRate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/subscribe-current-orders" /><br />
        /// Endpoint:<br />
        /// order
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<CoinWFuturesOrder[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user position updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/position/subscribe-position" /><br />
        /// Endpoint:<br />
        /// position
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(Action<DataEvent<CoinWPosition[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user position detail updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/position/subscribe-position-change" /><br />
        /// Endpoint:<br />
        /// position_change
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPositionDetailUpdatesAsync(Action<DataEvent<CoinWPositionChange[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balance updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/subscribe-assets" /><br />
        /// Endpoint:<br />
        /// assets
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<CoinWFuturesBalanceUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to margin configuration updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/subscribe-margin-mode" /><br />
        /// Endpoint:<br />
        /// user_setting
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarginConfigUpdatesAsync(Action<DataEvent<CoinWMarginInfo[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public ICoinWSocketClientFuturesApiShared SharedClient { get; }
    }
}
