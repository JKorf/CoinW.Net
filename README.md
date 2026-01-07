# ![CoinW.Net](https://raw.githubusercontent.com/JKorf/CoinW.Net/main/CoinW.Net/Icon/icon.png) CoinW.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/CoinW.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/CoinW.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/CoinW.Net?style=for-the-badge)

CoinW.Net is a client library for accessing the [CoinW REST and Websocket API](CoinW). 

## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* High performance
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Support for managing different accounts
* Extensive logging
* Support for different environments
* Easy integration with other exchange client based on the CryptoExchange.Net base library
* Native AOT support

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as the latest dotnet versions to use the latest framework features.

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/CoinW.net.svg?style=for-the-badge)](https://www.nuget.org/packages/CoinW.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/CoinW.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/CoinW.Net)

	dotnet add package CoinW.Net
	
### GitHub packages
CoinW.Net is available on [GitHub packages](https://github.com/JKorf/CoinW.Net/pkgs/nuget/CoinW.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/CoinW.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/CoinW.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/CoinW.Net/releases).

## How to use
* REST Endpoints
	```csharp
	// Get the ETH/USDT ticker via rest request
	var restClient = new CoinWRestClient();
	var tickerResult = await restClient.SpotApi.ExchangeData.GetTickersAsync();
	var lastPrice = tickerResult.Data.Single(x => x.Symbol == "ETH_USDT").LastPrice;
	```
* Websocket streams
	```csharp
	// Subscribe to ETH/USDT ticker updates via the websocket API
	var socketClient = new CoinWSocketClient();
	var tickerSubscriptionResult = socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH_USDT", (update) => 
	{
	  var lastPrice = update.Data.LastPrice;
	});
	```

For information on the clients, dependency injection, response processing and more see the [documentation](https://cryptoexchange.jkorf.dev/client-libs/getting-started), or have a look at the examples [here](https://github.com/JKorf/CoinW.Net/tree/main/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## CryptoExchange.Net
CoinW.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://jkorf.github.io/CryptoExchange.Net#idocs_shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Aster|[JKorf/Aster.Net](https://github.com/JKorf/Aster.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Aster.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Aster.Net)|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BitMEX|[JKorf/BitMEX.Net](https://github.com/JKorf/BitMEX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.BitMEX.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|DeepCoin|[JKorf/DeepCoin.Net](https://github.com/JKorf/DeepCoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=flat-square)](https://www.nuget.org/packages/DeepCoin.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Toobit|[JKorf/Toobit.Net](https://github.com/JKorf/Toobit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Toobit.net.svg?style=flat-square)](https://www.nuget.org/packages/Toobit.Net)|
|Upbit|[JKorf/Upbit.Net](https://github.com/JKorf/Upbit.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Upbit.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Upbit.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

When using multiple of these API's the [CryptoClients.Net](https://github.com/JKorf/CryptoClients.Net) package can be used which combines this and the other packages and allows easy access to all exchange API's.

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### Spot REST
|API|Supported|Location|
|--|--:|--|
|Market Information|✓|`restClient.SpotApi.ExchangeData`|
|Place Order|✓|`restClient.SpotApi.Trading`|
|Check Order|✓|`restClient.SpotApi.Trading`|
|Account Information|✓|`restClient.SpotApi.Account`|

### Futures Websocket
|API|Supported|Location|
|--|--:|--|
|Market Information|✓|`socketClient.SpotApi`|
|Check Orders|✓|`socketClient.SpotApi`|
|Account Information|✓|`socketClient.SpotApi`|

### Futures REST
|API|Supported|Location|
|--|--:|--|
|Market Information|✓|`restClient.FuturesApi.ExchangeData`|
|Place Orders|✓|`restClient.FuturesApi.Trading`|
|Check Orders|✓|`restClient.FuturesApi.Trading`|
|Position Information|✓|`restClient.FuturesApi.Account` / `restClient.FuturesApi.Trading`|
|Account & Assets|✓|`restClient.FuturesApi.Account`|
|Common|✓|`restClient.FuturesApi.Account`|

### Futures Websocket
|API|Supported|Location|
|--|--:|--|
|Market Information|✓|`socketClient.FuturesApi`|
|Check Orders|✓|`socketClient.FuturesApi`|
|Position Information|✓|`socketClient.FuturesApi`|
|Account & Assets|✓|`socketClient.FuturesApi`|

## Support the project
Any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1q277a5n54s2l2mzlu778ef7lpkwhjhyvghuv8qf  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd 

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 2.1.1 - 07 Jan 2026
    * Disabled default enabled AutoTimestamp as it's not support by API

* Version 2.1.0 - 07 Jan 2026
    * Updated CryptoExchange.Net version to 10.1.0, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Added DataTimeLocal and DataAge properties to DataEvent object
    * Added UpdateServerTime, UpdateLocalTime and DataAge properties to (I)SymbolOrderBook

* Version 2.0.1 - 17 Dec 2025
    * Fixed websocket unsubscribe response handling

* Version 2.0.0 - 16 Dec 2025
    * Added Net10.0 target framework
    * Updated CryptoExchange.Net version to 10.0.0, see https://github.com/JKorf/CryptoExchange.Net/releases/ for full release notes
    * Improved performance across the board, biggest gains in websocket message processing
    * Updated REST message response handling
    * Updated WebSocket message handling
    * Added UseUpdatedDeserialization socket client options to toggle by new and old message handling
    * Added SocketIndividualSubscriptionCombineTarget socket client option
    * Updated Shared API's subscription update types from ExchangeEvent to DataEvent
    * Fixed exception in restClient.SpotApi.ExchangeData.GetRecentTradesAsync depending on time of day

* Version 1.8.1 - 12 Nov 2025
    * Fixed request signing when parameters contain special characters

* Version 1.8.0 - 11 Nov 2025
    * Updated CryptoExchange.Net version to 9.13.0, see https://github.com/JKorf/CryptoExchange.Net/releases/

* Version 1.7.0 - 03 Nov 2025
    * Updated CryptoExchange.Net to version 9.12.0
    * Added support for using SharedSymbol.UsdOrStable in Shared APIs
    * Fixed exception when initial trade snapshot has no items in TradeTracker
    * Removed some unhelpful verbose logs

* Version 1.6.0 - 16 Oct 2025
    * Updated CryptoExchange.Net version to 9.10.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ClientOrderId mapping on SharedUserTrade models
    * Added ITransferRestClient.TransferAsync implementation
    * Fixed CoinWTradeHistory response model Timestamp and PositionSide types

* Version 1.5.0 - 30 Sep 2025
    * Updated CryptoExchange.Net version to 9.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ITrackerFactory to TrackerFactory implementation

* Version 1.4.0 - 01 Sep 2025
    * Updated CryptoExchange.Net version to 9.7.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * HTTP REST requests will now use HTTP version 2.0 by default

* Version 1.3.0 - 25 Aug 2025
    * Updated CryptoExchange.Net version to 9.6.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ClearUserClients method to user client provider

* Version 1.2.1 - 21 Aug 2025
    * Added missing Futures Shared GetBalancesAsync implementation
    * Added websocket authentication error mapping

* Version 1.2.0 - 20 Aug 2025
    * Updated CryptoExchange.Net to version 9.5.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added improved error parsing
    * Added random default value for clientOrderId if not provided in restClient.SpotApi.Trading.PlaceOrderAsync
    * Updated rest request sending too prevent duplicate parameter serialization

* Version 1.1.0 - 04 Aug 2025
    * Updated CryptoExchange.Net to version 9.4.0, see https://github.com/JKorf/CryptoExchange.Net/releases/

* Version 1.0.1 - 29 Jul 2025
    * Added missing ICoinWUserClientProvider DI registration
    * Fixed spot shared ticker percentage change being factor 100 off
    * Fixed shared spot ticker quote volume mapped as base volume

* Version 1.0.0 - 29 Jul 2025
    * Initial release

