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
    /// Trade history
    /// </summary>
    public record CoinWTradeHistory
    {
        /// <summary>
        /// Next id
        /// </summary>
        [JsonPropertyName("nextId")]
        public long? NextId { get; set; }
        /// <summary>
        /// Prev id
        /// </summary>
        [JsonPropertyName("prevId")]
        public long? PrevId { get; set; }
        /// <summary>
        /// Rows
        /// </summary>
        [JsonPropertyName("rows")]
        public CoinWHistoryTrade[] Rows { get; set; } = [];
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Trade info
    /// </summary>
    public record CoinWHistoryTrade
    {
        /// <summary>
        /// Trade quantity in contracts
        /// </summary>
        [JsonPropertyName("closedPiece")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("dealPrice")]
        public decimal Price { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
