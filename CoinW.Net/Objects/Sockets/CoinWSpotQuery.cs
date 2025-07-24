using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace CoinW.Net.Objects.Sockets
{
    internal class CoinWSpotQuery<T> : Query<T>
    {
        public CoinWSpotQuery(CoinWSpotSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            var topic = $"{request.Parameters.Type}{(request.Parameters.PairCode == null ? "" : ("-" + request.Parameters.PairCode))}{(request.Parameters.Interval == null ? "" : ("-" + EnumConverter.GetString(request.Parameters.Interval)))}";
            MessageMatcher = MessageMatcher.Create(
                new MessageHandlerLink<CoinWSocketResponse<T>>(MessageLinkType.Full, $"{topic}-subscribe", HandleMessage),
                new MessageHandlerLink<CoinWSocketResponse<T>>(MessageLinkType.Full, $"{topic}-unsub", HandleMessage)
                );
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DataEvent<CoinWSocketResponse<T>> message)
        {
            return message.As<T>(message.Data.Data).ToCallResult();
        }
    }
}
