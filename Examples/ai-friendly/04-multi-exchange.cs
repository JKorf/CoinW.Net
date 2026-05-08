// 04-multi-exchange.cs
//
// Demonstrates: writing exchange-agnostic code using CryptoExchange.Net.SharedApis.
// The same pattern works against CoinW, Binance, OKX, Bybit, Kraken, and other
// exchanges from the CryptoExchange.Net family.
//
// Setup:
//   dotnet add package CoinW.Net
//   dotnet add package JK.OKX.Net    // optional, for an OKX implementation
//   dotnet add package Bybit.Net     // optional, for a Bybit implementation

using CoinW.Net.Clients;
using CryptoExchange.Net.SharedApis;

// ---- THE PATTERN ----
// Each exchange client exposes a `.SharedClient` property on supported API surfaces.
// SharedClient implements common interfaces such as ISpotTickerRestClient and
// ISpotOrderRestClient so higher-level code can avoid per-exchange branches.

ISpotTickerRestClient coinwShared = new CoinWRestClient().SpotApi.SharedClient;

// To add OKX or Bybit, install the package and use the equivalent SharedClient:
//   ISpotTickerRestClient okxShared = new OKXRestClient().UnifiedApi.SharedClient;
//   ISpotTickerRestClient bybitShared = new BybitRestClient().V5Api.SharedClient;

var btcusdt = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");

await PrintTicker(coinwShared, btcusdt);

async Task PrintTicker(ISpotTickerRestClient client, SharedSymbol symbol)
{
    var result = await client.GetSpotTickerAsync(new GetTickerRequest(symbol));
    if (!result.Success)
    {
        Console.WriteLine($"[{client.Exchange}] Failed: {result.Error}");
        return;
    }

    Console.WriteLine($"[{client.Exchange}] {result.Data.Symbol}: {result.Data.LastPrice}");
}

// ---- WHY THIS MATTERS ----
// You can build multi-exchange scanners, routers, dashboards, and comparison tools
// without manually formatting CoinW spot symbols like BTC_USDT in the application layer.

// ---- AVAILABLE SHARED INTERFACES ----
// REST:
//   ISpotTickerRestClient, ISpotSymbolRestClient, ISpotOrderRestClient
//   IFuturesOrderRestClient, IFuturesSymbolRestClient, IBalanceRestClient
//   IPositionRestClient, IFeeRestClient, IOrderBookRestClient
//   IRecentTradeRestClient, IKlineRestClient, IDepositRestClient
//   IWithdrawalRestClient, ITransferRestClient, IBookTickerRestClient
// WebSocket:
//   ITickerSocketClient, IBookTickerSocketClient, IOrderBookSocketClient
//   ITradeSocketClient, IKlineSocketClient, IUserTradeSocketClient
//   IBalanceSocketClient, ISpotOrderSocketClient, IFuturesOrderSocketClient

// ---- WEBSOCKET EXAMPLE: SHARED SUBSCRIPTION ----
var coinwSocket = new CoinWSocketClient();
ITickerSocketClient coinwTickerSocket = coinwSocket.SpotApi.SharedClient;

var sub = await coinwTickerSocket.SubscribeToTickerUpdatesAsync(
    new SubscribeTickerRequest(btcusdt),
    update => Console.WriteLine($"[{coinwTickerSocket.Exchange}] {update.Data.Symbol}: {update.Data.LastPrice}"));

if (!sub.Success)
{
    Console.WriteLine($"Subscribe failed: {sub.Error}");
    return;
}

Console.WriteLine("Press Enter to exit");
Console.ReadLine();

await coinwSocket.UnsubscribeAsync(sub.Data);

// Common variations:
//   Multi-exchange arbitrage: loop over List<ISpotTickerRestClient>, find max bid / min ask
//   Cross-exchange order book: IOrderBookSocketClient on each exchange, merge into a composite book
//   Futures routing: use IFuturesOrderRestClient from each exchange's futures SharedClient
//   Symbol formatting: use SharedSymbol instead of hard-coded exchange symbols
