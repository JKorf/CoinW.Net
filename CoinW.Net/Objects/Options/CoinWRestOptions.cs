using CryptoExchange.Net.Objects.Options;

namespace CoinW.Net.Objects.Options
{
    /// <summary>
    /// Options for the CoinWRestClient
    /// </summary>
    public class CoinWRestOptions : RestExchangeOptions<CoinWEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static CoinWRestOptions Default { get; set; } = new CoinWRestOptions()
        {
            Environment = CoinWEnvironment.Live,
            AutoTimestamp = true
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
        public RestApiOptions FuturesOptions { get; private set; } = new RestApiOptions();

         /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();


        internal CoinWRestOptions Set(CoinWRestOptions targetOptions)
        {
            targetOptions = base.Set<CoinWRestOptions>(targetOptions);
            
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);

            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
