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
        /// ["<c>closePiece</c>"] Total contract quantity in tpsl order
        /// </summary>
        [JsonPropertyName("closePiece")]
        public decimal CloseQuantity { get; set; }
        /// <summary>
        /// ["<c>createdDate</c>"] Create time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>currentPiece</c>"] Current contract quantity in tpsl
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal CurrentQuantity { get; set; }
        /// <summary>
        /// ["<c>direction</c>"] Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>indexPrice</c>"] Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// ["<c>instrument</c>"] Symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// ["<c>openId</c>"] Open id
        /// </summary>
        [JsonPropertyName("openId")]
        public long? OpenId { get; set; }
        /// <summary>
        /// ["<c>positionModel</c>"] Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType? MarginType { get; set; }
        /// <summary>
        /// ["<c>priceType</c>"] Price type
        /// </summary>
        [JsonPropertyName("priceType")]
        public PriceType? PriceType { get; set; }
        /// <summary>
        /// ["<c>stopLossPrice</c>"] Stop loss price
        /// </summary>
        [JsonPropertyName("stopLossPrice")]
        public decimal? StopLossPrice { get; set; }
        /// <summary>
        /// ["<c>stopLossRate</c>"] Stop loss rate
        /// </summary>
        [JsonPropertyName("stopLossRate")]
        public decimal? StopLossRate { get; set; }
        /// <summary>
        /// ["<c>stopLossType</c>"] Stop loss order type
        /// </summary>
        [JsonPropertyName("stopLossType")]
        public FuturesOrderType? StopLossOrderType { get; set; }
        /// <summary>
        /// ["<c>stopProfitPrice</c>"] Stop profit price
        /// </summary>
        [JsonPropertyName("stopProfitPrice")]
        public decimal? StopProfitPrice { get; set; }
        /// <summary>
        /// ["<c>stopProfitRate</c>"] Stop profit rate
        /// </summary>
        [JsonPropertyName("stopProfitRate")]
        public decimal? StopProfitRate { get; set; }
        /// <summary>
        /// ["<c>stopProfitType</c>"] Stop profit order type
        /// </summary>
        [JsonPropertyName("stopProfitType")]
        public FuturesOrderType? StopProfitOrderType { get; set; }
        /// <summary>
        /// ["<c>stopType</c>"] Stop type
        /// </summary>
        [JsonPropertyName("stopType")]
        public FuturesOrderType StopType { get; set; }
        /// <summary>
        /// ["<c>triggerStatus</c>"] Trigger status
        /// </summary>
        [JsonPropertyName("triggerStatus")]
        public TriggerStatus TriggerStatus { get; set; }
        /// <summary>
        /// ["<c>updatedDate</c>"] Update time
        /// </summary>
        [JsonPropertyName("updatedDate")]
        public decimal UpdateTime { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
    }


}
