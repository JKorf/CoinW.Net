﻿using CoinW.Net.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    public record CoinWTradeUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Trade time
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }


}
