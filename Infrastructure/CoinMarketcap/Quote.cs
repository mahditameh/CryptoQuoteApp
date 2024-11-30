using System.Text.Json.Serialization;

namespace Infrastructure.CoinMarketcap
{
    public class Quote
    {
        [JsonPropertyName("USD")]
        public USD USD { get; set; }
    }
}
