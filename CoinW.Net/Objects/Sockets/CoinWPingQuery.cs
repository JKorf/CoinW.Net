using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CoinW.Net.Objects.Internal;
using System;
using CryptoExchange.Net.Sockets.Default;

namespace CoinW.Net.Objects.Sockets
{
    internal class CoinWPingQuery : Query<CoinWSocketResponse<CoinWSubscriptionResponse>>
    {
        public CoinWPingQuery() : base(new CoinWSpotSocketRequest
        {
            Event = "ping"
        }, false, 1)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<CoinWSocketResponse<CoinWSubscriptionResponse>>("pong", HandleMessage);
        }

        public CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, CoinWSocketResponse<CoinWSubscriptionResponse> message)
        {
            return new CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>>(message, originalData, null);
        }
    }
}
