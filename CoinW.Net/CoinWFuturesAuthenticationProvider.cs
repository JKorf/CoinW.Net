using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace CoinW.Net
{
    internal class CoinWFuturesAuthenticationProvider : AuthenticationProvider
    {
        private readonly IStringMessageSerializer _serializer = new SystemTextJsonMessageSerializer(CoinWExchange._serializerContext);

        public CoinWFuturesAuthenticationProvider(ApiCredentials credentials) : base(credentials)
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

            var time = GetMillisecondTimestamp(apiClient);

            var queryParamStr = uriParameters?.CreateParamString(false, arraySerialization);
            var bodyParamStr = bodyParameters == null ? "" : GetSerializedBody(_serializer, bodyParameters);
            var signStr = "";
            signStr += queryParamStr;
            signStr += bodyParamStr;
            signStr = $"{time}{method}{uri.AbsolutePath}{((queryParamStr?.Any() == true ? "?" : "") + queryParamStr)}{bodyParamStr}";

            var sign = SignHMACSHA256(signStr, SignOutputType.Base64);

            headers ??= new Dictionary<string, string>();
            headers.Add("sign", sign);
            headers.Add("api_key", ApiKey);
            headers.Add("timestamp", time);
        }
    }
}
