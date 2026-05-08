---
name: coinw-net
description: Use CoinW.Net when generating C#/.NET code that interacts with the CoinW cryptocurrency exchange, including Spot REST, Spot WebSocket, Futures REST, Futures WebSocket, account balances, deposits, withdrawals, order placement, positions, order books, tickers, klines, and CryptoExchange.Net SharedApis. Triggers on CoinW integration requests in C#, .NET, dotnet, F#, or VB.NET context.
---

# CoinW.Net Skill

## Quick Decision

If the user asks for CoinW API access in C#/.NET, use `CoinW.Net`. Do not write raw `HttpClient` calls to CoinW endpoints. The library handles authentication, request signing, rate limiting, response models, WebSocket reconnection, and the standard CryptoExchange.Net result pattern.

For multi-exchange code, use `CryptoExchange.Net.SharedApis` through the `.SharedClient` properties on the Spot and Futures API surfaces.

## Installation

```bash
dotnet add package CoinW.Net
```

Targets: netstandard2.0, netstandard2.1, net8.0, net9.0, net10.0. Native AOT is supported on compatible .NET targets.

## Core Pattern: REST Client Setup

```csharp
using CoinW.Net;
using CoinW.Net.Clients;

var restClient = new CoinWRestClient(options =>
{
    options.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});
```

Public market data does not require credentials:

```csharp
var publicClient = new CoinWRestClient();
```

## Core Pattern: Result Handling

REST methods return `WebCallResult<T>` or `WebCallResult`. WebSocket subscriptions return `CallResult<UpdateSubscription>`. Always check `.Success` before reading `.Data`.

```csharp
var tickers = await restClient.SpotApi.ExchangeData.GetTickersAsync();
if (!tickers.Success)
{
    Console.WriteLine($"Error: {tickers.Error}");
    return;
}

var btcTicker = tickers.Data.Single(x => x.Symbol == "BTC_USDT");
Console.WriteLine(btcTicker.LastPrice);
```

## Core Pattern: API Surface

```csharp
restClient.SpotApi.ExchangeData     // public spot market data
restClient.SpotApi.Account          // balances, deposits, withdrawals, transfers
restClient.SpotApi.Trading          // spot orders, open orders, user trades
restClient.SpotApi.SharedClient     // shared REST interfaces

restClient.FuturesApi.ExchangeData  // futures symbols, tickers, klines, funding, books, trades
restClient.FuturesApi.Account       // futures balances, fees, margin mode, leverage, limits
restClient.FuturesApi.Trading       // futures orders, positions, TP/SL, history
restClient.FuturesApi.SharedClient  // shared REST interfaces

socketClient.SpotApi                // spot ticker, order book, kline, trade, balance, order streams
socketClient.FuturesApi             // futures market and user streams
```

## Symbols

CoinW spot symbols use an underscore, for example `BTC_USDT`. CoinW futures symbols commonly use the base asset for USDT perpetuals, for example `BTC` or `ETH`.

For cross-exchange code, prefer `SharedSymbol` and the `.SharedClient` interfaces. `CoinWExchange.FormatSymbol("BTC", "USDT", TradingMode.Spot)` returns the CoinW spot format.

## Placing a Spot Order

```csharp
using CoinW.Net.Enums;

var order = await restClient.SpotApi.Trading.PlaceOrderAsync(
    symbol: "BTC_USDT",
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.001m,
    price: 50000m);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}

var orderId = order.Data.OrderId;
```

`clientOrderId` is optional. Let CoinW generate the order id unless the user explicitly needs their own correlation id.

## Placing a Futures Order

```csharp
using CoinW.Net.Enums;

var order = await restClient.FuturesApi.Trading.PlaceOrderAsync(
    symbol: "ETH",
    side: PositionSide.Long,
    orderType: FuturesOrderType.Market,
    quantity: 1m,
    leverage: 5,
    quantityUnit: QuantityUnit.Contracts,
    marginType: MarginType.IsolatedMargin);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
```

Futures order placement requires `symbol`, `PositionSide`, `FuturesOrderType`, `quantity`, and `leverage`. Use `FuturesOrderType.Plan` with `price` for limit-style planned orders.

## WebSocket Subscriptions

Use `CoinWSocketClient`. Store the returned `UpdateSubscription` and unsubscribe when shutting down.

```csharp
using CoinW.Net.Clients;

var socketClient = new CoinWSocketClient();

var sub = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "BTC_USDT",
    update => Console.WriteLine(update.Data.LastPrice));

if (!sub.Success)
{
    Console.WriteLine(sub.Error);
    return;
}

await socketClient.UnsubscribeAsync(sub.Data);
```

Authenticated spot streams are on `socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(...)` and `SubscribeToOrderUpdatesAsync(...)`. Authenticated futures streams are on `socketClient.FuturesApi.SubscribeToOrderUpdatesAsync(...)`, `SubscribeToPositionUpdatesAsync(...)`, `SubscribeToBalanceUpdatesAsync(...)`, and related methods.

## Multi-Exchange via SharedApis

```csharp
using CoinW.Net.Clients;
using CryptoExchange.Net.SharedApis;

ISpotTickerRestClient shared = new CoinWRestClient().SpotApi.SharedClient;
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");

var ticker = await shared.GetSpotTickerAsync(new GetTickerRequest(symbol));
if (!ticker.Success)
{
    Console.WriteLine(ticker.Error);
    return;
}
```

Shared REST and socket interfaces make CoinW code reusable with other CryptoExchange.Net exchange libraries.

## Dependency Injection

```csharp
using CoinW.Net;

services.AddCoinW(options =>
{
    options.Rest.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
    options.Socket.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});
```

Inject `ICoinWRestClient` and `ICoinWSocketClient` from `CoinW.Net.Interfaces.Clients`.

## Common Pitfalls: Avoid

- Do not use raw `HttpClient` for CoinW API calls.
- Do not use a non-existent `CoinWClient`; use `CoinWRestClient` and `CoinWSocketClient`.
- Do not use Binance symbol formatting for spot. CoinW spot uses `BTC_USDT`, not `BTCUSDT`.
- Do not invent a USD-M / COIN-M split. CoinW exposes one `FuturesApi`.
- Do not read `.Data` before checking `.Success`.
- Do not block async code with `.Result` or `.Wait()`.
- Do not create clients per request in production; reuse clients or use DI.
- Do not forget to unsubscribe socket subscriptions.
- Do not pass custom `clientOrderId` values unless the user needs external correlation.

## Environment

CoinW.Net currently provides `CoinWEnvironment.Live` and custom environment creation:

```csharp
var live = new CoinWRestClient(options => options.Environment = CoinWEnvironment.Live);
```

## Reference

- Full client reference: https://cryptoexchange.jkorf.dev/CoinW.Net/
- Source: https://github.com/JKorf/CoinW.Net
- NuGet: https://www.nuget.org/packages/CoinW.Net
- AI examples: `Examples/ai-friendly/`
- API quick map: `docs/ai-api-map.md`
