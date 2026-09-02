using CryptoExchange.Net.Objects;
using CoinW.Net.Interfaces.Clients.SpotApi;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using CryptoExchange.Net.RateLimiting.Guards;
using System.Collections.Generic;
using CoinW.Net.Objects.Models;
using CoinW.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace CoinW.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class CoinWRestClientSpotApiAccount : ICoinWRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly CoinWRestClientSpotApi _baseClient;

        internal CoinWRestClientSpotApiAccount(CoinWRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, decimal>>> GetBalancesAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnBalances");
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(3, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnBalances" + key));
            var result = await _baseClient.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Balances Details

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, CoinWBalance>>> GetBalancesDetailsAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnCompleteBalances");
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(2), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnCompleteBalances" + key));
            var result = await _baseClient.SendAsync<Dictionary<string, CoinWBalance>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Deposit Withdrawal History

        /// <inheritdoc />
        public Task<HttpResult<CoinWDepositWithdrawal[]>> GetDepositWithdrawalHistoryAsync(string asset, long? id = null, CancellationToken ct = default)
            => GetDepositWithdrawalHistoryAsync([asset], id, ct);

        /// <inheritdoc />
        public async Task<HttpResult<CoinWDepositWithdrawal[]>> GetDepositWithdrawalHistoryAsync(IEnumerable<string> assets, long? id = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnDepositsWithdrawals");
            parameters.AddCommaSeparated("symbol", assets);
            parameters.Add("id", id);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(3, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnDepositsWithdrawals" + key));
            var result = await _baseClient.SendAsync<CoinWDepositWithdrawal[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Get Deposit Addresses

        /// <inheritdoc />
        public async Task<HttpResult<CoinWDepositAddress[]>> GetDepositAddressesAsync(string asset, string network, CancellationToken ct = default)
        {
            var assetIdResult = await CoinWUtils.GetAssetIdFromNameAsync(_baseClient.BaseClient, asset).ConfigureAwait(false);
            if (!assetIdResult.Success)
                return HttpResult.Fail<CoinWDepositAddress[]>(_baseClient.Exchange, assetIdResult.Error);

            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "returnDepositAddresses");
            parameters.Add("symbolId", assetIdResult.Data);
            parameters.Add("chain", network);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(3, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "returnDepositAddresses" + key));
            var result = await _baseClient.SendAsync<CoinWDepositAddress[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<HttpResult<CoinWWithdrawResult>> WithdrawAsync(string asset, decimal quantity, string address, string network, string? memo = null, WithdrawType? type = null, InternalWithdrawType? internalWithdrawType = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "doWithdraw");
            parameters.Add("currency", asset);
            parameters.Add("amount", quantity);
            parameters.Add("address", address);
            parameters.Add("chain", network);
            parameters.Add("memo", memo);
            parameters.Add("type", type);
            parameters.Add("innerToType", internalWithdrawType, EnumSerialization.Number);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(3, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "doWithdraw" + key));
            var result = await _baseClient.SendAsync<CoinWWithdrawResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Withdrawal

        /// <inheritdoc />
        public async Task<HttpResult> CancelWithdrawalAsync(long withdrawalId, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "cancelWithdraw");
            parameters.Add("id", withdrawalId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(3, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "cancelWithdraw" + key));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<HttpResult> TransferAsync(AccountType fromAccount, AccountType toAccount, string asset, decimal quantity, CancellationToken ct = default)
        {
            var parameters = new Parameters(CoinWExchange._parameterSerializationSettings);
            parameters.Add("command", "spotWealthTransfer");
            parameters.Add("accountType", fromAccount);
            parameters.Add("targetAccountType", toAccount);
            parameters.Add("bizType", EnumConverter.GetString(fromAccount) + "_TO_" + EnumConverter.GetString(toAccount));
            parameters.Add("coinCode", asset);
            parameters.Add("amount", quantity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/private", CoinWExchange.RateLimiter.CoinW, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding, keySelector: (def, key) => "spotWealthTransfer" + key));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
