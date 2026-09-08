using CryptoExchange.Net.SharedApis;

namespace CoinW.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Futures rest API usage
    /// </summary>
    public interface ICoinWRestClientFuturesApiShared :
        IFeeRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        IFuturesSymbolRestClient,
        IFuturesTickerRestClient,
        IFuturesOrderRestClient,
        IFuturesTpSlRestClient,
        IBalanceRestClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface ICoinWRestClientFuturesSharedApi :
        IGetFeesRest,
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetRecentTradesRest,
        IGetFuturesSymbolsRest,
        IGetTickerRest,
        IGetAllTickersRest,
        IPlaceFuturesOrderRest,
        IGetFuturesOrderRest,
        IGetOpenFuturesOrdersRest,
        IGetClosedFuturesOrdersRest,
        IGetFuturesOrderTradesRest,
        IGetFuturesUserTradeHistoryRest,
        ICancelFuturesOrderRest,
        IGetPositionsRest,
        ICloseFullPositionRest,
        ISetFuturesTpSlRest,
        ICancelFuturesTpSlRest,
        IGetBalancesRest
    { }
}
