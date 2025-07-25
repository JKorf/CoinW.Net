using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Max trade info
    /// </summary>
    public record CoinWMaxTrade
    {
        /// <summary>
        /// Max buy quantity in number of contracts
        /// </summary>
        [JsonPropertyName("maxBuy")]
        public int MaxBuy { get; set; }
        /// <summary>
        /// Max sell quantity in number of contracts
        /// </summary>
        [JsonPropertyName("maxSell")]
        public int MaxSell { get; set; }
    }


}
