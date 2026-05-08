# CoinW.Net AI API Quick Map

Use this file to route common user intents to the correct CoinW.Net client member. If a method name or parameter is not listed here, inspect `CoinW.Net/Interfaces/Clients/**` before generating code.

## Client Roots

| Intent | Use |
|---|---|
| REST calls | `new CoinWRestClient()` |
| WebSocket streams | `new CoinWSocketClient()` |
| API key authentication | `options.ApiCredentials = new CoinWCredentials("key", "secret")` |
| Live environment | `CoinWEnvironment.Live` |
| Custom environment | `CoinWEnvironment.CreateCustom(...)` |
| Dependency injection | `services.AddCoinW(options => { ... })` |
| Spot symbol format | `"BTC_USDT"` |
| USDT futures symbol format | `"BTC"` / `"ETH"` |

## Spot REST

| User intent | CoinW.Net member |
|---|---|
| Get all spot tickers | `client.SpotApi.ExchangeData.GetTickersAsync()` |
| Get latest spot ticker for one symbol | `client.SpotApi.ExchangeData.GetTickersAsync()` then filter by `Symbol == "BTC_USDT"` |
| Get spot assets and networks | `client.SpotApi.ExchangeData.GetAssetsAsync()` |
| Get spot symbols | `client.SpotApi.ExchangeData.GetSymbolsAsync()` |
| Get spot order book | `client.SpotApi.ExchangeData.GetOrderBookAsync("BTC_USDT")` |
| Get spot order book with limit | `client.SpotApi.ExchangeData.GetOrderBookAsync("BTC_USDT", limit)` |
| Get recent spot trades | `client.SpotApi.ExchangeData.GetRecentTradesAsync("BTC_USDT")` |
| Get recent spot trades by time | `client.SpotApi.ExchangeData.GetRecentTradesAsync("BTC_USDT", startTime, endTime)` |
| Get spot klines/candles | `client.SpotApi.ExchangeData.GetKlinesAsync("BTC_USDT", KlineInterval.OneMinute)` |
| Get spot balances | `client.SpotApi.Account.GetBalancesAsync()` |
| Get detailed spot balances | `client.SpotApi.Account.GetBalancesDetailsAsync()` |
| Get deposit/withdrawal history for one asset | `client.SpotApi.Account.GetDepositWithdrawalHistoryAsync("USDT")` |
| Get deposit/withdrawal history for assets | `client.SpotApi.Account.GetDepositWithdrawalHistoryAsync(new[] { "BTC", "USDT" })` |
| Get deposit addresses | `client.SpotApi.Account.GetDepositAddressesAsync("USDT", "TRC20")` |
| Withdraw asset | `client.SpotApi.Account.WithdrawAsync(asset, quantity, address, network, ...)` |
| Cancel withdrawal | `client.SpotApi.Account.CancelWithdrawalAsync(withdrawalId)` |
| Transfer between account types | `client.SpotApi.Account.TransferAsync(fromAccount, toAccount, asset, quantity)` |
| Place spot order | `client.SpotApi.Trading.PlaceOrderAsync("BTC_USDT", OrderSide.Buy, OrderType.Limit, ...)` |
| Place spot market order | `client.SpotApi.Trading.PlaceOrderAsync("BTC_USDT", OrderSide.Buy, OrderType.Market, quantity: ...)` |
| Cancel spot order | `client.SpotApi.Trading.CancelOrderAsync(orderId)` |
| Cancel all spot orders | `client.SpotApi.Trading.CancelAllOrdersAsync("BTC_USDT")` |
| Get open spot orders | `client.SpotApi.Trading.GetOpenOrdersAsync("BTC_USDT")` |
| Get spot order details | `client.SpotApi.Trading.GetOrderAsync(orderId)` |
| Get spot order transaction history | `client.SpotApi.Trading.GetOrderTransactionHistoryAsync("BTC_USDT")` |
| Get spot user trades | `client.SpotApi.Trading.GetUserTradesAsync("BTC_USDT")` |

## Futures REST

| User intent | CoinW.Net member |
|---|---|
| Get futures symbols/instruments | `client.FuturesApi.ExchangeData.GetSymbolsAsync()` |
| Get one futures symbol/instrument | `client.FuturesApi.ExchangeData.GetSymbolsAsync("BTC")` |
| Get futures ticker | `client.FuturesApi.ExchangeData.GetTickerAsync("BTC")` |
| Get all futures tickers | `client.FuturesApi.ExchangeData.GetTickersAsync()` |
| Get futures klines | `client.FuturesApi.ExchangeData.GetKlinesAsync("BTC", FuturesKlineInterval.OneMinute, ...)` |
| Get last funding rate | `client.FuturesApi.ExchangeData.GetLastFundingRateAsync("BTC")` |
| Get futures order book | `client.FuturesApi.ExchangeData.GetOrderBookAsync("BTC")` |
| Get recent futures trades | `client.FuturesApi.ExchangeData.GetRecentTradesAsync("BTC")` |
| Get margin requirements | `client.FuturesApi.ExchangeData.GetMarginRequirementsAsync()` |
| Get futures public trade history | `client.FuturesApi.ExchangeData.GetTradeHistoryAsync("BTC")` |
| Get futures leverage by position or order | `client.FuturesApi.Account.GetLeverageAsync(positionId, orderId)` |
| Get margin rate for position | `client.FuturesApi.Account.GetMarginRateAsync(positionId)` |
| Get max buy/sell size | `client.FuturesApi.Account.GetMaxTradeSizeAsync(symbol, leverage, marginType, orderPrice)` |
| Get max transferable quantity | `client.FuturesApi.Account.GetMaxTransferableAsync()` |
| Get futures balances | `client.FuturesApi.Account.GetBalancesAsync()` |
| Get futures fees | `client.FuturesApi.Account.GetFeesAsync()` |
| Get futures margin mode | `client.FuturesApi.Account.GetMarginModeAsync()` |
| Set futures margin mode | `client.FuturesApi.Account.SetMarginModeAsync(marginType, positionCombineType)` |
| Enable or disable mega coupon | `client.FuturesApi.Account.ToggleMegaCouponAsync(enabled)` |
| Get max position size | `client.FuturesApi.Account.GetMaxPositionSizeAsync("BTC")` |
| Place futures order | `client.FuturesApi.Trading.PlaceOrderAsync("BTC", PositionSide.Long, FuturesOrderType.Market, quantity, leverage, ...)` |
| Place futures limit/planned order | `client.FuturesApi.Trading.PlaceOrderAsync("BTC", PositionSide.Long, FuturesOrderType.Plan, quantity, leverage, price: price)` |
| Place multiple futures orders | `client.FuturesApi.Trading.PlaceMultipleOrdersAsync(requests)` |
| Close futures position | `client.FuturesApi.Trading.ClosePositionAsync(positionId, ...)` |
| Close futures positions by client ids | `client.FuturesApi.Trading.ClosePositionsByClientOrderIdAsync(clientOrderIds)` |
| Close all futures positions for symbol | `client.FuturesApi.Trading.CloseAllPositionsAsync("BTC")` |
| Reverse futures position | `client.FuturesApi.Trading.ReversePositionAsync(positionId)` |
| Adjust position margin | `client.FuturesApi.Trading.AdjustMarginAsync(positionId, addMargin, reduceMargin)` |
| Set futures TP/SL | `client.FuturesApi.Trading.SetTpSlAsync(orderOrPositionId, symbol, ...)` |
| Set trailing TP/SL | `client.FuturesApi.Trading.SetTrailingTpSlAsync(positionId, callbackRate, ...)` |
| Edit futures order | `client.FuturesApi.Trading.EditOrderAsync(orderId, symbol, side, orderType, quantity, leverage, ...)` |
| Cancel futures order | `client.FuturesApi.Trading.CancelOrderAsync(orderId)` |
| Cancel multiple futures orders | `client.FuturesApi.Trading.CancelOrdersAsync(orderIds)` |
| Get futures open orders page | `client.FuturesApi.Trading.GetOpenOrdersAsync("BTC", FuturesOrderType.Plan)` |
| Get futures open orders by filter | `client.FuturesApi.Trading.GetOpenOrdersAsync(FuturesOrderType.Plan, symbol: "BTC")` |
| Get futures open order count | `client.FuturesApi.Trading.GetOpenOrderCountAsync()` |
| Get futures TP/SL orders | `client.FuturesApi.Trading.GetTpSlAsync(...)` |
| Get futures trailing TP/SL orders | `client.FuturesApi.Trading.GetTrailingTpSlAsync()` |
| Get futures order history, 7 days | `client.FuturesApi.Trading.GetOrderHistory7DaysAsync(...)` |
| Get futures order history, 3 months | `client.FuturesApi.Trading.GetOrderHistory3MonthsAsync(...)` |
| Get futures positions by symbol | `client.FuturesApi.Trading.GetPositionsAsync("BTC")` |
| Get all futures positions | `client.FuturesApi.Trading.GetPositionsAsync()` |
| Get futures position history | `client.FuturesApi.Trading.GetPositionHistoryAsync(...)` |
| Get futures transactions, 3 days | `client.FuturesApi.Trading.GetTransactionHistory3DaysAsync("BTC")` |
| Get futures transactions, 3 months | `client.FuturesApi.Trading.GetTransactionHistory3MonthsAsync("BTC")` |

## Spot WebSocket

| User intent | CoinW.Net member |
|---|---|
| Subscribe spot ticker updates | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync("BTC_USDT", handler)` |
| Subscribe all spot ticker updates | `socketClient.SpotApi.SubscribeToAllTickerUpdatesAsync(handler)` |
| Subscribe spot incremental order book | `socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync("BTC_USDT", handler)` |
| Subscribe spot partial order book snapshots | `socketClient.SpotApi.SubscribeToPartialOrderBookUpdatesAsync("BTC_USDT", handler)` |
| Subscribe spot klines | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync("BTC_USDT", KlineIntervalStream.OneMinute, handler)` |
| Subscribe spot trades | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync("BTC_USDT", handler)` |
| Subscribe spot balance updates | `socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(handler)` |
| Subscribe spot order updates | `socketClient.SpotApi.SubscribeToOrderUpdatesAsync(handler)` |

## Futures WebSocket

| User intent | CoinW.Net member |
|---|---|
| Subscribe futures ticker updates | `socketClient.FuturesApi.SubscribeToTickerUpdatesAsync("BTC", handler)` |
| Subscribe futures order book updates | `socketClient.FuturesApi.SubscribeToOrderBookUpdatesAsync("BTC", handler)` |
| Subscribe futures trade updates | `socketClient.FuturesApi.SubscribeToTradeUpdatesAsync("BTC", handler)` |
| Subscribe futures klines | `socketClient.FuturesApi.SubscribeToKlineUpdatesAsync("BTC", FuturesKlineIntervalStream.OneMinute, handler)` |
| Subscribe futures index price | `socketClient.FuturesApi.SubscribeToIndexPriceUpdatesAsync("BTC", handler)` |
| Subscribe futures mark price | `socketClient.FuturesApi.SubscribeToMarkPriceUpdatesAsync("BTC", handler)` |
| Subscribe futures funding rate | `socketClient.FuturesApi.SubscribeToFundingRateUpdatesAsync("BTC", handler)` |
| Subscribe futures order updates | `socketClient.FuturesApi.SubscribeToOrderUpdatesAsync(handler)` |
| Subscribe futures position updates | `socketClient.FuturesApi.SubscribeToPositionUpdatesAsync(handler)` |
| Subscribe futures position detail updates | `socketClient.FuturesApi.SubscribeToPositionDetailUpdatesAsync(handler)` |
| Subscribe futures balance updates | `socketClient.FuturesApi.SubscribeToBalanceUpdatesAsync(handler)` |
| Subscribe futures margin config updates | `socketClient.FuturesApi.SubscribeToMarginConfigUpdatesAsync(handler)` |

## SharedApis

Use SharedApis for exchange-agnostic code across CoinW, Binance, Bybit, OKX, Kraken, and other CryptoExchange.Net libraries.

| User intent | CoinW.Net member or interface |
|---|---|
| Shared spot REST client | `new CoinWRestClient().SpotApi.SharedClient` |
| Shared futures REST client | `new CoinWRestClient().FuturesApi.SharedClient` |
| Shared spot socket client | `new CoinWSocketClient().SpotApi.SharedClient` |
| Shared futures socket client | `new CoinWSocketClient().FuturesApi.SharedClient` |
| Shared spot ticker REST | `ISpotTickerRestClient.GetSpotTickerAsync(new GetTickerRequest(symbol))` |
| Shared spot order REST | `ISpotOrderRestClient.PlaceSpotOrderAsync(...)` |
| Shared futures order REST | `IFuturesOrderRestClient.PlaceFuturesOrderAsync(...)` |
| Shared ticker socket | `ITickerSocketClient.SubscribeToTickerUpdatesAsync(...)` |
| Shared order book socket | `IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(...)` |

For shared socket subscriptions, keep the concrete socket client and unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

## Result Handling

| Situation | Pattern |
|---|---|
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Retry decision | Retry only when `result.Error?.IsTransient == true` |
| Cancellation | Pass `ct: cancellationToken` |

## Common Routing Pitfalls

| Do not use | Use instead |
|---|---|
| `CoinWClient` | `CoinWRestClient` / `CoinWSocketClient` |
| `ApiCredentials` | `CoinWCredentials` |
| `SpotApi.ExchangeData.GetTickerAsync("BTC_USDT")` | `GetTickersAsync()` then filter the returned array |
| `SpotApi.ExchangeData.GetServerTimeAsync()` | CoinW.Net does not expose a server time method |
| `UsdFuturesApi` / `CoinFuturesApi` | `FuturesApi` |
| Binance spot symbol `"BTCUSDT"` | CoinW spot symbol `"BTC_USDT"` |
| Futures symbol `"BTC_USDT"` for USDT perpetuals | CoinW futures symbol `"BTC"` |
| `.Data` without `.Success` check | Check `.Success` first |
| Custom `clientOrderId` by default | Let CoinW generate ids unless correlation is required |
