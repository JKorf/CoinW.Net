using CryptoExchange.Net.Objects.Options;

namespace CoinW.Net.Objects.Options
{
    /// <summary>
    /// Options for the CoinWSocketClient
    /// </summary>
    public class CoinWSocketOptions : SocketExchangeOptions<CoinWEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static CoinWSocketOptions Default { get; set; } = new CoinWSocketOptions()
        {
            Environment = CoinWEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };


        /// <summary>
        /// ctor
        /// </summary>
        public CoinWSocketOptions()
        {
            Default?.Set(this);
        }


        
         /// <summary>
        /// Futures API options
        /// </summary>
        public SocketApiOptions FuturesOptions { get; private set; } = new SocketApiOptions();

         /// <summary>
        /// Spot API options
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();


        internal CoinWSocketOptions Set(CoinWSocketOptions targetOptions)
        {
            targetOptions = base.Set<CoinWSocketOptions>(targetOptions);
            
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);

            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
