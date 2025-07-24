using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order id
    /// </summary>
    public record CoinWOrderId
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("value")]
        public long OrderId { get; set; }
    }


}
