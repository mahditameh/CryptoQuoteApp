using System.Text.Json.Serialization;

namespace Applications.CoinMarketcap
{
    public class Quote
    {
        [JsonPropertyName("USD")]
        public USD USD { get; set; }
    }
}
