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
    public record CoinWFuturesOrderPage
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
        public CoinWFuturesOrder[] Rows { get; set; } = [];
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    /// <summary>
    /// Order info
    /// </summary>
    public record CoinWFuturesOrder
    {
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal BaseQuantity { get; set; }
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Quantity open in contracts
        /// </summary>
        #warning check
        [JsonPropertyName("currentPiece")]
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Frozen fee
        /// </summary>
        [JsonPropertyName("frozenFee")]
        public decimal FrozenFee { get; set; }
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
        /// Maker fee
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
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
        /// Original type
        /// </summary>
        [JsonPropertyName("originalType")]
        public FuturesOrderType OriginalOrderType { get; set; }
        /// <summary>
        /// Pos type
        /// </summary>
        [JsonPropertyName("posType")]
        public FuturesOrderType OrderType { get; set; }
        /// <summary>
        /// Position margin
        /// </summary>
        [JsonPropertyName("positionMargin")]
        public decimal PositionMargin { get; set; }
        /// <summary>
        /// Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
        /// <summary>
        /// Process status
        /// </summary>
        [JsonPropertyName("processStatus")]
        public int? ProcessStatus { get; set; }
        /// <summary>
        /// Order quantity in unit specified by QuantityUnit
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal OrderQuantity { get; set; }
        /// <summary>
        /// Quantity unit
        /// </summary>
        [JsonPropertyName("quantityUnit")]
        public QuantityUnit QuantityUnit { get; set; }
        /// <summary>
        /// Source
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public OpenStatus OpenStatus { get; set; }
        /// <summary>
        /// Taker fee
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// Total order quantity
        /// </summary>
        [JsonPropertyName("totalPiece")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updatedDate")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }

        /// <summary>
        /// Stop loss trigger price
        /// </summary>
        [JsonPropertyName("stopLossPrice")]
        public decimal? StopLossPrice { get; set; }
        /// <summary>
        /// Stop loss rate
        /// </summary>
        [JsonPropertyName("stopLossRate")]
        public decimal? StopLossRate { get; set; }
        /// <summary>
        /// Stop loss order price
        /// </summary>
        [JsonPropertyName("stopLossOrderPrice")]
        public decimal? StopLossOrderPrice { get; set; }
        /// <summary>
        /// Stop loss order type
        /// </summary>
        [JsonPropertyName("stopLossType")]
        public OrderType? StopLossType { get; set; }
        /// <summary>
        /// Take profit trigger price
        /// </summary>
        [JsonPropertyName("takeProfitPrice")]
        public decimal? TakeProfitPrice { get; set; }
        /// <summary>
        /// Take profit rate
        /// </summary>
        [JsonPropertyName("takeProfitRate")]
        public decimal? TakeProfitRate { get; set; }
        /// <summary>
        /// Take profit order price
        /// </summary>
        [JsonPropertyName("takeProfitOrderPrice")]
        public decimal? TakeProfitOrderPrice { get; set; }
        /// <summary>
        /// Take profit order type
        /// </summary>
        [JsonPropertyName("takeProfitType")]
        public OrderType? TakeProfitType { get; set; }
    }


}
