using CoinW.Net.Enums;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;

namespace CoinW.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class CoinWWrappedSubscription<T> : Subscription
    {
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
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
            var updateData = JsonSerializer.Deserialize<T>(message.Data, CoinWExchange._serializerContext)!;
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
            _handler.Invoke(receiveTime, originalData, updateData, message.Time);
            return new CallResult(null);
        }
    }
}
