using System.Text.Json.Serialization;

namespace Application.CoinMarketcap
{
    public class QuoteJsonModel
    {
        [JsonPropertyName("status")]
        public QuoteStatus Status { get; set; }
        [JsonPropertyName("data")]
        public Dictionary<string, CryptoQuote> Data { get; set; }
    }

    public class CryptoQuote
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("quote")]
        public Quote Quote { get; set; }
    }
}
