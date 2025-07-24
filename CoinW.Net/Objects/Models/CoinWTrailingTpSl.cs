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
    /// Trailing take profit / stop loss info
    /// </summary>
    public record CoinWTrailingTpSl
    {
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal BaseQuantity { get; set; }
        /// <summary>
        /// Callback rate
        /// </summary>
        [JsonPropertyName("callbackRate")]
        public decimal CallbackRate { get; set; }
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
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Finish status
        /// </summary>
        [JsonPropertyName("finishStatus")]
        public int FinishStatus { get; set; }
        /// <summary>
        /// Funding fee
        /// </summary>
        [JsonPropertyName("fundingFee")]
        public decimal FundingFee { get; set; }
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
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Position margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("openId")]
        public long PositionId { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("openPrice")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public decimal? OrderPrice { get; set; }
        /// <summary>
        /// Original type
        /// </summary>
        [JsonPropertyName("originalType")]
        public FuturesOrderType OriginalType { get; set; }
        /// <summary>
        /// Pos type
        /// </summary>
        [JsonPropertyName("posType")]
        public FuturesOrderType PosType { get; set; }
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
        public TrailingStatus ProcessStatus { get; set; }
        /// <summary>
        /// Quantity
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
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger type
        /// </summary>
        [JsonPropertyName("triggerType")]
        public OrderType TriggerType { get; set; }
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
