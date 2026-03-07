using CoinW.Net.Enums;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// CoinW Futures trading endpoints, placing and managing orders.
    /// </summary>
    public interface ICoinWRestClientFuturesApiTrading
    {
        /// <summary>
        /// Place a new order. Note that this can not be used to place a direct order to close an open position. Use <see cref="ClosePositionAsync(long, FuturesOrderType?, decimal?, decimal?, decimal?, CancellationToken)" /> to close a position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/place-an-order" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="side">["direction"] Position side</param>
        /// <param name="orderType">["positionType"] Order type</param>
        /// <param name="quantity">["quantity"] Quantity</param>
        /// <param name="leverage">["leverage"] Leverage</param>
        /// <param name="price">["openPrice"] Order price</param>
        /// <param name="quantityUnit">["quantityUnit"] Type of quantity. Defaults to contracts</param>
        /// <param name="marginType">["positionModel"] Margin type. Defaults to isolated margin</param>
        /// <param name="stopLossPrice">["stopLossPrice"] Stop loss price</param>
        /// <param name="takeProfitPrice">["stopProfitPrice"] Take profit price</param>
        /// <param name="triggerPrice">["triggerPrice"] Trigger price</param>
        /// <param name="triggerOrderType">["triggerType"] Trigger order type</param>
        /// <param name="goldenId">["goldId"] Golden Id</param>
        /// <param name="clientOrderId">["thirdOrderId"] Client order id</param>
        /// <param name="useMegaCoupon">["useAlmightyGold"] Use mega coupon</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderId>> PlaceOrderAsync(string symbol, PositionSide side, FuturesOrderType orderType, decimal quantity, int leverage, decimal? price = null, QuantityUnit? quantityUnit = null, MarginType? marginType = null, decimal? stopLossPrice = null, decimal? takeProfitPrice = null, decimal? triggerPrice = null, TriggerOrderType? triggerOrderType = null, int? goldenId = null, string? clientOrderId = null, bool? useMegaCoupon = null, CancellationToken ct = default);

        /// <summary>
        /// Place multiple orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/place-batch-orders" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/batchOrders
        /// </para>
        /// </summary>
        /// <param name="requests">Orders to place</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<CoinWBatchResult>[]>> PlaceMultipleOrdersAsync(IEnumerable<CoinWFuturesOrderRequest> requests, CancellationToken ct = default);

        /// <summary>
        /// Close an open position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/close-a-position" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/perpum/positions
        /// </para>
        /// </summary>
        /// <param name="positionId">["id"] Position id</param>
        /// <param name="orderType">["positionType"] Order type, defaults to market order</param>
        /// <param name="quantityToClose">["closeNum"] Quantity in contracts to close. Either this or factorToClose should be provided</param>
        /// <param name="factorToClose">["closeRate"] Factor from 0 to 1 to close. For example 0.5 would close 50% of the position. Either this or quantityToClose should be provided</param>
        /// <param name="price">["orderPrice"] Limit order price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderId>> ClosePositionAsync(long positionId, FuturesOrderType? orderType = null, decimal? quantityToClose = null, decimal? factorToClose = null, decimal? price = null, CancellationToken ct = default);

        /// <summary>
        /// Close positions by client order ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/close-batch-positions" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/perpum/batchClose
        /// </para>
        /// </summary>
        /// <param name="clientOrderIds">Client order ids to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWBatchResult[]>> ClosePositionsByClientOrderIdAsync(IEnumerable<string> clientOrderIds, CancellationToken ct = default);

        /// <summary>
        /// Close all open position for a symbol at market price
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/close-positions-at-market-price" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/perpum/allpositions
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CloseAllPositionsAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Reverse a position by closing the existing position and opening an equal position on the opposite side
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/reverse-a-position" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/positions/reverse
        /// </para>
        /// </summary>
        /// <param name="positionId">["id"] Id of position to reverse</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWOrderId>> ReversePositionAsync(long positionId, CancellationToken ct = default);

        /// <summary>
        /// Adjust margin for an open position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/adjust-margin" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/positions/margin
        /// </para>
        /// </summary>
        /// <param name="positionId">["id"] Position id</param>
        /// <param name="addMargin">["addMargin"] Margin to add</param>
        /// <param name="reduceMargin">["reduceMargin"] Margin to reduce</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> AdjustMarginAsync(long positionId, decimal addMargin, decimal reduceMargin, CancellationToken ct = default);

        /// <summary>
        /// Set take profit / stop loss for an order or position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/set-stop-loss-take-profit" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/TPSL
        /// </para>
        /// </summary>
        /// <param name="orderOrPositionId">["id"] Order id for an open order or position id for an open position</param>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="takeProfitPrice">["stopProfitPrice"] Take profit price</param>
        /// <param name="takeProfitOrderPrice">["stopProfitOrderPrice"] Take profit order price</param>
        /// <param name="takeProfitRate">["stopProfitRate"] Take profit rate</param>
        /// <param name="stopLossPrice">["stopLossPrice"] Stop loss price</param>
        /// <param name="stopLossOrderPrice">["stopLossOrderPrice"] Stop loss order price</param>
        /// <param name="stopLossRate">["stopLossRate"] Stop loss rate</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> SetTpSlAsync(long orderOrPositionId, string symbol, decimal? takeProfitPrice = null, decimal? takeProfitOrderPrice = null, decimal? takeProfitRate = null, decimal? stopLossPrice = null, decimal? stopLossOrderPrice = null, decimal? stopLossRate = null, CancellationToken ct = default);

        /// <summary>
        /// Set trailing take profit / stop loss for a position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/set-trailing-stop-loss-take-profit" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/moveTPSL
        /// </para>
        /// </summary>
        /// <param name="positionId">["openId"] Position id</param>
        /// <param name="callbackRate">["callbackRate"] Callback rate, with a valid range from 0 to 1. For example: 0.5 represents a 50% callback rate</param>
        /// <param name="triggerPrice">["triggerPrice"] Trigger price</param>
        /// <param name="quantity">["quantity"] Quantity</param>
        /// <param name="quantityType">["quantityUnit"] Quantity type, defaults to contracts</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> SetTrailingTpSlAsync(long positionId, decimal callbackRate, decimal? triggerPrice = null, decimal? quantity = null, QuantityUnit? quantityType = null, CancellationToken ct = default);

        /// <summary>
        /// Edit an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/modify-an-order" /><br />
        /// Endpoint:<br />
        /// PUT /v1/perpum/order
        /// </para>
        /// </summary>
        /// <param name="orderId">Id of the order to edit</param>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="side">["direction"] Position side</param>
        /// <param name="orderType">["positionType"] Order type</param>
        /// <param name="quantity">["quantity"] Quantity</param>
        /// <param name="leverage">["leverage"] Leverage</param>
        /// <param name="price">["openPrice"] Order price</param>
        /// <param name="quantityUnit">["quantityUnit"] Type of quantity. Defaults to contracts</param>
        /// <param name="marginType">["positionModel"] Margin type. Defaults to isolated margin</param>
        /// <param name="stopLossPrice">["stopLossPrice"] Stop loss price</param>
        /// <param name="takeProfitPrice">["stopProfitPrice"] Take profit price</param>
        /// <param name="triggerPrice">["triggerPrice"] Trigger price</param>
        /// <param name="triggerOrderType">["triggerType"] Trigger order type</param>
        /// <param name="goldenId">["goldId"] Golden Id</param>
        /// <param name="clientOrderId">["thirdOrderId"] Client order id</param>
        /// <param name="useMegaCoupon">["useAlmightyGold"] Use mega coupon</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWEditResult>> EditOrderAsync(long orderId, string symbol, PositionSide side, FuturesOrderType orderType, decimal quantity, int leverage, decimal? price = null, QuantityUnit? quantityUnit = null, MarginType? marginType = null, decimal? stopLossPrice = null, decimal? takeProfitPrice = null, decimal? triggerPrice = null, TriggerOrderType? triggerOrderType = null, int? goldenId = null, string? clientOrderId = null, bool? useMegaCoupon = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/cancel-an-order" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/perpum/order
        /// </para>
        /// </summary>
        /// <param name="orderId">["id"] Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CancelOrderAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders by order id
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/trade/cancel-batch-orders" /><br />
        /// Endpoint:<br />
        /// DELETE /v1/perpum/batchOrders
        /// </para>
        /// </summary>
        /// <param name="orderIds">["sourceIds"] Order ids to cancel, max 20</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CancelOrdersAsync(IEnumerable<long> orderIds, CancellationToken ct = default);

        /// <summary>
        /// Get open orders for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/get-current-orders" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/open
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="orderType">["positionType"] Order type</param>
        /// <param name="page">["page"] Page number</param>
        /// <param name="pageSize">["pageSize"] Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesOrderPage>> GetOpenOrdersAsync(string symbol, FuturesOrderType orderType, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Get open orders for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/get-order-information" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/order
        /// </para>
        /// </summary>
        /// <param name="orderType">["positionType"] Order type</param>
        /// <param name="symbol">["instrument"] Filter by symbol, for example `ETH`</param>
        /// <param name="orderIds">["sourceIds"] Filter by order ids</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesOrder[]>> GetOpenOrdersAsync(FuturesOrderType orderType, string? symbol = null, IEnumerable<long>? orderIds = null, CancellationToken ct = default);

        /// <summary>
        /// Get the total number of open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/get-pending-order-count" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/openQuantity
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetOpenOrderCountAsync(CancellationToken ct = default);

        /// <summary>
        /// Get take profit / stop loss info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/get-stop-loss-take-profit-information" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/TPSL
        /// </para>
        /// </summary>
        /// <param name="orderId">["orderId"] Order id</param>
        /// <param name="positionId">["openId"] Position id</param>
        /// <param name="planOrderId">["planOrderId"] Plan order id</param>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWTpSl[]>> GetTpSlAsync(long? orderId = null, long? positionId = null, long? planOrderId = null, string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get trailing take profit / stop loss info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/get-trailing-stop-loss-take-profit-information" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/moveTPSL
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWTrailingTpSl[]>> GetTrailingTpSlAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order history of the last 7 days. Does not return fully canceled orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/get-historical-orders-7days" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/history
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="orderType">["originType"] Order type filed</param>
        /// <param name="page">["page"] Page number</param>
        /// <param name="pageSize">["pageSize"] Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWHistOrderPage>> GetOrderHistory7DaysAsync(string? symbol = null, FuturesOrderType? orderType = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Get order history of the last 3 months. Does not return fully canceled orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/check/get-historical-orders-7days" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/archive
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="orderType">["originType"] Order type filed</param>
        /// <param name="page">["page"] Page number</param>
        /// <param name="pageSize">["pageSize"] Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWHistOrderPage>> GetOrderHistory3MonthsAsync(string? symbol = null, FuturesOrderType? orderType = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Get open positions
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/position/get-current-position-information" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/positions
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWPosition[]>> GetPositionsAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get position history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/position/get-historical-position-information" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/positions/history
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="marginType">["positionModel"] Margin type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWPositionHistoryPage>> GetPositionHistoryAsync(string? symbol = null, MarginType? marginType = null, CancellationToken ct = default);

        /// <summary>
        /// Get all open positions
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/position/get-current-positions" /><br />
        /// Endpoint:<br />
        /// GET v1/perpum/positions/all
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWPosition[]>> GetPositionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get transaction history for the last 3 days
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-transaction-details-3days" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/deals
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="orderType">["originType"] Filter by order type</param>
        /// <param name="marginType">["positionModel"] Filter by margin type</param>
        /// <param name="page">["page"] page number</param>
        /// <param name="pageSize">["pageSize"] Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesTransactionPage>> GetTransactionHistory3DaysAsync(string symbol, OrderType? orderType = null, MarginType? marginType = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Get transaction history for the last 3 months
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-transaction-details-3months" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/deals/history
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="orderType">["originType"] Filter by order type</param>
        /// <param name="marginType">["positionModel"] Filter by margin type</param>
        /// <param name="page">["page"] page number</param>
        /// <param name="pageSize">["pageSize"] Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesTransactionPage>> GetTransactionHistory3MonthsAsync(string symbol, OrderType? orderType = null, MarginType? marginType = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

    }
}
