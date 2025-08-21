using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;

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
}
