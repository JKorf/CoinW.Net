using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Position info
    /// </summary>
    public record CoinWPosition
    {
        /// <summary>
        /// ["<c>contractType</c>"] Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// ["<c>base</c>"] Base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>baseSize</c>"] Base asset quantity
        /// </summary>
        [JsonPropertyName("baseSize")]
        public decimal BaseQuantity { get; set; }
        /// <summary>
        /// ["<c>hedgeId</c>"] Hedge id
        /// </summary>
        [JsonPropertyName("hedgeId")]
        public long? HedgeId { get; set; }
        /// <summary>
        /// ["<c>closedPiece</c>"] Quantity closed in contracts
        /// </summary>
        [JsonPropertyName("closedPiece")]
        public decimal QuantityClosed { get; set; }
        /// <summary>
        /// ["<c>closedQuantity</c>"] Quantity closed in QuantityUnit
        /// </summary>
        [JsonPropertyName("closedQuantity")]
        public decimal? QuantityClosedUnit { get; set; }
        /// <summary>
        /// ["<c>liquidationPrice</c>"] Liquidation price
        /// </summary>
        [JsonPropertyName("liquidationPrice")]
        public decimal? LiquidationPrice { get; set; }
        /// <summary>
        /// ["<c>createdDate</c>"] Create time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>currentPiece</c>"] Current position size in contracts
        /// </summary>
        [JsonPropertyName("currentPiece")]
        public decimal PositionSize { get; set; }
        /// <summary>
        /// ["<c>remainCurrentPiece</c>"] Remaining number of contracts
        /// </summary>
        [JsonPropertyName("remainCurrentPiece")]
        public int? QuantityRemaining { get; set; }
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
        /// ["<c>fundingFee</c>"] Funding fee
        /// </summary>
        [JsonPropertyName("fundingFee")]
        public decimal FundingFee { get; set; }
        /// <summary>
        /// ["<c>fundingSettle</c>"] Funding settle
        /// </summary>
        [JsonPropertyName("fundingSettle")]
        public decimal FundingSettle { get; set; }
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
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// ["<c>openPrice</c>"] Open price
        /// </summary>
        [JsonPropertyName("openPrice")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>orderPrice</c>"] Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public decimal? OrderPrice { get; set; }
        /// <summary>
        /// ["<c>originalType</c>"] Order type
        /// </summary>
        [JsonPropertyName("originalType")]
        public FuturesOrderType OriginalOrderType { get; set; }
        /// <summary>
        /// ["<c>posType</c>"] Order type
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
        /// ["<c>profitReal</c>"] Realized profit and loss
        /// </summary>
        [JsonPropertyName("profitReal")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// ["<c>profitUnreal</c>"] Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("profitUnreal")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Order quantity
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
        /// ["<c>totalPiece</c>"] Total quantity in contracts
        /// </summary>
        [JsonPropertyName("totalPiece")]
        public decimal TotalQuantity { get; set; }
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
    }


}
