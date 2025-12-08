using CryptoExchange.Net.SharedApis;

namespace CoinW.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot socket API usage
    /// </summary>
    public interface ICoinWSocketClientSpotApiShared :
        IBalanceSocketClient,
        IKlineSocketClient,
        IOrderBookSocketClient,
        ITickerSocketClient,
        ITickersSocketClient,
        ITradeSocketClient,
        ISpotOrderSocketClient
    {
    }
}
