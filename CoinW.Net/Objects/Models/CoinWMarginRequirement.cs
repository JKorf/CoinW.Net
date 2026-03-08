using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    internal record CoinWMarginRequirementWrapper
    {
        /// <summary>
        /// ["<c>ladderConfig</c>"] Ladder config
        /// </summary>
        [JsonPropertyName("ladderConfig")]
        public CoinWMarginRequirement[] LadderConfig { get; set; } = [];
    }

    /// <summary>
    /// Symbol margin requirement
    /// </summary>
    public record CoinWMarginRequirement
    {
        /// <summary>
        /// ["<c>name</c>"] Symbol
        /// </summary>
        [JsonPropertyName("name")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ladderList</c>"] Ladder list
        /// </summary>
        [JsonPropertyName("ladderList")]
        public CoinWMarginRequirementTier[] Tiers { get; set; } = [];
    }

    /// <summary>
    /// Margin tier info
    /// </summary>
    public record CoinWMarginRequirementTier
    {
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>instrument</c>"] Symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ladder</c>"] Tier
        /// </summary>
        [JsonPropertyName("ladder")]
        public decimal Tier { get; set; }
        /// <summary>
        /// ["<c>lastLadder</c>"] Last tier
        /// </summary>
        [JsonPropertyName("lastLadder")]
        public bool IsLastTier { get; set; }
        /// <summary>
        /// ["<c>marginKeepRate</c>"] Margin keep rate
        /// </summary>
        [JsonPropertyName("marginKeepRate")]
        public decimal MarginKeepRate { get; set; }
        /// <summary>
        /// ["<c>marginStartRate</c>"] Maintenance margin rate
        /// </summary>
        [JsonPropertyName("marginStartRate")]
        public decimal MaintenanceMarginRate { get; set; }
        /// <summary>
        /// ["<c>maxLeverage</c>"] Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public decimal MaxLeverage { get; set; }
        /// <summary>
        /// ["<c>maxPiece</c>"] Max position size
        /// </summary>
        [JsonPropertyName("maxPiece")]
        public decimal MaxPositionSize { get; set; }
    }
}
