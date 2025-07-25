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
    /// Transaction page
    /// </summary>
    public record CoinWFuturesTransactionPage
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
        public CoinWFuturesTransaction[] Rows { get; set; } = [];
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Transaction info
    /// </summary>
    public record CoinWFuturesTransaction
    {
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal BaseQuantity { get; set; }
        /// <summary>
        /// Price at which the position was closed
        /// </summary>
        [JsonPropertyName("closePrice")]
        public decimal? ClosePrice { get; set; }
        /// <summary>
        /// Closed number of contracts
        /// </summary>
        [JsonPropertyName("closedPiece")]
        public decimal QuantityClosed { get; set; }
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// Created time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Open quantity in number of contracts
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal QuantityOpen { get; set; }
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
        /// Fee rate
        /// </summary>
        [JsonPropertyName("feeRate")]
        public decimal FeeRate { get; set; }
        /// <summary>
        /// Funding fee
        /// </summary>
        [JsonPropertyName("fundingFee")]
        public decimal FundingFee { get; set; }
        /// <summary>
        /// Funding settled
        /// </summary>
        [JsonPropertyName("fundingSettle")]
        public decimal FundingSettled { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
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
        /// Realized net profit and loss
        /// </summary>
        [JsonPropertyName("netProfit")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("openId")]
        public long PositionId { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("openPrice")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public decimal OrderPrice { get; set; }
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
        /// Position margin
        /// </summary>
        [JsonPropertyName("positionMargin")]
        public decimal PositionMargin { get; set; }
        /// <summary>
        /// Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public decimal MarginType { get; set; }
        /// <summary>
        /// Process status
        /// </summary>
        [JsonPropertyName("processStatus")]
        public ProcessingStatus ProcessStatus { get; set; }
        /// <summary>
        /// Order quantity in QuantityUnit
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
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
        public TrailingOrderStatus Status { get; set; }
        /// <summary>
        /// Trade role
        /// </summary>
        [JsonPropertyName("takerMaker")]
        public decimal Role { get; set; }
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
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger type
        /// </summary>
        [JsonPropertyName("triggerType")]
        public TriggerOrderType? TriggerType { get; set; }
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
    }
}
