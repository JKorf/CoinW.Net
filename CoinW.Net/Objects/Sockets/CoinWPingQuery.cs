using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Internal;
using System;

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
            MessageMatcher = MessageMatcher.Create<CoinWSocketResponse<CoinWSubscriptionResponse>>("pong", HandleMessage);
        }

        public CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>> HandleMessage(SocketConnection connection, DataEvent<CoinWSocketResponse<CoinWSubscriptionResponse>> message)
        {
            return message.ToCallResult();
        }
    }
}
