using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System;
using CryptoExchange.Net.Sockets.Default;

namespace CoinW.Net.Objects.Sockets
{
    internal class CoinWLoginQuery : Query<CoinWSocketResponse<CoinWSubscriptionResponse>>
    {
        private readonly SocketApiClient _client;

        public CoinWLoginQuery(SocketApiClient client, string key, string secret) : base(new CoinWLoginRequest
        {
            Event = "login",
            Parameters = new CoinWLoginRequestParameters
            {
                ApiKey = key,
                Passphrase = secret
            }
        }, false, 1)
        {
            _client = client;
            MessageMatcher = MessageMatcher.Create<CoinWSocketResponse<CoinWSubscriptionResponse>>("login", HandleMessage);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<CoinWSocketResponse<CoinWSubscriptionResponse>>("login", HandleMessage);
        }

        public CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, CoinWSocketResponse<CoinWSubscriptionResponse> message)
        {
            if (!message.Data.Success)
                return new CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>>(new ServerError(message.Data.ErrorCode!.Value, _client.GetErrorInfo(message.Data.ErrorCode!.Value, message.Data.ErrorMessage!)));

            return new CallResult<CoinWSocketResponse<CoinWSubscriptionResponse>>(message, originalData, null);
        }
    }
}
