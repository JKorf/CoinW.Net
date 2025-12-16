using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.MessageHandlers
{
    internal class CoinWRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = CoinWExchange._serializerContext;

        public CoinWRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not CoinWResponse coinWResponse)
                return null;

            if (coinWResponse.Success)
                return null;

            if (coinWResponse.Code == 0 || coinWResponse.Code == 200)
                return null;

            return new ServerError(coinWResponse.Code!.Value, _errorMapping.GetErrorInfo(coinWResponse.Code.Value.ToString(), coinWResponse.Message!));
        }


        public override async ValueTask<Error> ParseErrorResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            if (httpStatusCode == 401 || httpStatusCode == 403)
                return new ServerError(new ErrorInfo(ErrorType.Unauthorized, "Unauthorized"));

            using var streamReader = new StreamReader(responseStream);
            return new ServerError(ErrorInfo.Unknown with { Message = await streamReader.ReadToEndAsync().ConfigureAwait(false) });
        }
    }
}
