﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Futures ticker update
    /// </summary>
    public record CoinWFuturesTickerUpdate
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("currencyCode")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Percentage change
        /// </summary>
        [JsonPropertyName("changeRate")]
        public decimal PercentageChange { get; set; }
        /// <summary>
        /// Highest price in last 24h
        /// </summary>
        [JsonPropertyName("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Open price 24h ago
        /// </summary>
        [JsonPropertyName("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Volume in base asset
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Volume in USDT
        /// </summary>
        [JsonPropertyName("volUsdt")]
        public decimal VolumeUsdt { get; set; }
    }
}
