// 02-futures.cs
//
// Demonstrates: CoinW futures ticker, margin configuration, market order,
// retrieve open position, close position.
//
// Setup: dotnet add package CoinW.Net
// Substitute API_KEY / API_SECRET. The API key must have futures trading enabled.

using CoinW.Net;
using CoinW.Net.Clients;
using CoinW.Net.Enums;

var client = new CoinWRestClient(options =>
{
    options.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});

const string symbol = "ETH";

// ---- 1. PUBLIC FUTURES MARKET DATA ----
var ticker = await client.FuturesApi.ExchangeData.GetTickerAsync(symbol);
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get futures ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"{symbol} futures last price: {ticker.Data.LastPrice}");

// ---- 2. SET MARGIN MODE ----
// Margin mode is account configuration. Pick the position-combine mode that matches
// the user's account strategy before placing real orders.
var marginMode = await client.FuturesApi.Account.SetMarginModeAsync(
    MarginType.IsolatedMargin,
    PositionCombineType.Split);

if (!marginMode.Success)
{
    Console.WriteLine($"Failed to set margin mode: {marginMode.Error}");
    return;
}

// ---- 3. PLACE MARKET ORDER (open long position) ----
// Futures order placement requires symbol, side, order type, quantity, and leverage.
// quantityUnit clarifies whether the quantity is contracts, base asset, or quote asset.
var openOrder = await client.FuturesApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: PositionSide.Long,
    orderType: FuturesOrderType.Market,
    quantity: 1m,
    leverage: 5,
    quantityUnit: QuantityUnit.Contracts,
    marginType: MarginType.IsolatedMargin);

if (!openOrder.Success)
{
    Console.WriteLine($"Failed to open position: {openOrder.Error}");
    return;
}

Console.WriteLine($"Opened position via order {openOrder.Data.OrderId}");

// ---- 4. GET CURRENT POSITION ----
var positions = await client.FuturesApi.Trading.GetPositionsAsync(symbol);
if (!positions.Success)
{
    Console.WriteLine($"Failed to get positions: {positions.Error}");
    return;
}

var position = positions.Data.FirstOrDefault(p => p.Status == OpenStatus.Open && p.TotalQuantity != 0);
if (position == null)
{
    Console.WriteLine("No open position found. The order may not have created an open position yet.");
    return;
}

Console.WriteLine($"Position {position.Id}: {position.PositionSide} {position.TotalQuantity} {position.Symbol}");
Console.WriteLine($"Open price: {position.OpenPrice}, unrealized PnL: {position.UnrealizedPnl}");

// ---- 5. CLOSE THE POSITION ----
var close = await client.FuturesApi.Trading.ClosePositionAsync(position.Id, FuturesOrderType.Market);
if (close.Success)
{
    Console.WriteLine($"Closed position via order {close.Data.OrderId}");
}
else
{
    Console.WriteLine($"Failed to close position: {close.Error}");
}

// Common variations:
//   Limit/planned order: FuturesOrderType.Plan with price: 2000m
//   Short position: PositionSide.Short
//   Cross margin: MarginType.CrossMargin
//   Close all positions for symbol: client.FuturesApi.Trading.CloseAllPositionsAsync(symbol)
//   Take profit / stop loss: client.FuturesApi.Trading.SetTpSlAsync(...)
