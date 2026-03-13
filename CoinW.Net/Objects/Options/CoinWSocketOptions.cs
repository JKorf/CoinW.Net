using CryptoExchange.Net.Objects.Options;

namespace CoinW.Net.Objects.Options
{
    /// <summary>
    /// Options for the CoinWSocketClient
    /// </summary>
    public class CoinWSocketOptions : SocketExchangeOptions<CoinWEnvironment, CoinWCredentials>
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
        public SocketApiOptions<CoinWCredentials> FuturesOptions { get; private set; } = new SocketApiOptions<CoinWCredentials>();

        /// <summary>
        /// Spot API options
        /// </summary>
        public SocketApiOptions<CoinWCredentials> SpotOptions { get; private set; } = new SocketApiOptions<CoinWCredentials>();


        internal CoinWSocketOptions Set(CoinWSocketOptions targetOptions)
        {
            targetOptions = base.Set<CoinWSocketOptions>(targetOptions);            
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            return targetOptions;
        }
    }
}
