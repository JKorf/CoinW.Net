using CoinW.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order request
    /// </summary>
    public record CoinWFuturesOrderRequest
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide Side { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Quantity unit
        /// </summary>
        [JsonPropertyName("quantityUnit")]
        public QuantityUnit QuantityUnit { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity"), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Margin type
        /// </summary>
        [JsonPropertyName("positionModel")]
        public MarginType MarginType { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("positionType")]
        public FuturesOrderType OrderType { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("openPrice"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        /// <summary>
        /// Stop loss price
        /// </summary>
        [JsonPropertyName("stopLossPrice"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? StopLossPrice { get; set; }
        /// <summary>
        /// Take profit price
        /// </summary>
        [JsonPropertyName("stopProfitPrice"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? TakeProfitPrice { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger order type
        /// </summary>
        [JsonPropertyName("triggerType"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TriggerOrderType? TriggerOrderType { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("thirdOrderId"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Golden Id
        /// </summary>
        [JsonPropertyName("goldId"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? GoldenId { get; set; }
        /// <summary>
        /// Use mega coupon
        /// </summary>
        [JsonPropertyName("useAlmightyGold"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? UseMegaCoupon { get; set; }
    }
}
