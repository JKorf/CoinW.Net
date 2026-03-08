using CoinW.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Trade history
    /// </summary>
    public record CoinWTradeHistory
    {
        /// <summary>
        /// ["<c>nextId</c>"] Next id
        /// </summary>
        [JsonPropertyName("nextId")]
        public long? NextId { get; set; }
        /// <summary>
        /// ["<c>prevId</c>"] Prev id
        /// </summary>
        [JsonPropertyName("prevId")]
        public long? PrevId { get; set; }
        /// <summary>
        /// ["<c>rows</c>"] Rows
        /// </summary>
        [JsonPropertyName("rows")]
        public CoinWHistoryTrade[] Rows { get; set; } = [];
        /// <summary>
        /// ["<c>total</c>"] Total number of results
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
        /// ["<c>closedPiece</c>"] Trade quantity in contracts
        /// </summary>
        [JsonPropertyName("closedPiece")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>createdDate</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("createdDate")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>dealPrice</c>"] Trade price
        /// </summary>
        [JsonPropertyName("dealPrice")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>direction</c>"] Position side
        /// </summary>
        [JsonPropertyName("direction")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
