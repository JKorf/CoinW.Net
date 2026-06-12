using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
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
    internal partial class CoinWRestClientSpotApi : RestApiClient<CoinWEnvironment, CoinWSpotAuthenticationProvider, CoinWCredentials>, ICoinWRestClientSpotApi
    {
        #region fields 
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
            : base(logger, CoinWExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.SpotOptions)
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
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(CoinWExchange._serializerContext);


        /// <inheritdoc />
        protected override CoinWSpotAuthenticationProvider CreateAuthenticationProvider(CoinWCredentials credentials)
            => new CoinWSpotAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<CoinWResponse>( definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (!result.Data.Success)
                return HttpResult.Fail(result, new ServerError(result.Data.Code!.Value, GetErrorInfo(result.Data.Code!.Value, result.Data.Message!)));

            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<CoinWResponse<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            if (!result.Data.Success)
                return HttpResult.Fail<T>(result, new ServerError(result.Data.Code!.Value, GetErrorInfo(result.Data.Code.Value, result.Data.Message!)));

            return HttpResult.Ok(result, result.Data.Data);
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => CoinWExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public ICoinWRestClientSpotApiShared SharedClient => this;

    }
}
