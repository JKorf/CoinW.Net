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
            await tester.ValidateAsync(client => client.SpotApi.Account.GetDepositAddressesAsync("UnitTest", "123"), "GetDepositAddresses", nestedJsonProperty: "data");
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
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetAssetsAsync(), "GetAssets", nestedJsonProperty: "data", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("123", 5), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetRecentTradesAsync("123"), "GetRecentTrades", nestedJsonProperty: "data", ignoreProperties: ["time"]);
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
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderAsync(123), "GetOrder", nestedJsonProperty: "data", ignoreProperties: ["date"]);
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderTransactionHistoryAsync("123"), "GetOrderTransactionHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetUserTradesAsync(), "GetUserTrades", nestedJsonProperty: "data.list");
        }

        [Test]
        public async Task ValidateFuturesAccountCalls()
        {
            var client = new CoinWRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<CoinWRestClient>(client, "Endpoints/Spot/Trading", "https://api.coinw.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetLeverageAsync(123), "GetLeverage", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateFuturesExchangeDataCalls()
        {
            var client = new CoinWRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<CoinWRestClient>(client, "Endpoints/Futures/ExchangeData", "https://api.coinw.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data", ignoreProperties: ["depthPrecision", "selected"]);
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetTickerAsync("123"), "GetTicker", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetKlinesAsync("123", FuturesKlineInterval.OneDay), "GetKlines", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetLastFundingRateAsync("123"), "GetLastFundingRate", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetOrderBookAsync("123"), "GetOrderBook", nestedJsonProperty: "data", ignoreProperties: ["t"]);
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetRecentTradesAsync("123"), "GetRecentTrades", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateFuturesTradingCalls()
        {
            var client = new CoinWRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<CoinWRestClient>(client, "Endpoints/Spot/Trading", "https://api.coinw.com", IsAuthenticated); 
            await tester.ValidateAsync(client => client.FuturesApi.Trading.PlaceOrderAsync("123", PositionSide.Long, FuturesOrderType.Market, 0.1m, 123), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelOrderAsync(123), "CancelOrder");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelOrdersAsync([123]), "CancelOrders");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.ClosePositionAsync(123), "ClosePosition", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.ClosePositionsByClientOrderIdAsync(["123"]), "ClosePositionsByClientOrderId", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CloseAllPositionsAsync("123"), "CloseAllPositions");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.ReversePositionAsync(123), "ReversePosition", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.AdjustMarginAsync(123, 0.1m, 0.1m), "AdjustMargin");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.SetTpSlAsync(123, "123"), "SetTpSl");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.SetTrailingTpSlAsync(123, 0.1m), "SetTrailingTpSl");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOpenOrdersAsync("123", FuturesOrderType.Plan), "GetOpenOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOpenOrderCountAsync(), "GetOpenOrderQuantity", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetTpSlAsync(1), "GetTpSl", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetTrailingTpSlAsync(), "GetTrailingTpSl", nestedJsonProperty: "data", ignoreProperties: ["version"]);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOrderHistory7DaysAsync(), "GetOrderHistory7D", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetPositionsAsync("123"), "GetPositions", nestedJsonProperty: "data");
        }



        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl?.Contains("sign") == true;
        }
    }
}
