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

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface ICoinWSocketClientFuturesSharedApi :
        ISubscribeBalancesSocket,
        ISubscribeKlinesSocket,
        ISubscribeOrderBookSocket,
        ISubscribeTickerSocket,
        ISubscribeTradesSocket,
        ISubscribeFuturesOrdersSocket,
        ISubscribePositionsSocket
    {
    }
}
