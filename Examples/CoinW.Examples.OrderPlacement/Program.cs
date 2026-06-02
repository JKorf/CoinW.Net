using CoinW.Net;
using CoinW.Net.Clients;
using CoinW.Net.Enums;
using System.Linq;

const string spotSymbol = "BTC_USDT";
const string futuresSymbol = "ETH_USDT";

// Replace with valid credentials or order placement will always fail
var apiKey = "KEY";
var apiSecret = "SECRET";

Console.WriteLine("CoinW.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new CoinWRestClient(options =>
{
    options.ApiCredentials = new CoinWCredentials(apiKey, apiSecret);
});

await PlaceSpotLimitOrderAsync(client);
Console.WriteLine();
await PlaceFuturesLimitOrderExampleAsync(client);

static async Task PlaceSpotLimitOrderAsync(CoinWRestClient client)
{
    Console.WriteLine($"Placing spot limit buy order for {spotSymbol}...");

    var tickers = await client.SpotApi.ExchangeData.GetTickersAsync();
    if (!tickers.Success)
    {
        Console.WriteLine($"Failed to get spot tickers: {tickers.Error}");
        return;
    }

    var ticker = tickers.Data.Single(x => x.Symbol == spotSymbol);
    var safePrice = Math.Round(ticker.LastPrice * 0.95m, 2);
    var order = await client.SpotApi.Trading.PlaceOrderAsync(
        symbol: spotSymbol,
        side: OrderSide.Buy,
        type: OrderType.Limit,
        quantity: 0.001m,
        quoteQuantity: null,
        price: safePrice);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place spot order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed spot order {order.Data.OrderId}");

    var orderStatus = await client.SpotApi.Trading.GetOrderAsync(order.Data.OrderId);
    if (orderStatus.Success)
        Console.WriteLine($"Spot order status: {orderStatus.Data.Status}, filled: {orderStatus.Data.QuantityFilled}");
    else
        Console.WriteLine($"Failed to query spot order: {orderStatus.Error}");

    var cancel = await client.SpotApi.Trading.CancelOrderAsync(order.Data.OrderId);
    Console.WriteLine(cancel.Success
        ? $"Cancelled spot order {order.Data.OrderId}"
        : $"Failed to cancel spot order: {cancel.Error}");
}

static async Task PlaceFuturesLimitOrderExampleAsync(CoinWRestClient client)
{
    Console.WriteLine($"Placing futures limit short order for {futuresSymbol}...");

    var ticker = await client.FuturesApi.ExchangeData.GetTickerAsync(futuresSymbol);
    if (!ticker.Success)
    {
        Console.WriteLine($"Failed to get futures ticker: {ticker.Error}");
        return;
    }

    var safePrice = Math.Round(ticker.Data.LastPrice * 1.05m, 2);
    var order = await client.FuturesApi.Trading.PlaceOrderAsync(
        symbol: futuresSymbol,
        side: PositionSide.Short,
        orderType: FuturesOrderType.Plan,
        quantity: 1,
        leverage: 1,
        price: safePrice,
        quantityUnit: QuantityUnit.Contracts,
        marginType: MarginType.IsolatedMargin);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place futures order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed futures order {order.Data.OrderId}");

    var openOrders = await client.FuturesApi.Trading.GetOpenOrdersAsync(
        FuturesOrderType.Plan,
        symbol: futuresSymbol,
        orderIds: new[] { order.Data.OrderId });
    if (openOrders.Success)
    {
        var orderStatus = openOrders.Data.SingleOrDefault(x => x.Id == order.Data.OrderId);
        Console.WriteLine(orderStatus == null
            ? "Futures order was not found in open orders"
            : $"Futures order status: {orderStatus.OrderStatus}, filled: {orderStatus.QuantityFilled}");
    }
    else
    {
        Console.WriteLine($"Failed to query futures order: {openOrders.Error}");
    }

    var cancel = await client.FuturesApi.Trading.CancelOrderAsync(order.Data.OrderId);
    Console.WriteLine(cancel.Success
        ? $"Cancelled futures order {order.Data.OrderId}"
        : $"Failed to cancel futures order: {cancel.Error}");
}
