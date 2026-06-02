
using CoinW.Net.Clients;
using System.Linq;

// REST
var restClient = new CoinWRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetTickersAsync();
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client ticker price for ETH_USDT: {ticker.Data.Single(x => x.Symbol == "ETH_USDT").LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new CoinWSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH_USDT", update =>
{
    Console.WriteLine($"Websocket client ticker price for ETH_USDT: {update.Data.LastPrice}");
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to ticker updates: {subscription.Error}");
    return;
}

Console.ReadLine();
