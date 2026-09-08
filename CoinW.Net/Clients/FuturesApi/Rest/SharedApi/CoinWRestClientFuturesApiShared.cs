using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using CoinW.Net.Interfaces.Clients.FuturesApi;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using System.Linq;
using CryptoExchange.Net;
using CoinW.Net.Enums;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net.Objects.Errors;

namespace CoinW.Net.Clients.FuturesApi
{
    internal partial class CoinWRestClientFuturesSharedApi :
        SharedApiBase,
        ICoinWRestClientFuturesApiShared,
        ICoinWRestClientFuturesSharedApi
    {
        private readonly CoinWRestClientFuturesApi _api;

        private const string _topicId = "CoinWFutures";
        private const string _exchangeName = "CoinW";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(CoinWExchange.Metadata, this);

        private static readonly HashSet<string> _cryptoPartitions = ["2012", "2013", "2029"];

        public CoinWRestClientFuturesSharedApi(CoinWRestClientFuturesApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  [TradingMode.PerpetualLinear],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetBalancesOptions,
                GetFeeOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                PlaceFuturesOrderOptions,
                GetFuturesOrderOptions,
                GetOpenFuturesOrdersOptions,
                GetClosedFuturesOrdersOptions,
                GetFuturesOrderTradesOptions,
                GetFuturesUserTradeHistoryOptions,
                CancelFuturesOrderOptions,
                GetPositionsOptions,
                CloseFullPositionOptions,
                GetFuturesSymbolsOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                SetFuturesTpSlOptions,
                CancelFuturesTpSlOptions
                );
        }
    }
}
