using CryptoExchange.Net.Objects.Errors;

namespace CoinW.Net
{
    internal static class CoinWErrors
    {
        public static ErrorMapping SpotErrors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.Unauthorized, false, "Authentication error", "1003"),

                new ErrorInfo(ErrorType.Unauthorized, false, "Authentication error", "6000"),

                new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many requests", "29001", "429"),

                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", "-103", "-3"),

                new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "200005"),

                new ErrorInfo(ErrorType.SystemError, true, "System error", "-200"),
            ]
        );

        public static ErrorMapping FuturesErrors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.Unauthorized, false, "Invalid signature", "6001"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Authentication error", "6000"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Insufficient permissions", "6003"),

                new ErrorInfo(ErrorType.RateLimitRequest, false, "Too many requests", "29001", "429"),

                new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", "1"),

                new ErrorInfo(ErrorType.MissingParameter, false, "Missing parameter", "402"),

                new ErrorInfo(ErrorType.UnavailableSymbol, false, "Symbol currently doesn't allow trading", "9111")
            ]
        );
    }
}
