using System.Text.Json.Serialization;

namespace Infrastructure.CoinMarketcap
{
    public class QuoteData
    {
        [JsonPropertyName("BTC")]
        public BTC BTC { get; set; }
    }
}
