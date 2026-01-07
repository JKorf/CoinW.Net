using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using CoinW.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace CoinW.Net.UnitTests
{
    [TestFixture()]
    public class CoinWRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new CoinWSpotAuthenticationProvider(new ApiCredentials("XXX", "XXX"));
            var client = (RestApiClient)new CoinWRestClient().SpotApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "/api/v3/order",
                (uriParams, bodyParams, headers) =>
                {
                    return uriParams["sign"].ToString();
                },
                "AD26D501953EC615636AC668146268B4",
                new Dictionary<string, object>
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromDouble(1499827320559),
                true,
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<CoinWRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<CoinWSocketClient>();
        }
    }
}
