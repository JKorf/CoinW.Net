using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Internal;
using System.Linq;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text;
using CoinW.Net.Enums;

namespace CoinW.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class CoinWWrappedSubscription<T> : Subscription<CoinWSocketResponse<CoinWSubscriptionResponse>, CoinWSocketResponse<CoinWSubscriptionResponse>>
    {
        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly IByteMessageAccessor _innerAccessor = new SystemTextJsonByteMessageAccessor(SerializerOptions.WithConverters(CoinWExchange._serializerContext));
        private readonly Action<DataEvent<T>> _handler;
        private string _topic;
        private string? _pairCode;
        private string? _symbolName;
        private KlineIntervalStream? _interval;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(CoinWSocketResponse<string>);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public CoinWWrappedSubscription(ILogger logger, string topic, string? pairCode, string? symbolName, KlineIntervalStream? interval, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _pairCode = pairCode;
            _symbolName = symbolName;
            _interval = interval;

            ListenerIdentifiers = new HashSet<string>() { topic + (pairCode == null ? "" : ("-" + pairCode)) + (interval == null ? "" : ("-" + EnumConverter.GetString(interval))) };
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new CoinWQuery<CoinWSubscriptionResponse>(new CoinWSocketRequest
            {
                Event = "sub",
                Parameters = new CoinWSocketRequestParameters
                {
                    Biz = "exchange",
                    Type = _topic,
                    PairCode = _pairCode,
                    Interval = _interval
                }
            }, false);
        }

        /// <inheritdoc />
        public override Query? GetUnsubQuery()
        {
            return new CoinWQuery<CoinWSubscriptionResponse>(new CoinWSocketRequest
            {
                Event = "unsub",
                Parameters = new CoinWSocketRequestParameters
                {
                    Biz = "exchange",
                    Type = _topic,
                    PairCode = _pairCode,
                    Interval = _interval
                }
            }, false);
        }

        /// <inheritdoc />
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (CoinWSocketResponse<string>)message.Data!;
            var innerReadResult = _innerAccessor.Read(Encoding.UTF8.GetBytes(data.Data));
            if (!innerReadResult)
                return innerReadResult;

            var desData = _innerAccessor.Deserialize<T>();
            if (!desData)
                return desData;

            _handler.Invoke(message.As(desData.Data, data.Type, _symbolName, SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
