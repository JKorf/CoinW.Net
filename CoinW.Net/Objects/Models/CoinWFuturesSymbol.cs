using CoinW.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public record CoinWFuturesSymbol
    {
        /// <summary>
        /// ["<c>base</c>"] Base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>closeSpread</c>"] Spread
        /// </summary>
        [JsonPropertyName("closeSpread")]
        public decimal Spread { get; set; }
        /// <summary>
        /// ["<c>commissionRate</c>"] Fee rate
        /// </summary>
        [JsonPropertyName("commissionRate")]
        public decimal FeeRate { get; set; }
        /// <summary>
        /// ["<c>configBo</c>"] Config bo
        /// </summary>
        [JsonPropertyName("configBo")]
        public CoinWFuturesSymbolMarginInfo ConfigBo { get; set; } = null!;
        /// <summary>
        /// ["<c>createdDate</c>"] Created time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public decimal CreateTime { get; set; }
        /// <summary>
        /// ["<c>defaultLeverage</c>"] Default leverage
        /// </summary>
        [JsonPropertyName("defaultLeverage")]
        public decimal DefaultLeverage { get; set; }
        /// <summary>
        /// ["<c>defaultStopLossRate</c>"] Default stop loss rate
        /// </summary>
        [JsonPropertyName("defaultStopLossRate")]
        public decimal DefaultStopLossRate { get; set; }
        /// <summary>
        /// ["<c>defaultStopProfitRate</c>"] Default take profit rate
        /// </summary>
        [JsonPropertyName("defaultStopProfitRate")]
        public decimal DefaultTakeProfitRate { get; set; }
        /// <summary>
        /// ["<c>iconUrl</c>"] Icon url
        /// </summary>
        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// ["<c>indexId</c>"] Index id
        /// </summary>
        [JsonPropertyName("indexId")]
        public long IndexId { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Supported leverages
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal[] Leverage { get; set; } = [];
        /// <summary>
        /// ["<c>makerFee</c>"] Maker fee rate
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// ["<c>maxLeverage</c>"] Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public decimal MaxLeverage { get; set; }
        /// <summary>
        /// ["<c>maxPosition</c>"] Max position size
        /// </summary>
        [JsonPropertyName("maxPosition")]
        public decimal MaxPositionQuantity { get; set; }
        /// <summary>
        /// ["<c>minLeverage</c>"] Min leverage
        /// </summary>
        [JsonPropertyName("minLeverage")]
        public decimal MinLeverage { get; set; }
        /// <summary>
        /// ["<c>minSize</c>"] Min position size
        /// </summary>
        [JsonPropertyName("minSize")]
        public decimal MinPositionQuantity { get; set; }
        /// <summary>
        /// ["<c>name</c>"] Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>oneLotMargin</c>"] Required margin per lot
        /// </summary>
        [JsonPropertyName("oneLotMargin")]
        public decimal RequiredMarginPerLot { get; set; }
        /// <summary>
        /// ["<c>oneLotSize</c>"] Lot quantity
        /// </summary>
        [JsonPropertyName("oneLotSize")]
        public decimal LotSize { get; set; }
        /// <summary>
        /// ["<c>oneMaxPosition</c>"] One max position
        /// </summary>
        [JsonPropertyName("oneMaxPosition")]
        public decimal OneMaxPosition { get; set; }
        /// <summary>
        /// ["<c>openSpread</c>"] Opening spread
        /// </summary>
        [JsonPropertyName("openSpread")]
        public decimal OpenSpread { get; set; }
        /// <summary>
        /// ["<c>partitionIds</c>"] Partition ids
        /// </summary>
        [JsonPropertyName("partitionIds")]
        public string PartitionIds { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pricePrecision</c>"] Price decimal places
        /// </summary>
        [JsonPropertyName("pricePrecision")]
        public int PriceDecimals { get; set; }
        /// <summary>
        /// ["<c>quote</c>"] QuoteAsset
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>settledAt</c>"] Settlement time
        /// </summary>
        [JsonPropertyName("settledAt")]
        public DateTime? SettleTime { get; set; }
        /// <summary>
        /// ["<c>settledPeriod</c>"] Settlement period in hours
        /// </summary>
        [JsonPropertyName("settledPeriod")]
        public decimal SettlementPeriod { get; set; }
        /// <summary>
        /// ["<c>settlementRate</c>"] Settlement rate
        /// </summary>
        [JsonPropertyName("settlementRate")]
        public decimal SettlementRate { get; set; }
        /// <summary>
        /// ["<c>sort</c>"] Sort
        /// </summary>
        [JsonPropertyName("sort")]
        public int Sort { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public FuturesSymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>stopCrossPositionRate</c>"] Cross margin risk rate
        /// </summary>
        [JsonPropertyName("stopCrossPositionRate")]
        public decimal CrossMarginRiskRate { get; set; }
        /// <summary>
        /// ["<c>stopSurplusRate</c>"] Minimum remaining margin ratio
        /// </summary>
        [JsonPropertyName("stopSurplusRate")]
        public decimal MinRemainingMarginRatio { get; set; }
        /// <summary>
        /// ["<c>takerFee</c>"] Taker fee rate
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// ["<c>updatedDate</c>"] Update time
        /// </summary>
        [JsonPropertyName("updatedDate")]
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// Margin info
    /// </summary>
    public record CoinWFuturesSymbolMarginInfo
    {
        /// <summary>
        /// ["<c>margins</c>"] Margins
        /// </summary>
        [JsonPropertyName("margins")]
        public Dictionary<string, decimal> Margins { get; set; } = null!;
        /// <summary>
        /// ["<c>simulatedMargins</c>"] Simulated margins
        /// </summary>
        [JsonPropertyName("simulatedMargins")]
        public Dictionary<string, decimal> SimulatedMargins { get; set; } = null!;
    }
}
