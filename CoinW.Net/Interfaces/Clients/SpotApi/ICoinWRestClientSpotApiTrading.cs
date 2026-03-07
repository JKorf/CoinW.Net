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
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/trade/place-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/private
        /// </para>
        /// </summary>
        /// <param name="symbol">["symbol"] The symbol, for example `ETH_USDT`</param>
        /// <param name="side">["type"] Order side</param>
        /// <param name="type">["isMarket"] Order type</param>
        /// <param name="quantity">["amount"] Order quantity</param>
        /// <param name="price">["rate"] Limit price</param>
        /// <param name="quoteQuantity">["funds"] Order quantity in quote asset (only valid for market orders)</param>
        /// <param name="clientOrderId">["out_trade_no"] Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/trade/cancel-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/private
        /// </para>
        /// </summary>
        /// <param name="orderId">["orderNumber"] Order id to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CancelOrderAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/trade/cancel-all-orders" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/private
        /// </para>
        /// </summary>
        /// <param name="symbol">["currencyPair"] Filter by symbol, for example `ETH_USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-current-orders" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/private
        /// </para>
        /// </summary>
        /// <param name="symbol">["currencyPair"] Filter by symbol, for example `ETH_USDT`</param>
        /// <param name="startTime">["startAt"] Filter by start time</param>
        /// <param name="endTime">["endAT"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrder[]>> GetOpenOrdersAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get order details
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-order-details" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/private
        /// </para>
        /// </summary>
        /// <param name="orderId">["orderNumber"] Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderDetails>> GetOrderAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Get user order transaction history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-transaction-history" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/private
        /// </para>
        /// </summary>
        /// <param name="symbol">["currencyPair"] The symbol, for example `ETHUSDT`</param>
        /// <param name="startTime">["startAt"] Filter by start time</param>
        /// <param name="endTime">["endAt"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderTransaction[]>> GetOrderTransactionHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/spot-trading/check/get-historical-orders" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/private
        /// </para>
        /// </summary>
        /// <param name="symbol">["symbol"] The symbol, for example `ETH_USDT`</param>
        /// <param name="fromId">["after"] Filter by id</param>
        /// <param name="toId">["before"] Filter by id</param>
        /// <param name="startTime">["startAt"] Filter by start time</param>
        /// <param name="endTime">["endAt"] Filter by end time</param>
        /// <param name="limit">["limit"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWUserTrade[]>> GetUserTradesAsync(string? symbol = null, long? fromId = null, long? toId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    }
}
