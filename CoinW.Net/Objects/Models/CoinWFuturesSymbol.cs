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
    /// Symbol info
    /// </summary>
    public record CoinWFuturesSymbol
    {
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Spread
        /// </summary>
        [JsonPropertyName("closeSpread")]
        public decimal Spread { get; set; }
        /// <summary>
        /// Fee rate
        /// </summary>
        [JsonPropertyName("commissionRate")]
        public decimal FeeRate { get; set; }
        /// <summary>
        /// Config bo
        /// </summary>
        [JsonPropertyName("configBo")]
        public CoinWFuturesSymbolMarginInfo ConfigBo { get; set; } = null!;
        /// <summary>
        /// Created time
        /// </summary>
        [JsonPropertyName("createdDate")]
        public decimal CreateTime { get; set; }
        /// <summary>
        /// Default leverage
        /// </summary>
        [JsonPropertyName("defaultLeverage")]
        public decimal DefaultLeverage { get; set; }
        /// <summary>
        /// Default stop loss rate
        /// </summary>
        [JsonPropertyName("defaultStopLossRate")]
        public decimal DefaultStopLossRate { get; set; }
        /// <summary>
        /// Default take profit rate
        /// </summary>
        [JsonPropertyName("defaultStopProfitRate")]
        public decimal DefaultTakeProfitRate { get; set; }
        /// <summary>
        /// Icon url
        /// </summary>
        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; } = string.Empty;
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
        /// <summary>
        /// Index id
        /// </summary>
        [JsonPropertyName("indexId")]
        public long IndexId { get; set; }
        /// <summary>
        /// Supported leverages
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal[] Leverage { get; set; } = [];
        /// <summary>
        /// Maker fee rate
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public decimal MaxLeverage { get; set; }
        /// <summary>
        /// Max position size
        /// </summary>
        [JsonPropertyName("maxPosition")]
        public decimal MaxPositionQuantity { get; set; }
        /// <summary>
        /// Min leverage
        /// </summary>
        [JsonPropertyName("minLeverage")]
        public decimal MinLeverage { get; set; }
        /// <summary>
        /// Min position size
        /// </summary>
        [JsonPropertyName("minSize")]
        public decimal MinPositionQuantity { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Required margin per lot
        /// </summary>
        [JsonPropertyName("oneLotMargin")]
        public decimal RequiredMarginPerLot { get; set; }
        /// <summary>
        /// Lot quantity
        /// </summary>
        [JsonPropertyName("oneLotSize")]
        public decimal LotSize { get; set; }
        /// <summary>
        /// One max position
        /// </summary>
        [JsonPropertyName("oneMaxPosition")]
        public decimal OneMaxPosition { get; set; }
        /// <summary>
        /// Opening spread
        /// </summary>
        [JsonPropertyName("openSpread")]
        public decimal OpenSpread { get; set; }
        /// <summary>
        /// Partition ids
        /// </summary>
        [JsonPropertyName("partitionIds")]
        public string PartitionIds { get; set; } = string.Empty;
        /// <summary>
        /// Price decimal places
        /// </summary>
        [JsonPropertyName("pricePrecision")]
        public int PriceDecimals { get; set; }
        /// <summary>
        /// QuoteAsset
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Settlement time
        /// </summary>
        [JsonPropertyName("settledAt")]
        public DateTime? SettleTime { get; set; }
        /// <summary>
        /// Settlement period in hours
        /// </summary>
        [JsonPropertyName("settledPeriod")]
        public decimal SettlementPeriod { get; set; }
        /// <summary>
        /// Settlement rate
        /// </summary>
        [JsonPropertyName("settlementRate")]
        public decimal SettlementRate { get; set; }
        /// <summary>
        /// Sort
        /// </summary>
        [JsonPropertyName("sort")]
        public int Sort { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public FuturesSymbolStatus Status { get; set; }
        /// <summary>
        /// Cross margin risk rate
        /// </summary>
        [JsonPropertyName("stopCrossPositionRate")]
        public decimal CrossMarginRiskRate { get; set; }
        /// <summary>
        /// Minimum remaining margin ratio
        /// </summary>
        [JsonPropertyName("stopSurplusRate")]
        public decimal MinRemainingMarginRatio { get; set; }
        /// <summary>
        /// Taker fee rate
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// Update time
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
        /// Margins
        /// </summary>
        [JsonPropertyName("margins")]
        public Dictionary<string, decimal> Margins { get; set; } = null!;
        /// <summary>
        /// Simulated margins
        /// </summary>
        [JsonPropertyName("simulatedMargins")]
        public Dictionary<string, decimal> SimulatedMargins { get; set; } = null!;
    }
}
