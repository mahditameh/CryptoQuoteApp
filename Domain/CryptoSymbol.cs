using System.Text.Json.Serialization;

namespace Domain
{
    public class CryptoSymbol
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
