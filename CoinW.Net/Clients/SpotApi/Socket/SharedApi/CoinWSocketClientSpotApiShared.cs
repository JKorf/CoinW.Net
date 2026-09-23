using CoinW.Net.Clients.FuturesApi;
using CoinW.Net.Interfaces.Clients.SpotApi;
using CoinW.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.SpotApi
{
    internal partial class CoinWSocketClientSpotSharedApi :
        SharedApiBase,
        ICoinWSocketClientSpotApiShared,
        ICoinWSocketClientSpotSharedApi
    {
        private readonly CoinWSocketClientSpotApi _api;

        private const string _topicId = "CoinWSpot";
        private const string _exchangeName = "CoinW";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(CoinWExchange.Metadata, this);

        public CoinWSocketClientSpotSharedApi(CoinWSocketClientSpotApi api)
            : base(
                  SharedTransport.Socket,
                  api,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeBalanceOptions,
                SubscribeKlineOptions,
                SubscribeOrderBookOptions,
                SubscribeTickerOptions,
                SubscribeAllTickersOptions,
                SubscribeTradeOptions,
                SubscribeSpotOrderOptions
                );
        }
    }
}
