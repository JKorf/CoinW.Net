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
    /// Position change
    /// </summary>
    public record CoinWPositionChange
    {
        /// <summary>
        /// Current position size in contracts
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal PositionSize { get; set; }
        /// <summary>
        /// Is profession
        /// </summary>
        [JsonPropertyName("isProfession")]
        public int IsProfession { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Original order type
        /// </summary>
        [JsonPropertyName("originalType")]
        public FuturesOrderType OriginalOrderType { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("openId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("openPrice")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("orderStatus")]
        public FuturesOrderStatus OrderStatus { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
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
        /// Update time
        /// </summary>
        [JsonPropertyName("updatedDate")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Marin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
        /// <summary>
        /// Fee rate
        /// </summary>
        [JsonPropertyName("feeRate")]
        public decimal FeeRate { get; set; }
        /// <summary>
        /// Realized net profit and loss
        /// </summary>
        [JsonPropertyName("netProfit")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal BaseQuantity { get; set; }
        /// <summary>
        /// Liquidate by
        /// </summary>
        [JsonPropertyName("liquidateBy")]
        public string LiquidateBy { get; set; } = string.Empty;
        /// <summary>
        /// Total quantity in contracts
        /// </summary>
        [JsonPropertyName("totalPiece")]
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public decimal OrderPrice { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Funding settled
        /// </summary>
        [JsonPropertyName("fundingSettle")]
        public decimal FundingSettled { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// Taker of maker
        /// </summary>
        [JsonPropertyName("takerMaker")]
        public Role Role { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        [JsonPropertyName("indexPrice")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Quantity in QuantityUnit
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        /// <summary>
        /// Number of contracts that have been closed.
        /// </summary>
        [JsonPropertyName("closedPiece")]
        public decimal QuantityClosed { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Hedge id
        /// </summary>
        [JsonPropertyName("hedgeId")]
        public string HedgeId { get; set; } = string.Empty;
        /// <summary>
        /// Close price
        /// </summary>
        [JsonPropertyName("closePrice")]
        public decimal? ClosePrice { get; set; }
        /// <summary>
        /// Position margin
        /// </summary>
        [JsonPropertyName("positionMargin")]
        public decimal PositionMargin { get; set; }
        /// <summary>
        /// Real price
        /// </summary>
        [JsonPropertyName("realPrice")]
        public decimal RealPrice { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public OpenStatus Status { get; set; }
    }
}
