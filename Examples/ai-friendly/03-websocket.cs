// 03-websocket.cs
//
// Demonstrates: WebSocket subscriptions for public spot data, authenticated
// spot user data, and futures market data. Includes proper teardown.
//
// Setup: dotnet add package CoinW.Net

using CoinW.Net;
using CoinW.Net.Clients;
using CoinW.Net.Enums;

const string spotSymbol = "BTC_USDT";
const string futuresSymbol = "BTC";

// ---- 1. PUBLIC SOCKET CLIENT ----
// Reuse a single socket client instance across subscriptions.
// Subscription methods return WebSocketResult<UpdateSubscription>.
var publicSocket = new CoinWSocketClient();

var tickerSub = await publicSocket.SpotApi.SubscribeToTickerUpdatesAsync(
    spotSymbol,
    update =>
    {
        Console.WriteLine($"{spotSymbol}: {update.Data.LastPrice} (24h vol {update.Data.Volume})");
    });

if (!tickerSub.Success)
{
    Console.WriteLine($"Failed to subscribe spot ticker: {tickerSub.Error}");
    return;
}

var klineSub = await publicSocket.SpotApi.SubscribeToKlineUpdatesAsync(
    spotSymbol,
    KlineIntervalStream.OneMinute,
    update =>
    {
        Console.WriteLine($"{spotSymbol} 1m: O={update.Data.OpenPrice} H={update.Data.HighPrice} L={update.Data.LowPrice} C={update.Data.ClosePrice}");
    });

if (!klineSub.Success)
{
    Console.WriteLine($"Failed to subscribe spot klines: {klineSub.Error}");
    await publicSocket.UnsubscribeAsync(tickerSub.Data);
    return;
}

var futuresTickerSub = await publicSocket.FuturesApi.SubscribeToTickerUpdatesAsync(
    futuresSymbol,
    update =>
    {
        Console.WriteLine($"{futuresSymbol} futures: {update.Data.LastPrice}");
    });

if (!futuresTickerSub.Success)
{
    Console.WriteLine($"Failed to subscribe futures ticker: {futuresTickerSub.Error}");
    await publicSocket.UnsubscribeAsync(tickerSub.Data);
    await publicSocket.UnsubscribeAsync(klineSub.Data);
    return;
}

// ---- 2. AUTHENTICATED SOCKET CLIENT ----
// User streams require credentials.
var authSocket = new CoinWSocketClient(options =>
{
    options.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});

var orderSub = await authSocket.SpotApi.SubscribeToOrderUpdatesAsync(update =>
{
    var order = update.Data;
    Console.WriteLine($"Spot order {order.OrderId} {order.Symbol}: {order.EventType}, remaining {order.QuantityRemaining}");
});

if (!orderSub.Success)
{
    Console.WriteLine($"Failed to subscribe spot orders: {orderSub.Error}");
    await publicSocket.UnsubscribeAsync(tickerSub.Data);
    await publicSocket.UnsubscribeAsync(klineSub.Data);
    await publicSocket.UnsubscribeAsync(futuresTickerSub.Data);
    return;
}

var balanceSub = await authSocket.SpotApi.SubscribeToBalanceUpdatesAsync(update =>
{
    Console.WriteLine($"Balance {update.Data.Asset}: available={update.Data.Available}, hold={update.Data.Hold}");
});

if (!balanceSub.Success)
{
    Console.WriteLine($"Failed to subscribe spot balances: {balanceSub.Error}");
    await publicSocket.UnsubscribeAsync(tickerSub.Data);
    await publicSocket.UnsubscribeAsync(klineSub.Data);
    await publicSocket.UnsubscribeAsync(futuresTickerSub.Data);
    await authSocket.UnsubscribeAsync(orderSub.Data);
    return;
}

Console.WriteLine("Subscriptions active. Press Enter to teardown...");
Console.ReadLine();

// ---- 3. TEARDOWN ----
await publicSocket.UnsubscribeAsync(tickerSub.Data);
await publicSocket.UnsubscribeAsync(klineSub.Data);
await publicSocket.UnsubscribeAsync(futuresTickerSub.Data);
await authSocket.UnsubscribeAsync(orderSub.Data);
await authSocket.UnsubscribeAsync(balanceSub.Data);

Console.WriteLine("Clean shutdown complete.");

// Common variations:
//   Spot order book: publicSocket.SpotApi.SubscribeToOrderBookUpdatesAsync(spotSymbol, handler)
//   Spot trades: publicSocket.SpotApi.SubscribeToTradeUpdatesAsync(spotSymbol, handler)
//   Futures klines: publicSocket.FuturesApi.SubscribeToKlineUpdatesAsync(futuresSymbol, FuturesKlineIntervalStream.OneMinute, handler)
//   Futures user positions: authSocket.FuturesApi.SubscribeToPositionUpdatesAsync(handler)
