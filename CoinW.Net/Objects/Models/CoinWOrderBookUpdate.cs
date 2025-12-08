using System.Text.Json.Serialization;

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
