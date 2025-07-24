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
    internal class CoinWSubscription<T> : Subscription<CoinWSocketResponse<CoinWSubscriptionResponse>, CoinWSocketResponse<CoinWSubscriptionResponse>>
    {
        private readonly Action<DataEvent<T>> _handler;
        private string _topic;
        private string? _pairCode;
        private string? _symbolName;
        private KlineIntervalStream? _interval;

        /// <summary>
        /// ctor
        /// </summary>
        public CoinWSubscription(ILogger logger, string topic, string? pairCode, string? symbolName, KlineIntervalStream? interval, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _pairCode = pairCode;
            _symbolName = symbolName;
            _interval = interval;

            MessageMatcher = MessageMatcher.Create<CoinWSocketResponse<T>>(MessageLinkType.Full, topic + (pairCode == null ? "" : ("-" + pairCode)) + (interval == null ? "" : ("-" + EnumConverter.GetString(interval))), DoHandleMessage);
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new CoinWSpotQuery<CoinWSubscriptionResponse>(new CoinWSpotSocketRequest
            {
                Event = "sub",
                Parameters = new CoinWSpotSocketRequestParameters
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
            return new CoinWSpotQuery<CoinWSubscriptionResponse>(new CoinWSpotSocketRequest
            {
                Event = "unsub",
                Parameters = new CoinWSpotSocketRequestParameters
                {
                    Biz = "exchange",
                    Type = _topic,
                    PairCode = _pairCode,
                    Interval = _interval
                }
            }, false);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<CoinWSocketResponse<T>> message)
        {
            _handler.Invoke(message.As(message.Data.Data, message.Data.Type, _symbolName, SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
