using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Converters.MessageParsing;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Objects.Errors;
using System.Net.Http.Headers;
using CoinW.Net.Clients.MessageHandlers;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;

namespace CoinW.Net.Clients.FuturesApi
{
    /// <inheritdoc cref="ICoinWRestClientFuturesApi" />
    internal partial class CoinWRestClientFuturesApi : RestApiClient<CoinWEnvironment, CoinWFuturesAuthenticationProvider, CoinWCredentials>, ICoinWRestClientFuturesApi
    {
        #region fields 
        protected override IRestMessageHandler MessageHandler => new CoinWRestMessageHandler(CoinWErrors.FuturesErrors);
        internal static ErrorMapping RestErrorMapping => CoinWErrors.FuturesErrors;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public ICoinWRestClientFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public ICoinWRestClientFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public ICoinWRestClientFuturesApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "CoinW";
        #endregion

        #region constructor/destructor
        internal CoinWRestClientFuturesApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, CoinWRestOptions options)
            : base(loggerFactory, CoinWExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.FuturesOptions)
        {
            Account = new CoinWRestClientFuturesApiAccount(this);
            ExchangeData = new CoinWRestClientFuturesApiExchangeData(_logger, this);
            Trading = new CoinWRestClientFuturesApiTrading(_logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(CoinWExchange._serializerContext);


        /// <inheritdoc />
        protected override CoinWFuturesAuthenticationProvider CreateAuthenticationProvider(CoinWCredentials credentials)
            => new CoinWFuturesAuthenticationProvider(credentials);


        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            return await base.SendAsync<CoinWResponse>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<CoinWResponse<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            if (result.Data.Code != 0)
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
        public ICoinWRestClientFuturesApiShared SharedClient => this;

    }
}
