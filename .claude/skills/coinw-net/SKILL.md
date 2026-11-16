---
name: coinw-net
description: Implement or review CoinW.Net spot and futures REST calls, account and trading workflows, WebSocket subscriptions, authentication, Shared API integration, and dependency injection in .NET applications.
---
# CoinW.Net

Install with `dotnet add package CoinW.Net`.

- Reuse `CoinWRestClient` and `CoinWSocketClient`, or use dependency injection. Both expose `SpotApi` and `FuturesApi`; REST groups are `ExchangeData`, `Account`, and `Trading`.
- Public market data needs no credentials. Private operations use `CoinWCredentials(key, secret)` without a passphrase.
- Native spot symbols use underscores, such as `BTC_USDT`; USDT futures commonly use base symbols such as `BTC` or `ETH`. Discover supported instruments rather than copying another exchange's format.
- REST returns `HttpResult<T>` / `HttpResult`, subscriptions return `WebSocketResult<UpdateSubscription>`, and non-I/O shared helpers can return `ExchangeCallResult<T>`. Check `Success` before reading `Data`, and handle `Error` on failure. Inspect individual results after a successful batch request.
- Keep the concrete socket client and unsubscribe successful subscriptions on shutdown.
- Spot ticker reads use `GetTickersAsync()` and filter the array. Futures have separate single/all-ticker methods. Spot REST, spot socket, futures REST, and futures socket candle intervals use different enum types.
- Futures placement uses `PositionSide`, `FuturesOrderType`, quantity, and required leverage. Specify quantity units and margin type for the intended workflow. Closing positions uses separate methods.
- Use each API root's `SharedApi` for Shared API V2 and depend on the required capability interface. Inspect the chosen capability's options and prerequisites before using a portable request.

## References

- Read [API surfaces](references/api-surfaces.md) for routing, Shared API capabilities, symbol catalogs, quantity representations, and position prerequisites.
- Read [usage](references/usage.md) for authentication, market/account reads, trading, subscription cleanup, Shared API V2, and DI snippets.
- Read [operational considerations](references/safety.md) for futures exposure, batch outcomes, submission recovery, transfers, and withdrawals.

Checkout-only: [repository instructions](../../../AGENTS.md) and [complete examples](../../../Examples/ai-friendly/).
