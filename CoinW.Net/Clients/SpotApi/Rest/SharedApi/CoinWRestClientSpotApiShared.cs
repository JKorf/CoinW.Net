using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Enums;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.SpotApi
{
    internal partial class CoinWRestClientSpotSharedApi :
        SharedApiBase,
        ICoinWRestClientSpotApiShared,
        ICoinWRestClientSpotSharedApi
    {
        private readonly CoinWRestClientSpotApi _api;

        private const string _topicId = "CoinWSpot";
        private const string _exchangeName = "CoinW";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(CoinWExchange.Metadata, this);

        public CoinWRestClientSpotSharedApi(CoinWRestClientSpotApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetAssetOptions,
                GetAllAssetsOptions,
                GetBalancesOptions,
                GetDepositAddressesOptions,
                GetDepositHistoryOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetWithdrawalHistoryOptions,
                WithdrawOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                GetSpotSymbolsOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                GetSpotOrderTradesOptions,
                GetSpotUserTradeHistoryOptions,
                CancelSpotOrderOptions,
                TransferOptions
                );
        }
    }
}
