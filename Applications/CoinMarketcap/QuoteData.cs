using System.Text.Json.Serialization;

namespace Applications.CoinMarketcap
{
    public class QuoteData
    {
        [JsonPropertyName("BTC")]
        public BTC BTC { get; set; }
    }
}
