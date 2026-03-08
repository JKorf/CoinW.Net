using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Transaction page
    /// </summary>
    public record CoinWFuturesTransactionPage
    {
        /// <summary>
        /// ["<c>nextId</c>"] Next id
        /// </summary>
        [JsonPropertyName("nextId")]
        public long NextId { get; set; }
        /// <summary>
        /// ["<c>prevId</c>"] Prev id
        /// </summary>
        [JsonPropertyName("prevId")]
        public long PrevId { get; set; }
        /// <summary>
        /// ["<c>rows</c>"] Rows
        /// </summary>
        [JsonPropertyName("rows")]
        public CoinWFuturesTransaction[] Rows { get; set; } = [];
        /// <summary>
        /// ["<c>total</c>"] Total number of results
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
        /// ["<c>baseSize</c>"] Quantity in base asset
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal BaseQuantity { get; set; }
        /// <summary>
        /// ["<c>closePrice</c>"] Price at which the position was closed
        /// </summary>
        [JsonPropertyName("closePrice")]
        public decimal? ClosePrice { get; set; }
        /// <summary>
        /// ["<c>closedPiece</c>"] Closed number of contracts
        /// </summary>
        [JsonPropertyName("closedPiece")]
        public decimal QuantityClosed { get; set; }
        /// <summary>
        /// ["<c>contractType</c>"] Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// ["<c>createdDate</c>"] Created time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>currentPiece</c>"] Open quantity in number of contracts
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal QuantityOpen { get; set; }
        /// <summary>
        /// ["<c>direction</c>"] Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>feeRate</c>"] Fee rate
        /// </summary>
        [JsonPropertyName("feeRate")]
        public decimal FeeRate { get; set; }
        /// <summary>
        /// ["<c>fundingFee</c>"] Funding fee
        /// </summary>
        [JsonPropertyName("fundingFee")]
        public decimal FundingFee { get; set; }
        /// <summary>
        /// ["<c>fundingSettle</c>"] Funding settled
        /// </summary>
        [JsonPropertyName("fundingSettle")]
        public decimal FundingSettled { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>instrument</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// ["<c>liquidateBy</c>"] Liquidate by
        /// </summary>
        [JsonPropertyName("liquidateBy")]
        public string LiquidateBy { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// ["<c>netProfit</c>"] Realized net profit and loss
        /// </summary>
        [JsonPropertyName("netProfit")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// ["<c>openId</c>"] Position id
        /// </summary>
        [JsonPropertyName("openId")]
        public long PositionId { get; set; }
        /// <summary>
        /// ["<c>openPrice</c>"] Open price
        /// </summary>
        [JsonPropertyName("openPrice")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>orderPrice</c>"] Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public decimal OrderPrice { get; set; }
        /// <summary>
        /// ["<c>orderStatus</c>"] Order status
        /// </summary>
        [JsonPropertyName("orderStatus")]
        public FuturesOrderStatus OrderStatus { get; set; }
        /// <summary>
        /// ["<c>originalType</c>"] Original order type
        /// </summary>
        [JsonPropertyName("originalType")]
        public FuturesOrderType OriginalOrderType { get; set; }
        /// <summary>
        /// ["<c>positionMargin</c>"] Position margin
        /// </summary>
        [JsonPropertyName("positionMargin")]
        public decimal PositionMargin { get; set; }
        /// <summary>
        /// ["<c>positionModel</c>"] Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public decimal MarginType { get; set; }
        /// <summary>
        /// ["<c>processStatus</c>"] Process status
        /// </summary>
        [JsonPropertyName("processStatus")]
        public ProcessingStatus ProcessStatus { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Order quantity in QuantityUnit
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>quantityUnit</c>"] Quantity unit
        /// </summary>
        [JsonPropertyName("quantityUnit")]
        public QuantityUnit QuantityUnit { get; set; }
        /// <summary>
        /// ["<c>source</c>"] Source
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public TrailingOrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>takerMaker</c>"] Trade role
        /// </summary>
        [JsonPropertyName("takerMaker")]
        public Role Role { get; set; }
        /// <summary>
        /// ["<c>thirdOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("thirdOrderId")]
        public string ClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>totalPiece</c>"] Total quantity in contracts
        /// </summary>
        [JsonPropertyName("totalPiece")]
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// ["<c>triggerPrice</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>triggerType</c>"] Trigger type
        /// </summary>
        [JsonPropertyName("triggerType")]
        public TriggerOrderType? TriggerType { get; set; }
        /// <summary>
        /// ["<c>updatedDate</c>"] Update time
        /// </summary>
        [JsonPropertyName("updatedDate")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>userId</c>"] User id
        /// </summary>
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
    }
}
