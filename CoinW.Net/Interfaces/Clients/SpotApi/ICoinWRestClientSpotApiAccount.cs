using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using CoinW.Net.Objects.Models;
using CoinW.Net.Enums;

namespace CoinW.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// CoinW Spot account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface ICoinWRestClientSpotApiAccount
    {
        /// <summary>
        /// Get account balances
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/get-spot-account-balance" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account balances with details
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/get-total-spot-account-balance" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<Dictionary<string, CoinWBalance>>> GetBalancesDetailsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get deposit/withdrawal history
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/get-deposit-withdrawal-history" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="id">Filter by id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWDepositWithdrawal[]>> GetDepositWithdrawalHistoryAsync(string asset, long? id = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit/withdrawal history
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/get-deposit-withdrawal-history" /></para>
        /// </summary>
        /// <param name="assets">Filter by assets, max 20 per request</param>
        /// <param name="id">Filter by id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWDepositWithdrawal[]>> GetDepositWithdrawalHistoryAsync(IEnumerable<string> assets, long? id = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit addresses for an asset
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/get-deposit-withdrawal-address" /></para>
        /// </summary>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="network">The network</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWDepositAddress[]>> GetDepositAddressesAsync(string asset, string network, CancellationToken ct = default);

        /// <summary>
        /// Withdraw funds
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/initiate-withdrawal" /></para>
        /// </summary>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="quantity">Quantity to withdraw</param>
        /// <param name="address">Address to withdraw to</param>
        /// <param name="network">Network to use</param>
        /// <param name="memo">Memo</param>
        /// <param name="type">Withdraw type</param>
        /// <param name="internalWithdrawType">Inner withdraw type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CoinWWithdrawResult>> WithdrawAsync(string asset, decimal quantity, string address, string network, string? memo = null, WithdrawType? type = null, InternalWithdrawType? internalWithdrawType = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel a pending withdrawal
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/cancel-withdrawal" /></para>
        /// </summary>
        /// <param name="withdrawalId">Id to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> CancelWithdrawalAsync(long withdrawalId, CancellationToken ct = default);

        /// <summary>
        /// Transfer between account types
        /// <para><a href="https://www.coinw.com/api-doc/en/spot-trading/account/transfer-assets" /></para>
        /// </summary>
        /// <param name="fromAccount">From account type</param>
        /// <param name="toAccount">To account type</param>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="quantity">Quantity to transfer</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> TransferAsync(AccountType fromAccount, AccountType toAccount, string asset, decimal quantity, CancellationToken ct = default);

    }
}
