using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using CoinW.Net.Clients;
using CoinW.Net.Objects.Models;

namespace CoinW.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new CoinWSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<CoinWSocketClient>(client, "Subscriptions/Spot", "XXX");
            //await tester.ValidateAsync<CoinWModel>((client, handler) => client.SpotApi.SubscribeToXXXUpdatesAsync(handler), "XXX");
        }
    }
}
