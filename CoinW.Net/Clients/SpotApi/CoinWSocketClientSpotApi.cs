using CoinW.Net.Clients.MessageHandlers;
using CoinW.Net.Enums;
using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Internal;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Options;
using CoinW.Net.Objects.Sockets;
using CoinW.Net.Objects.Sockets.Subscriptions;
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
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.SpotApi
{
    /// <summary>
    /// Client providing access to the CoinW Spot websocket Api
    /// </summary>
    internal partial class CoinWSocketClientSpotApi : SocketApiClient, ICoinWSocketClientSpotApi
    {
        #region fields
        private static readonly MessagePath _symbolPath = MessagePath.Get().Property("pairCode");
        private static readonly MessagePath _typePath = MessagePath.Get().Property("type");
        private static readonly MessagePath _channel = MessagePath.Get().Property("channel");
        private static readonly MessagePath _event = MessagePath.Get().Property("event");
        private static readonly MessagePath _interval = MessagePath.Get().Property("interval");

        private ICoinWRestClient _restClient;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal CoinWSocketClientSpotApi(ILogger logger, CoinWSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.SpotOptions)
        {
            _restClient = new CoinWRestClient(opts =>
            {
                opts.Environment = options.Environment;
                opts.Proxy = options.Proxy;
            });

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
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new CoinWSocketSpotMessageHandler();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new CoinWSpotAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<CoinWTickerUpdate>> onMessage, CancellationToken ct = default)
        {
            var result = await CoinWUtils.GetSymbolIdFromNameAsync(_restClient, symbol).ConfigureAwait(false);
            if (!result)
                return result.As<UpdateSubscription>(default);

            var subscription = new CoinWWrappedSubscription<CoinWTickerUpdate>(_logger, "ticker", result.Data.ToString(), symbol, null, onMessage, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<CoinWSymbolUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new CoinWWrappedSubscription<CoinWSymbolUpdate[]>(_logger, "ticker_all", null, null, null, onMessage, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWOrderBookUpdate>> onMessage, CancellationToken ct = default)
        {
            var result = await CoinWUtils.GetSymbolIdFromNameAsync(_restClient, symbol).ConfigureAwait(false);
            if (!result)
                return result.As<UpdateSubscription>(default);

            var subscription = new CoinWWrappedSubscription<CoinWOrderBookUpdate>(_logger, "depth", result.Data.ToString(), symbol, null, onMessage, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, Action<DataEvent<CoinWOrderBook>> onMessage, CancellationToken ct = default)
        {
            var result = await CoinWUtils.GetSymbolIdFromNameAsync(_restClient, symbol).ConfigureAwait(false);
            if (!result)
                return result.As<UpdateSubscription>(default);

            var subscription = new CoinWWrappedSubscription<CoinWOrderBook>(_logger, "depth_snapshot", result.Data.ToString(), symbol, null, onMessage, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineIntervalStream interval, Action<DataEvent<CoinWKlineUpdate>> onMessage, CancellationToken ct = default)
        {
            var result = await CoinWUtils.GetSymbolIdFromNameAsync(_restClient, symbol).ConfigureAwait(false);
            if (!result)
                return result.As<UpdateSubscription>(default);

            var subscription = new CoinWWrappedSubscription<CoinWKlineUpdate>(_logger, "candles", result.Data.ToString(), symbol, interval, onMessage, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<CoinWTradeUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var result = await CoinWUtils.GetSymbolIdFromNameAsync(_restClient, symbol).ConfigureAwait(false);
            if (!result)
                return result.As<UpdateSubscription>(default);

            var subscription = new CoinWWrappedSubscription<CoinWTradeUpdate[]>(_logger, "fills", result.Data.ToString(), symbol, null, onMessage, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<CoinWBalanceUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWBalanceUpdate>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWBalanceUpdate>(CoinWExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                    );
            });

            var subscription = new CoinWSubscription<CoinWBalanceUpdate>(_logger, "assets", null, null, null, internalHandler, true);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<CoinWOrderUpdate>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, CoinWSocketResponse<CoinWOrderUpdate>>((receiveTime, originalData, data) =>
            {
                onMessage(
                    new DataEvent<CoinWOrderUpdate>(CoinWExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Type)
                    );
            });

            var subscription = new CoinWSubscription<CoinWOrderUpdate>(_logger, "order", null, null, null, internalHandler, true);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
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

            if (symbol != null)
                return $"{type}-{symbol.ToLowerInvariant()}{(interval == null ? "" : $"-{interval}")}";

            return $"{type}";
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(new CoinWLoginQuery(this, ApiCredentials!.Key, ApiCredentials.Secret));

        /// <inheritdoc />
        public ICoinWSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => CoinWExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
