using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order book update
    /// </summary>
    public record CoinWOrderBookUpdate
    {
        /// <summary>
        /// Start sequence
        /// </summary>
        [JsonPropertyName("startSeq")]
        public long StartSequence { get; set; }
        /// <summary>
        /// End sequence
        /// </summary>
        [JsonPropertyName("endSeq")]
        public long EndSequence { get; set; }

        /// <summary>
        /// Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public CoinWOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public CoinWOrderBookEntry[] Bids { get; set; } = [];
    }

}
