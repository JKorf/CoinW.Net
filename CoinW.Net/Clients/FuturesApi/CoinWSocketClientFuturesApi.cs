using CoinW.Net.Clients.MessageHandlers;
using CoinW.Net.Enums;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Objects.Internal;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Options;
using CoinW.Net.Objects.Sockets;
using CoinW.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.FuturesApi
{
    /// <summary>
    /// Client providing access to the CoinW Futures websocket Api
    /// </summary>
    internal partial class CoinWSocketClientFuturesApi : SocketApiClient, ICoinWSocketClientFuturesApi
    {
        #region fields
        private static readonly MessagePath _symbolPath = MessagePath.Get().Property("pairCode");
        private static readonly MessagePath _typePath = MessagePath.Get().Property("type");
        private static readonly MessagePath _channel = MessagePath.Get().Property("channel");
        private static readonly MessagePath _event = MessagePath.Get().Property("event");
        private static readonly MessagePath _interval = MessagePath.Get().Property("interval");

        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal CoinWSocketClientFuturesApi(ILogger logger, CoinWSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.FuturesOptions)
        {
            RegisterPeriodicQuery(
                "ping",
                TimeSpan.FromSeconds(5),
                x => new CoinWPingQuery(),
                (connection, result) =>
                {
                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(CoinWExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(CoinWExchange._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new CoinWSocketFuturesMessageHandler();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new CoinWSpotAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWFuturesTickerUpdate>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWFuturesTickerUpdate>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWFuturesTickerUpdate>(_logger, "ticker_swap", symbol, symbol, null, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesOrderBook>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWFuturesOrderBook>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWFuturesOrderBook>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWFuturesOrderBook>(_logger, "depth", symbol, symbol, null, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<CoinWFuturesTrade[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWFuturesTrade[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWFuturesTrade[]>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.PairCode)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWFuturesTrade[]>(_logger, "fills", symbol, symbol, null, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, FuturesKlineIntervalStream interval, Action<DataEvent<CoinWFuturesStreamKline>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWFuturesStreamKline>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWFuturesStreamKline>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.PairCode)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWFuturesStreamKline>(_logger, "candles_swap_utc", symbol, symbol, interval, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(string symbol, Action<DataEvent<CoinWPrice>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWPrice>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWPrice>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWPrice>(_logger, "index_price", symbol, symbol, null, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, Action<DataEvent<CoinWPrice>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWPrice>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWPrice>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWPrice>(_logger, "mark_price", symbol, symbol, null, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToFundingRateUpdatesAsync(string symbol, Action<DataEvent<CoinWFundingRate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWFundingRate>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWFundingRate>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWFundingRate>(_logger, "funding_rate", symbol, symbol, null, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<CoinWFuturesOrder[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWFuturesOrder[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWFuturesOrder[]>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.FirstOrDefault()?.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWFuturesOrder[]>(_logger, "order", null, null, null, internalHandler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(Action<DataEvent<CoinWPosition[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWPosition[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWPosition[]>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.FirstOrDefault()?.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWPosition[]>(_logger, "position", null, null, null, internalHandler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPositionDetailUpdatesAsync(Action<DataEvent<CoinWPositionChange[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWPositionChange[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWPositionChange[]>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                        .WithSymbol(data.Data.FirstOrDefault()?.Symbol)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWPositionChange[]>(_logger, "position_change", null, null, null, internalHandler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<CoinWFuturesBalanceUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWFuturesBalanceUpdate[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWFuturesBalanceUpdate[]>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWFuturesBalanceUpdate[]>(_logger, "assets", null, null, null, internalHandler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarginConfigUpdatesAsync(Action<DataEvent<CoinWMarginInfo[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWMarginInfo[]>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWMarginInfo[]>(data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                    );
            });

            var subscription = new CoinWFuturesSubscription<CoinWMarginInfo[]>(_logger, "user_setting", null, null, null, internalHandler, true);
            return await SubscribeAsync(BaseAddress.AppendPath("perpum"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var @event = message.GetValue<string>(_event);
            if (@event == "pong")
                return "pong";

            var type = message.GetValue<string>(_typePath);
            var symbol = message.GetValue<string>(_symbolPath);
            var channel = message.GetValue<string?>(_channel);
            var interval = message.GetValue<string?>(_interval);
            if (channel != null)
            {
                if (channel.Equals("login", StringComparison.Ordinal))
                    return "login";

                return $"{type}-{(symbol == null ? "" : $"{symbol.ToLowerInvariant()}-")}{(interval == null ? "" : $"{interval}-")}{channel}";
            }

            if (type!.Equals("order", StringComparison.Ordinal)
                || type.Equals("position", StringComparison.Ordinal)
                || type.Equals("position_change", StringComparison.Ordinal)
                || type.Equals("assets", StringComparison.Ordinal))
            {
                return type;
            }

            if (symbol != null)
                return $"{type}-{symbol.ToLowerInvariant()}{(interval == null ? "" : $"-{interval}")}";

            return type;
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(new CoinWLoginQuery(this, ApiCredentials!.Key, ApiCredentials.Secret));

        /// <inheritdoc />
        public ICoinWSocketClientFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => CoinWExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
