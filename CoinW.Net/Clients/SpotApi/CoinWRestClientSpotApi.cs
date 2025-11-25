using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.MessageParsing;
using CoinW.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Errors;
using System.Net.Http.Headers;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CoinW.Net.Clients.MessageHandlers;

namespace CoinW.Net.Clients.SpotApi
{
    /// <inheritdoc cref="ICoinWRestClientSpotApi" />
    internal partial class CoinWRestClientSpotApi : RestApiClient, ICoinWRestClientSpotApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Spot Api");

        private readonly MessagePath _successPath = MessagePath.Get().Property("success");
        private readonly MessagePath _codePath = MessagePath.Get().Property("code");
        private readonly MessagePath _messagePath = MessagePath.Get().Property("message");
        private readonly MessagePath _messagePath2 = MessagePath.Get().Property("msg");

        protected override ErrorMapping ErrorMapping => CoinWErrors.SpotErrors;

        protected override IRestMessageHandler MessageHandler => new CoinWRestMessageHandler(CoinWErrors.SpotErrors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public ICoinWRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public ICoinWRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public ICoinWRestClientSpotApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "CoinW";

        internal ICoinWRestClient BaseClient { get; }
        #endregion

        #region constructor/destructor
        internal CoinWRestClientSpotApi(ICoinWRestClient baseClient, ILogger logger, HttpClient? httpClient, CoinWRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.SpotOptions)
        {
            BaseClient = baseClient;

            Account = new CoinWRestClientSpotApiAccount(this);
            ExchangeData = new CoinWRestClientSpotApiExchangeData(logger, this);
            Trading = new CoinWRestClientSpotApiTrading(logger, this);

            RequestBodyEmptyContent = "";
            ParameterPositions[HttpMethod.Post] = HttpMethodParameterPosition.InUri;
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(CoinWExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(CoinWExchange._serializerContext);


        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new CoinWSpotAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<CoinWResponse>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            if (!result)
                return result.AsDataless();

            if (!result.Data.Success)
                return result.AsDatalessError(new ServerError(result.Data.Code!.Value, GetErrorInfo(result.Data.Code!.Value, result.Data.Message!)));

            return result.AsDataless();
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<CoinWResponse<T>>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            if (!result)
                return result.As<T>(default);

            if (!result.Data.Success)
                return result.AsError<T>(new ServerError(result.Data.Code!.Value, GetErrorInfo(result.Data.Code.Value, result.Data.Message!)));

            return result.As(result.Data.Data);
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? uriParameters, ParameterCollection? bodyParameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, uriParameters, bodyParameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? uriParameters, ParameterCollection? bodyParameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<CoinWResponse<T>>(baseAddress, definition, uriParameters, bodyParameters, cancellationToken, null, weight).ConfigureAwait(false);

            if (!result)
                return result.As<T>(default);

            if (!result.Data.Success)
                return result.AsError<T>(new ServerError(result.Data.Code!.Value, GetErrorInfo(result.Data.Code.Value, result.Data.Message!)));

            return result.As(result.Data.Data);
        }

        protected override Error? TryParseError(RequestDefinition request, HttpResponseHeaders responseHeaders, IMessageAccessor accessor)
        {
            if (!accessor.IsValid)
                return new ServerError(ErrorInfo.Unknown);

            if (accessor.GetValue<bool>(_successPath))
                return null;

            var code = accessor.GetValue<int>(_codePath);
            var msg = accessor.GetValue<string>(_messagePath) ?? accessor.GetValue<string>(_messagePath2);
            return new ServerError(code, GetErrorInfo(code, msg!));
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, false, ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => null;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => CoinWExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public ICoinWRestClientSpotApiShared SharedClient => this;

    }
}
