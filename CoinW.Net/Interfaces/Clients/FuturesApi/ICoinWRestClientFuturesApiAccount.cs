using System.Threading.Tasks;
using System.Threading;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects;

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

    }
}
