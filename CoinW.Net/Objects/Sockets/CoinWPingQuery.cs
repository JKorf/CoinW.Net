using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Internal;

namespace CoinW.Net.Objects.Sockets
{
    internal class CoinWPingQuery : Query<CoinWSocketResponse<CoinWSubscriptionResponse>>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public CoinWPingQuery() : base(new CoinWSocketRequest
        {
            Event = "ping"
        }, false, 1)
        {
            ListenerIdentifiers = new HashSet<string> { "pong" };
        }

        public override CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>> HandleMessage(SocketConnection connection, DataEvent<CoinWSocketResponse<CoinWSubscriptionResponse>> message)
        {
            return message.ToCallResult();
        }
    }
}
