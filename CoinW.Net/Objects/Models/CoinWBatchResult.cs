using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Batch order result
    /// </summary>
    public record CoinWBatchResult
    {
        /// <summary>
        /// Result code
        /// </summary>
        [JsonPropertyName("msgCode")]
        public int Code { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("openId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("thirdOrderId")]
        public string? ClientOrderId { get; set; }
    }
}
