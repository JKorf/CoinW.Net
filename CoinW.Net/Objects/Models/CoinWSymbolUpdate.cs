using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Symbol update
    /// </summary>
    public record CoinWSymbolUpdate
    {
        /// <summary>
        /// ["<c>rose</c>"] Percentage change
        /// </summary>
        [JsonPropertyName("rose")]
        public decimal PercentageChange { get; set; }
        /// <summary>
        /// ["<c>oneDayHighest</c>"] Highest price in last 24h
        /// </summary>
        [JsonPropertyName("oneDayHighest")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>oneDayLowest</c>"] Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("oneDayLowest")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>oneDayTotal</c>"] Volume in base asset
        /// </summary>
        [JsonPropertyName("oneDayTotal")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>currencyVol</c>"] Volume in quote asset
        /// </summary>
        [JsonPropertyName("currencyVol")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// ["<c>leftCoinName</c>"] Base asset
        /// </summary>
        [JsonPropertyName("leftCoinName")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonIgnore]
        public string Symbol => BaseAsset + "_" + QuoteAsset;
        /// <summary>
        /// ["<c>rightCoinName</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("rightCoinName")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>leftCoinUrl</c>"] Base asset img url
        /// </summary>
        [JsonPropertyName("leftCoinUrl")]
        public string BaseAssetImg { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tmId</c>"] Symbol id
        /// </summary>
        [JsonPropertyName("tmId")]
        public int SymbolId { get; set; }
        /// <summary>
        /// ["<c>hotCoinSort</c>"] Rank when sorting assets by 'hot'
        /// </summary>
        [JsonPropertyName("hotCoinSort")]
        public int RankHotSort { get; set; }
        /// <summary>
        /// ["<c>newCoinSort</c>"] Rank when sorting assets by 'new'
        /// </summary>
        [JsonPropertyName("newCoinSort")]
        public int RankNewSort { get; set; }
    }
}
