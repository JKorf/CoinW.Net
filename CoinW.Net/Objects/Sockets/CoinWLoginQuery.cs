using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Internal;

namespace CoinW.Net.Objects.Sockets
{
    internal class CoinWLoginQuery : Query<CoinWSocketResponse<CoinWSubscriptionResponse>>
    {
        public CoinWLoginQuery(string key, string secret) : base(new CoinWLoginRequest
        {
            Event = "login",
            Parameters = new CoinWLoginRequestParameters
            {
                ApiKey = key,
                Passphrase = secret
            }
        }, false, 1)
        {
            MessageMatcher = MessageMatcher.Create<CoinWSocketResponse<CoinWSubscriptionResponse>>("login", HandleMessage);
        }

        public CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>> HandleMessage(SocketConnection connection, DataEvent<CoinWSocketResponse<CoinWSubscriptionResponse>> message)
        {
            if (!message.Data.Data.Success)
                return message.ToCallResult<CoinWSocketResponse<CoinWSubscriptionResponse>>(new ServerError(message.Data.Data.ErrorCode!.Value, message.Data.Data.ErrorMessage!));

            return message.ToCallResult();
        }
    }
}
