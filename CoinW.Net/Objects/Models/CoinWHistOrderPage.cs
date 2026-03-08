using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order page
    /// </summary>
    public record CoinWHistOrderPage
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
        public CoinWHistOrder[] Rows { get; set; } = [];
        /// <summary>
        /// ["<c>total</c>"] Total number of results
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
        /// ["<c>avgPrice</c>"] Average price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// ["<c>baseSize</c>"] Base quantity
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal QuantityOpenBase { get; set; }
        /// <summary>
        /// ["<c>cancelPiece</c>"] Canceled quantity
        /// </summary>
        [JsonPropertyName("cancelPiece")]
        public decimal QuantityCanceled { get; set; }
        /// <summary>
        /// ["<c>completeUsdt</c>"] USDT value filled
        /// </summary>
        [JsonPropertyName("completeUsdt")]
        public decimal ValueFilled { get; set; }
        /// <summary>
        /// ["<c>tradePiece</c>"] Quantity filled in contracts
        /// </summary>
        [JsonPropertyName("tradePiece")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>contractType</c>"] Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// ["<c>createdDate</c>"] Create time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>currentPiece</c>"] Open quantity
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal QuantityOpen { get; set; }
        /// <summary>
        /// ["<c>direction</c>"] Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>entrustUsdt</c>"] Order quantity in USDT
        /// </summary>
        [JsonPropertyName("entrustUsdt")]
        public decimal OrderQuantityUsdt { get; set; }
        /// <summary>
        /// ["<c>havShortfall</c>"] Is at risk of liquidation
        /// </summary>
        [JsonPropertyName("havShortfall")]
        public bool LiquidationRisk { get; set; }
        /// <summary>
        /// ["<c>hedgeId</c>"] Hedge id
        /// </summary>
        [JsonPropertyName("hedgeId")]
        public long HedgeId { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Order id
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
        /// ["<c>openId</c>"] Position id
        /// </summary>
        [JsonPropertyName("openId")]
        public long PositionId { get; set; }
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>orderPrice</c>"] Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public decimal? OrderPrice { get; set; }
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
        /// ["<c>posType</c>"] Order type
        /// </summary>
        [JsonPropertyName("posType")]
        public FuturesOrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>positionModel</c>"] Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Quantity of the order
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal OrderQuantity { get; set; }
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
        /// ["<c>status</c>"] Open status
        /// </summary>
        [JsonPropertyName("status")]
        public OpenStatus Status { get; set; }
        /// <summary>
        /// ["<c>thirdOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("thirdOrderId")]
        public string? ClientOrderId { get; set; }
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
        /// ["<c>triggerType</c>"] Trigger order type
        /// </summary>
        [JsonPropertyName("triggerType")]
        public TriggerOrderType? TriggerOrderType { get; set; }
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
