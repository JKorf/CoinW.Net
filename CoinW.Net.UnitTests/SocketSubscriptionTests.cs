using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using CoinW.Net.Clients;
using CoinW.Net.Objects.Models;
using Microsoft.Extensions.Logging;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Options;
using CoinW.Net.Objects.Options;

namespace CoinW.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateConcurrentSpotSubscriptions()
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new CoinWSocketClient(Options.Create(new CoinWSocketOptions
            {
                OutputOriginalData = true
            }), logger);

            var tester = new SocketSubscriptionValidator<CoinWSocketClient>(client, "Subscriptions/Spot", "wss://ws.futurescw.com");
            await tester.ValidateConcurrentAsync<CoinWKlineUpdate>(
                (client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("UnitTest", Enums.KlineIntervalStream.OneDay, handler),
                (client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("UnitTest", Enums.KlineIntervalStream.OneHour, handler),
                "Concurrent");
        }

        [Test]
        public async Task ValidateSpotSubscriptions()
        {
            var loggerFact = new LoggerFactory();
            loggerFact.AddProvider(new TraceLoggerProvider());

            var client = new CoinWSocketClient(Options.Create(new Objects.Options.CoinWSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456")
            }), loggerFact);
            var tester = new SocketSubscriptionValidator<CoinWSocketClient>(client, "Subscriptions/Spot", "wss://ws.futurescw.com");
            await tester.ValidateAsync<CoinWTickerUpdate>((client, handler) => client.SpotApi.SubscribeToTickerUpdatesAsync("UnitTest", handler), "Ticker", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWSymbolUpdate[]>((client, handler) => client.SpotApi.SubscribeToAllTickerUpdatesAsync(handler), "AllTicker", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWOrderBookUpdate>((client, handler) => client.SpotApi.SubscribeToOrderBookUpdatesAsync("UnitTest", handler), "Book", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWOrderBook>((client, handler) => client.SpotApi.SubscribeToPartialOrderBookUpdatesAsync("UnitTest", handler), "PartialBook", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWKlineUpdate>((client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("UnitTest", Enums.KlineIntervalStream.OneDay, handler), "Kline", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWTradeUpdate[]>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("UnitTest", handler), "Trades", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWBalanceUpdate>((client, handler) => client.SpotApi.SubscribeToBalanceUpdatesAsync(handler), "Balance", nestedJsonProperty: "data", ignoreProperties: ["type", "account"]);
            await tester.ValidateAsync<CoinWOrderUpdate>((client, handler) => client.SpotApi.SubscribeToOrderUpdatesAsync(handler), "Order", nestedJsonProperty: "data", ignoreProperties: ["type", "account"]);
        }

        [Test]
        public async Task ValidateFuturesSubscriptions()
        {
            var loggerFact = new LoggerFactory();
            loggerFact.AddProvider(new TraceLoggerProvider());

            var client = new CoinWSocketClient(Options.Create(new Objects.Options.CoinWSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456")
            }), loggerFact);
            var tester = new SocketSubscriptionValidator<CoinWSocketClient>(client, "Subscriptions/Futures", "wss://ws.futurescw.com");
            await tester.ValidateAsync<CoinWFuturesTickerUpdate>((client, handler) => client.FuturesApi.SubscribeToTickerUpdatesAsync("ETH", handler), "Ticker", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWFuturesOrderBook>((client, handler) => client.FuturesApi.SubscribeToOrderBookUpdatesAsync("ETH", handler), "OrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWFuturesTrade[]>((client, handler) => client.FuturesApi.SubscribeToTradeUpdatesAsync("ETH", handler), "Trades", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWFuturesStreamKline>((client, handler) => client.FuturesApi.SubscribeToKlineUpdatesAsync("ETH", Enums.FuturesKlineIntervalStream.OneHour, handler), "Klines", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWPrice>((client, handler) => client.FuturesApi.SubscribeToIndexPriceUpdatesAsync("ETH", handler), "IndexPrice", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWPrice>((client, handler) => client.FuturesApi.SubscribeToMarkPriceUpdatesAsync("ETH", handler), "MarkPrice", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWFundingRate>((client, handler) => client.FuturesApi.SubscribeToFundingRateUpdatesAsync("ETH", handler), "FundingRate", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWFuturesOrder[]>((client, handler) => client.FuturesApi.SubscribeToOrderUpdatesAsync(handler), "Order", nestedJsonProperty: "data");
            await tester.ValidateAsync<CoinWPosition[]>((client, handler) => client.FuturesApi.SubscribeToPositionUpdatesAsync(handler), "Position", nestedJsonProperty: "data", ignoreProperties: ["isProfession", "distId", "processStatus", "leaderId", "salesId", "parentId", "partnerId"]);
            await tester.ValidateAsync<CoinWPositionChange[]>((client, handler) => client.FuturesApi.SubscribeToPositionDetailUpdatesAsync(handler), "PositionDetail", nestedJsonProperty: "data", ignoreProperties: ["isProfession", "distId", "processStatus", "leaderId", "salesId", "parentId", "partnerId"]);
            await tester.ValidateAsync<CoinWFuturesBalanceUpdate[]>((client, handler) => client.FuturesApi.SubscribeToBalanceUpdatesAsync(handler), "Balance", nestedJsonProperty: "data", ignoreProperties: ["type"]);
            await tester.ValidateAsync<CoinWMarginInfo[]>((client, handler) => client.FuturesApi.SubscribeToMarginConfigUpdatesAsync(handler), "MarginConfig", nestedJsonProperty: "data", ignoreProperties: ["userId"]);
        }
    }
}
