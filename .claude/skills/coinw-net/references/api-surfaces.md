# API surfaces

## Client roots and symbols

`client` below is a `CoinWRestClient`; `socketClient` is a `CoinWSocketClient`. Both expose `SpotApi` and `FuturesApi`. Each REST root has `ExchangeData`, `Account`, and `Trading` groups. Socket methods live directly on the product root.

Spot product IDs use underscores, for example `BTC_USDT`; USDT perpetual futures commonly use `BTC` or `ETH`. Query the selected API's symbol metadata. `CoinWExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode)` provides native formatting when shared requests are not used.

Spot REST candles use `KlineInterval`, spot socket candles use `KlineIntervalStream`, futures REST uses `FuturesKlineInterval`, and futures sockets use `FuturesKlineIntervalStream`.

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

## Shared API V2

Use `client.SpotApi.SharedApi`, `client.FuturesApi.SharedApi`, `socketClient.SpotApi.SharedApi`, and `socketClient.FuturesApi.SharedApi`. The DI aggregate `ICoinWSharedApiClient` exposes `SpotRest`, `FuturesRest`, `SpotSocket`, and `FuturesSocket`.

| Surface | Representative capabilities |
| --- | --- |
| Spot REST | `IGetSpotSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IGetBalancesRest`, `IGetAssetRest`, `IGetAllAssetsRest`, deposits/withdrawals, `ITransferRest`, `IPlaceSpotOrderRest`, order lifecycle and trades |
| Futures REST | `IGetFuturesSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IGetFeesRest`, `IGetBalancesRest`, `IPlaceFuturesOrderRest`, order lifecycle and trades, `IGetPositionsRest`, `ICloseFullPositionRest`, `ISetFuturesTpSlRest`, `ICancelFuturesTpSlRest` |
| Spot socket | `ISubscribeBalancesSocket`, `ISubscribeKlinesSocket`, `ISubscribeOrderBookSocket`, `ISubscribeTickerSocket`, `ISubscribeAllTickersSocket`, `ISubscribeTradesSocket`, `ISubscribeSpotOrdersSocket` |
| Futures socket | `ISubscribeBalancesSocket`, `ISubscribeKlinesSocket`, `ISubscribeOrderBookSocket`, `ISubscribeTickerSocket`, `ISubscribeTradesSocket`, `ISubscribeFuturesOrdersSocket`, `ISubscribePositionsSocket` |

Both REST surfaces provide candles, order books, and recent trades. Native futures leverage/configuration and funding endpoints are broader than the shared capability interface; do not infer shared support from the native method alone.

For dynamic selection, call the aggregate's `GetCapability` with a descriptor such as `SharedCapabilities.Tickers.GetTicker.Rest` and the required trading mode. Check for a missing match before accessing `Capability` and `Options`. `GetCapabilities` enumerates implementations and `Discover()` describes supported operations. With multiple exchanges registered, resolve `IEnumerable<TCapability>` and select by exchange and supported mode.

### Catalogs, quantities, and futures prerequisites

Call `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())` or `IGetFuturesSymbolsRest.GetFuturesSymbolsAsync(new GetSymbolsRequest())` successfully before reading `SpotSymbolCatalog` or `FuturesSymbolCatalog`. Requests support base/quote asset type and subtype filters; results include `DisplayName`, constraints, and classifications (`Crypto`, `Fiat`, `TradFi`; applicable `StableCoin`, `Equity`, `Commodity` subtypes). CoinW spot metadata classifies crypto and known stablecoins; futures can classify known commodity/equity underlyings.

Shared quantities use `SharedOrderQuantity`. Spot limit orders use base quantity; market orders can use base or quote quantity. Futures opening requests can use base, quote, or contracts, while closing requests use contracts. Read the capability's quantity support for the selected order side/type. Spot shared books use `SharedQuantityType.BaseAsset`; futures REST and socket books use `SharedQuantityType.Contracts`.

Shared futures placement requires `PositionSide`, and opening a position also requires `Leverage`. Side and position side determine whether the adapter opens or closes exposure. Closing requests require `PositionId` in `ExchangeParameters` (alias `id` accepted by placement); full-position close requires `PositionId`. Do not substitute an order ID for a position ID. Shared limit opening maps to native `FuturesOrderType.Plan`; market opening maps to `Market`.

Shared REST returns `HttpResult<T>` / `HttpResult`, subscriptions return `WebSocketResult<UpdateSubscription>`, and non-I/O symbol/cache helpers can return `ExchangeCallResult<T>`.

## Environments and helpers

`CoinWEnvironment.Live` is the built-in environment. `CreateCustom` takes a name, REST address, and socket address; custom endpoints do not by themselves provide simulated trading.

Checkout-only: [public interfaces](../../../../CoinW.Net/Interfaces/Clients/), [local order books](../../../../CoinW.Net/SymbolOrderBooks/), and [tracker factory](../../../../CoinW.Net/CoinWTrackerFactory.cs). Prefer a maintained local book when the application needs synchronized depth rather than individual incremental messages.
