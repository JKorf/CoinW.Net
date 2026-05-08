// 01-spot-quickstart.cs
//
// Demonstrates: client setup, public market data, authenticated balances,
// limit order placement, order status check.
//
// Setup:
//   dotnet new console -n SpotQuickstart && cd SpotQuickstart
//   dotnet add package CoinW.Net
//   Copy this file content into Program.cs
//   Substitute API_KEY / API_SECRET below
//   dotnet run

using CoinW.Net;
using CoinW.Net.Clients;
using CoinW.Net.Enums;

const string spotSymbol = "BTC_USDT";

// ---- 1. PUBLIC CLIENT (no credentials needed for market data) ----
// Reuse this client across the application. Do not create clients per request.
var publicClient = new CoinWRestClient();

// CoinW spot exposes all tickers in one call. Filter the returned array for one symbol.
var tickers = await publicClient.SpotApi.ExchangeData.GetTickersAsync();
if (!tickers.Success)
{
    Console.WriteLine($"Failed to get tickers: {tickers.Error}");
    return;
}

var ticker = tickers.Data.SingleOrDefault(x => x.Symbol == spotSymbol);
if (ticker == null)
{
    Console.WriteLine($"Ticker {spotSymbol} was not returned by CoinW.");
    return;
}

Console.WriteLine($"{spotSymbol} last price: {ticker.LastPrice}");
Console.WriteLine($"24h volume: {ticker.Volume}");

// ---- 2. AUTHENTICATED CLIENT (for account / trading) ----
var tradingClient = new CoinWRestClient(options =>
{
    options.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});

var balances = await tradingClient.SpotApi.Account.GetBalancesDetailsAsync();
if (!balances.Success)
{
    Console.WriteLine($"Failed to get balances: {balances.Error}");
    return;
}

foreach (var balance in balances.Data.Where(x => x.Value.Available + x.Value.OnOrders > 0))
{
    Console.WriteLine($"{balance.Key}: {balance.Value.Available} available, {balance.Value.OnOrders} on orders");
}

// ---- 3. PLACE A LIMIT BUY ORDER ----
// Limit buy 0.001 BTC at a price below the current price. This is only an example:
// validate symbol rules, minimum quantity, and current liquidity before using real funds.
var safePrice = Math.Round(ticker.LastPrice * 0.95m, 2);

var order = await tradingClient.SpotApi.Trading.PlaceOrderAsync(
    symbol: spotSymbol,
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.001m,
    price: safePrice);

if (!order.Success)
{
    Console.WriteLine($"Failed to place order: {order.Error}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId} at {safePrice}");

// ---- 4. CHECK ORDER STATUS ----
var status = await tradingClient.SpotApi.Trading.GetOrderAsync(order.Data.OrderId);
if (status.Success)
{
    Console.WriteLine($"Order status: {status.Data.Status}, filled: {status.Data.QuantityFilled}");
}
else
{
    Console.WriteLine($"Failed to query order: {status.Error}");
}

// ---- 5. CANCEL THE ORDER (cleanup for this example) ----
var cancel = await tradingClient.SpotApi.Trading.CancelOrderAsync(order.Data.OrderId);
if (cancel.Success)
{
    Console.WriteLine($"Cancelled order {order.Data.OrderId}");
}

// Common variations:
//   Market order: type: OrderType.Market, omit price
//   Quote quantity: pass quoteQuantity instead of quantity when that is the desired order size
//   Open orders: tradingClient.SpotApi.Trading.GetOpenOrdersAsync(spotSymbol)
//   Symbol list: publicClient.SpotApi.ExchangeData.GetSymbolsAsync()
