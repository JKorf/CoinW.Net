using System.Text.Json.Serialization;

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
