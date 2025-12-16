using CoinW.Net.Enums;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// 
    /// </summary>
    public record CoinWPositionHistoryPage
    {
        /// <summary>
        /// Next id
        /// </summary>
        [JsonPropertyName("nextId")]
        public long NextId { get; set; }
        /// <summary>
        /// Prev id
        /// </summary>
        [JsonPropertyName("prevId")]
        public long PrevId { get; set; }
        /// <summary>
        /// Rows
        /// </summary>
        [JsonPropertyName("rows")]
        public CoinWPositionHistory[] Rows { get; set; } = [];
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Position history
    /// </summary>
    public record CoinWPositionHistory
    {
        /// <summary>
        /// Average open price
        /// </summary>
        [JsonPropertyName("avgOpenPrice")]
        public decimal AverageOpenPrice { get; set; }
        /// <summary>
        /// Quantity traded in USDT
        /// </summary>
        [JsonPropertyName("completeUsdt")]
        public decimal QuantityFilledUsdt { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Order quantity in USDT
        /// </summary>
        [JsonPropertyName("entrustUsdt")]
        public decimal OrderQuantityUsdt { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Liquidation risk
        /// </summary>
        [JsonPropertyName("havShortfall")]
        public bool LiquidationRisk { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Liquidate by
        /// </summary>
        [JsonPropertyName("liquidateBy")]
        public string LiquidateBy { get; set; } = string.Empty;
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("openId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("orderStatus")]
        public FuturesOrderStatus OrderStatus { get; set; }
        /// <summary>
        /// Original order type
        /// </summary>
        [JsonPropertyName("originalType")]
        public FuturesOrderType OriginalOrderType { get; set; }
        /// <summary>
        /// Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
        /// <summary>
        /// Open status
        /// </summary>
        [JsonPropertyName("status")]
        public OpenStatus Status { get; set; }
        /// <summary>
        /// Total quantity in contracts
        /// </summary>
        [JsonPropertyName("totalPiece")]
        public decimal QuantityTotal { get; set; }
        /// <summary>
        /// Filled quantity in contracts
        /// </summary>
        [JsonPropertyName("tradePiece")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("tradeStartDate")]
        public decimal CreateTime { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;
    }
}
