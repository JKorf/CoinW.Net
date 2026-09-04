using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;

namespace CoinW.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of CoinW
    /// </summary>
    public interface ICoinWSharedApiClient
    {
        /// <summary>
        /// Spot REST shared API implementations
        /// </summary>
        ICoinWRestClientSpotSharedApi SpotRest { get; }

        /// <summary>
        /// Futures REST shared API implementations
        /// </summary>
        ICoinWRestClientFuturesSharedApi FuturesRest { get; }

        /// <summary>
        /// Spot WebSocket shared API implementations
        /// </summary>
        ICoinWSocketClientSpotSharedApi SpotSocket { get; }

        /// <summary>
        /// Futures WebSocket shared API implementations
        /// </summary>
        ICoinWSocketClientFuturesSharedApi FuturesSocket { get; }
    }
}
