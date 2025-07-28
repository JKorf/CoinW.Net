using CoinW.Net.Interfaces.Clients;
using CoinW.Net.Objects.Models;
using CoinW.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinW.Net
{
    /// <summary>
    /// Util methods for the CoinW API
    /// </summary>
    public static class CoinWUtils
    {
        private static Dictionary<string, CoinWTicker[]> _spotSymbolInfo = new Dictionary<string, CoinWTicker[]>();
        private static Dictionary<string, CoinWAsset[]> _spotAssetInfo = new Dictionary<string, CoinWAsset[]>();
        private static Dictionary<string, DateTime> _lastSpotSymbolUpdateTime = new Dictionary<string, DateTime>();
        private static Dictionary<string, DateTime> _lastSpotAssetUpdateTime = new Dictionary<string, DateTime>();

        private static readonly SemaphoreSlim _semaphoreSpot = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Update the internal spot symbol info
        /// </summary>
        public static async Task<CallResult> UpdateSpotSymbolInfoAsync(ICoinWRestClient client)
        {
            await _semaphoreSpot.WaitAsync().ConfigureAwait(false);
            try
            {
                var envName = ((CoinWRestOptions)client.ClientOptions).Environment.Name;
                _lastSpotSymbolUpdateTime.TryGetValue(envName, out var lastUpdateTime);
                if (DateTime.UtcNow - lastUpdateTime < TimeSpan.FromHours(1))
                    return CallResult.SuccessResult;

                var symbolInfo = await client.SpotApi.ExchangeData.GetTickersAsync().ConfigureAwait(false);
                if (!symbolInfo)
                    return symbolInfo.AsDataless();

                _spotSymbolInfo[envName] = symbolInfo.Data;
                _lastSpotSymbolUpdateTime[envName] = DateTime.UtcNow;
                return CallResult.SuccessResult;
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }

        /// <summary>
        /// Update the internal spot symbol info
        /// </summary>
        public static async Task<CallResult> UpdateSpotAssetInfoAsync(ICoinWRestClient client)
        {
            await _semaphoreSpot.WaitAsync().ConfigureAwait(false);
            try
            {
                var envName = ((CoinWRestOptions)client.ClientOptions).Environment.Name;
                _lastSpotAssetUpdateTime.TryGetValue(envName, out var lastUpdateTime);
                if (DateTime.UtcNow - lastUpdateTime < TimeSpan.FromHours(1))
                    return CallResult.SuccessResult;

                var assetInfo = await client.SpotApi.ExchangeData.GetAssetsAsync().ConfigureAwait(false);
                if (!assetInfo)
                    return assetInfo.AsDataless();

                _spotAssetInfo[envName] = assetInfo.Data;
                _lastSpotAssetUpdateTime[envName] = DateTime.UtcNow;
                return CallResult.SuccessResult;
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }

        /// <summary>
        /// Get symbol id from a symbol name
        /// </summary>
        /// <param name="client">Client to make a request to retrieve exchange info if necessary</param>
        /// <param name="symbolName">Symbol name</param>
        /// <returns></returns>
        public static async Task<CallResult<int>> GetSymbolIdFromNameAsync(ICoinWRestClient client, string symbolName)
        {
            if (symbolName == "UnitTest")
                return new CallResult<int>(1);

            var update = await UpdateSpotSymbolInfoAsync(client).ConfigureAwait(false);
            if (!update)
                return new CallResult<int>(update.Error!);

            var envName = ((CoinWRestOptions)client.ClientOptions).Environment.Name;
            var symbol = _spotSymbolInfo[envName].SingleOrDefault(x => string.Equals(x.Symbol, symbolName, StringComparison.InvariantCultureIgnoreCase));
            if (symbol == null)
                return new CallResult<int>(new ServerError("Symbol not found"));

            return new CallResult<int>(symbol.Id);
        }

        /// <summary>
        /// Get asset id from an asset name
        /// </summary>
        /// <param name="client">Client to make a request to retrieve exchange info if necessary</param>
        /// <param name="assetName">Asset name</param>
        /// <returns></returns>
        public static async Task<CallResult<int>> GetAssetIdFromNameAsync(ICoinWRestClient client, string assetName)
        {
            if (assetName == "UnitTest")
                return new CallResult<int>(1);

            var update = await UpdateSpotAssetInfoAsync(client).ConfigureAwait(false);
            if (!update)
                return new CallResult<int>(update.Error!);

            var envName = ((CoinWRestOptions)client.ClientOptions).Environment.Name;
            var symbol = _spotAssetInfo[envName].SingleOrDefault(x => string.Equals(x.Asset, assetName, StringComparison.InvariantCultureIgnoreCase));
            if (symbol == null)
                return new CallResult<int>(new ServerError("Asset not found"));

            return new CallResult<int>(symbol.Id);
        }
    }
}
