using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoinW.Net.Clients;
using NUnit.Framework.Constraints;
using CoinW.Net.Enums;
using System.Drawing;

namespace CoinW.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateSpotAccountCalls()
        {
            var client = new CoinWRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<CoinWRestClient>(client, "Endpoints/Spot/Account", "https://api.coinw.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositWithdrawalHistoryAsync("123", 123), "GetDepositWithdrawalHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositAddressesAsync("123", "123"), "GetDepositAddresses", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.WithdrawAsync("123", 0.1m, "123", "123"), "Withdraw", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Account.CancelWithdrawalAsync(123), "CancelWithdrawal");
            await tester.ValidateAsync(client => client.SpotApi.Account.TransferAsync(AccountType.Spot, AccountType.Funding, "123", 0.1m), "Transfer");
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            var client = new CoinWRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<CoinWRestClient>(client, "Endpoints/Spot/ExchangeData", "https://api.coinw.com", IsAuthenticated);
            //await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetServerTimeAsync(), "GetServerTime");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetAssetsAsync(), "GetAssets", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("123", 5), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetRecentTradesAsync("123"), "GetRecentTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("123", KlineInterval.OneMinute), "GetKlines", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            var client = new CoinWRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<CoinWRestClient>(client, "Endpoints/Spot/Trading", "https://api.coinw.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceOrderAsync("123", OrderSide.Buy, OrderType.Limit, 0.1m, 0.1m, 0.1m), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOrderAsync(123), "CancelOrder");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelAllOrdersAsync("123"), "CancelAllOrders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOpenOrdersAsync("123"), "GetOpenOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderAsync(123), "GetOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderTransactionHistoryAsync("123"), "GetOrderTransactionHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetUserTradesAsync(), "GetUserTrades", nestedJsonProperty: "data.list");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl?.Contains("signature") == true || result.RequestBody?.Contains("signature=") == true;
        }
    }
}
