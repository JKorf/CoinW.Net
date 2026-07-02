using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using CoinW.Net.Clients;
using CoinW.Net.Objects.Options;
using System.Threading;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Authentication;
using System.Collections.Generic;

namespace CoinW.Net.UnitTests
{
    [NonParallelizable]
    public class CoinWRestIntegrationTests : RestIntegrationTest<CoinWRestClient>
    {
        public override bool Run { get; set; } = false;

        public override CoinWRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new CoinWRestClient(null, loggerFactory, Options.Create(new CoinWRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new CoinWCredentials(key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().SpotApi.ExchangeData.GetRecentTradesAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorCode, Is.EqualTo("-3"));
        }

        [Test]
        public async Task TestSpotAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetBalancesAsync(CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetBalancesDetailsAsync(CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetDepositWithdrawalHistoryAsync("USDT", null, CancellationToken.None), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetTickersAsync(CancellationToken.None), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetAssetsAsync(CancellationToken.None), false);
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetSymbolsAsync(CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETH_USDT", null, CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETH_USDT", null, null, CancellationToken.None), false, "data", ignoreProperties: ["time"]);
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetKlinesAsync("ETH_USDT", Enums.KlineInterval.OneDay, null, null, CancellationToken.None), false, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotTrading()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetOpenOrdersAsync("ETH_USDT", null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetOrderTransactionHistoryAsync("ETH_USDT", null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetUserTradesAsync("ETH_USDT", null, null, null, null, null, CancellationToken.None), true, "data.list");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestFuturesAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetBalancesAsync(CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetMaxTransferableAsync(CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetMaxTradeSizeAsync("ETH", 1, Enums.MarginType.IsolatedMargin, 1, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetFeesAsync(CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetMarginModeAsync(CancellationToken.None), true, "data", ignoreProperties: ["leverageMap"]);
            //await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetMaxPositionSizeAsync("ETH", CancellationToken.None), true);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestFuturesExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetSymbolsAsync(null, CancellationToken.None), false, "data", ignoreProperties: ["selected", "platform"]);
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetTickerAsync("ETH", CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetTickersAsync(CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetKlinesAsync("ETH", Enums.FuturesKlineInterval.OneDay, null, null, null, CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetLastFundingRateAsync("ETH", CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetOrderBookAsync("ETH", CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetRecentTradesAsync("ETH", CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetMarginRequirementsAsync(CancellationToken.None), true, "data.ladderConfig");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetTradeHistoryAsync("ETH", null, null, CancellationToken.None), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestFuturesTrading()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetOpenOrdersAsync("ETH", Enums.FuturesOrderType.Plan, null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetOpenOrdersAsync(Enums.FuturesOrderType.Plan, null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetOpenOrderCountAsync(CancellationToken.None), true, "data");
            await RunAndCheckResult(client => client.FuturesApi.Trading.GetTrailingTpSlAsync(CancellationToken.None), true);
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetOrderHistory7DaysAsync(null, null, null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetOrderHistory3MonthsAsync(null, null, null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetPositionHistoryAsync(null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetPositionsAsync(CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetTransactionHistory3DaysAsync("ETH", null, null, null, null, CancellationToken.None), true, "data");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetTransactionHistory3MonthsAsync("ETH", null, null, null, null, CancellationToken.None), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }
    }
}
