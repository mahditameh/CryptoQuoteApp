using System.Text.Json.Serialization;

namespace Domain.Services.Models
{
    public class CryptoSymbol
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
