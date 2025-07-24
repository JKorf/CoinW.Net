using CoinW.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order page
    /// </summary>
    public record CoinWHistOrderPage
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
        public CoinWHistOrder[] Rows { get; set; } = [];
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonPropertyName("total")]
        public long Total { get; set; }
    }

    /// <summary>
    /// Order info
    /// </summary>
    public record CoinWHistOrder
    {
        /// <summary>
        /// Average price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// Base quantity
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal BaseQuantity { get; set; }
        /// <summary>
        /// Canceled quantity
        /// </summary>
        [JsonPropertyName("cancelPiece")]
        public decimal QuantityCanceled { get; set; }
        /// <summary>
        /// USDT value filled
        /// </summary>
        [JsonPropertyName("completeUsdt")]
        public decimal ValueFilled { get; set; }
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public decimal CreateTime { get; set; }
        /// <summary>
        /// Open quantity
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal QuantityOpen { get; set; }
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
        /// Is at risk of liquidation
        /// </summary>
        [JsonPropertyName("havShortfall")]
        public bool LiquidationRisk { get; set; }
        /// <summary>
        /// Hedge id
        /// </summary>
        [JsonPropertyName("hedgeId")]
        public long HedgeId { get; set; }
        /// <summary>
        /// Order id
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
        public long PositionId { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public decimal? OrderPrice { get; set; }
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
        /// Order type
        /// </summary>
        [JsonPropertyName("posType")]
        public FuturesOrderType OrderType { get; set; }
        /// <summary>
        /// Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
        /// <summary>
        /// Quantity of the order
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal OrderQuantity { get; set; }
        /// <summary>
        /// Quantity unit
        /// </summary>
        [JsonPropertyName("quantityUnit")]
        public decimal QuantityUnit { get; set; }
        /// <summary>
        /// Source
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// Open status
        /// </summary>
        [JsonPropertyName("status")]
        public OpenStatus Status { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("thirdOrderId")]
        public string ClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// Total quantity in contracts
        /// </summary>
        [JsonPropertyName("totalPiece")]
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// Trade quantity in contracts
        /// </summary>
        [JsonPropertyName("tradePiece")]
        public decimal TradeQuantity { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger order type
        /// </summary>
        [JsonPropertyName("triggerType")]
        public TriggerOrderType? TriggerOrderType { get; set; }
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
