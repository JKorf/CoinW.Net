using CryptoExchange.Net.Authentication;

namespace CoinW.Net.Interfaces.Clients
{
    /// <summary>
    /// Provider for clients with credentials for specific users
    /// </summary>
    public interface ICoinWUserClientProvider
    {
        /// <summary>
        /// Initialize a client for the specified user identifier. This can be used so to initialize a client for a user so ApiCredentials do not need to be passed later.
        /// </summary>
        /// <param name="userIdentifier">The identifier for the user</param>
        /// <param name="credentials">The credentials for the user</param>
        /// <param name="environment">The environment to use</param>
        void InitializeUserClient(string userIdentifier, ApiCredentials credentials, CoinWEnvironment? environment = null);

        /// <summary>
        /// Get the Rest client for a specific user. In case the client does not exist yet it will be created and the <paramref name="credentials"/> should be provided, unless <see cref="InitializeUserClient" /> has been called prior for this user.
        /// </summary>
        /// <param name="userIdentifier">The identifier for user</param>
        /// <param name="credentials">The credentials for the user. Required the first time a client is requested for this user unless <see cref="InitializeUserClient" /> has been called prior for this user.</param>
        /// <param name="environment">The environment to use</param>
        ICoinWRestClient GetRestClient(string userIdentifier, ApiCredentials? credentials = null, CoinWEnvironment? environment = null);

        /// <summary>
        /// Get the Socket client for a specific user. In case the client does not exist yet it will be created and the <paramref name="credentials"/> should be provided, unless <see cref="InitializeUserClient" /> has been called prior for this user.
        /// </summary>
        /// <param name="userIdentifier">The identifier for user</param>
        /// <param name="credentials">The credentials for the user. Required the first time a client is requested for this user unless <see cref="InitializeUserClient" /> has been called prior for this user.</param>
        /// <param name="environment">The environment to use</param>
        ICoinWSocketClient GetSocketClient(string userIdentifier, ApiCredentials? credentials = null, CoinWEnvironment? environment = null);
    }
}