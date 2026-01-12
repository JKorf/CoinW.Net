using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text;
using CoinW.Net.Enums;
using CryptoExchange.Net.Sockets.Default;

namespace CoinW.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class CoinWWrappedSubscription<T> : Subscription
    {

        private readonly IByteMessageAccessor _innerAccessor = new SystemTextJsonByteMessageAccessor(CoinWExchange._serializerContext);
        private readonly Action<DateTime, string?, T, long> _handler;
        private string _topic;
        private string? _pairCode;
        private KlineIntervalStream? _interval;

        /// <summary>
        /// ctor
        /// </summary>
        public CoinWWrappedSubscription(ILogger logger, string topic, string? pairCode, KlineIntervalStream? interval, Action<DateTime, string?, T, long> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _pairCode = pairCode;
            _interval = interval;

            MessageMatcher = MessageMatcher.Create<CoinWSocketResponse<string>>(MessageLinkType.Full, topic + (pairCode == null ? "" : ("-" + pairCode)) + (interval == null ? "" : ("-" + EnumConverter.GetString(interval))), DoHandleMessage);
            MessageRouter = MessageRouter.CreateWithTopicFilter<CoinWSocketResponse<string>>(topic, _topic + _pairCode?.ToLowerInvariant() + EnumConverter.GetString(interval), DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
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
        protected override Query? GetUnsubQuery(SocketConnection connection)
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
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, CoinWSocketResponse<string> message)
        {
            var innerReadResult = _innerAccessor.Read(Encoding.UTF8.GetBytes(message.Data));
            if (!innerReadResult)
                return innerReadResult;

            var desData = _innerAccessor.Deserialize<T>();
            if (!desData)
                return desData;

            _handler.Invoke(receiveTime, originalData, desData.Data, message.Time);
            return new CallResult(null);
        }
    }
}
