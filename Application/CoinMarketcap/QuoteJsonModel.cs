using System.Text.Json.Serialization;

namespace Application.CoinMarketcap
{
    public class QuoteJsonModel
    {
        [JsonPropertyName("status")]
        public QuoteStatus Status { get; set; }
        [JsonPropertyName("data")]
        public QuoteData Data { get; set; }
    }
}
