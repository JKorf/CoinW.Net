using System.Threading.Tasks;
using System.Threading;
using CoinW.Net.Enums;
using CryptoExchange.Net.Objects;
using CoinW.Net.Objects.Models;
using System;

namespace CoinW.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// CoinW Spot trading endpoints, placing and managing orders.
    /// </summary>
    public interface ICoinWRestClientSpotApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/trade/place-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH_USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="type">Order type</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="price">Limit price</param>
        /// <param name="quoteQuantity">Order quantity in quote asset (only valid for market orders)</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/trade/cancel-order" /></para>
        /// </summary>
        /// <param name="orderId">Order id to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CancelOrderAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/trade/cancel-all-orders" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH_USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-current-orders" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH_USDT`</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrder[]>> GetOpenOrdersAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get order details
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-order-details" /></para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderDetails>> GetOrderAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Get user order transaction history
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-transaction-history" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderTransaction[]>> GetOrderTransactionHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trades
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-historical-orders" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH_USDT`</param>
        /// <param name="fromId">Filter by id</param>
        /// <param name="toId">Filter by id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWUserTrade[]>> GetUserTradesAsync(string? symbol = null, long? fromId = null, long? toId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    }
}
