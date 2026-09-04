using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Enums;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.SpotApi
{
    internal partial class CoinWRestClientSpotSharedApi
    {

        #region Get Withdrawal History

        async Task<ICallResult<SharedWithdrawal[]>> IGetWithdrawalHistory.GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageToken, CancellationToken ct)
            => await GetWithdrawalHistoryAsync(request, pageToken, ct).ConfigureAwait(false);

        Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => GetWithdrawalHistoryAsync(request, pageRequest, ct);
        GetWithdrawalHistoryOptions IWithdrawalRestClient.GetWithdrawalsOptions => GetWithdrawalHistoryOptions;

        public GetWithdrawalHistoryOptions GetWithdrawalHistoryOptions { get; } = new GetWithdrawalHistoryOptions(_exchangeName, true, true, false, 1000)
        {
            RequiredRequestParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetWithdrawalsRequest.Asset), typeof(string), "Asset name", "ETH")
            }
        };
        public async Task<HttpResult<SharedWithdrawal[]>> GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageToken, CancellationToken ct)
        {
            var validationError = GetWithdrawalHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

            // Get data
            var result = await _api.Account.GetDepositWithdrawalHistoryAsync(
                request.Asset!,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(result);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, request.Direction ?? DataDirection.Ascending)
                .Select(x => 
                    new SharedWithdrawal(
                        x.Asset, 
                        x.Address,
                        x.Quantity,
                        x.Status == Enums.MovementStatus.Success,
                        x.Timestamp,
                        GetWithdrawalStatus(x))
                    {
                        Network = x.Network,
                        TransactionId = x.TransactionId,
                        Confirmations = x.Confirmations,
                        Id = x.Id.ToString()
                    }).ToArray());
        }

        #endregion

        private SharedTransferStatus GetWithdrawalStatus(CoinWDepositWithdrawal x)
        {
            if (x.Status == MovementStatus.Success)
                return SharedTransferStatus.Completed;

            if (x.Status == MovementStatus.Waiting)
                return SharedTransferStatus.InProgress;

            return SharedTransferStatus.Unknown;
        }


        #region Withdraw

        async Task<ICallResult<SharedId>> IWithdraw.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
            => await WithdrawAsync(request, ct).ConfigureAwait(false);

        public WithdrawOptions WithdrawOptions { get; } = new WithdrawOptions(_exchangeName)
        {
            RequiredRequestParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(WithdrawRequest.Network), typeof(string), "Network name", "ETH")
            }
        };
        public async Task<HttpResult<SharedId>> WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = WithdrawOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            // Get data
            var withdrawal = await _api.Account.WithdrawAsync(
                request.Asset,
                request.Quantity,
                request.Address,
                network: request.Network!,
                memo: request.AddressTag,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal.Success)
                return HttpResult.Fail<SharedId>(withdrawal);

            return HttpResult.Ok(withdrawal, new SharedId(withdrawal.Data.Id.ToString()));
        }

        #endregion

    }
}
