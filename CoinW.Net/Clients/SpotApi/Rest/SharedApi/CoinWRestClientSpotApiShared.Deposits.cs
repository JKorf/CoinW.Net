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

        #region Get Deposit Addresses

        async Task<IExchangeCallResult<SharedDepositAddress[]>> IGetDepositAddresses.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
            => await GetDepositAddressesAsync(request, ct).ConfigureAwait(false);

        Task<HttpResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => GetDepositHistoryAsync(request, pageRequest, ct);
        GetDepositHistoryOptions IDepositRestClient.GetDepositsOptions => GetDepositHistoryOptions;

        public GetDepositAddressesOptions GetDepositAddressesOptions { get; } = new GetDepositAddressesOptions(_exchangeName, true)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<GetDepositAddressesRequest>.Required(x => x.Network)
            ]
        };
        public async Task<HttpResult<SharedDepositAddress[]>> GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = GetDepositAddressesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await _api.Account.GetDepositAddressesAsync(request.Asset, request.Network!, ct: ct).ConfigureAwait(false);
            if (!depositAddresses.Success)
                return HttpResult.Fail<SharedDepositAddress[]>(depositAddresses);

            return HttpResult.Ok(depositAddresses, new[] { new SharedDepositAddress(request.Asset, depositAddresses.Data[0].Address)
            {
                TagOrMemo = depositAddresses.Data[0].Memo,
                Network = depositAddresses.Data[0].NetworkName
            }
            });
        }

        #endregion

        #region Get Deposit History

        async Task<IExchangeCallResult<SharedDeposit[]>> IGetDepositHistory.GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetDepositHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetDepositHistoryOptions GetDepositHistoryOptions { get; } = new GetDepositHistoryOptions(_exchangeName, true, true, false, 1000)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<GetDepositsRequest>.Required(x => x.Asset)
            ]
        };
        public async Task<HttpResult<SharedDeposit[]>> GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetDepositHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

            // Get data
            var deposits = await _api.Account.GetDepositWithdrawalHistoryAsync(
                request.Asset!,
                ct: ct).ConfigureAwait(false);
            if (!deposits.Success)
                return HttpResult.Fail<SharedDeposit[]>(deposits);

            var result = deposits.Data.Where(x => x.Type == Enums.MovementType.Deposit);
            return HttpResult.Ok(deposits, ExchangeHelpers.ApplyFilter(result, x => x.Timestamp, request.StartTime, request.EndTime, request.Direction ?? DataDirection.Ascending)
                .Select(x => 
                    new SharedDeposit(
                        x.Asset,
                        x.Quantity,
                        x.Status == Enums.MovementStatus.Success,
                        x.Timestamp,
                        GetTransferStatus(x.Status))
                    {
                        Network = x.Network,
                        TransactionId = x.TransactionId,
                        Confirmations = x.Confirmations,
                        Id = x.Id.ToString()
                    }).ToArray());
        }

        #endregion

        private SharedTransferStatus GetTransferStatus(MovementStatus status)
        {
            if (status == MovementStatus.Success)
                return SharedTransferStatus.Completed;
            if (status == MovementStatus.Waiting)
                return SharedTransferStatus.InProgress;

            return SharedTransferStatus.Unknown;
        }

    }
}
