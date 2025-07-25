using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Max position info
    /// </summary>
    public record CoinWMaxPosition
    {
        /// <summary>
        /// Available buy
        /// </summary>
        [JsonPropertyName("availBuy")]
        public decimal AvailableBuy { get; set; }
        /// <summary>
        /// Available sell
        /// </summary>
        [JsonPropertyName("availSell")]
        public decimal AvailableSell { get; set; }
    }


}
