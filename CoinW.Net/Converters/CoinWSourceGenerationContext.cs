using CoinW.Net.Objects.Internal;
using CoinW.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoinW.Net.Converters
{
    [JsonSerializable(typeof(CoinWResponse<Dictionary<string, CoinWTicker>>))]
    [JsonSerializable(typeof(CoinWResponse<Dictionary<string, CoinWAsset>>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWSymbol[]>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWOrderBook>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWTrade[]>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWKline[]>))]
    [JsonSerializable(typeof(CoinWResponse<Dictionary<string, decimal>>))]
    [JsonSerializable(typeof(CoinWResponse<Dictionary<string, CoinWBalance>>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWDepositWithdrawal[]>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWDepositAddress[]>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWWithdrawResult>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWOrderResult>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWOrder[]>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWOrderDetails>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWOrderTransaction[]>))]
    [JsonSerializable(typeof(CoinWResponse<CoinWUserTradeWrapper>))]
    [JsonSerializable(typeof(CoinWResponse))]

    [JsonSerializable(typeof(CoinWSocketRequest))]
    [JsonSerializable(typeof(CoinWLoginRequest))]
    [JsonSerializable(typeof(CoinWSocketResponse<CoinWSubscriptionResponse>))]
    [JsonSerializable(typeof(CoinWSocketResponse<string>))]
    [JsonSerializable(typeof(CoinWSocketResponse<CoinWBalanceUpdate>))]
    [JsonSerializable(typeof(CoinWSocketResponse<CoinWOrderUpdate>))]

    [JsonSerializable(typeof(CoinWTickerUpdate))]
    [JsonSerializable(typeof(CoinWSymbolUpdate[]))]
    [JsonSerializable(typeof(CoinWOrderBookUpdate))]
    [JsonSerializable(typeof(CoinWOrderBook))]
    [JsonSerializable(typeof(CoinWKlineUpdate))]
    [JsonSerializable(typeof(CoinWTradeUpdate[]))]
    [JsonSerializable(typeof(CoinWBalanceUpdate))]
    [JsonSerializable(typeof(CoinWOrderUpdate))]


    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    [JsonSerializable(typeof(IDictionary<string, object>))]
    internal partial class CoinWSourceGenerationContext : JsonSerializerContext
    {
    }
}
