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
    internal partial class CoinWRestClientFuturesApi : RestApiClient, ICoinWRestClientFuturesApi
    {
        #region fields 
        private readonly MessagePath _codePath = MessagePath.Get().Property("code");
        private readonly MessagePath _messagePath = MessagePath.Get().Property("msg");
        private readonly MessagePath _messagePath2 = MessagePath.Get().Property("message");

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
        internal CoinWRestClientFuturesApi(ILogger logger, HttpClient? httpClient, CoinWRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.FuturesOptions)
        {
            Account = new CoinWRestClientFuturesApiAccount(this);
            ExchangeData = new CoinWRestClientFuturesApiExchangeData(logger, this);
            Trading = new CoinWRestClientFuturesApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(CoinWExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(CoinWExchange._serializerContext);


        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new CoinWFuturesAuthenticationProvider(credentials);


        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<CoinWResponse>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            return result.AsDataless();
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<CoinWResponse<T>>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            if (!result)
                return result.As<T>(default);

            return result.As(result.Data.Data);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => CoinWExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public ICoinWRestClientFuturesApiShared SharedClient => this;

    }
}
