using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using System.Linq;
using CryptoExchange.Net;
using CoinW.Net.Enums;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects.Errors;

namespace CoinW.Net.Clients.FuturesApi
{
    internal partial class CoinWRestClientFuturesSharedApi
    {
        #region Set Futures Tp Sl

        async Task<ICallResult<SharedId>> ISetFuturesTpSl.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
            => await SetFuturesTpSlAsync(request, ct).ConfigureAwait(false);

        public SetFuturesTpSlOptions SetFuturesTpSlOptions { get; } = new SetFuturesTpSlOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = SetFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var positionResult = await _api.Trading.GetPositionsAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
            if (!positionResult.Success)
                return HttpResult.Fail<SharedId>(Exchange, positionResult.Error!);

            if (positionResult.Data.Length == 0)
                return HttpResult.Fail<SharedId>(Exchange, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var position = positionResult.Data[0];

            var result = await _api.Trading.SetTpSlAsync(
                position.Id,
                request.Symbol.GetSymbol(FormatSymbol),
                takeProfitPrice: request.TpSlSide == SharedTpSlSide.TakeProfit ? request.TriggerPrice : position.TakeProfitPrice,
                stopLossPrice: request.TpSlSide == SharedTpSlSide.StopLoss ? request.TriggerPrice: position.StopLossPrice,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(position.Id.ToString()));
        }

        #endregion

        #region Cancel Futures Tp Sl

        async Task<ICallResult<bool>> ICancelFuturesTpSl.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
            => await CancelFuturesTpSlAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesTpSlOptions CancelFuturesTpSlOptions { get; } = new CancelFuturesTpSlOptions(_exchangeName, true);
        public async Task<HttpResult<bool>> CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<bool>(Exchange, validationError);

            var positionResult = await _api.Trading.GetPositionsAsync(request.Symbol!.GetSymbol(FormatSymbol)).ConfigureAwait(false);
            if (!positionResult.Success)
                return HttpResult.Fail<bool>(Exchange, positionResult.Error!);

            if (positionResult.Data.Length == 0)
                return HttpResult.Fail<bool>(Exchange, new ServerError(new ErrorInfo(ErrorType.NoPosition, "Position not found")));

            var position = positionResult.Data[0];

            var result = await _api.Trading.SetTpSlAsync(
                position.Id, 
                request.Symbol.GetSymbol(FormatSymbol),
                request.TpSlSide == SharedTpSlSide.TakeProfit ? null : position.TakeProfitPrice,
                request.TpSlSide == SharedTpSlSide.StopLoss ? null : position.StopLossPrice,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<bool>(result);

            // Return
            return HttpResult.Ok(result, true);
        }

        #endregion

    }
}
