# Usage examples

Snippets are independent. Those using `client` assume a reused `CoinWRestClient`, configured with credentials for private calls. Trading snippets submit real account changes when executed; see [operational considerations](safety.md).

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using CoinW.Net;
using CoinW.Net.Clients;
using CoinW.Net.Enums;
using CoinW.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
```

## Public spot tickers and results

```csharp
using var client = new CoinWRestClient();
var tickers = await client.SpotApi.ExchangeData.GetTickersAsync();
if (!tickers.Success)
{
    Console.WriteLine(tickers.Error);
    return;
}
Console.WriteLine(tickers.Data.FirstOrDefault(x => x.Symbol == "BTC_USDT")?.LastPrice);
```

## Credentials and balances

```csharp
using var client = new CoinWRestClient(options =>
{
    options.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});
var balances = await client.SpotApi.Account.GetBalancesDetailsAsync();
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
foreach (var balance in balances.Data)
    Console.WriteLine($"{balance.Key}: available={balance.Value.Available}");
```

## Spot limit order and cancellation

```csharp
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    "BTC_USDT", OrderSide.Buy, OrderType.Limit, quantity: 0.001m, price: 1000m);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
var cancel = await client.SpotApi.Trading.CancelOrderAsync(order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine(cancel.Error);
```

## Futures reads

```csharp
var positions = await client.FuturesApi.Trading.GetPositionsAsync("ETH");
if (!positions.Success)
{
    Console.WriteLine(positions.Error);
    return;
}
foreach (var position in positions.Data)
    Console.WriteLine($"{position.Id}: {position.PositionSide}, quantity={position.TotalQuantity}");
var balances = await client.FuturesApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
Console.WriteLine(balances.Data.AvailableUsdt);
```

## Futures planned order

This opens leveraged exposure when executed. Select quantity units, leverage, margin, and position side for the intended account workflow.

```csharp
var order = await client.FuturesApi.Trading.PlaceOrderAsync(
    symbol: "ETH", side: PositionSide.Long, orderType: FuturesOrderType.Plan,
    quantity: 1m, leverage: 5, price: 1000m,
    quantityUnit: QuantityUnit.Contracts, marginType: MarginType.IsolatedMargin);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
Console.WriteLine(order.Data.OrderId);
```

## WebSocket lifecycle

```csharp
using var socketClient = new CoinWSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "BTC_USDT", update => Console.WriteLine(update.Data.LastPrice));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

Configure credentials on the socket client for private spot/futures balance, order, and position streams. Use the same success checks and cleanup for private subscriptions.

## Shared API V2

```csharp
IGetTickerRest ticker = client.SpotApi.SharedApi;
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");
var result = await ticker.GetTickerAsync(new GetTickerRequest(symbol));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Read [API surfaces](api-surfaces.md#shared-api-v2) for catalogs, quantity options, and futures position prerequisites.

## Dependency injection

```csharp
var services = new ServiceCollection();
services.AddCoinW(options =>
{
    options.Rest.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
    options.Socket.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});
using var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<ICoinWRestClient>();
var shared = provider.GetRequiredService<ICoinWSharedApiClient>();
```

## Complete examples (checkout-only)

The documentation test compiles these programs without executing exchange calls. Running them can contact CoinW and execute their illustrated account operations.

- [Spot quickstart](../../../../Examples/ai-friendly/01-spot-quickstart.cs)
- [Futures](../../../../Examples/ai-friendly/02-futures.cs)
- [WebSockets](../../../../Examples/ai-friendly/03-websocket.cs)
- [Shared API](../../../../Examples/ai-friendly/04-multi-exchange.cs)
- [Error handling](../../../../Examples/ai-friendly/05-error-handling.cs)
