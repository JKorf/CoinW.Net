using CoinW.Net.Clients;
using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class CoinWFuturesSymbolOrderBook : SymbolOrderBook
    {
        private readonly bool _clientOwner;
        private readonly ICoinWSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public CoinWFuturesSymbolOrderBook(string symbol, Action<CoinWOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="restClient">Rest client instance</param>
        /// <param name="socketClient">Socket client instance</param>
        public CoinWFuturesSymbolOrderBook(
            string symbol,
            Action<CoinWOrderBookOptions>? optionsDelegate,
            ILoggerFactory? logger,
            ICoinWRestClient? restClient,
            ICoinWSocketClient? socketClient) : base(logger, "CoinW", "Futures", symbol)
        {
            var options = CoinWOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _strictLevels = false;
            _sequencesAreConsecutive = options?.Limit == null;

            Levels = options?.Limit;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new CoinWSocketClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var subResult = await _socketClient.FuturesApi.SubscribeToOrderBookUpdatesAsync(Symbol, HandleUpdate).ConfigureAwait(false);
            if (!subResult)
                return new CallResult<UpdateSubscription>(subResult.Error!);

            Status = OrderBookStatus.Syncing;

            var result = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
            if (!result)
            {
                _ = subResult.Data.CloseAsync();
                return result.AsError<UpdateSubscription>(result.Error!);
            }

            return result.As(subResult.Data);
        }

        private void HandleUpdate(DataEvent<CoinWFuturesOrderBook> @event)
        {
            // The event sequence number here is a `time` value on the message, though it doesn't seem to be an actual timstamp
            SetSnapshot(@event.SequenceNumber!.Value, @event.Data.Bids, @event.Data.Asks, @event.DataTime, @event.DataTimeLocal);
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
            {
                _socketClient?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
