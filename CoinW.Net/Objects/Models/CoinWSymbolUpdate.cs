using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Symbol update
    /// </summary>
    public record CoinWSymbolUpdate
    {
        /// <summary>
        /// Percentage change
        /// </summary>
        [JsonPropertyName("rose")]
        public decimal PercentageChange { get; set; }
        /// <summary>
        /// Highest price in last 24h
        /// </summary>
        [JsonPropertyName("oneDayHighest")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("oneDayLowest")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Volume in base asset
        /// </summary>
        [JsonPropertyName("oneDayTotal")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Volume in quote asset
        /// </summary>
        [JsonPropertyName("currencyVol")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("leftCoinName")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonIgnore]
        public string Symbol => BaseAsset + "_" + QuoteAsset;
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("rightCoinName")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Base asset img url
        /// </summary>
        [JsonPropertyName("leftCoinUrl")]
        public string BaseAssetImg { get; set; } = string.Empty;
        /// <summary>
        /// Symbol id
        /// </summary>
        [JsonPropertyName("tmId")]
        public int SymbolId { get; set; }
        /// <summary>
        /// Rank when sorting assets by 'hot'
        /// </summary>
        [JsonPropertyName("hotCoinSort")]
        public int RankHotSort { get; set; }
        /// <summary>
        /// Rank when sorting assets by 'new'
        /// </summary>
        [JsonPropertyName("newCoinSort")]
        public int RankNewSort { get; set; }
    }
}
