using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using CryptoExchange.Net.Sockets.Default;

namespace CoinW.Net.Objects.Sockets
{
    internal class CoinWFuturesQuery<T> : Query<T>
    {
        public CoinWFuturesQuery(CoinWFuturesSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            var topic = $"{request.Parameters.Type}{(request.Parameters.PairCode == null ? "" : ("-" + request.Parameters.PairCode.ToLowerInvariant()))}{(request.Parameters.Interval == null ? "" : ("-" + EnumConverter.GetString(request.Parameters.Interval)))}";
            MessageMatcher = MessageMatcher.Create(
                new MessageHandlerLink<CoinWSocketResponse<T>>(MessageLinkType.Full, $"{topic}-subscribe", HandleMessage),
                new MessageHandlerLink<CoinWSocketResponse<T>>(MessageLinkType.Full, $"{topic}-unsub", HandleMessage)
                );

            MessageRouter = MessageRouter.CreateWithTopicFilter<CoinWSocketResponse<T>>("SubResponse", request.Parameters.Type + request.Parameters.PairCode?.ToLowerInvariant() + EnumConverter.GetString(request.Parameters.Interval), HandleMessage);
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, CoinWSocketResponse<T> message)
        {
            return new CallResult<T>(message.Data, originalData, null);
        }
    }
}
