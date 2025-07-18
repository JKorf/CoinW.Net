using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    internal record CoinWMarginRequirementWrapper
    {
        /// <summary>
        /// Ladder config
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
        /// Symbol
        /// </summary>
        [JsonPropertyName("name")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Ladder list
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
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Tier
        /// </summary>
        [JsonPropertyName("ladder")]
        public decimal Tier { get; set; }
        /// <summary>
        /// Last tier
        /// </summary>
        [JsonPropertyName("lastLadder")]
        public bool IsLastTier { get; set; }
        /// <summary>
        /// Margin keep rate
        /// </summary>
        [JsonPropertyName("marginKeepRate")]
        public decimal MarginKeepRate { get; set; }
        /// <summary>
        /// Maintenance margin rate
        /// </summary>
        [JsonPropertyName("marginStartRate")]
        public decimal MaintenanceMarginRate { get; set; }
        /// <summary>
        /// Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public decimal MaxLeverage { get; set; }
        /// <summary>
        /// Max position size
        /// </summary>
        [JsonPropertyName("maxPiece")]
        public decimal MaxPositionSize { get; set; }
    }
}
