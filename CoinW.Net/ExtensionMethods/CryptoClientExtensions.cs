using CryptoExchange.Net.Interfaces;
using CoinW.Net.Clients;
using CoinW.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the CoinW REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static ICoinWRestClient CoinW(this ICryptoRestClient baseClient) => baseClient.TryGet<ICoinWRestClient>(() => new CoinWRestClient());

        /// <summary>
        /// Get the CoinW Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static ICoinWSocketClient CoinW(this ICryptoSocketClient baseClient) => baseClient.TryGet<ICoinWSocketClient>(() => new CoinWSocketClient());
    }
}
