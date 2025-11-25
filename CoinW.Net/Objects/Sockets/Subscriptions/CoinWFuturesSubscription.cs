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
    internal class CoinWFuturesSubscription<T> : Subscription<CoinWSocketResponse<CoinWSubscriptionResponse>, CoinWSocketResponse<CoinWSubscriptionResponse>>
    {
        private readonly Action<DateTime, string?, CoinWSocketResponse<T>> _handler;
        private string _topic;
        private string? _pairCode;
        private FuturesKlineIntervalStream? _interval;

        /// <summary>
        /// ctor
        /// </summary>
        public CoinWFuturesSubscription(ILogger logger, string topic, string? pairCode, string? symbolName, FuturesKlineIntervalStream? interval, Action<DateTime, string?, CoinWSocketResponse<T>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _pairCode = pairCode;
            _interval = interval;

            MessageMatcher = MessageMatcher.Create<CoinWSocketResponse<T>>(MessageLinkType.Full, topic + (pairCode == null ? "" : ("-" + pairCode.ToLowerInvariant())) + (interval == null ? "" : ("-" + EnumConverter.GetString(interval))), DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new CoinWFuturesQuery<CoinWSubscriptionResponse>(new CoinWFuturesSocketRequest
            {
                Event = "sub",
                Parameters = new CoinWFuturesSocketRequestParameters
                {
                    Biz = "futures",
                    Type = _topic,
                    PairCode = _pairCode,
                    Interval = _interval
                }
            }, false);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new CoinWFuturesQuery<CoinWSubscriptionResponse>(new CoinWFuturesSocketRequest
            {
                Event = "unsub",
                Parameters = new CoinWFuturesSocketRequestParameters
                {
                    Biz = "futures",
                    Type = _topic,
                    PairCode = _pairCode,
                    Interval = _interval
                }
            }, false);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, CoinWSocketResponse<T> message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return new CallResult(null);
        }
    }
}
