using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace CoinW.Net.Objects.Options
{
    /// <summary>
    /// CoinW options
    /// </summary>
    public class CoinWOptions : LibraryOptions<CoinWRestOptions, CoinWSocketOptions, CoinWCredentials, CoinWEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
