using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using CoinW.Net.Interfaces.Clients.FuturesApi;

namespace CoinW.Net.Clients.FuturesApi
{
    internal partial class CoinWRestClientFuturesApi : ICoinWRestClientFuturesApiShared
    {
        private const string _topicId = "CoinWFutures";
        public string Exchange => "CoinW";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
    }
}
