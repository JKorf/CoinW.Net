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

        #region Transfer

        async Task<ICallResult<SharedId>> ITransfer.TransferAsync(TransferRequest request, CancellationToken ct)
            => await TransferAsync(request, ct).ConfigureAwait(false);

        public TransferOptions TransferOptions { get; } = new TransferOptions(_exchangeName, [
            SharedAccountType.Funding,
            SharedAccountType.Spot
            ])
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<TransferRequest>.NotSupported(x => x.FromSymbol),
                RequestParameterRuleOverride<TransferRequest>.NotSupported(x => x.ToSymbol),
                ]
        };
        public async Task<HttpResult<SharedId>> TransferAsync(TransferRequest request, CancellationToken ct)
        {
            var validationError = TransferOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var fromType = GetTransferType(request.FromAccountType);
            var toType = GetTransferType(request.ToAccountType);
            if (fromType == null || toType == null)
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("To/From AccountType", "invalid to/from account combination"));

            // Get data
            var transfer = await _api.Account.TransferAsync(
                fromType.Value,
                toType.Value,
                request.Asset,
                request.Quantity,
                ct: ct).ConfigureAwait(false);
            if (!transfer.Success)
                return HttpResult.Fail<SharedId>(transfer);

            return HttpResult.Ok(transfer, new SharedId(""));
        }

        #endregion

        private AccountType? GetTransferType(SharedAccountType type)
        {
            if (type == SharedAccountType.Spot) return AccountType.Spot;
            if (type == SharedAccountType.Funding) return AccountType.Funding;
            return null;
        }

    }
}
