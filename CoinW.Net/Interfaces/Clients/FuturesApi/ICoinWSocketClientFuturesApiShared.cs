using CryptoExchange.Net.SharedApis;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Futures socket API usage
    /// </summary>
    public interface ICoinWSocketClientFuturesApiShared :
        IBalanceSocketClient,
        IKlineSocketClient,
        IOrderBookSocketClient,
        ITickerSocketClient,
        ITradeSocketClient,
        IFuturesOrderSocketClient,
        IPositionSocketClient
    {
    }
}
