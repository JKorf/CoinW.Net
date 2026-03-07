using System.Threading.Tasks;
using System.Threading;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CoinW.Net.Enums;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// CoinW Futures account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface ICoinWRestClientFuturesApiAccount
    {
        /// <summary>
        /// Get leverage for a position or order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/common/get-leverage-information" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/positions/leverage
        /// </para>
        /// </summary>
        /// <param name="positionId">["positionId"] Position id, either this or orderId should be provided</param>
        /// <param name="orderId">["orderId"] Order id, either this or positionId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetLeverageAsync(long? positionId = null, long? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get margin rate for an open position
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/position/get-position-margin-rate" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/positions/marginRate
        /// </para>
        /// </summary>
        /// <param name="positionId">["positionId"] Required for isolated positions, leave empty for cross margin</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetMarginRateAsync(long positionId, CancellationToken ct = default);

        /// <summary>
        /// Get max buy/sell size
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/position/get-max-contract-size" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/maxSize
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="leverage">["leverage"] Leverage</param>
        /// <param name="marginType">["positionModel"] Margin type</param>
        /// <param name="orderPrice">["orderPrice"] Order price to use for calculation</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWMaxTrade>> GetMaxTradeSizeAsync(string symbol, int leverage, MarginType marginType, decimal orderPrice, CancellationToken ct = default);

        /// <summary>
        /// Get max transferable quantity
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-max-transferable-balance" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/account/available
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetMaxTransferableAsync(CancellationToken ct = default);

        /// <summary>
        /// Get balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-futures-account-assets" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/account/getUserAssets
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesBalance>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trading fees
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-futures-account-fees" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/account/fees
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFees>> GetFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get margin configuration
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-margin-mode" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/positions/type
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWMarginInfo>> GetMarginModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Set margin mode
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/set-margin-mode" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/positions/type
        /// </para>
        /// </summary>
        /// <param name="marginType">["positionModel"] Margin type</param>
        /// <param name="positionCombineType">["layout"] Position combine type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> SetMarginModeAsync(MarginType marginType, PositionCombineType positionCombineType, CancellationToken ct = default);

        /// <summary>
        /// Enable or disable mega coupon usage
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/enable-disable-mega-coupon" /><br />
        /// Endpoint:<br />
        /// POST /v1/perpum/account/almightyGoldInfo
        /// </para>
        /// </summary>
        /// <param name="enabled">["status"] Whether to use mega coupon</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> ToggleMegaCouponAsync(bool enabled, CancellationToken ct = default);

        /// <summary>
        /// Get max position size
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-max-users-contract-size" /><br />
        /// Endpoint:<br />
        /// GET /v1/perpum/orders/availSize
        /// </para>
        /// </summary>
        /// <param name="symbol">["instrument"] The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWMaxPosition>> GetMaxPositionSizeAsync(string symbol, CancellationToken ct = default);

    }
}
