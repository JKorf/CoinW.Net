using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CoinW.Net
{
    internal class CoinWFuturesAuthenticationProvider : AuthenticationProvider
    {
        private readonly IStringMessageSerializer _serializer = new SystemTextJsonMessageSerializer(CoinWExchange._serializerContext);

        public CoinWFuturesAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            if (!request.Authenticated)
                return;

            var time = GetMillisecondTimestamp(apiClient);
            var queryParams = request.GetQueryString(false);
            if (!string.IsNullOrEmpty(queryParams))
                queryParams = $"?{queryParams}";

            var body = request.BodyParameters?.Count > 0 ? GetSerializedBody(_serializer, request.BodyParameters) : string.Empty;
            var signStr = $"{time}{request.Method}{request.Path}{queryParams}{body}";
            var signature = SignHMACSHA256(signStr, SignOutputType.Base64);

            request.Headers ??= new Dictionary<string, string>();
            request.Headers.Add("sign", signature);
            request.Headers.Add("api_key", ApiKey);
            request.Headers.Add("timestamp", time);

            request.SetQueryString(queryParams);
            request.SetBodyContent(body);
        }
    }
}
