using System.Text.Json.Serialization;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Order result
    /// </summary>
    public record CoinWOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderNumber")]
        public long OrderId { get; set; }
    }


}
