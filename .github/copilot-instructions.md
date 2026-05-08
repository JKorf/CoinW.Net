# Copilot Instructions for CoinW.Net

This repository is **CoinW.Net**, a strongly typed C#/.NET client library for the CoinW cryptocurrency exchange API. It is part of the CryptoExchange.Net ecosystem.

When generating code that consumes CoinW.Net, follow these conventions.

## Use CoinW.Net, Not Raw HTTP

Never generate raw `HttpClient` calls to CoinW endpoints. Always use `CoinWRestClient` or `CoinWSocketClient` so request signing, rate limiting, WebSocket reconnects, and result handling stay correct.

## Client Setup

```csharp
using CoinW.Net;
using CoinW.Net.Clients;

var restClient = new CoinWRestClient(options =>
{
    options.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});
```

## Result Handling

REST methods return `WebCallResult<T>` or `WebCallResult`. WebSocket subscriptions return `CallResult<UpdateSubscription>`. Always check `.Success` before reading `.Data`; errors are on `.Error`.

## API Structure

- `restClient.SpotApi.ExchangeData` for public spot market data
- `restClient.SpotApi.Account` for balances, deposits, withdrawals, transfers
- `restClient.SpotApi.Trading` for spot orders and user trades
- `restClient.FuturesApi.ExchangeData` for futures symbols, tickers, klines, books, trades, funding
- `restClient.FuturesApi.Account` for futures balances, fees, margin mode, leverage, limits
- `restClient.FuturesApi.Trading` for futures orders, positions, TP/SL, order history
- `socketClient.SpotApi` for spot WebSocket streams
- `socketClient.FuturesApi` for futures WebSocket streams

## Symbols

CoinW spot symbols use underscores, for example `BTC_USDT`. CoinW USDT futures commonly use base symbols, for example `BTC` or `ETH`. Do not generate Binance-style spot symbols such as `BTCUSDT` for spot endpoints.

## Order Placement

Spot:

```csharp
var order = await restClient.SpotApi.Trading.PlaceOrderAsync(
    "BTC_USDT", OrderSide.Buy, OrderType.Limit,
    quantity: 0.001m, price: 50000m);
```

Futures:

```csharp
var order = await restClient.FuturesApi.Trading.PlaceOrderAsync(
    "ETH", PositionSide.Long, FuturesOrderType.Market, 1m, 5,
    quantityUnit: QuantityUnit.Contracts,
    marginType: MarginType.IsolatedMargin);
```

Let CoinW generate order ids unless a user explicitly needs `clientOrderId` for external correlation.

## WebSocket Pattern

Store returned subscriptions and unsubscribe on shutdown:

```csharp
var socketClient = new CoinWSocketClient();
var sub = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "BTC_USDT",
    update => Console.WriteLine(update.Data.LastPrice));
if (!sub.Success) { Console.WriteLine(sub.Error); return; }

await socketClient.UnsubscribeAsync(sub.Data);
```

## Cross-Exchange

For code that needs to work across multiple exchanges, use `CryptoExchange.Net.SharedApis` interfaces (`ISpotTickerRestClient`, `ISpotOrderRestClient`, `IFuturesOrderRestClient`, etc.) accessed via `.SharedClient`.

## Avoid

- Raw `HttpClient` calls to CoinW
- Non-existent `CoinWClient` class
- Binance-specific `UsdFuturesApi` / `CoinFuturesApi` properties
- Binance spot symbols like `BTCUSDT` for CoinW spot endpoints
- Generic `ApiCredentials` when `CoinWCredentials` is available
- Synchronous `.Result` / `.Wait()`
- Instantiating clients per request
- Reading `.Data` before `.Success`

## Reference

For detailed patterns and pitfalls see `CLAUDE.md`, `llms.txt`, and `llms-full.txt` in the repository root. For compilable examples, see `Examples/ai-friendly/`.
