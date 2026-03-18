using CryptoExchange.Net.Authentication;

namespace CoinW.Net
{
    /// <summary>
    /// CoinW API credentials
    /// </summary>
    public class CoinWCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials providing only credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        public CoinWCredentials(string key, string secret) : base(key, secret)
        {
        }
    }
}
