using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Price ticker info
    /// </summary>
    public record CoinWFuturesTicker
    {
        /// <summary>
        /// ["<c>fair_price</c>"] Index price
        /// </summary>
        [JsonPropertyName("fair_price")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// ["<c>max_leverage</c>"] Max leverage
        /// </summary>
        [JsonPropertyName("max_leverage")]
        public decimal MaxLeverage { get; set; }
        /// <summary>
        /// ["<c>total_volume</c>"] Total volume
        /// </summary>
        [JsonPropertyName("total_volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>price_coin</c>"] Price asset
        /// </summary>
        [JsonPropertyName("price_coin")]
        public string PriceAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>contract_id</c>"] Contract id
        /// </summary>
        [JsonPropertyName("contract_id")]
        public long ContractId { get; set; }
        /// <summary>
        /// ["<c>base_coin</c>"] Base asset
        /// </summary>
        [JsonPropertyName("base_coin")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>high</c>"] Highest price last 24h
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>rise_fall_rate</c>"] Price change percentage
        /// </summary>
        [JsonPropertyName("rise_fall_rate")]
        public decimal PriceChangePercentage { get; set; }
        /// <summary>
        /// ["<c>low</c>"] Lowest price last 24h
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>name</c>"] Symbol
        /// </summary>
        [JsonPropertyName("name")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>contract_size</c>"] Contract size
        /// </summary>
        [JsonPropertyName("contract_size")]
        public decimal ContractSize { get; set; }
        /// <summary>
        /// ["<c>quote_coin</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quote_coin")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>last_price</c>"] Last price
        /// </summary>
        [JsonPropertyName("last_price")]
        public decimal LastPrice { get; set; }
    }


}
