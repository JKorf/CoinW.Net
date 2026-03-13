using CryptoExchange.Net.Authentication;

namespace CoinW.Net
{
    /// <summary>
    /// CoinW credentials
    /// </summary>
    public class CoinWCredentials : ApiCredentials
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        public CoinWCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }
       
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public CoinWCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new CoinWCredentials(Hmac!);
    }
}
