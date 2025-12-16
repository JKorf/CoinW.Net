using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Price ticker info
    /// </summary>
    public record CoinWFuturesTicker
    {
        /// <summary>
        /// Index price
        /// </summary>
        [JsonPropertyName("fair_price")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Max leverage
        /// </summary>
        [JsonPropertyName("max_leverage")]
        public decimal MaxLeverage { get; set; }
        /// <summary>
        /// Total volume
        /// </summary>
        [JsonPropertyName("total_volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Price asset
        /// </summary>
        [JsonPropertyName("price_coin")]
        public string PriceAsset { get; set; } = string.Empty;
        /// <summary>
        /// Contract id
        /// </summary>
        [JsonPropertyName("contract_id")]
        public long ContractId { get; set; }
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("base_coin")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Highest price last 24h
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Price change percentage
        /// </summary>
        [JsonPropertyName("rise_fall_rate")]
        public decimal PriceChangePercentage { get; set; }
        /// <summary>
        /// Lowest price last 24h
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("name")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Contract size
        /// </summary>
        [JsonPropertyName("contract_size")]
        public decimal ContractSize { get; set; }
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("quote_coin")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Last price
        /// </summary>
        [JsonPropertyName("last_price")]
        public decimal LastPrice { get; set; }
    }


}
