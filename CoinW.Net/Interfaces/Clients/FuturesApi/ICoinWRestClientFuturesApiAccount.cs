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
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/common/get-leverage-information" /></para>
        /// </summary>
        /// <param name="positionId">Position id, either this or orderId should be provided</param>
        /// <param name="orderId">Order id, either this or positionId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetLeverageAsync(long? positionId = null, long? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get margin rate for an open position
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/position/get-position-margin-rate" /></para>
        /// </summary>
        /// <param name="positionId">Required for isolated positions, leave empty for cross margin</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetMarginRateAsync(long positionId, CancellationToken ct = default);

        /// <summary>
        /// Get max buy/sell size
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/position/get-max-contract-size" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="leverage">Leverage</param>
        /// <param name="marginType">Margin type</param>
        /// <param name="orderPrice">Order price to use for calculation</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWMaxTrade>> GetMaxTradeSizeAsync(string symbol, int leverage, MarginType marginType, decimal orderPrice, CancellationToken ct = default);

        /// <summary>
        /// Get max transferable quantity
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-max-transferable-balance" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWValue>> GetMaxTransferableAsync(CancellationToken ct = default);

        /// <summary>
        /// Get balances
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-futures-account-assets" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFuturesBalance>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trading fees
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-futures-account-fees" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWFees>> GetFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get margin configuration
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-margin-mode" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWMarginInfo>> GetMarginModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Set margin mode
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/assets/set-margin-mode" /></para>
        /// </summary>
        /// <param name="marginType">Margin type</param>
        /// <param name="positionCombineType">Position combine type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> SetMarginModeAsync(MarginType marginType, PositionCombineType positionCombineType, CancellationToken ct = default);

        /// <summary>
        /// Enable or disable mega coupon usage
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/assets/enable-disable-mega-coupon" /></para>
        /// </summary>
        /// <param name="enabled">Whether to use mega coupon</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> ToggleMegaCouponAsync(bool enabled, CancellationToken ct = default);

        /// <summary>
        /// Get max position size
        /// <para><a href="https://www.coinw.com/api-doc/en/futures-trading/assets/get-max-users-contract-size" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWMaxPosition>> GetMaxPositionSizeAsync(string symbol, CancellationToken ct = default);

    }
}
