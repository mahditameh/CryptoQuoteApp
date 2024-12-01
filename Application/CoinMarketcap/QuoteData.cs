using System.Text.Json.Serialization;

namespace Application.CoinMarketcap
{
    public class QuoteData
    {
        [JsonPropertyName("BTC")]
        public BTC BTC { get; set; }
    }
}
