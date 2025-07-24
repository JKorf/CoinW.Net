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

        public override void AuthenticateRequest(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            ref IDictionary<string, object>? uriParameters,
            ref IDictionary<string, object>? bodyParameters,
            ref Dictionary<string, string>? headers,
            bool auth,
            ArrayParametersSerialization arraySerialization,
            HttpMethodParameterPosition parameterPosition,
            RequestBodyFormat requestBodyFormat)
        {
            headers = new Dictionary<string, string>() { };

            if (!auth)
                return;

            uriParameters!.Add("api_key", ApiKey);
            var parameters = uriParameters.Where(x => x.Key != "command").OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            var paramString = parameters.ToFormData();
            var signParams = (paramString + "&secret_key=" + _credentials.Secret).TrimStart('&');

            var sign = SignMD5(signParams, SignOutputType.Hex).ToUpperInvariant();
            uriParameters.Add("sign", sign);
        }
    }
}
