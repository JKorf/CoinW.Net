using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order book update
    /// </summary>
    public record CoinWOrderBookUpdate
    {
        /// <summary>
        /// ["<c>startSeq</c>"] Start sequence
        /// </summary>
        [JsonPropertyName("startSeq")]
        public long StartSequence { get; set; }
        /// <summary>
        /// ["<c>endSeq</c>"] End sequence
        /// </summary>
        [JsonPropertyName("endSeq")]
        public long EndSequence { get; set; }

        /// <summary>
        /// ["<c>asks</c>"] Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public CoinWOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>bids</c>"] Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public CoinWOrderBookEntry[] Bids { get; set; } = [];
    }

}
