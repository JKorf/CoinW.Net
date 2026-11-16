# Operational considerations

## Credentials and account scope

Private operations require `CoinWCredentials(key, secret)` with no passphrase. Keep credentials outside source and select permissions for the intended spot/futures operations. `CoinWEnvironment.Live` is the built-in environment; a custom endpoint does not imply test execution.

Spot balances are keyed by asset. Futures balances include separate available USDT, available margin, frozen, and holding values; do not treat those fields as interchangeable withdrawable balances.

## Spot execution

Use spot symbol metadata for quantity minimums, steps, and price precision. Choose base `quantity` or market `quoteQuantity` deliberately. Limit orders can fill immediately when the price crosses the book. The library generates a client-order ID when omitted; keep an explicit ID if the workflow needs durable correlation.

Cancellation can race with a fill. Check the cancellation result and reconcile order history/trades rather than assuming cancellation prevented execution. Preserve symbol scope for cancel-all operations.

## Futures exposure and closing

Opening uses `PositionSide.Long`/`Short`, `FuturesOrderType`, quantity, and required leverage. Specify `QuantityUnit.Contracts`, `BaseAsset`, or `QuoteAsset` for the intended size; contract counts differ from asset exposure. `FuturesOrderType.Plan` is used for limit-style planned orders with a price.

Margin type and `PositionCombineType` affect account behavior. Read current configuration and positions before changing margin mode, combining/splitting positions, adjusting margin, reversing exposure, or changing TP/SL and trailing settings.

Closing uses a distinct endpoint and a position ID. Verify the position belongs to the intended symbol, side, and account before closing it; a position lookup may return exposure that predates a newly submitted order. Broad close-all calls can affect more than the example trade.

Shared futures opening requires leverage and position side; closing requires `PositionId` and contract quantity (or the supported full-close path). Do not assume an opposite order automatically closes a specific position.

## Batch outcomes and submission recovery

Futures batch placement returns `HttpResult<CallResult<CoinWBatchResult>[]>`. Check outer success, then each inner result and error before using IDs. Outer success does not imply the whole batch succeeded.

A timeout or lost response can occur after acceptance of an order, cancellation, transfer, withdrawal, or settings change. Reconcile order, transaction, position, or account state before deciding whether to resubmit. An explicit client-order ID helps correlation but does not itself prove retries are idempotent. Transient errors do not make account-changing calls safe to repeat; bound public-read retries separately.

## Transfers and withdrawals

Transfers move funds between account types. Withdrawals can move funds internally or off exchange depending on the parameters. Preserve source/destination account type, asset, network, address, applicable memo/tag, and amount. Request acceptance is distinct from completed withdrawal settlement.
