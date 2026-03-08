using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Edit result
    /// </summary>
    public record CoinWEditResult
    {
        /// <summary>
        /// ["<c>editId</c>"] New order id
        /// </summary>
        [JsonPropertyName("editId")]
        public long EditId { get; set; }
        /// <summary>
        /// ["<c>originId</c>"] Previous order id
        /// </summary>
        [JsonPropertyName("originId")]
        public long OriginalId { get; set; }
    }
}
