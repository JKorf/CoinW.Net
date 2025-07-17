using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace CoinW.Net.Objects.Sockets
{
    internal class CoinWQuery<T> : Query<CoinWSocketResponse<T>, T>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public CoinWQuery(CoinWSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            var topic = $"{request.Parameters.Type}{(request.Parameters.PairCode == null ? "" : ("-" + request.Parameters.PairCode))}{(request.Parameters.Interval == null ? "" : ("-" + EnumConverter.GetString(request.Parameters.Interval)))}";
            ListenerIdentifiers = new HashSet<string> { 
                $"{topic}-subscribe",
                $"{topic}-unsub",
            };
        }

        public override CallResult<T> HandleMessage(SocketConnection connection, DataEvent<CoinWSocketResponse<T>> message)
        {
            return message.As<T>(message.Data.Data).ToCallResult();
        }
    }
}
