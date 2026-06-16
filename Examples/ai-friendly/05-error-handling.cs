// 05-error-handling.cs
//
// Demonstrates: HttpResult patterns, retry logic, and common CoinW error scenarios.
//
// Setup: dotnet add package CoinW.Net

using CoinW.Net;
using CoinW.Net.Clients;
using CoinW.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;

var client = new CoinWRestClient(options =>
{
    options.ApiCredentials = new CoinWCredentials("API_KEY", "API_SECRET");
});

// ---- 1. THE BASIC PATTERN ----
// Every REST method returns HttpResult<T> or HttpResult.
// .Success is true/false. .Data is valid only when .Success is true.
// .Error contains structured error info when .Success is false.

var result = await client.FuturesApi.ExchangeData.GetTickerAsync("BTC");

if (result.Success)
{
    Console.WriteLine($"BTC futures price: {result.Data.LastPrice}");
}
else
{
    Console.WriteLine($"Code:      {result.Error?.Code}");
    Console.WriteLine($"Message:   {result.Error?.Message}");
    Console.WriteLine($"Type:      {result.Error?.ErrorType}");
    Console.WriteLine($"Transient: {result.Error?.IsTransient}");
}

// ---- 2. SIMPLE RETRY WITH BACKOFF ----
// Retry only on transient errors such as rate limits, network blips, or server overload.
// Do not retry validation errors, bad credentials, or insufficient balance.

async Task<HttpResult<T>> WithRetry<T>(
    Func<Task<HttpResult<T>>> call,
    int maxAttempts = 3)
{
    HttpResult<T> last = default!;
    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        last = await call();
        if (last.Success)
            return last;

        if (last.Error?.IsTransient != true)
            return last;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt)));
    }

    return last;
}

var tickers = await WithRetry(() => client.SpotApi.ExchangeData.GetTickersAsync());
if (!tickers.Success)
{
    Console.WriteLine($"Ticker lookup failed: {tickers.Error}");
}

// ---- 3. COMMON COINW ERROR SCENARIOS ----
//
// Authentication or signature error:
//   API key, secret, permissions, timestamp, or selected environment is wrong.
//   Fix credentials and permissions; do not retry blindly.
//
// Invalid symbol:
//   Spot symbols use BTC_USDT. Futures USDT perpetuals commonly use BTC.
//   Use GetSymbolsAsync or SharedSymbol instead of guessing.
//
// Insufficient balance:
//   Permanent for this request. Surface to the caller.
//
// Invalid price, quantity, leverage, or margin mode:
//   Validate market rules, min quantities, quantity unit, leverage, and margin mode.
//
// Rate limit, network, or 5xx error:
//   May be transient. Retry only when result.Error?.IsTransient == true.

// ---- 4. SYMBOL FORMATTING ----
// CoinW spot and futures symbol formats differ. Use helpers or SharedApis when possible.
var spotSymbol = CoinWExchange.FormatSymbol("BTC", "USDT", TradingMode.Spot);
var futuresSymbol = CoinWExchange.FormatSymbol("BTC", "USDT", TradingMode.PerpetualLinear);

Console.WriteLine($"Spot symbol: {spotSymbol}");
Console.WriteLine($"Futures symbol: {futuresSymbol}");

// ---- 5. ORDER PLACEMENT WITH SUCCESS CHECKS ----
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: spotSymbol,
    side: OrderSide.Buy,
    type: OrderType.Market,
    quantity: 0.001m);

if (!order.Success)
{
    var category = order.Error?.IsTransient == true
        ? "Transient; retry may be appropriate"
        : "Permanent or validation issue; surface to caller";

    Console.WriteLine($"{category}: {order.Error?.Code} {order.Error?.Message}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId}");

// ---- 6. EXCEPTIONS VS ERROR RESULTS ----
// CoinW.Net returns API failures via HttpResult.Error, not thrown exceptions.
// Exceptions are reserved for misconfiguration, disposal, cancellation, or programming errors.

// Common variations:
//   With CancellationToken: pass `ct: cancellationToken` to any method
//   With timeout per request: options.RequestTimeout = TimeSpan.FromSeconds(10)
//   Polly integration: use Error.IsTransient as the retry predicate
