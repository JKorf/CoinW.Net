using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Edit result
    /// </summary>
    public record CoinWEditResult
    {
        /// <summary>
        /// New order id
        /// </summary>
        [JsonPropertyName("editId")]
        public long EditId { get; set; }
        /// <summary>
        /// Previous order id
        /// </summary>
        [JsonPropertyName("originId")]
        public long OriginalId { get; set; }
    }
}
