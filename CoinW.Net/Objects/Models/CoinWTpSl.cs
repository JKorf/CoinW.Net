using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Take profit / stop loss info
    /// </summary>
    public record CoinWTpSl
    {
        /// <summary>
        /// Total contract quantity in tpsl order
        /// </summary>
        [JsonPropertyName("closePiece")]
        public decimal CloseQuantity { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Current contract quantity in tpsl
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal CurrentQuantity { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Open id
        /// </summary>
        [JsonPropertyName("openId")]
        public long? OpenId { get; set; }
        /// <summary>
        /// Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType? MarginType { get; set; }
        /// <summary>
        /// Price type
        /// </summary>
        [JsonPropertyName("priceType")]
        public PriceType? PriceType { get; set; }
        /// <summary>
        /// Stop loss price
        /// </summary>
        [JsonPropertyName("stopLossPrice")]
        public decimal? StopLossPrice { get; set; }
        /// <summary>
        /// Stop loss rate
        /// </summary>
        [JsonPropertyName("stopLossRate")]
        public decimal? StopLossRate { get; set; }
        /// <summary>
        /// Stop loss order type
        /// </summary>
        [JsonPropertyName("stopLossType")]
        public FuturesOrderType? StopLossOrderType { get; set; }
        /// <summary>
        /// Stop profit price
        /// </summary>
        [JsonPropertyName("stopProfitPrice")]
        public decimal? StopProfitPrice { get; set; }
        /// <summary>
        /// Stop profit rate
        /// </summary>
        [JsonPropertyName("stopProfitRate")]
        public decimal? StopProfitRate { get; set; }
        /// <summary>
        /// Stop profit order type
        /// </summary>
        [JsonPropertyName("stopProfitType")]
        public FuturesOrderType? StopProfitOrderType { get; set; }
        /// <summary>
        /// Stop type
        /// </summary>
        [JsonPropertyName("stopType")]
        public FuturesOrderType StopType { get; set; }
        /// <summary>
        /// Trigger status
        /// </summary>
        [JsonPropertyName("triggerStatus")]
        public TriggerStatus TriggerStatus { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updatedDate")]
        public decimal UpdateTime { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
    }


}
