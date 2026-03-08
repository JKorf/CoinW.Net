using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order page
    /// </summary>
    public record CoinWFuturesOrderPage
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
        public CoinWFuturesOrder[] Rows { get; set; } = [];
        /// <summary>
        /// ["<c>total</c>"] Total number of results
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
        /// ["<c>baseSize</c>"] Quantity in base asset
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal QuantityOpenBase { get; set; }
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
        /// ["<c>currentPiece</c>"] Quantity open in contracts
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// ["<c>tradePiece</c>"] Quantity filled in contracts
        /// </summary>
        [JsonPropertyName("tradePiece")]
        public decimal QuantityFilled { get; set; }
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
        /// ["<c>avgPrice</c>"] Average price
        /// </summary>
        [JsonPropertyName("avgPrice")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// ["<c>frozenFee</c>"] Frozen fee
        /// </summary>
        [JsonPropertyName("frozenFee")]
        public decimal FrozenFee { get; set; }
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
        /// ["<c>makerFee</c>"] Maker fee
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
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
        /// ["<c>originalType</c>"] Original type
        /// </summary>
        [JsonPropertyName("originalType")]
        public FuturesOrderType OriginalOrderType { get; set; }
        /// <summary>
        /// ["<c>posType</c>"] Pos type
        /// </summary>
        [JsonPropertyName("posType")]
        public FuturesOrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>positionMargin</c>"] Position margin
        /// </summary>
        [JsonPropertyName("positionMargin")]
        public decimal PositionMargin { get; set; }
        /// <summary>
        /// ["<c>positionModel</c>"] Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
        /// <summary>
        /// ["<c>processStatus</c>"] Process status
        /// </summary>
        [JsonPropertyName("processStatus")]
        public int? ProcessStatus { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Order quantity in unit specified by QuantityUnit
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
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public OpenStatus OpenStatus { get; set; }
        /// <summary>
        /// ["<c>takerFee</c>"] Taker fee
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// ["<c>totalPiece</c>"] Total order quantity
        /// </summary>
        [JsonPropertyName("totalPiece")]
        public decimal Quantity { get; set; }
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

        /// <summary>
        /// ["<c>stopLossPrice</c>"] Stop loss trigger price
        /// </summary>
        [JsonPropertyName("stopLossPrice")]
        public decimal? StopLossPrice { get; set; }
        /// <summary>
        /// ["<c>stopLossRate</c>"] Stop loss rate
        /// </summary>
        [JsonPropertyName("stopLossRate")]
        public decimal? StopLossRate { get; set; }
        /// <summary>
        /// ["<c>stopLossOrderPrice</c>"] Stop loss order price
        /// </summary>
        [JsonPropertyName("stopLossOrderPrice")]
        public decimal? StopLossOrderPrice { get; set; }
        /// <summary>
        /// ["<c>stopLossType</c>"] Stop loss order type
        /// </summary>
        [JsonPropertyName("stopLossType")]
        public FuturesOrderType? StopLossType { get; set; }
        /// <summary>
        /// ["<c>stopProfitPrice</c>"] Take profit trigger price
        /// </summary>
        [JsonPropertyName("stopProfitPrice")]
        public decimal? TakeProfitPrice { get; set; }
        /// <summary>
        /// ["<c>stopProfitRate</c>"] Take profit rate
        /// </summary>
        [JsonPropertyName("stopProfitRate")]
        public decimal? TakeProfitRate { get; set; }
        /// <summary>
        /// ["<c>stopProfitOrderPrice</c>"] Take profit order price
        /// </summary>
        [JsonPropertyName("stopProfitOrderPrice")]
        public decimal? TakeProfitOrderPrice { get; set; }
        /// <summary>
        /// ["<c>stopProfitType</c>"] Take profit order type
        /// </summary>
        [JsonPropertyName("stopProfitType")]
        public FuturesOrderType? TakeProfitType { get; set; }
    
        /// <summary>
        /// ["<c>thirdOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("thirdOrderId")]
        public string? ClientOrderId { get; set; }
    }


}
