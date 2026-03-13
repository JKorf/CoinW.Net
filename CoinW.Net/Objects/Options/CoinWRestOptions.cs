using CryptoExchange.Net.Objects.Options;

namespace CoinW.Net.Objects.Options
{
    /// <summary>
    /// Options for the CoinWRestClient
    /// </summary>
    public class CoinWRestOptions : RestExchangeOptions<CoinWEnvironment, CoinWCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static CoinWRestOptions Default { get; set; } = new CoinWRestOptions()
        {
            Environment = CoinWEnvironment.Live
        };

        /// <summary>
        /// ctor
        /// </summary>
        public CoinWRestOptions()
        {
            Default?.Set(this);
        }
                
         /// <summary>
        /// Futures API options
        /// </summary>
        public RestApiOptions<CoinWCredentials> FuturesOptions { get; private set; } = new RestApiOptions<CoinWCredentials>();

         /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions<CoinWCredentials> SpotOptions { get; private set; } = new RestApiOptions<CoinWCredentials>();

        internal CoinWRestOptions Set(CoinWRestOptions targetOptions)
        {
            targetOptions = base.Set<CoinWRestOptions>(targetOptions);            
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            return targetOptions;
        }
    }
}
