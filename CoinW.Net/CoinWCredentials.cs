using CryptoExchange.Net.Authentication;
using System;

namespace CoinW.Net
{
    /// <summary>
    /// CoinW credentials
    /// </summary>
    public class CoinWCredentials : ApiCredentials
    {
        /// <summary>
        /// </summary>
        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use parameterized constructor instead.")]
        public CoinWCredentials() { }

        /// <summary>
        /// Create credentials using an HMAC key and secret.
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        public CoinWCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }

        /// <summary>
        /// Create CoinW credentials using HMAC credentials
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public CoinWCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
        public override ApiCredentials Copy() => new CoinWCredentials { CredentialPairs = CredentialPairs };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
