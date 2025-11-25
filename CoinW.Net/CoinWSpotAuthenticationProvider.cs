using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;

namespace CoinW.Net
{
    internal class CoinWSpotAuthenticationProvider : AuthenticationProvider
    {
        public CoinWSpotAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            if (!request.Authenticated)
                return;

            request.QueryParameters ??= new Dictionary<string, object>();
            request.QueryParameters.Add("api_key", ApiKey);
            var signParameters = request.QueryParameters.Where(x => x.Key != "command").OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            var queryString = signParameters.CreateParamString(false, request.ArraySerialization);
            var signParams = (queryString + "&secret_key=" + _credentials.Secret).TrimStart('&');

            var sign = SignMD5(signParams, SignOutputType.Hex).ToUpperInvariant();
            request.QueryParameters.Add("sign", sign);

            if (request.QueryParameters.TryGetValue("command", out var command))
                request.SetQueryString($"command={command}&{queryString}&sign={sign}");
            else
                request.SetQueryString($"{queryString}&sign={sign}");
        }
    }
}
