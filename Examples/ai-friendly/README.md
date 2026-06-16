# AI-Friendly Examples

These examples are optimized for AI coding assistants and quick onboarding. Each file is:

- **Compilable**: drop into a console project with `dotnet add package CoinW.Net` and it builds.
- **Self-contained**: single file, no external setup, no shared helpers.
- **Commented**: explains why the calls are made and where CoinW differs from Binance-style APIs.
- **Idiomatic**: follows current CoinW.Net 2.x and CryptoExchange.Net patterns.

## Files

| File | What it shows |
|---|---|
| `01-spot-quickstart.cs` | Client setup, public ticker lookup, authenticated balances, place limit order, query order status |
| `02-futures.cs` | Futures: ticker, margin mode, market order, position lookup, close position |
| `03-websocket.cs` | Subscribe to spot ticker, spot klines, spot user streams, and futures ticker streams with teardown |
| `04-multi-exchange.cs` | `CryptoExchange.Net.SharedApis` pattern for exchange-agnostic code |
| `05-error-handling.cs` | `HttpResult` patterns, retry, common CoinW error scenarios |

## Running

```bash
dotnet new console -n MyCoinWApp
cd MyCoinWApp
dotnet add package CoinW.Net
# Copy the example .cs file content into Program.cs
# Replace API_KEY / API_SECRET placeholders with your own for private endpoints
dotnet run
```
