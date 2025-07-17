using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Withdraw result
    /// </summary>
    public record CoinWWithdrawResult
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("depositNumber")]
        public long Id { get; set; }
    }


}
