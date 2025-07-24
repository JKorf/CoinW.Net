using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Index price
    /// </summary>
    public record CoinWPrice
    {
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal Price { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("n")]
        public string Symbol { get; set; } = string.Empty;
    }
}
